using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace SchoolClearanceSystem
{
    public class DatabaseManager
    {
        // Using a relative path is better for portability. 
        // Ensure "Copy to Output Directory" is set to "Copy if Newer" for the .db file in VS.
        private string connectionString = "Data Source=ClearanceSystem.db";

        public void SaveUser(User user)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Users (UserID, FullName, Program, Year, Role, Password) " +
                             "VALUES (@id, @name, @prog, @year, @role, @pass)";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", user.UserID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@name", user.FullName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@prog", user.Program ?? "N/A");
                    command.Parameters.AddWithValue("@year", user.Year ?? "N/A");
                    command.Parameters.AddWithValue("@role", user.Role ?? "Student");
                    command.Parameters.AddWithValue("@pass", user.Password ?? (object)DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool ValidateLogin(string userId, string password)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT COUNT(*) FROM Users WHERE UserID = @id AND Password = @pass";
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", userId);
                    command.Parameters.AddWithValue("@pass", password);
                    // SQLite returns long for COUNT(*)
                    var result = command.ExecuteScalar();
                    return result != null && Convert.ToInt64(result) > 0;
                }
            }
        }

        public User GetUserDetails(string userId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT FullName, UserID, Program, Year, Role FROM Users WHERE UserID = @id";
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", userId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                FullName = reader["FullName"].ToString(),
                                UserID = reader["UserID"].ToString(),
                                Program = reader["Program"].ToString(),
                                Year = reader["Year"].ToString(),
                                Role = reader["Role"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool SubmitClearanceRequest(string studentId, string dept, string semester, string acadYear)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = @"INSERT INTO ClearanceRequests (StudentID, Department, Status, DateSubmitted, Semester, AcademicYear) 
                               VALUES (@id, @dept, 'Pending', @date, @sem, @ay)";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", studentId);
                    command.Parameters.AddWithValue("@dept", dept);
                    command.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@sem", semester);
                    command.Parameters.AddWithValue("@ay", acadYear);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public DataTable GetDepartmentRequests(string departmentName)
        {
            DataTable dt = new DataTable();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                // CRITICAL: We select u.FullName AS 'Name' to match your Grid Column Caption if necessary, 
                // but usually, it's better to keep FieldNames consistent.
                string sql = @"SELECT  r.StudentID, u.FullName, u.Program, u.Year, 
                                      r.Semester, r.AcademicYear, r.Status 
                               FROM ClearanceRequests r
                               INNER JOIN Users u ON r.StudentID = u.UserID
                               WHERE r.Department = @dept";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@dept", departmentName);
                    using (var reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }
    }
}