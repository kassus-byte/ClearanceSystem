using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Helpers;

namespace SchoolClearanceSystem.Repository
{
    public class UserRepository : BaseRepository
    {
        // ── Called by Login.cs ────────────────────────────────────────
        public User ValidateLogin(string userId, string password)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = "SELECT * FROM Users WHERE UserID = @id";
                var user = db.QueryFirstOrDefault<User>(sql, new { id = userId });

                if (user == null) return null;

                bool valid = user.Password.StartsWith("$2")
                    ? PasswordHelper.Verify(password, user.Password)
                    : user.Password == password;

                return valid ? user : null;
            }
        }

        // ── Called by Login.cs ────────────────────────────────────────
        public User GetUserDetails(string userId)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = "SELECT * FROM Users WHERE UserID = @id";
                return db.QueryFirstOrDefault<User>(sql, new { id = userId });
            }
        }

        // ── Called by Registration.cs and UserInfoForm (Register mode) ─
        // FIX: Use LastName, FirstName, MiddleName instead of non-existent FullName column
        public bool AddUser(User user)
        {
            using (var db = dbManager.GetConnection())
            {
                string checkSql = "SELECT COUNT(1) FROM Users WHERE UserID = @UserID";
                int exists = db.ExecuteScalar<int>(checkSql, new { user.UserID });
                if (exists > 0) return false;

                // Hash is applied by the caller (UserInfoForm/Registration) — do NOT hash again here

                if (string.IsNullOrEmpty(user.DateCreated))
                    user.DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string sql = @"INSERT INTO Users 
                               (UserID, Password, LastName, FirstName, MiddleName, Program, Year, Role, DateCreated)
                               VALUES 
                               (@UserID, @Password, @LastName, @FirstName, @MiddleName, @Program, @Year, @Role, @DateCreated)";

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
                    user.DateCreated
                }) > 0;
            }
        }

        // ── Called by UserInfoForm (Edit mode) ────────────────────────
        // FIX: Use LastName, FirstName, MiddleName instead of non-existent FullName column
        public bool EditUser(User user)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"UPDATE Users 
                               SET Password = @Password,
                                   LastName = @LastName,
                                   FirstName = @FirstName,
                                   MiddleName = @MiddleName,
                                   Program  = @Program,
                                   Year     = @Year,
                                   Role     = @Role
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

        // ── Called by AdminDashboard.cs → RefreshData() ───────────────
        public IEnumerable<User> GetUsersByRole(string role, bool statusFlag = true)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = role == "Student"
                    ? "SELECT * FROM Users WHERE Role = 'Student'"
                    : "SELECT * FROM Users WHERE Role != 'Student'";
                return db.Query<User>(sql).ToList();
            }
        }

        // ── Called by AdminDashboard.cs ───────────────────────────────
        public bool DeleteUser(string userId)
        {
            using (var db = dbManager.GetConnection())
            {
                db.Execute("DELETE FROM ClearanceRequests WHERE UserID = @id", new { id = userId });
                return db.Execute("DELETE FROM Users WHERE UserID = @id", new { id = userId }) > 0;
            }
        }

        // ── Called by StudentPortal.cs ────────────────────────────────
        public IEnumerable<dynamic> GetStudentStatus(string userId, string semester, string academicYear)
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
                               WHERE UserID    = @id
                                 AND Semester     = @semester
                                 AND AcademicYear = @academicYear";

                return db.Query(sql, new { id = userId, semester, academicYear }).ToList();
            }
        }

        // ── Called by AdminDashboard.cs → LoadDashboardStats() ────────
        public int GetUserCount(string role)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = role == "Student" ? "SELECT COUNT(*) FROM Users WHERE Role = 'Student'"
                           : role == "Staff" ? "SELECT COUNT(*) FROM Users WHERE Role != 'Student'"
                                               : "SELECT COUNT(*) FROM Users";
                return db.ExecuteScalar<int>(sql);
            }
        }

        // ── Called by AdminDashboard.cs → LoadDashboardStats() ────────
        public int GetNewRegistrationsThisWeek()
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"SELECT COUNT(*) FROM Users
                               WHERE DateCreated >= date('now', '-7 days')";
                return db.ExecuteScalar<int>(sql);
            }
        }

        // ── Called by AdminDashboard.cs → LoadDashboardStats() ────────
        public IEnumerable<User> GetUsersRegisteredThisWeek()
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"SELECT * FROM Users
                               WHERE DateCreated >= date('now', '-7 days')
                               ORDER BY DateCreated DESC";
                return db.Query<User>(sql).ToList();
            }
        }

        // ── Called by AdminDashboard.cs ───────────────────────────────
        // FIX: ORDER BY needs to handle the computed FullName property
        public IEnumerable<User> GetAllUsers()
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = "SELECT * FROM Users ORDER BY Role, LastName, FirstName";
                return db.Query<User>(sql).ToList();
            }
        }

        // Gets distinct clearance periods a student has fully completed
        public IEnumerable<ClearanceRecord> GetStudentClearancePeriods(string userId)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"
                    SELECT DISTINCT Semester, AcademicYear, 'Completed' AS Status
                    FROM ClearanceRequests
                    WHERE UserID = @id
                    GROUP BY Semester, AcademicYear
                    HAVING COUNT(CASE WHEN Status = 'Approved' THEN 1 END) = COUNT(*)
                    ORDER BY AcademicYear DESC, Semester DESC";

                return db.Query<ClearanceRecord>(sql, new { id = userId }).ToList();
            }
        }

        // ── Called by StudentPortal.cs → UpdateDashboard() ───────────
        public int GetClearedCount(string userId, string semester, string academicYear)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"SELECT COUNT(*)
                               FROM ClearanceRequests
                               WHERE UserID    = @id
                                 AND Status       = 'Approved'
                                 AND Semester     = @semester
                                 AND AcademicYear = @academicYear";

                return db.ExecuteScalar<int>(sql, new { id = userId, semester, academicYear });
            }
        }
    }
}