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
        // ── Called by Login.cs ────────────────────────────────────────
        public User ValidateLogin(string userId, string password)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = "SELECT * FROM Users WHERE UserID = @id AND Password = @pass";
                return db.QueryFirstOrDefault<User>(sql, new { id = userId, pass = password });
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
        public bool AddUser(User user)
        {
            using (var db = dbManager.GetConnection())
            {
                // Guard: block duplicate UserID before inserting
                string checkSql = "SELECT COUNT(1) FROM Users WHERE UserID = @UserID";
                int exists = db.ExecuteScalar<int>(checkSql, new { UserID = user.UserID });
                if (exists > 0) return false;

                string sql = @"INSERT INTO Users 
                               (UserID, Password, LastName, FirstName, MiddleName, 
                                Program, Year, Role, DateCreated)
                               VALUES 
                               (@UserID, @Password, @LastName, @FirstName, @MiddleName, 
                                @Program, @Year, @Role, @DateCreated)";

                if (string.IsNullOrEmpty(user.DateCreated))
                    user.DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                return db.Execute(sql, user) > 0;
            }
        }

        // ── Called by UserInfoForm (Edit mode) ────────────────────────
        public bool EditUser(User user)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"UPDATE Users 
                               SET Password    = @Password,
                                   LastName    = @LastName,
                                   FirstName   = @FirstName,
                                   MiddleName  = @MiddleName,
                                   Program     = @Program,
                                   Year        = @Year,
                                   Role        = @Role,
                                   UploadPath  = @UploadPath
                               WHERE UserID = @UserID";
                return db.Execute(sql, user) > 0;
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
                // Delete dependent clearance records first (FK constraint)
                db.Execute("DELETE FROM ClearanceRequests WHERE StudentID = @id", new { id = userId });
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
                               WHERE StudentID    = @id
                                 AND Semester     = @semester
                                 AND AcademicYear = @academicYear";

                return db.Query(sql, new { id = userId, semester, academicYear }).ToList();
            }
        }

        // ── Called by AdminDashboard.cs → LoadDashboardStats() ────────
        // role: "Student" = students only, "Staff" = non-students, "All" = everyone
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
        // Counts ALL new accounts this week (students + staff)
        // FIX: removed Role = 'Student' filter so office accounts are counted too
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
        // Populates the Registered Accounts grid — all users added this week
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

        // ── Called by AdminDashboard.cs → LoadDashboardStats() ────────
        // FIX: ORDER BY LastName, FirstName instead of FullName
        // FullName is no longer a DB column — it is computed in the User model
        public IEnumerable<User> GetAllUsers()
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = "SELECT * FROM Users ORDER BY Role, LastName, FirstName";
                return db.Query<User>(sql).ToList();
            }
        }

        // ── Called by StudentPortal.cs → UpdateDashboard() ───────────
        public int GetClearedCount(string userId, string semester, string academicYear)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"SELECT COUNT(*)
                               FROM ClearanceRequests
                               WHERE StudentID    = @id
                                 AND Status       = 'Approved'
                                 AND Semester     = @semester
                                 AND AcademicYear = @academicYear";

                return db.ExecuteScalar<int>(sql, new { id = userId, semester, academicYear });
            }
        }
    }
}