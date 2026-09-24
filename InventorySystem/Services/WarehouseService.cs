using System.Data;
using InventorySystem.Data;
using InventorySystem.Models;

namespace InventorySystem.Services
{
    public static class WarehouseService
    {
        public static DataTable Search(string keyword)
        {
            const string sql = @"
                SELECT WarehouseID, WarehouseCode, WarehouseName, Location, IsActive
                FROM Warehouses
                WHERE (@kw = '' OR WarehouseCode LIKE '%' + @kw + '%'
                                OR WarehouseName LIKE '%' + @kw + '%')
                ORDER BY WarehouseName;";
            return DbHelper.GetData(sql, CommandType.Text, DbHelper.P("@kw", keyword ?? ""));
        }

        public static void Insert(Warehouse w)
        {
            DbHelper.Execute(
                "INSERT INTO Warehouses (WarehouseCode, WarehouseName, Location, IsActive) VALUES (@code,@name,@loc,@act);",
                CommandType.Text,
                DbHelper.P("@code", w.WarehouseCode),
                DbHelper.P("@name", w.WarehouseName),
                DbHelper.P("@loc", w.Location),
                DbHelper.P("@act", w.IsActive));
        }

        public static void Update(Warehouse w)
        {
            DbHelper.Execute(
                "UPDATE Warehouses SET WarehouseCode=@code, WarehouseName=@name, Location=@loc, IsActive=@act WHERE WarehouseID=@id;",
                CommandType.Text,
                DbHelper.P("@code", w.WarehouseCode),
                DbHelper.P("@name", w.WarehouseName),
                DbHelper.P("@loc", w.Location),
                DbHelper.P("@act", w.IsActive),
                DbHelper.P("@id", w.WarehouseID));
        }

        public static string Delete(int warehouseId)
        {
            bool used = System.Convert.ToInt32(DbHelper.Scalar(
                @"SELECT (SELECT COUNT(*) FROM StockBalance WHERE WarehouseID=@id AND QtyOnHand<>0)
                        + (SELECT COUNT(*) FROM StockTransactionLine WHERE FromWarehouseID=@id OR ToWarehouseID=@id);",
                CommandType.Text, DbHelper.P("@id", warehouseId))) > 0;

            if (used)
            {
                DbHelper.Execute("UPDATE Warehouses SET IsActive = 0 WHERE WarehouseID = @id;",
                    CommandType.Text, DbHelper.P("@id", warehouseId));
                return "This warehouse has stock history, so it was deactivated instead of deleted.";
            }

            DbHelper.Execute("DELETE FROM Warehouses WHERE WarehouseID = @id;",
                CommandType.Text, DbHelper.P("@id", warehouseId));
            return "Warehouse deleted.";
        }
    }
}