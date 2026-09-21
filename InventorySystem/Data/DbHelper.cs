using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace InventorySystem.Data
{
    public static class DbHelper
    {
        private static readonly string ConnStr =
            ConfigurationManager.ConnectionStrings["InventoryDB"].ConnectionString;

        public static SqlConnection GetConnection() => new SqlConnection(ConnStr);

        public static bool TestConnection(out string message)
        {
            try
            {
                using (var cn = GetConnection()) { cn.Open(); }
                message = "Connected.";
                return true;
            }
            catch (Exception ex) { message = ex.Message; return false; }
        }

        private static SqlCommand Build(SqlConnection cn, string sql,
                                        CommandType type, SqlParameter[] ps)
        {
            var cmd = new SqlCommand(sql, cn) { CommandType = type, CommandTimeout = 60 };
            if (ps != null) cmd.Parameters.AddRange(ps);
            return cmd;
        }

        public static DataTable GetData(string sql, CommandType type = CommandType.Text,
                                        params SqlParameter[] ps)
        {
            using (var cn = GetConnection())
            using (var cmd = Build(cn, sql, type, ps))
            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static int Execute(string sql, CommandType type = CommandType.Text,
                                  params SqlParameter[] ps)
        {
            using (var cn = GetConnection())
            using (var cmd = Build(cn, sql, type, ps))
            {
                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static object Scalar(string sql, CommandType type = CommandType.Text,
                                    params SqlParameter[] ps)
        {
            using (var cn = GetConnection())
            using (var cmd = Build(cn, sql, type, ps))
            {
                cn.Open();
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>Runs several statements as one all-or-nothing unit.</summary>
        public static void RunInTransaction(Action<SqlConnection, SqlTransaction> work)
        {
            using (var cn = GetConnection())
            {
                cn.Open();
                using (var tx = cn.BeginTransaction())
                {
                    try { work(cn, tx); tx.Commit(); }
                    catch { tx.Rollback(); throw; }
                }
            }
        }

        public static SqlParameter P(string name, object value)
            => new SqlParameter(name, value ?? DBNull.Value);
    }
}