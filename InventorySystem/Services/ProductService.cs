using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using InventorySystem.Data;
using InventorySystem.Models;

namespace InventorySystem.Services
{
    public static class ProductService
    {
        public static DataTable Search(string keyword, int? categoryId, bool? activeOnly)
        {
            const string sql = @"
                SELECT p.ProductID, p.SKU, p.Barcode, p.ProductName, p.Description,
                       p.CategoryID, c.CategoryName, p.UomID, u.UomCode,
                       p.SupplierID, s.SupplierName,
                       p.ReorderLevel, p.ReorderQty, p.AverageCost, p.SellingPrice,
                       p.IsBatchTracked, p.IsActive,
                       ISNULL((SELECT SUM(QtyOnHand) FROM StockBalance b
                               WHERE b.ProductID = p.ProductID), 0) AS OnHand
                FROM Products p
                JOIN Categories c    ON c.CategoryID = p.CategoryID
                JOIN UnitOfMeasure u ON u.UomID = p.UomID
                LEFT JOIN Suppliers s ON s.SupplierID = p.SupplierID
                WHERE (@kw = '' OR p.SKU LIKE '%' + @kw + '%'
                                OR p.ProductName LIKE '%' + @kw + '%'
                                OR ISNULL(p.Barcode,'') LIKE '%' + @kw + '%')
                  AND (@cat IS NULL OR p.CategoryID = @cat)
                  AND (@act IS NULL OR p.IsActive = @act)
                ORDER BY p.ProductName;";

            return DbHelper.GetData(sql, CommandType.Text,
                DbHelper.P("@kw", keyword ?? ""),
                DbHelper.P("@cat", (object)categoryId),
                DbHelper.P("@act", (object)activeOnly));
        }

        public static void Insert(Product p)
        {
            DbHelper.Execute(@"
                INSERT INTO Products (SKU, Barcode, ProductName, Description, CategoryID, UomID,
                                      SupplierID, ReorderLevel, ReorderQty, SellingPrice,
                                      IsBatchTracked, IsActive)
                VALUES (@sku,@bar,@name,@desc,@cat,@uom,@sup,@rl,@rq,@price,@batch,@act);",
                CommandType.Text, Params(p));
        }

        public static void Update(Product p)
        {
            var ps = new List<SqlParameter>(Params(p));
            ps.Add(DbHelper.P("@id", p.ProductID));
            DbHelper.Execute(@"
                UPDATE Products SET SKU=@sku, Barcode=@bar, ProductName=@name, Description=@desc,
                       CategoryID=@cat, UomID=@uom, SupplierID=@sup, ReorderLevel=@rl,
                       ReorderQty=@rq, SellingPrice=@price, IsBatchTracked=@batch, IsActive=@act
                WHERE ProductID=@id;", CommandType.Text, ps.ToArray());
        }

        public static string Delete(int productId)
        {
            bool used = System.Convert.ToInt32(DbHelper.Scalar(
                @"SELECT (SELECT COUNT(*) FROM StockTransactionLine WHERE ProductID=@id)
                        + (SELECT COUNT(*) FROM StockBalance WHERE ProductID=@id AND QtyOnHand<>0);",
                CommandType.Text, DbHelper.P("@id", productId))) > 0;

            if (used)
            {
                DbHelper.Execute("UPDATE Products SET IsActive = 0 WHERE ProductID = @id;",
                    CommandType.Text, DbHelper.P("@id", productId));
                return "This product has stock or transaction history, so it was deactivated instead of deleted.";
            }

            DbHelper.Execute("DELETE FROM Products WHERE ProductID = @id;",
                CommandType.Text, DbHelper.P("@id", productId));
            return "Product deleted.";
        }

        private static SqlParameter[] Params(Product p) => new[]
        {
            DbHelper.P("@sku",  p.SKU),
            DbHelper.P("@bar",  p.Barcode),
            DbHelper.P("@name", p.ProductName),
            DbHelper.P("@desc", p.Description),
            DbHelper.P("@cat",  p.CategoryID),
            DbHelper.P("@uom",  p.UomID),
            DbHelper.P("@sup",  p.SupplierID),
            DbHelper.P("@rl",   p.ReorderLevel),
            DbHelper.P("@rq",   p.ReorderQty),
            DbHelper.P("@price",p.SellingPrice),
            DbHelper.P("@batch",p.IsBatchTracked),
            DbHelper.P("@act",  p.IsActive)
        };
    }
}