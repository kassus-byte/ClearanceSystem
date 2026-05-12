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
        private IDbConnection GetConnection()
        {
            var conn = new SqliteConnection(connectionString);
            conn.Open();
            return conn;
        }

        // --- SYSTEM SETTINGS ---
        public bool IsClearanceActive()
        {
            using (var db = GetConnection())
            {
                return db.ExecuteScalar<int>("SELECT ClearanceIsActive FROM SystemSettings LIMIT 1") == 1;
            }
        }

        public void ToggleClearanceSeason(bool isActive)
        {
            using (var db = GetConnection())
            {
                db.Execute("UPDATE SystemSettings SET ClearanceIsActive = @val", new { val = isActive ? 1 : 0 });
            }
        }

        // --- USER MANAGEMENT ---

        /// <summary>
        /// Retrieves users based on their role using Dapper for automatic OOP mapping.
        /// </summary>
        public List<User> GetUsersByRole(string role, bool isStudent = true)
        {
            using (var db = GetConnection())
            {
                // Fixed the "CORM" typo to "FROM"
                string sql = isStudent
                    ? "SELECT * FROM Users WHERE Role = 'Student' ORDER BY FullName ASC"
                    : "SELECT * FROM Users WHERE Role != 'Student' AND Role != 'Admin' ORDER BY Role ASC";

                return db.Query<User>(sql).ToList() ?? new List<User>();
            }
        }

        public User GetUserDetails(string userId)
        {
            using (var db = GetConnection())
            {
                return db.QueryFirstOrDefault<User>("SELECT * FROM Users WHERE UserID = @id", new { id = userId });
            }
        }

        public bool SaveUser(User user)
        {
            try
            {
                user.DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string sql = @"INSERT INTO Users (UserID, FullName, Program, Year, Role, Password, UploadPath, DateCreated) 
                               VALUES (@UserID, @FullName, @Program, @Year, @Role, @Password, @UploadPath, @DateCreated)";

                using (var db = GetConnection())
                {
                    return db.Execute(sql, user) > 0;
                }
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
            {
                MessageBox.Show($"The User ID '{user.UserID}' is already registered!", "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// OOP Refactor: Passes the User object instead of multiple strings.
        /// </summary>
        public bool UpdateUser(User user)
        {
            using (var db = GetConnection())
            {
                string sql = @"UPDATE Users 
                               SET FullName = @FullName, Program = @Program, Year = @Year, Role = @Role 
                               WHERE UserID = @UserID";

                return db.Execute(sql, user) > 0;
            }
        }

        public bool ValidateLogin(string userId, string password)
        {
            using (var db = GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM Users WHERE UserID = @id COLLATE NOCASE AND Password = @pass";
                return db.ExecuteScalar<int>(sql, new { id = userId.Trim(), pass = password.Trim() }) > 0;
            }
        }

        public bool DeleteUser(string userId)
        {
            using (var db = GetConnection())
            {
                return db.Execute("DELETE FROM Users WHERE UserID = @id", new { id = userId }) > 0;
            }
        }

        // --- CLEARANCE REQUESTS ---

        public bool SubmitClearanceRequest(string studentId, string dept, string semester, string acadYear)
        {
            using (var db = GetConnection())
            {
                string sql = @"INSERT INTO ClearanceRequests (StudentID, Department, Status, DateSubmitted, Semester, AcademicYear) 
                               VALUES (@id, @dept, 'Pending', @date, @sem, @ay)";

                return db.Execute(sql, new
                {
                    id = studentId,
                    dept,
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    sem = semester,
                    ay = acadYear
                }) > 0;
            }
        }

        public bool UpdateRequestStatus(string studentId, string department, string newStatus, string remarks)
        {
            using (var db = GetConnection())
            {
                string sql = @"UPDATE ClearanceRequests 
                               SET Status = @status, Remarks = @remarks, DateProcessed = @date 
                               WHERE StudentID = @id AND Department = @dept";

                return db.Execute(sql, new
                {
                    status = newStatus,
                    remarks,
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    id = studentId,
                    dept = department
                }) > 0;
            }
        }

        // --- LEGACY SUPPORT ---
        // Keep this if you still have old code using DataTables, 
        // but try to migrate to the List<User> methods above!
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