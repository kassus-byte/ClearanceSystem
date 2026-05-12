using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolClearanceSystem.Models;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;

namespace SchoolClearanceSystem.Repository
{
    public class UserRepository: BaseRepository
    {
        public List<User> GetUsersByRole(string role, bool isStudent = true)
        {
            using (var db = dbManager.GetConnection())
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
            using (var db = dbManager.GetConnection())
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

                using (var db = dbManager.GetConnection())
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
            using (var db = dbManager.GetConnection())
            {
                string sql = @"UPDATE Users 
                               SET FullName = @FullName, Program = @Program, Year = @Year, Role = @Role 
                               WHERE UserID = @UserID";

                return db.Execute(sql, user) > 0;
            }
        }

        public bool ValidateLogin(string userId, string password)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM Users WHERE UserID = @id COLLATE NOCASE AND Password = @pass";
                return db.ExecuteScalar<int>(sql, new { id = userId.Trim(), pass = password.Trim() }) > 0;
            }
        }

        public bool DeleteUser(string userId)
        {
            using (var db = dbManager.GetConnection())
            {
                return db.Execute("DELETE FROM Users WHERE UserID = @id", new { id = userId }) > 0;
            }
        }
    }
}
