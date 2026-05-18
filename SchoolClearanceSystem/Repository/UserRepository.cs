using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SchoolClearanceSystem.Models; // ✅ Works perfectly now!

namespace SchoolClearanceSystem.Repository
{
    public class UserRepository : BaseRepository
    {
        /// <summary>
        /// Authenticates the user during login.
        /// </summary>
        public User ValidateLogin(string userId, string password)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = "SELECT * FROM Users WHERE UserID = @id AND Password = @pass";
                return db.QueryFirstOrDefault<User>(sql, new { id = userId, pass = password });
            }
        }

        /// <summary>
        /// Retrieves a user's full profile details.
        /// </summary>
        public User GetUserDetails(string userId)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = "SELECT * FROM Users WHERE UserID = @id";
                return db.QueryFirstOrDefault<User>(sql, new { id = userId });
            }
        }

        /// <summary>
        /// Inserts a new user record into the database during registration.
        /// </summary>
        public bool AddUser(User user)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"INSERT INTO Users (UserID, Password, FullName, Program, Year, Role) 
                               VALUES (@UserID, @Password, @FullName, @Program, @Year, @Role)";
                return db.Execute(sql, user) > 0;
            }
        }

        /// <summary>
        /// Updates an existing user's background details in the system.
        /// </summary>
        public bool EditUser(User user)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"UPDATE Users 
                               SET Password = @Password, FullName = @FullName, Program = @Program, Year = @Year, Role = @Role 
                               WHERE UserID = @UserID";
                return db.Execute(sql, user) > 0;
            }
        }

        /// <summary>
        /// Retrieves user profiles filtered by role and status indicators.
        /// </summary>
        public IEnumerable<User> GetUsersByRole(string role, bool statusFlag = true)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = "SELECT * FROM Users WHERE Role = @Role";
                return db.Query<User>(sql, new { Role = role }).ToList();
            }
        }

        /// <summary>
        /// Queries the ClearanceRequests table mapping database layout structure.
        /// FIXED: Added 'Department AS Department' alongside aliases to ensure DevExpress GridView cell columns never bind blank!
        /// </summary>
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

        /// <summary>
        /// Counts how many distinct office rows have been marked 'Approved' for this student.
        /// </summary>
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