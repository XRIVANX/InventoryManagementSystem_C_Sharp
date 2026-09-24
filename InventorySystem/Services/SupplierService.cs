using System.Data;
using InventorySystem.Data;
using InventorySystem.Models;

namespace InventorySystem.Services
{
    public static class SupplierService
    {
        public static DataTable Search(string keyword)
        {
            const string sql = @"
                SELECT SupplierID, SupplierCode, SupplierName, ContactPerson, Phone, Email, Address, IsActive
                FROM Suppliers
                WHERE (@kw = '' OR SupplierCode LIKE '%' + @kw + '%'
                                OR SupplierName LIKE '%' + @kw + '%')
                ORDER BY SupplierName;";
            return DbHelper.GetData(sql, CommandType.Text, DbHelper.P("@kw", keyword ?? ""));
        }

        public static void Insert(Supplier s)
        {
            DbHelper.Execute(@"
                INSERT INTO Suppliers (SupplierCode, SupplierName, ContactPerson, Phone, Email, Address, IsActive)
                VALUES (@code,@name,@cp,@ph,@em,@addr,@act);",
                CommandType.Text, Params(s));
        }

        public static void Update(Supplier s)
        {
            var ps = new System.Collections.Generic.List<System.Data.SqlClient.SqlParameter>(Params(s));
            ps.Add(DbHelper.P("@id", s.SupplierID));
            DbHelper.Execute(@"
                UPDATE Suppliers SET SupplierCode=@code, SupplierName=@name, ContactPerson=@cp,
                       Phone=@ph, Email=@em, Address=@addr, IsActive=@act
                WHERE SupplierID=@id;", CommandType.Text, ps.ToArray());
        }

        public static string Delete(int supplierId)
        {
            bool used = System.Convert.ToInt32(DbHelper.Scalar(
                "SELECT COUNT(*) FROM Products WHERE SupplierID=@id",
                CommandType.Text, DbHelper.P("@id", supplierId))) > 0;

            if (used)
            {
                DbHelper.Execute("UPDATE Suppliers SET IsActive = 0 WHERE SupplierID = @id;",
                    CommandType.Text, DbHelper.P("@id", supplierId));
                return "This supplier is linked to existing products, so it was deactivated instead of deleted.";
            }

            DbHelper.Execute("DELETE FROM Suppliers WHERE SupplierID = @id;",
                CommandType.Text, DbHelper.P("@id", supplierId));
            return "Supplier deleted.";
        }

        private static System.Data.SqlClient.SqlParameter[] Params(Supplier s) => new[]
        {
            DbHelper.P("@code", s.SupplierCode),
            DbHelper.P("@name", s.SupplierName),
            DbHelper.P("@cp",   s.ContactPerson),
            DbHelper.P("@ph",   s.Phone),
            DbHelper.P("@em",   s.Email),
            DbHelper.P("@addr", s.Address),
            DbHelper.P("@act",  s.IsActive)
        };
    }
}