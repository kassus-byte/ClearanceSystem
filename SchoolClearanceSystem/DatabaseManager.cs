using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Windows.Forms;

namespace SchoolClearanceSystem
{
    public class DatabaseManager
    {
        private string connectionString = "Data Source=ClearanceSystem.db";

        // --- FIXED: CLEARANCE SEASON CONTROL WITH SAFETY ---
        public bool IsClearanceActive()
        {
            try
            {
                // We use a direct check here to avoid the global try-catch in GetDataTable 
                // if we just want a quiet true/false check.
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT ClearanceIsActive FROM SystemSettings LIMIT 1";
                    using (var command = new SqliteCommand(sql, connection))
                    {
                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            return result.ToString() == "1";
                        }
                    }
                }
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 1) // Table not found
            {
                // Table doesn't exist yet? Don't crash, just say clearance is inactive.
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        public void ToggleClearanceSeason(bool isActive)
        {
            int value = isActive ? 1 : 0;
            // Using parameterized query for safety
            string sql = "UPDATE SystemSettings SET ClearanceIsActive = @val";
            var param = new SqliteParameter("@val", value);
            ExecuteNonQuery(sql, new[] { param });
        }

        // --- GENERIC DATA FETCHER ---
        public DataTable GetDataTable(string sql, SqliteParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqliteCommand(sql, connection))
                    {
                        if (parameters != null)
                            command.Parameters.AddRange(parameters);

                        using (var reader = command.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Only show message box for unexpected errors, not missing tables during startup
                MessageBox.Show("Database Error: " + ex.Message);
            }
            return dt;
        }

        // --- DELETE USER LOGIC ---
        public bool DeleteUser(string userId)
        {
            string sql = "DELETE FROM Users WHERE UserID = @id";
            var param = new SqliteParameter("@id", userId);
            return ExecuteNonQuery(sql, new[] { param }) > 0;
        }

        // Helper method to reduce code repetition
        private int ExecuteNonQuery(string sql, SqliteParameter[] parameters = null)
        {
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqliteCommand(sql, connection))
                    {
                        if (parameters != null) command.Parameters.AddRange(parameters);
                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Execution Error: " + ex.Message);
                return -1;
            }
        }

        // --- REMAINING METHODS (SaveUser, ValidateLogin, GetUserDetails, etc.) ---
        // Keep your existing implementations for these below...
        public bool SaveUser(User user)
        {
            // Added UploadPath to the SQL and values
            string sql = "INSERT INTO Users (UserID, FullName, Program, Year, Role, Password, UploadPath) " +
                         "VALUES (@id, @name, @prog, @year, @role, @pass, @path)";

            SqliteParameter[] ps = {
        new SqliteParameter("@id", user.UserID),
        new SqliteParameter("@name", user.FullName),
        new SqliteParameter("@prog", user.Program ?? "N/A"),
        new SqliteParameter("@year", user.Year ?? "N/A"),
        new SqliteParameter("@role", user.Role ?? "Student"),
        new SqliteParameter("@pass", user.Password),
        new SqliteParameter("@path", user.UploadPath ?? "") // Save the file path here
    };

            // Changed to return bool so your Registration form knows if it succeeded
            return ExecuteNonQuery(sql, ps) > 0;
        }

        public bool ValidateLogin(string userId, string password)
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE UserID = @id AND Password = @pass";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", userId);
                    command.Parameters.AddWithValue("@pass", password);
                    return Convert.ToInt64(command.ExecuteScalar()) > 0;
                }
            }
        }

        // --- ADD THIS BACK TO DATABASEMANAGER.CS ---
        public DataTable GetDepartmentRequests(string departmentName)
        {
            string sql = @"SELECT r.StudentID, u.FullName, u.Program, u.Year, 
                          r.Semester, r.AcademicYear, r.Status 
                   FROM ClearanceRequests r
                   INNER JOIN Users u ON r.StudentID = u.UserID
                   WHERE r.Department = @dept";

            var param = new SqliteParameter("@dept", departmentName);
            return GetDataTable(sql, new[] { param });
        }

        public User GetUserDetails(string userId)
        {
            // Added UploadPath to the SELECT
            string sql = "SELECT FullName, UserID, Program, Year, Role, UploadPath FROM Users WHERE UserID = @id";
            DataTable dt = GetDataTable(sql, new[] { new SqliteParameter("@id", userId) });

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                return new User
                {
                    FullName = dr["FullName"].ToString(),
                    UserID = dr["UserID"].ToString(),
                    Program = dr["Program"].ToString(),
                    Year = dr["Year"].ToString(),
                    Role = dr["Role"].ToString(),
                    UploadPath = dr["UploadPath"].ToString() // Now we can show the ID photo!
                };
            }
            return null;
        }

        public bool SubmitClearanceRequest(string studentId, string dept, string semester, string acadYear)
        {
            string sql = @"INSERT INTO ClearanceRequests (StudentID, Department, Status, DateSubmitted, Semester, AcademicYear) 
                           VALUES (@id, @dept, 'Pending', @date, @sem, @ay)";

            SqliteParameter[] ps = {
                new SqliteParameter("@id", studentId),
                new SqliteParameter("@dept", dept),
                new SqliteParameter("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new SqliteParameter("@sem", semester),
                new SqliteParameter("@ay", acadYear)
            };
            return ExecuteNonQuery(sql, ps) > 0;
        }
    }
}