using Dapper;
using SchoolClearanceSystem.Helpers;
using SchoolClearanceSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolClearanceSystem.Repository
{
    public class UserRepository : BaseRepository
    {
        public User ValidateLogin(string userId, string password)
        {
            using (var db = dbManager.GetConnection())
            {
                var user = db.QueryFirstOrDefault<User>("SELECT * FROM Users WHERE UserID = @id AND IsActive = 1", new { id = userId });
                if (user == null) return null;

                bool valid = user.Password.StartsWith("$2")
                    ? PasswordHelper.Verify(password, user.Password)
                    : user.Password == password;

                return valid ? user : null;
            }
        }

        public User GetUserDetails(string userId)
        {
            using (var db = dbManager.GetConnection())
                return db.QueryFirstOrDefault<User>("SELECT * FROM Users WHERE UserID = @id", new { id = userId });
        }

        public bool AddUser(User user)
        {
            using (var db = dbManager.GetConnection())
            {
                int exists = db.ExecuteScalar<int>("SELECT COUNT(1) FROM Users WHERE UserID = @UserID", new { user.UserID });
                if (exists > 0) return false;

                if (string.IsNullOrEmpty(user.DateCreated))
                    user.DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                // FIX: typed query — no dynamic property access risk
                string activeYear = db.QueryFirstOrDefault<string>(
                    "SELECT AcademicYear FROM ClearancePeriods WHERE IsActive = 1 LIMIT 1");

                string sql = @"INSERT INTO Users 
                               (UserID, Password, LastName, FirstName, MiddleName, Program, Year, Role, DateCreated, IsActive, CurrentPeriodPromotionYear)
                               VALUES 
                               (@UserID, @Password, @LastName, @FirstName, @MiddleName, @Program, @Year, @Role, @DateCreated, 1, @PromoYear)";

                return db.Execute(sql, new
                {
                    user.UserID,
                    user.Password,
                    user.LastName,
                    user.FirstName,
                    user.MiddleName,
                    user.Program,
                    user.Year,
                    user.Role,
                    user.DateCreated,
                    PromoYear = activeYear
                }) > 0;
            }
        }

        public bool EditUser(User user)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"UPDATE Users 
                               SET Password = @Password, LastName = @LastName, FirstName = @FirstName,
                                   MiddleName = @MiddleName, Program = @Program, Year = @Year, Role = @Role
                               WHERE UserID = @UserID";

                return db.Execute(sql, new
                {
                    user.Password,
                    user.LastName,
                    user.FirstName,
                    user.MiddleName,
                    user.Program,
                    user.Year,
                    user.Role,
                    user.UserID
                }) > 0;
            }
        }

        public IEnumerable<User> GetUsersByRole(string role, bool statusFlag = true)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = role == "Student"
                    ? "SELECT * FROM Users WHERE Role = 'Student' AND IsActive = 1 ORDER BY DateCreated DESC"
                    : "SELECT * FROM Users WHERE Role != 'Student' AND IsActive = 1 ORDER BY DateCreated DESC";
                return db.Query<User>(sql).ToList();
            }
        }

        public bool DeleteUser(string userId)
        {
            using (var db = dbManager.GetConnection())
            {
                db.Execute("DELETE FROM ClearanceRequests WHERE UserID = @id", new { id = userId });
                return db.Execute("DELETE FROM Users WHERE UserID = @id", new { id = userId }) > 0;
            }
        }

        public IEnumerable<dynamic> GetStudentStatus(string userId, string semester, string academicYear)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"SELECT Department AS Office, Department AS OfficeName,
                                      Department AS Department, Status, Remarks
                               FROM ClearanceRequests
                               WHERE UserID = @id AND Semester = @semester AND AcademicYear = @academicYear";

                return db.Query(sql, new { id = userId, semester, academicYear }).ToList();
            }
        }

        public int GetUserCount(string role)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = role == "Student" ? "SELECT COUNT(*) FROM Users WHERE Role = 'Student' AND IsActive = 1"
                           : role == "Staff" ? "SELECT COUNT(*) FROM Users WHERE Role != 'Student' AND IsActive = 1"
                                               : "SELECT COUNT(*) FROM Users WHERE IsActive = 1";
                return db.ExecuteScalar<int>(sql);
            }
        }

        public int GetNewRegistrationsThisWeek()
        {
            using (var db = dbManager.GetConnection())
                return db.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM Users WHERE DateCreated >= date('now', '-7 days') AND IsActive = 1");
        }

        public IEnumerable<User> GetUsersRegisteredThisWeek()
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"SELECT * FROM Users
                               WHERE IsActive = 1
                               ORDER BY DateCreated DESC
                               LIMIT 5";
                return db.Query<User>(sql).ToList();
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            using (var db = dbManager.GetConnection())
                return db.Query<User>(
                    "SELECT * FROM Users WHERE IsActive = 1 ORDER BY Role, LastName, FirstName").ToList();
        }

        public IEnumerable<ClearanceRecord> GetStudentClearancePeriods(string userId)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"SELECT DISTINCT Semester, AcademicYear, 'Completed' AS Status
                               FROM ClearanceRequests
                               WHERE UserID = @id
                               GROUP BY Semester, AcademicYear
                               HAVING COUNT(CASE WHEN Status = 'Approved' THEN 1 END) = COUNT(*)
                               ORDER BY AcademicYear DESC, Semester DESC";

                return db.Query<ClearanceRecord>(sql, new { id = userId }).ToList();
            }
        }

        public int GetClearedCount(string userId, string semester, string academicYear)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"SELECT COUNT(*) FROM ClearanceRequests
                               WHERE UserID = @id AND Status = 'Approved'
                                 AND Semester = @semester AND AcademicYear = @academicYear";
                return db.ExecuteScalar<int>(sql, new { id = userId, semester, academicYear });
            }
        }
    }
}