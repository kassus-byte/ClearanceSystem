using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SchoolClearanceSystem.Models;

namespace SchoolClearanceSystem.Repository
{
    public class UserRepository : BaseRepository
    {
        public User ValidateLogin(string userId, string password)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = "SELECT * FROM Users WHERE UserID = @id AND Password = @pass";
                return db.QueryFirstOrDefault<User>(sql, new { id = userId, pass = password });
            }
        }

        public User GetUserDetails(string userId)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = "SELECT * FROM Users WHERE UserID = @id";
                return db.QueryFirstOrDefault<User>(sql, new { id = userId });
            }
        }

        public bool AddUser(User user)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"INSERT INTO Users (UserID, Password, FullName, Program, Year, Role, UploadPath, DateCreated) 
                               VALUES (@UserID, @Password, @FullName, @Program, @Year, @Role, @UploadPath, @DateCreated)";

                if (string.IsNullOrEmpty(user.DateCreated))
                {
                    user.DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }

                return db.Execute(sql, user) > 0;
            }
        }

        public bool EditUser(User user)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"UPDATE Users 
                               SET Password = @Password, FullName = @FullName, Program = @Program, Year = @Year, Role = @Role, UploadPath = @UploadPath 
                               WHERE UserID = @UserID";
                return db.Execute(sql, user) > 0;
            }
        }

        /// <summary>
        /// FIXED: Separates Student records from Administrative Staff roles cleanly.
        /// </summary>
        public IEnumerable<User> GetUsersByRole(string role, bool statusFlag = true)
        {
            using (var db = dbManager.GetConnection())
            {
                if (role == "Student")
                {
                    string sql = "SELECT * FROM Users WHERE Role = 'Student'";
                    return db.Query<User>(sql).ToList();
                }
                else
                {
                    // FIXED: Returns all users who are NOT students (Admin, Treasurer, Technical Office, etc.)
                    string sql = "SELECT * FROM Users WHERE Role != 'Student'";
                    return db.Query<User>(sql).ToList();
                }
            }
        }

        public IEnumerable<dynamic> GetStudentStatus(string userId)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"SELECT 
                                Department AS Office,
                                Department AS OfficeName, 
                                Department AS Department, 
                                Status, 
                                Remarks 
                               FROM ClearanceRequests 
                               WHERE StudentID = @id";

                return db.Query(sql, new { id = userId }).ToList();
            }
        }

        public int GetClearedCount(string userId)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"SELECT COUNT(*) 
                               FROM ClearanceRequests 
                               WHERE StudentID = @id AND Status = 'Approved'";

                return db.ExecuteScalar<int>(sql, new { id = userId });
            }
        }
    }
}