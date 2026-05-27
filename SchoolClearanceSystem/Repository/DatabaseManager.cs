using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace SchoolClearanceSystem
{
    public class DatabaseManager
    {
        private readonly string connectionString =
            $"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClearanceSystem.db")}";

        public IDbConnection GetConnection()
        {
            var conn = new SqliteConnection(connectionString);
            conn.Open();
            return conn;
        }

        public DataTable GetDataTable(string sql, object parameters = null)
        {
            var dt = new DataTable();
            try
            {
                using (var db = GetConnection())
                {
                    var reader = db.ExecuteReader(sql, parameters);
                    dt.Load(reader);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Query Error: " + ex.Message, "Database Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }
    }
}