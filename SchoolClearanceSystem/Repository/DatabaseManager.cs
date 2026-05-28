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
        // Permanently locking onto your new database path to stop file duplication completely
        private readonly string connectionString =
            @"Data Source=C:\Users\cuizo\source\repos\ClearanceSystem\SchoolClearanceSystem\ClearanceSystem.db;";

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

                    // Pre-define all columns as string to prevent Byte[] type mismatch
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dt.Columns.Add(reader.GetName(i), typeof(string));
                    }

                    // Manually load rows instead of dt.Load(reader)
                    while (reader.Read())
                    {
                        var row = dt.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader.IsDBNull(i) ? string.Empty : reader.GetValue(i).ToString();
                        }
                        dt.Rows.Add(row);
                    }
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