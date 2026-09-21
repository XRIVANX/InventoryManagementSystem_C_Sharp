using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace InventorySystem.Helpers
{
    public static class ErrorHandler
    {
        public static void Handle(Exception ex, string context = "")
        {
            Log(ex, context);

            string msg;
            if (ex is SqlException sql)
            {
                switch (sql.Number)
                {
                    case 2627:
                    case 2601: msg = "This record already exists. Please use a unique value."; break;
                    case 547: msg = "This record is linked to other data and cannot be changed or deleted."; break;
                    case 53:
                    case -1: msg = "Cannot reach the database server. Check that SQL Server is running."; break;
                    default: msg = sql.Message; break;   // our THROW messages land here
                }
            }
            else msg = ex.Message;

            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Log(Exception ex, string context)
        {
            try
            {
                string dir = Path.Combine(Application.StartupPath, "Logs");
                Directory.CreateDirectory(dir);
                File.AppendAllText(Path.Combine(dir, "error.log"),
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {context} | {ex}{Environment.NewLine}{Environment.NewLine}");
            }
            catch { /* logging must never crash the app */ }
        }
    }
}