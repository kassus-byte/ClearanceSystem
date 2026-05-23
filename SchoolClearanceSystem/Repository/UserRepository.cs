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

        // called by Login.cs
        public User ValidateLogin(string userId, string password)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = "SELECT * FROM Users WHERE UserID = @id AND Password = @pass";
                return db.QueryFirstOrDefault<User>(sql, new { id = userId, pass = password });
            }
        }
        // called by Login.cs
        public User GetUserDetails(string userId)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = "SELECT * FROM Users WHERE UserID = @id";
                return db.QueryFirstOrDefault<User>(sql, new { id = userId });
            }
        }

        // called by Registration.cs
        public bool AddUser(User user)
        {
            using (var db = dbManager.GetConnection())
            {
                // for duplicate UserID
                string checkSql = "SELECT COUNT(1) FROM Users WHERE UserID = @UserID";
                int exists = db.ExecuteScalar<int>(checkSql, new { UserID = user.UserID });

                if (exists > 0)
                {
                    return false;
                }

                string sql = @"INSERT INTO Users (UserID, Password, FullName, Program, Year, Role, UploadPath, DateCreated) 
                               VALUES (@UserID, @Password, @FullName, @Program, @Year, @Role, @UploadPath, @DateCreated)";

                if (string.IsNullOrEmpty(user.DateCreated))
                {
                    user.DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }

                return db.Execute(sql, user) > 0;
            }
        }
        // called by UserInfoForm (EditMode)
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
        // called by AdminDashboard.cs (RefreshData)
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
                    string sql = "SELECT * FROM Users WHERE Role != 'Student'";
                    return db.Query<User>(sql).ToList();
                }
            }
        }
        // called by AdminDashboard.cs
        public bool DeleteUser(string userId)
        {
            using (var db = dbManager.GetConnection())
            {
                // 1. Clear out all dependent clearance request records linked to this student first
                string deleteRequestsSql = "DELETE FROM ClearanceRequests WHERE StudentID = @id;";
                db.Execute(deleteRequestsSql, new { id = userId });

                // 2. Now that the dependencies are gone, safely delete the user record
                string deleteUserSql = "DELETE FROM Users WHERE UserID = @id;";
                return db.Execute(deleteUserSql, new { id = userId }) > 0;
            }
        }
        
        // called by StudentPortal.cs
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
                               WHERE StudentID = @id 
                                 AND Semester = @semester 
                                 AND AcademicYear = @academicYear";

                return db.Query(sql, new { id = userId, semester = semester, academicYear = academicYear }).ToList();
            }
        }
        // called by StudentPortal (UpdateDashboard)
        public int GetClearedCount(string userId, string semester, string academicYear)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"SELECT COUNT(*) 
                               FROM ClearanceRequests 
                               WHERE StudentID = @id 
                                 AND Status = 'Approved'
                                 AND Semester = @semester 
                                 AND AcademicYear = @academicYear";

                return db.ExecuteScalar<int>(sql, new { id = userId, semester = semester, academicYear = academicYear });
            }
        }

    }
}   