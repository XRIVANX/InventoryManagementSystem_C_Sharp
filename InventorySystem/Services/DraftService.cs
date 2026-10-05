using System;
using System.Data;
using System.Data.SqlClient;
using InventorySystem.Data;

namespace InventorySystem.Services
{
    internal sealed class SavedDraft
    {
        public DataRow Header { get; set; }
        public DataTable Lines { get; set; }
    }

    internal static class DraftService
    {
        public static DataTable Search(string keyword, string type)
        {
            return DbHelper.GetData(@"
                SELECT t.TransactionID, t.TransactionType, t.TransactionNo AS [Document No],
                       CASE t.TransactionType WHEN 'IN' THEN 'Stock In' WHEN 'OUT' THEN 'Stock Out'
                            WHEN 'TRANSFER' THEN 'Transfer' WHEN 'ADJUST' THEN 'Adjustment' END AS [Type],
                       t.TransactionDate AS [Date], t.ReferenceNo AS [Reference],
                       u.FullName AS [Created By], t.Remarks,
                       (SELECT COUNT(*) FROM StockTransactionLine l
                        WHERE l.TransactionID=t.TransactionID) AS [Items]
                FROM StockTransaction t LEFT JOIN Users u ON u.UserID=t.CreatedBy
                WHERE t.Status='DRAFT' AND t.TransactionType IN ('IN','OUT','TRANSFER','ADJUST')
                  AND (@manager=1 OR t.TransactionType<>'ADJUST')
                  AND (@type='' OR t.TransactionType=@type)
                  AND (@kw='' OR t.TransactionNo LIKE '%' + @kw + '%'
                       OR t.ReferenceNo LIKE '%' + @kw + '%' OR u.FullName LIKE '%' + @kw + '%'
                       OR t.Remarks LIKE '%' + @kw + '%')
                ORDER BY t.TransactionDate DESC, t.TransactionID DESC;", CommandType.Text,
                DbHelper.P("@kw", keyword ?? ""), DbHelper.P("@type", type ?? ""),
                DbHelper.P("@manager", Session.IsManager));
        }

        public static SavedDraft Load(int transactionId, string expectedType)
        {
            SavedDraft draft = null;
            DbHelper.RunInTransaction((cn, tx) =>
            {
                var header = Read(cn, tx, @"
                    SELECT t.*, s.SupplierName FROM StockTransaction t WITH (HOLDLOCK)
                    LEFT JOIN Suppliers s ON s.SupplierID=t.SupplierID
                    WHERE t.TransactionID=@id AND t.Status='DRAFT';", transactionId);
                if (header.Rows.Count == 0)
                    throw new InvalidOperationException("This draft is no longer available. Refresh Saved Drafts.");
                if (header.Rows[0]["TransactionType"].ToString() != expectedType)
                    throw new InvalidOperationException("The draft does not match this transaction form.");
                if (expectedType == "ADJUST" && !Session.IsManager)
                    throw new InvalidOperationException("Only a Manager or Administrator can open adjustments.");

                var lines = Read(cn, tx, @"
                    SELECT l.*, p.SKU, p.ProductName, ISNULL(b.BatchNo,'-') AS BatchNo,
                           fw.WarehouseName AS FromWarehouseName, tw.WarehouseName AS ToWarehouseName
                    FROM StockTransactionLine l
                    JOIN Products p ON p.ProductID=l.ProductID
                    LEFT JOIN ProductBatches b ON b.BatchID=l.BatchID
                    LEFT JOIN Warehouses fw ON fw.WarehouseID=l.FromWarehouseID
                    LEFT JOIN Warehouses tw ON tw.WarehouseID=l.ToWarehouseID
                    WHERE l.TransactionID=@id;", transactionId);
                if (lines.Rows.Count == 0)
                    throw new InvalidOperationException("This draft has no items to reopen.");
                // The transaction forms use one source/destination warehouse per document.
                foreach (DataRow line in lines.Rows)
                {
                    if (!Equals(line["FromWarehouseID"], lines.Rows[0]["FromWarehouseID"]) ||
                        !Equals(line["ToWarehouseID"], lines.Rows[0]["ToWarehouseID"]))
                        throw new InvalidOperationException("This draft contains multiple warehouses and cannot be edited in this form.");
                }
                draft = new SavedDraft { Header = header.Rows[0], Lines = lines };
            });
            return draft;
        }

        public static void Remove(int transactionId)
        {
            DbHelper.RunInTransaction((cn, tx) => RemoveInTransaction(cn, tx, transactionId));
        }

        internal static void RemoveInTransaction(SqlConnection cn, SqlTransaction tx, int transactionId)
        {
            var header = Read(cn, tx, @"
                SELECT TransactionType FROM StockTransaction WITH (UPDLOCK, HOLDLOCK)
                WHERE TransactionID=@id AND Status='DRAFT';", transactionId);
            if (header.Rows.Count == 0)
                throw new InvalidOperationException("This draft is no longer available. Refresh Saved Drafts.");
            string type = header.Rows[0]["TransactionType"].ToString();
            if (type == "ADJUST" && !Session.IsManager)
                throw new InvalidOperationException("Only a Manager or Administrator can remove adjustment drafts.");
            if (type != "IN" && type != "OUT" && type != "TRANSFER" && type != "ADJUST")
                throw new InvalidOperationException("This transaction type cannot be removed from Saved Drafts.");

            using (var cmd = new SqlCommand(@"
                DELETE FROM StockTransactionLine WHERE TransactionID=@id;
                DELETE FROM StockTransaction WHERE TransactionID=@id AND Status='DRAFT';", cn, tx))
            {
                cmd.Parameters.AddWithValue("@id", transactionId);
                cmd.ExecuteNonQuery();
            }
        }

        private static DataTable Read(SqlConnection cn, SqlTransaction tx, string sql, int id)
        {
            using (var cmd = new SqlCommand(sql, cn, tx))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@id", id);
                var table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }
    }
}
