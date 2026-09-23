using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using InventorySystem.Data;
using InventorySystem.Models;

namespace InventorySystem.Services
{
    public static class CategoryService
    {
        public static DataTable Search(string keyword)
        {
            const string sql = @"
                SELECT CategoryID, CategoryName, IsActive
                FROM Categories
                WHERE (@kw = '' OR CategoryName LIKE '%' + @kw + '%')
                ORDER BY CategoryName;";
            return DbHelper.GetData(sql, CommandType.Text, DbHelper.P("@kw", keyword ?? ""));
        }

        public static void Insert(Category c)
        {
            DbHelper.Execute(
                "INSERT INTO Categories (CategoryName, IsActive) VALUES (@name, @act);",
                CommandType.Text,
                DbHelper.P("@name", c.CategoryName),
                DbHelper.P("@act", c.IsActive));
        }

        public static void Update(Category c)
        {
            DbHelper.Execute(
                "UPDATE Categories SET CategoryName=@name, IsActive=@act WHERE CategoryID=@id;",
                CommandType.Text,
                DbHelper.P("@name", c.CategoryName),
                DbHelper.P("@act", c.IsActive),
                DbHelper.P("@id", c.CategoryID));
        }

        public static string Delete(int categoryId)
        {
            bool used = System.Convert.ToInt32(DbHelper.Scalar(
                "SELECT COUNT(*) FROM Products WHERE CategoryID=@id",
                CommandType.Text, DbHelper.P("@id", categoryId))) > 0;

            if (used)
            {
                DbHelper.Execute("UPDATE Categories SET IsActive = 0 WHERE CategoryID = @id;",
                    CommandType.Text, DbHelper.P("@id", categoryId));
                return "This category is used by existing products, so it was deactivated instead of deleted.";
            }

            DbHelper.Execute("DELETE FROM Categories WHERE CategoryID = @id;",
                CommandType.Text, DbHelper.P("@id", categoryId));
            return "Category deleted.";
        }
    }
}
