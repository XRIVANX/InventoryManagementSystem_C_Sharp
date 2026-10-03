using System;
using System.Data;
using InventorySystem.Data;

namespace InventorySystem.Services
{
    public enum InventoryReport { Valuation, StockCard, LowStock, Movement }

    public static class ReportService
    {
        public static DataTable Warehouses() => DbHelper.GetData("SELECT WarehouseID, WarehouseName FROM Warehouses ORDER BY WarehouseName");
        public static DataTable Products() => DbHelper.GetData("SELECT ProductID, SKU + ' - ' + ProductName AS Display FROM Products ORDER BY ProductName");

        public static DataTable TransactionLines(int transactionId)
        {
            return DbHelper.GetData(@"SELECT t.TransactionDate AS [Date],t.TransactionNo AS [Document],t.TransactionType AS [Type],
                       p.SKU,p.ProductName AS [Product],ISNULL(fw.WarehouseName,'-') AS [From Warehouse],
                       ISNULL(tw.WarehouseName,'-') AS [To Warehouse],tl.Quantity AS [Quantity],
                       tl.UnitCost AS [Unit Cost],tl.Quantity*tl.UnitCost AS [Total Cost]
                FROM StockTransaction t JOIN StockTransactionLine tl ON tl.TransactionID=t.TransactionID
                JOIN Products p ON p.ProductID=tl.ProductID
                LEFT JOIN Warehouses fw ON fw.WarehouseID=tl.FromWarehouseID
                LEFT JOIN Warehouses tw ON tw.WarehouseID=tl.ToWarehouseID
                WHERE t.TransactionID=@id AND t.Status='POSTED' ORDER BY p.ProductName",CommandType.Text,DbHelper.P("@id",transactionId));
        }


        public static DataTable Generate(InventoryReport report, int warehouse, int product, DateTime from, DateTime to)
        {
            if ((report == InventoryReport.StockCard || report == InventoryReport.Movement) && from.Date > to.Date)
                throw new ArgumentException("The start date must be on or before the end date.");
            if (report == InventoryReport.StockCard && product == 0)
                throw new ArgumentException("Choose a product to generate its stock card.");
            string sql;
            switch (report)
            {
                case InventoryReport.Valuation:
                    sql = @"SELECT w.WarehouseName AS [Warehouse], p.SKU, p.ProductName AS [Product],
                                   ISNULL(pb.BatchNo, '-') AS [Batch], u.UomCode AS [UOM],
                                   sb.QtyOnHand AS [On Hand], ISNULL(p.AverageCost,0) AS [Average Cost],
                                   sb.QtyOnHand * ISNULL(p.AverageCost,0) AS [Stock Value]
                            FROM StockBalance sb
                            JOIN Products p ON p.ProductID=sb.ProductID
                            JOIN Warehouses w ON w.WarehouseID=sb.WarehouseID
                            LEFT JOIN ProductBatches pb ON pb.BatchID=sb.BatchID
                            LEFT JOIN UnitOfMeasure u ON u.UomID=p.UomID
                            WHERE sb.QtyOnHand<>0 AND (@w=0 OR sb.WarehouseID=@w) AND (@p=0 OR sb.ProductID=@p)
                            ORDER BY w.WarehouseName,p.ProductName,pb.BatchNo";
                    break;
                case InventoryReport.LowStock:
                    // Aggregate batches before comparing to the product's reorder level; keep zero-stock products.
                    sql = @"WITH balances AS (
                                SELECT ProductID,SUM(QtyOnHand) AS Qty FROM StockBalance
                                WHERE @w=0 OR WarehouseID=@w GROUP BY ProductID)
                            SELECT p.SKU,p.ProductName AS [Product],ISNULL(s.SupplierName,'-') AS [Supplier],
                                   u.UomCode AS [UOM],ISNULL(b.Qty,0) AS [On Hand],p.ReorderLevel AS [Reorder Level],
                                   CASE WHEN p.ReorderLevel>ISNULL(b.Qty,0) THEN p.ReorderLevel-ISNULL(b.Qty,0) ELSE 0 END AS [Shortfall],
                                   CASE WHEN p.ReorderQty > p.ReorderLevel-ISNULL(b.Qty,0) THEN p.ReorderQty
                                        ELSE p.ReorderLevel-ISNULL(b.Qty,0) END AS [Suggested Order]
                            FROM Products p LEFT JOIN balances b ON b.ProductID=p.ProductID
                            LEFT JOIN Suppliers s ON s.SupplierID=p.SupplierID LEFT JOIN UnitOfMeasure u ON u.UomID=p.UomID
                            WHERE p.IsActive=1 AND ISNULL(b.Qty,0)<=p.ReorderLevel AND (@p=0 OR p.ProductID=@p)
                            ORDER BY [Shortfall] DESC,p.ProductName";
                    break;
                case InventoryReport.StockCard:
                    // Anchor historical opening stock to today's balance. Signed adjustments and transfers
                    // follow the same source/destination warehouse conventions as the transaction forms.
                    sql = @"WITH movements AS (
                                SELECT t.TransactionID,t.TransactionDate,t.TransactionNo,t.TransactionType,
                                       SUM(CASE WHEN tl.ToWarehouseID IS NOT NULL AND (@w=0 OR tl.ToWarehouseID=@w) THEN tl.Quantity ELSE 0 END
                                         - CASE WHEN tl.FromWarehouseID IS NOT NULL AND (@w=0 OR tl.FromWarehouseID=@w) THEN tl.Quantity ELSE 0 END) AS Delta
                                FROM StockTransaction t JOIN StockTransactionLine tl ON tl.TransactionID=t.TransactionID
                                WHERE t.Status='POSTED' AND tl.ProductID=@p
                                  AND (@w=0 OR tl.ToWarehouseID=@w OR tl.FromWarehouseID=@w)
                                GROUP BY t.TransactionID,t.TransactionDate,t.TransactionNo,t.TransactionType),
                            opening AS (
                                SELECT ISNULL((SELECT SUM(QtyOnHand) FROM StockBalance WHERE ProductID=@p AND (@w=0 OR WarehouseID=@w)),0)
                                     - ISNULL((SELECT SUM(Delta) FROM movements WHERE TransactionDate>=@from),0) AS Qty),
                            card AS (
                                SELECT 0 AS SortOrder,0 AS TransactionID,@from AS [Date],CAST('Opening balance' AS nvarchar(100)) AS [Document],
                                       CAST('OPENING' AS nvarchar(30)) AS [Type],CAST(0 AS decimal(28,6)) AS [Stock In],CAST(0 AS decimal(28,6)) AS [Stock Out],Qty AS [Balance]
                                FROM opening
                                UNION ALL
                                SELECT 1,TransactionID,TransactionDate,TransactionNo,TransactionType,
                                       CASE WHEN Delta>0 THEN Delta ELSE 0 END,CASE WHEN Delta<0 THEN -Delta ELSE 0 END,
                                       (SELECT Qty FROM opening)+SUM(Delta) OVER(ORDER BY TransactionDate,TransactionID ROWS UNBOUNDED PRECEDING)
                                FROM movements WHERE TransactionDate>=@from AND TransactionDate<@until)
                            SELECT [Date],[Document],[Type],[Stock In],[Stock Out],[Balance] FROM card ORDER BY SortOrder,[Date],TransactionID";
                    break;
                default:
                    sql = @"SELECT t.TransactionDate AS [Date],t.TransactionNo AS [Document],t.TransactionType AS [Type],
                                   p.SKU,p.ProductName AS [Product],ISNULL(fw.WarehouseName,'-') AS [From Warehouse],
                                   ISNULL(tw.WarehouseName,'-') AS [To Warehouse],tl.Quantity AS [Quantity],
                                   tl.UnitCost AS [Unit Cost],tl.Quantity*tl.UnitCost AS [Total Cost]
                            FROM StockTransaction t JOIN StockTransactionLine tl ON tl.TransactionID=t.TransactionID
                            JOIN Products p ON p.ProductID=tl.ProductID
                            LEFT JOIN Warehouses fw ON fw.WarehouseID=tl.FromWarehouseID
                            LEFT JOIN Warehouses tw ON tw.WarehouseID=tl.ToWarehouseID
                            WHERE t.Status='POSTED' AND t.TransactionDate>=@from AND t.TransactionDate<@until
                              AND (@w=0 OR tl.FromWarehouseID=@w OR tl.ToWarehouseID=@w) AND (@p=0 OR tl.ProductID=@p)
                            ORDER BY t.TransactionDate,t.TransactionID,p.ProductName";
                    break;
            }
            return DbHelper.GetData(sql, CommandType.Text, DbHelper.P("@w",warehouse),DbHelper.P("@p",product),
                DbHelper.P("@from",from.Date),DbHelper.P("@until",to.Date.AddDays(1)));
        }
    }
}

