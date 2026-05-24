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

        public bool DeleteUser(string userId)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = "DELETE FROM Users WHERE UserID = @id";
                return db.Execute(sql, new { id = userId }) > 0;
            }
        }

        // RESTORED: Standard parameter fallback match signatures
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

        // FIXED OVERLOAD: Pulls student status data isolated strictly to the current active clearance period
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

        // RESTORED: Standard parameter fallback match signatures
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

        // FIXED OVERLOAD: Counts approved offices ONLY within the context of the active term parameters
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

        /// <summary>
        /// HISTORICAL TIMELINE RETRIEVAL ENGINE
        /// Queries the distinct historical semesters for a student and transforms them into 
        /// bound view models that match your DevExpress ItemTemplate configurations.
        /// </summary>
        public IEnumerable<ClearanceHistoryViewModel> GetStudentClearanceHistory(string userId, string currentSem, string currentYear)
        {
            using (var db = dbManager.GetConnection())
            {
                // FIX: Changed COUNT(CASE) to SUM(CASE) to handle conditional arithmetic reliably across standard SQL databases
                string sql = @"SELECT 
                                AcademicYear, 
                                Semester,
                                SUM(CASE WHEN Status = 'Approved' THEN 1 ELSE 0 END) as ApprovedCount
                               FROM ClearanceRequests 
                               WHERE StudentID = @id
                               GROUP BY AcademicYear, Semester
                               ORDER BY AcademicYear DESC, Semester DESC";

                var rawList = db.Query(sql, new { id = userId }).ToList();
                var processedList = new List<ClearanceHistoryViewModel>();

                foreach (var record in rawList)
                {
                    string year = record.AcademicYear?.ToString();
                    string sem = record.Semester?.ToString();

                    // Safe parsing engine mechanics to check null dynamic values
                    int approvedCount = record.ApprovedCount != null ? Convert.ToInt32(record.ApprovedCount) : 0;

                    // Compute dynamic text status ruleset engine values
                    string calculatedStatus = "Incomplete";

                    if (year == currentYear && sem == currentSem)
                    {
                        calculatedStatus = "Clearance Processing Active";
                    }
                    else if (approvedCount >= 3)
                    {
                        calculatedStatus = "Clearance Done";
                    }

                    processedList.Add(new ClearanceHistoryViewModel
                    {
                        AcademicYear = year,
                        Semester = sem,
                        PeriodName = $"{year} {sem}",
                        StatusText = calculatedStatus // Directly maps fields onto 'element2' text component from template designer
                    });
                }

                return processedList;
            }
        }
    }
}