using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Dapper;

namespace SchoolClearanceSystem
{
    public class DatabaseManager
    {
        // Dynamically finds your main project directory by removing the \bin\ folder structure from the path
        private static readonly string projectFolder = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { "\\bin\\" }, StringSplitOptions.None)[0];

        // Combines the safe root folder with your database filename
        private readonly string connectionString = $"Data Source={Path.Combine(projectFolder, "ClearanceSystem.db")}";

        public IDbConnection GetConnection()
        {
            var conn = new SqliteConnection(connectionString);
            conn.Open(); // Establishes the real-time stream with the .db file
            return conn;
        }

        public DataTable GetDataTable(string sql, object parameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var db = GetConnection()) // Connection safely closes when code exits this block
                {
                    // Executes raw query text safely using Dapper parameterization
                    var reader = db.ExecuteReader(sql, parameters);
                    dt.Load(reader); // Hydrates the empty table structure with structural data records
                }
            }
            catch (Exception ex)
            {
                // Catches software anomalies and warns developer/user elegantly
                MessageBox.Show("Query Error: " + ex.Message, "Database Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt; // Returns either populated records or an empty schema skeleton
        }
    }
}