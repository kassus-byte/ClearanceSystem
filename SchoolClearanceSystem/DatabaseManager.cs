using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SchoolClearanceSystem
{
    public class DatabaseManager
    {
        private string connectionString = "Data Source=ClearanceSystem.db";

        // --- 1. CLEARANCE SEASON CONTROL (With Safety Catch) ---
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
                // Table doesn't exist? Assume inactive and don't crash.
                return false;
            }
            catch { return false; }
        }

        public void ToggleClearanceSeason(bool isActive)
        {
            int value = isActive ? 1 : 0;
            string sql = "UPDATE SystemSettings SET ClearanceIsActive = @val";
            var param = new SqliteParameter("@val", value);
            ExecuteNonQuery(sql, new[] { param });
        }

        // --- 2. REGISTRATION LOGIC (Fixed to return bool) ---
        public bool SaveUser(User user)
        {
            try
            {
                string sql = "INSERT INTO Users (UserID, FullName, Program, Year, Role, Password) " +
                             "VALUES (@id, @name, @prog, @year, @role, @pass)";

                SqliteParameter[] ps = {
                    new SqliteParameter("@id", user.UserID),
                    new SqliteParameter("@name", user.FullName),
                    new SqliteParameter("@prog", user.Program ?? "N/A"),
                    new SqliteParameter("@year", user.Year ?? "N/A"),
                    new SqliteParameter("@role", user.Role ?? "Student"),
                    new SqliteParameter("@pass", user.Password)
                };

                return ExecuteNonQuery(sql, ps) > 0;
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19) // UNIQUE constraint failed
            {
                XtraMessageBox.Show("This User ID is already registered. Please use a different one.",
                                    "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Database Error: " + ex.Message, "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // --- 3. LOGIN & USER DETAILS ---
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

        public User GetUserDetails(string userId)
        {
            string sql = "SELECT FullName, UserID, Program, Year, Role FROM Users WHERE UserID = @id";
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
                    Role = dr["Role"].ToString()
                };
            }
            return null;
        }

        // --- 4. CLEARANCE REQUESTS ---
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

        // --- 5. CORE DATABASE HELPERS ---
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
                        using (var reader = command.ExecuteReader()) { dt.Load(reader); }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Data Load Error: " + ex.Message); }
            return dt;
        }

        private int ExecuteNonQuery(string sql, SqliteParameter[] parameters = null)
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

        public bool DeleteUser(string userId)
        {
            string sql = "DELETE FROM Users WHERE UserID = @id";
            var param = new SqliteParameter("@id", userId);
            return ExecuteNonQuery(sql, new[] { param }) > 0;
        }
    }
}