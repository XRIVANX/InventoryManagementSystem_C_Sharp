using InventorySystem.Data;
using System;
using System.Data;
using System.Security.Cryptography;
using static System.Collections.Specialized.BitVector32;

namespace InventorySystem.Services
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public static class AuthService
    {
        private const int SaltSize = 16, HashSize = 32, Iterations = 20000;

        public static string CreateSalt()
        {
            var bytes = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider()) rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public static string HashPassword(string password, string salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), Iterations))
                return Convert.ToBase64String(pbkdf2.GetBytes(HashSize));
        }

        public static AuthResult Login(string username, string password)
        {
            var dt = DbHelper.GetData("sp_GetUserForLogin", CommandType.StoredProcedure,
                                      DbHelper.P("@Username", username));

            if (dt.Rows.Count == 0)
                return new AuthResult { Success = false, Message = "Invalid username or password." };

            var r = dt.Rows[0];

            if (!Convert.ToBoolean(r["IsActive"]))
                return new AuthResult { Success = false, Message = "This account has been deactivated." };

            if (r["LockedUntil"] != DBNull.Value && Convert.ToDateTime(r["LockedUntil"]) > DateTime.Now)
                return new AuthResult
                {
                    Success = false,
                    Message = "Account locked. Try again after " +
                              Convert.ToDateTime(r["LockedUntil"]).ToString("hh:mm tt") + "."
                };

            int userId = Convert.ToInt32(r["UserID"]);
            string expected = r["PasswordHash"].ToString();
            string actual = HashPassword(password, r["PasswordSalt"].ToString());

            if (!FixedTimeEquals(expected, actual))
            {
                DbHelper.Execute("sp_RecordLoginResult", CommandType.StoredProcedure,
                    DbHelper.P("@UserID", userId), DbHelper.P("@Success", false));
                return new AuthResult { Success = false, Message = "Invalid username or password." };
            }

            DbHelper.Execute("sp_RecordLoginResult", CommandType.StoredProcedure,
                DbHelper.P("@UserID", userId), DbHelper.P("@Success", true));

            Session.UserID = userId;
            Session.Username = r["Username"].ToString();
            Session.FullName = r["FullName"].ToString();
            Session.RoleName = r["RoleName"].ToString();

            return new AuthResult { Success = true, Message = "Welcome, " + Session.FullName + "!" };
        }

        private static bool FixedTimeEquals(string a, string b)
        {
            if (a.Length != b.Length) return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++) diff |= a[i] ^ b[i];
            return diff == 0;
        }

        public static void CreateUser(string username, string password, string fullName,
                                      string email, int roleId)
        {
            string salt = CreateSalt();
            DbHelper.Execute(
                @"INSERT INTO Users (Username, PasswordHash, PasswordSalt, FullName, Email, RoleID)
                  VALUES (@u, @h, @s, @f, @e, @r);",
                CommandType.Text,
                DbHelper.P("@u", username),
                DbHelper.P("@h", HashPassword(password, salt)),
                DbHelper.P("@s", salt),
                DbHelper.P("@f", fullName),
                DbHelper.P("@e", email),
                DbHelper.P("@r", roleId));
        }

        public static void EnsureDefaultAdmin()
        {
            var n = DbHelper.Scalar("SELECT COUNT(*) FROM Users");
            if (Convert.ToInt32(n) == 0)
                CreateUser("admin", "Admin@123", "System Administrator", "admin@inventory.local", 1);
        }
    }
}