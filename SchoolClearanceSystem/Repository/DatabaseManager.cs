using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Dapper;

namespace SchoolClearanceSystem
{
    public class DatabaseManager
    {
        private readonly string connectionString = $"Data Source={AppDomain.CurrentDomain.BaseDirectory}ClearanceSystem.db";

        // Helper to get an open connection
        public IDbConnection GetConnection()
        {
            var conn = new SqliteConnection(connectionString);
            conn.Open();
            return conn;
        }

        public DataTable GetDataTable(string sql, object parameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var db = GetConnection())
                {
                    var reader = db.ExecuteReader(sql, parameters);
                    dt.Load(reader);
                }
            }
            catch (Exception ex) { MessageBox.Show("Query Error: " + ex.Message); }
            return dt;
        }
    }
}