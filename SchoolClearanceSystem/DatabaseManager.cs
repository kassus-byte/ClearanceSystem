using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Windows.Forms;

namespace SchoolClearanceSystem
{
    public class DatabaseManager
    {
        // Update this line at the top of DatabaseManager.cs
        private string connectionString = $"Data Source={AppDomain.CurrentDomain.BaseDirectory}ClearanceSystem.db";

        // --- CLEARANCE SEASON CONTROL ---
        public bool IsClearanceActive()
        {
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT ClearanceIsActive FROM SystemSettings LIMIT 1";
                    using (var command = new SqliteCommand(sql, connection))
                    {
                        var result = command.ExecuteScalar();
                        return result != null && result.ToString() == "1";
                    }
                }
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 1)
            {
                return false; // Table doesn't exist
            }
            catch { return false; }
        }

        public void ToggleClearanceSeason(bool isActive)
        {
            string sql = "UPDATE SystemSettings SET ClearanceIsActive = @val";
            var param = new SqliteParameter("@val", isActive ? 1 : 0);
            ExecuteNonQuery(sql, new[] { param });
        }

        // --- GENERIC DATA TOOLS ---
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
                        if (parameters != null) command.Parameters.AddRange(parameters);
                        using (var reader = command.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Query Error: " + ex.Message);
            }
            return dt;
        }

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

        // --- USER MANAGEMENT ---
        public bool SaveUser(User user)
        {
            string sql = "INSERT INTO Users (UserID, FullName, Program, Year, Role, Password, UploadPath) " +
                         "VALUES (@id, @name, @prog, @year, @role, @pass, @path)";

            SqliteParameter[] ps = {
                new SqliteParameter("@id", user.UserID),
                new SqliteParameter("@name", user.FullName),
                new SqliteParameter("@prog", user.Program ?? "N/A"),
                new SqliteParameter("@year", user.Year ?? "N/A"),
                new SqliteParameter("@role", user.Role ?? "Student"),
                new SqliteParameter("@pass", user.Password),
                new SqliteParameter("@path", user.UploadPath ?? "")
            };

            return ExecuteNonQuery(sql, ps) > 0;
        }

        public bool ValidateLogin(string userId, string password)
        {
            try
            {
                // Added COLLATE NOCASE for case-insensitive UserID check
                string sql = "SELECT COUNT(*) FROM Users WHERE UserID = @id COLLATE NOCASE AND Password = @pass";
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqliteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", userId.Trim());
                        command.Parameters.AddWithValue("@pass", password.Trim());

                        var result = command.ExecuteScalar();
                        return result != null && Convert.ToInt64(result) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login Validation Error: " + ex.Message);
                return false;
            }
        }

        public User GetUserDetails(string userId)
        {
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
                    UploadPath = dr["UploadPath"].ToString()
                };
            }
            return null;
        }

        public bool DeleteUser(string userId)
        {
            string sql = "DELETE FROM Users WHERE UserID = @id";
            return ExecuteNonQuery(sql, new[] { new SqliteParameter("@id", userId) }) > 0;
        }

        // --- CLEARANCE REQUESTS ---
        public DataTable GetDepartmentRequests(string departmentName)
        {
            string sql = @"SELECT r.StudentID, u.FullName, u.Program, u.Year, 
                                  r.Semester, r.AcademicYear, r.Status, r.Remarks
                           FROM ClearanceRequests r
                           INNER JOIN Users u ON r.StudentID = u.UserID
                           WHERE r.Department = @dept";

            return GetDataTable(sql, new[] { new SqliteParameter("@dept", departmentName) });
        }

        public bool UpdateRequestStatus(string studentId, string department, string newStatus, string remarks)
        {
            string sql = @"UPDATE ClearanceRequests 
                           SET Status = @status, Remarks = @remarks, DateProcessed = @date
                           WHERE StudentID = @id AND Department = @dept";

            SqliteParameter[] ps = {
                new SqliteParameter("@status", newStatus),
                new SqliteParameter("@remarks", remarks),
                new SqliteParameter("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new SqliteParameter("@id", studentId),
                new SqliteParameter("@dept", department)
            };

            return ExecuteNonQuery(sql, ps) > 0;
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