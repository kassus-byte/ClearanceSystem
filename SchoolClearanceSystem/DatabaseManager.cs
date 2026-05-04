using DevExpress.XtraEditors;
using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Data.SQLite;

namespace SchoolClearanceSystem
{
    internal class DatabaseManager
    {
        // FIX: Use the full path to your .db file to avoid the "No such table" error
        // Replace the path below with the one from your DB Browser Title Bar
        private string connectionString = @"Data Source=C:\Users\cuizo\source\repos\ClearanceSystem\SchoolClearanceSystem\bin\Debug\ClearanceDB.db;";

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }

        // Method to save a new student from the Registration form
        public bool SaveUser(User user)
        {
            try
            {
                using (SQLiteConnection conn = GetConnection())
                {
                    // Ensure table name 'Users' matches your DB Browser exactly (Case Sensitive)
                    string query = "INSERT INTO Users (UserID, FullName, Program, Year, Role, Password, UploadPath) " +
                                   "VALUES (@id, @name, @prog, @year, @role, @pass, @path)";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", user.UserID);
                        cmd.Parameters.AddWithValue("@name", user.FullName);
                        cmd.Parameters.AddWithValue("@prog", user.Program);
                        cmd.Parameters.AddWithValue("@year", user.Year);
                        cmd.Parameters.AddWithValue("@role", user.Role);
                        cmd.Parameters.AddWithValue("@pass", user.Password);
                        cmd.Parameters.AddWithValue("@path", user.UploadPath);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Database Error: " + ex.Message, "Error");
                return false;
            }
        }

        // Method to fetch data for your GridControls
        public DataTable GetDataTable(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SQLiteConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Fetch Error: " + ex.Message);
            }
            return dt;
        }

        // Method for simple updates (like the Toggle Switch)
        public void ExecuteQuery(string query)
        {
            try
            {
                using (SQLiteConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Execution Error: " + ex.Message);
            }
        }
    }
}