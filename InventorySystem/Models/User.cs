using System.Data;
using InventorySystem.Data;
using InventorySystem.Models;

namespace InventorySystem.Services
{
    public static class UserService
    {
        public static DataTable Search(string keyword)
        {
            const string sql = @"
                SELECT u.UserID, u.Username, u.FullName, u.Email, u.RoleID, r.RoleName, u.IsActive
                FROM Users u
                JOIN Roles r ON r.RoleID = u.RoleID
                WHERE (@kw = '' OR u.Username LIKE '%' + @kw + '%'
                                OR u.FullName LIKE '%' + @kw + '%')
                ORDER BY u.Username;";
            return DbHelper.GetData(sql, CommandType.Text, DbHelper.P("@kw", keyword ?? ""));
        }

        public static void Update(User u)
        {
            DbHelper.Execute(
                "UPDATE Users SET FullName=@fn, Email=@em, RoleID=@r, IsActive=@act WHERE UserID=@id;",
                CommandType.Text,
                DbHelper.P("@fn", u.FullName),
                DbHelper.P("@em", u.Email),
                DbHelper.P("@r", u.RoleID),
                DbHelper.P("@act", u.IsActive),
                DbHelper.P("@id", u.UserID));
        }

        public static void ResetPassword(int userId, string newPassword)
        {
            string salt = AuthService.CreateSalt();
            string hash = AuthService.HashPassword(newPassword, salt);
            DbHelper.Execute(
                "UPDATE Users SET PasswordHash=@h, PasswordSalt=@s WHERE UserID=@id;",
                CommandType.Text,
                DbHelper.P("@h", hash),
                DbHelper.P("@s", salt),
                DbHelper.P("@id", userId));
        }

        public static string Deactivate(int userId)
        {
            DbHelper.Execute("UPDATE Users SET IsActive = 0 WHERE UserID = @id;",
                CommandType.Text, DbHelper.P("@id", userId));
            return "User deactivated.";
        }
    }
}