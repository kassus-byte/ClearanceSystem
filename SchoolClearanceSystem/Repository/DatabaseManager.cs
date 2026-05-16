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

        // Standard helper for backward compatibility with older components
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

        
        public bool SubmitClearanceRequest(ClearanceRequest request)
        {
            try
            {
                string sql = @"INSERT INTO ClearanceRequests 
                               (UserId, Status, Remarks, Semester, AcademicYear, DateSubmitted) 
                               VALUES (@UserId, @Status, @Remarks, @Semester, @AcademicYear, @DateSubmitted)";

                using (var db = GetConnection())
                {
                    // Dapper reads properties directly from the object (including those inherited from User)
                    int rowsAffected = db.Execute(sql, request);
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Insert Error: " + ex.Message);
                return false;
            }
        }

        // Fetch method used to live-sync and refresh student notification states
        public List<ClearanceRequest> GetRequestsByStudent(string userId)
        {
            try
            {
                string sql = "SELECT * FROM ClearanceRequests WHERE UserId = @UserId";

                using (var db = GetConnection())
                {
                    // Dapper map engine processes data directly into a collection list
                    return db.Query<ClearanceRequest>(sql, new { UserId = userId }).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fetch Error: " + ex.Message);
                return new List<ClearanceRequest>();
            }
        }
    }
}