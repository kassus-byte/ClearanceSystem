using Microsoft.Data.Sqlite;
using System;

namespace ClearanceSystem
{
    public class DatabaseManager
    {
        // TIP: Use a full path if the DB isn't found, e.g., "Data Source=C:\\data\\ClearanceSystem.db"
        private string connectionString = "Data Source=ClearanceSystem.db";

        public void SaveUser(User user)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Using @parameter names to prevent SQL Injection
                string sql = "INSERT INTO \"Users\" (UserID, FullName, Program, Year, Role, Password) " +
                             "VALUES (@id, @name, @prog, @year, @role, @pass)";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", user.UserID);
                    command.Parameters.AddWithValue("@name", user.FullName);
                    command.Parameters.AddWithValue("@prog", user.Program);
                    command.Parameters.AddWithValue("@year", user.Year);
                    command.Parameters.AddWithValue("@role", user.Role);
                    command.Parameters.AddWithValue("@pass", user.Password);

                    command.ExecuteNonQuery();
                }
            }
        }

        public bool ValidateLogin(string userId, string password)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                // We count how many users match both the ID and Password
                string sql = "SELECT COUNT(*) FROM Users WHERE UserID = @id AND Password = @pass";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", userId);
                    command.Parameters.AddWithValue("@pass", password);

                    long count = (long)command.ExecuteScalar();
                    return count > 0; // If count is 1, the user is registered
                }
            }
        }
    }
}