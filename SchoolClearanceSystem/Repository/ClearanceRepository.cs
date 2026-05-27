using Dapper;
using SchoolClearanceSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SchoolClearanceSystem.Repository
{
    public class ClearanceRepository : BaseRepository
    {
        public bool HasExistingRequest(string studentId, string semester, string academicYear)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"SELECT COUNT(1) FROM ClearanceRequests 
                               WHERE UserID = @id AND Semester = @sem AND AcademicYear = @ay";
                return db.ExecuteScalar<int>(sql, new { id = studentId, sem = semester, ay = academicYear }) > 0;
            }
        }

        public bool SubmitClearanceRequest(string studentId, string dept, string semester, string acadYear, string filePath)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"INSERT INTO ClearanceRequests (UserID, Department, Status, DateSubmitted, Semester, AcademicYear, FilePath) 
                               VALUES (@id, @dept, 'Pending', @date, @sem, @ay, @path)";

                return db.Execute(sql, new
                {
                    id = studentId,
                    dept,
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    sem = semester,
                    ay = acadYear,
                    path = filePath
                }) > 0;
            }
        }

        public IEnumerable<dynamic> GetRequestsForOffice(string officeDept)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"SELECT 
                    c.UserID AS UserID,
                    (u.LastName || ', ' || u.FirstName || 
                        CASE WHEN u.MiddleName IS NOT NULL AND u.MiddleName != '' 
                             THEN ' ' || u.MiddleName ELSE '' END) AS FullName,
                    u.Program AS Program, u.Year AS Year,
                    c.Semester AS Semester, c.Status AS Status,
                    c.Department AS Office, '' AS Action,
                    c.Remarks AS Remarks, c.FilePath AS FilePath,
                    c.DateProcessed AS DateProcessed
                FROM ClearanceRequests c
                INNER JOIN Users u ON c.UserID = u.UserID
                WHERE c.Department = @dept
                ORDER BY CASE WHEN c.Status = 'Pending' THEN 0 ELSE 1 END ASC, c.DateProcessed ASC";

                return db.Query(sql, new { dept = officeDept }).ToList();
            }
        }

        public IEnumerable<dynamic> GetRequestsForOffice(string officeDept, string semester = "", string academicYear = "")
        {
            using (var db = dbManager.GetConnection())
            {
                string periodFilter = (!string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(academicYear))
                    ? "AND c.Semester = @semester AND c.AcademicYear = @academicYear" : "";

                string sql = $@"SELECT 
            c.UserID AS UserID,
            (u.LastName || ', ' || u.FirstName || 
                CASE WHEN u.MiddleName IS NOT NULL AND u.MiddleName != '' 
                     THEN ' ' || u.MiddleName ELSE '' END) AS FullName,
            u.Program AS Program, u.Year AS Year,
            c.Semester AS Semester, c.AcademicYear AS AcademicYear,
            c.Status AS Status,
            c.Department AS Office, '' AS Action,
            c.Remarks AS Remarks, c.FilePath AS FilePath
        FROM ClearanceRequests c
        INNER JOIN Users u ON c.UserID = u.UserID
        WHERE c.Department = @dept {periodFilter}";

                return db.Query(sql, new { dept = officeDept, semester, academicYear }).ToList();
            }
        }

        public bool UpdateRequestStatus(string studentId, string department, string newStatus, string remarks = "")
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"UPDATE ClearanceRequests 
                               SET Status = @status, Remarks = @remarks, DateProcessed = @date 
                               WHERE UserID = @id AND Department = @dept";

                var parameters = new
                {
                    status = newStatus,
                    remarks,
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    id = studentId,
                    dept = department
                };

                try
                {
                    return db.Execute(sql, parameters) > 0;
                }
                catch (Microsoft.Data.Sqlite.SqliteException ex) when (ex.Message.Contains("no such column: DateProcessed"))
                {
                    db.Execute("ALTER TABLE ClearanceRequests ADD COLUMN DateProcessed TEXT;");
                    return db.Execute(sql, parameters) > 0;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public bool DeleteRequestsByStudent(string studentId)
        {
            using (var db = dbManager.GetConnection())
                return db.Execute("DELETE FROM ClearanceRequests WHERE UserID = @id", new { id = studentId }) >= 0;
        }

        public int GetStatusCountForOffice(string officeDept, string status)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"SELECT COUNT(DISTINCT UserID) FROM ClearanceRequests 
                               WHERE Department = @dept AND Status = @status";
                return db.ExecuteScalar<int>(sql, new { dept = officeDept, status });
            }
        }

        public int GetStatusCountForOffice(string officeDept, string status, string semester = "", string academicYear = "")
        {
            using (var db = dbManager.GetConnection())
            {
                string periodFilter = (!string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(academicYear))
                    ? "AND Semester = @semester AND AcademicYear = @academicYear" : "";

                string sql = $@"SELECT COUNT(*) FROM ClearanceRequests
                                WHERE Department = @dept AND Status = @status {periodFilter}";
                return db.ExecuteScalar<int>(sql, new { dept = officeDept, status, semester, academicYear });
            }
        }

        public int GetTotalStudentsForOffice(string officeDept)
        {
            using (var db = dbManager.GetConnection())
            {
                return db.ExecuteScalar<int>(
                    "SELECT COUNT(DISTINCT UserID) FROM ClearanceRequests WHERE Department = @dept",
                    new { dept = officeDept });
            }
        }

        public int GetTotalStudentsForOffice(string officeDept, string semester = "", string academicYear = "")
        {
            using (var db = dbManager.GetConnection())
            {
                string periodFilter = (!string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(academicYear))
                    ? "AND Semester = @semester AND AcademicYear = @academicYear" : "";

                string sql = $"SELECT COUNT(DISTINCT UserID) FROM ClearanceRequests WHERE Department = @dept {periodFilter}";
                return db.ExecuteScalar<int>(sql, new { dept = officeDept, semester, academicYear });
            }
        }

        public IEnumerable<dynamic> GetRecentRequestsForOffice(string officeDept, int limit = 10,
            string semester = "", string academicYear = "")
        {
            using (var db = dbManager.GetConnection())
            {
                string periodFilter = (!string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(academicYear))
                    ? "AND c.Semester = @semester AND c.AcademicYear = @academicYear" : "";

                string sql = $@"SELECT 
                    c.UserID AS UserID,
                    (u.LastName || ', ' || u.FirstName || 
                        CASE WHEN u.MiddleName IS NOT NULL AND u.MiddleName != '' 
                             THEN ' ' || u.MiddleName ELSE '' END) AS FullName,
                    u.Program AS Program, u.Year AS Year,
                    c.Semester AS Semester, c.Status AS Status, c.Remarks AS Remarks
                FROM ClearanceRequests c
                INNER JOIN Users u ON c.UserID = u.UserID
                WHERE c.Department = @dept {periodFilter}
                ORDER BY c.rowid DESC LIMIT @limit";

                return db.Query(sql, new { dept = officeDept, limit, semester, academicYear }).ToList();
            }
        }

        public IEnumerable<dynamic> GetArchivedRequests(string semester, string academicYear, string officeName)
        {
            using (var db = dbManager.GetConnection())
            {
                string deptFilter = string.IsNullOrEmpty(officeName) || officeName == "All"
                    ? "" : "AND c.Department = @dept";

                string sql = $@"SELECT
                    c.UserID AS UserID,
                    (u.LastName || ', ' || u.FirstName ||
                        CASE WHEN u.MiddleName IS NOT NULL AND u.MiddleName != ''
                             THEN ' ' || u.MiddleName ELSE '' END) AS FullName,
                    u.Program AS Program, u.Year AS Year,
                    c.Semester AS Semester, c.AcademicYear AS AcademicYear,
                    c.Department AS Office, c.Status AS Status,
                    c.Remarks AS Remarks, c.DateProcessed AS DateProcessed
                FROM ClearanceRequests c
                INNER JOIN Users u ON c.UserID = u.UserID
                WHERE c.Semester = @semester AND c.AcademicYear = @academicYear {deptFilter}
                ORDER BY u.LastName ASC";

                return db.Query(sql, new { semester, academicYear, dept = officeName }).ToList();
            }
        }

        public DataTable GetClearanceReportData(string semester, string academicYear, string status, string officeName)
        {
            string sql = @"SELECT 
                    c.UserID AS student_id,
                    (u.LastName || ', ' || u.FirstName) AS student_name,
                    u.Program AS program, u.Year AS year, c.Status AS status
                FROM ClearanceRequests c
                INNER JOIN Users u ON c.UserID = u.UserID
                WHERE c.Semester = @sem AND c.AcademicYear = @ay 
                  AND c.Status = @status AND c.Department = @office";

            return dbManager.GetDataTable(sql, new { sem = semester, ay = academicYear, status, office = officeName });
        }
    }
}