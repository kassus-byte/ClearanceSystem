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
        private readonly string connectionString =
     $"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClearanceSystem.db")}";

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