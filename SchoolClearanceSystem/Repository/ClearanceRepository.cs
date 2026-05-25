using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SchoolClearanceSystem.Models;

namespace SchoolClearanceSystem.Repository
{
    /// <summary>
    /// OOP CONCEPT: INHERITANCE (Code Reusability)
    /// 'ClearanceRepository' inherits directly from 'BaseRepository'. 
    /// This means it automatically inherits the 'dbManager' instance variable without needing to 
    /// re-instantiate it, strictly following the DRY (Don't Repeat Yourself) principle.
    /// 
    /// OOP CONCEPT: SINGLE RESPONSIBILITY PRINCIPLE (SRP)
    /// While your 'UserRepository' handles account profiles and identity tracking, this class is 
    /// solely dedicated to processing operational data workflows (Clearance Requests states).
    /// </summary>
    public class ClearanceRepository : BaseRepository
    {
        /// <summary>
        /// NEW METHOD: Verifies if a student record exists matching specific term criteria constraints.
        /// Prevents duplicate request transactions for the exact same Year and Semester.
        /// </summary>
        public bool HasExistingRequest(string studentId, string semester, string academicYear)
        {
            using (var db = dbManager.GetConnection())
            {
                // Queries the tracking table checking for overlapping record instances
                string sql = @"SELECT COUNT(1) 
                               FROM ClearanceRequests 
                               WHERE UserID = @id AND Semester = @sem AND AcademicYear = @ay";

                int recordCount = db.ExecuteScalar<int>(sql, new
                {
                    id = studentId,
                    sem = semester,
                    ay = academicYear
                });

                return recordCount > 0;
            }
        }

        /// <summary>
        /// HOW IT WORKS & CONNECTS TO DATABASE MANAGER:
        /// 1. Taps into 'dbManager.GetConnection()' to acquire a live, open database connection channel.
        /// 2. Defines a clean structural INSERT query string mapping data parameters safely including file paths.
        /// 3. OOP CONCEPT: POLYMORPHISM / DATA ENCAPSULATION (Anonymous Types)
        ///    'new { id = studentId, dept ... }' creates an anonymous object on the fly. 
        ///    Dapper reads this dynamic structure to pass parameters safely to the SQL statement,
        ///    completely eliminating SQL Injection threats.
        /// </summary>
        public bool SubmitClearanceRequest(string studentId, string dept, string semester, string acadYear, string filePath)
        {
            using (var db = dbManager.GetConnection())
            {
                // Omit Remarks and DateProcessed so they default to NULL in the database until an Admin updates them
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

        /// <summary>
        /// NEW METHOD: Fetches and populates incoming student data records specific to an administrative office profile.
        /// Performs a relational JOIN structure to map clear student profiles over to the DevExpress GridControl dashboard view.
        /// </summary>
        public IEnumerable<dynamic> GetRequestsForOffice(string officeDept)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"SELECT 
                        c.UserID AS UserID,
                        (u.LastName || ', ' || u.FirstName || 
                            CASE WHEN u.MiddleName IS NOT NULL AND u.MiddleName != '' 
                                 THEN ' ' || u.MiddleName ELSE '' END) AS FullName,
                        u.Program  AS Program,
                        u.Year     AS Year,
                        c.Semester AS Semester,
                        c.Status   AS Status,
                        c.Department AS Office,
                        ''         AS Action,
                        c.Remarks  AS Remarks,
                        c.FilePath AS FilePath
                       FROM ClearanceRequests c
                       INNER JOIN Users u ON c.UserID = u.UserID
                       WHERE c.Department = @dept AND c.Status = 'Pending'";

                return db.Query(sql, new { dept = officeDept }).ToList();
            }
        }

        /// <summary>
        /// HOW IT WORKS (Update Pipeline):
        /// 1. Triggered exclusively when an Office Staff user (Treasurer, Technical, SSG) clicks Approve/Reject.
        /// 2. Binds the dynamic variables parsed from the UI into a secure query map context.
        /// 3. Updates persistent records matching specific compound conditions (UserID + Department).
        /// </summary>
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
                    remarks = remarks,
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    id = studentId,
                    dept = department
                };

                try
                {
                    // 1. Attempt the standard update transaction query
                    return db.Execute(sql, parameters) > 0;
                }
                catch (Microsoft.Data.Sqlite.SqliteException ex) when (ex.Message.Contains("no such column: DateProcessed"))
                {
                    // 2. OOP Concept: Fault-Tolerance & Self-Healing
                    // The database column is missing. Let's create it dynamically on the fly!
                    string alterSql = "ALTER TABLE ClearanceRequests ADD COLUMN DateProcessed TEXT;";
                    db.Execute(alterSql);

                    // 3. Re-execute the original transaction query now that the schema is fixed
                    return db.Execute(sql, parameters) > 0;
                }
                catch (Exception)
                {
                    // Catch any other unexpected system errors (e.g., connection losses) safely
                    throw;
                }
            }
        }

        /// <summary>
        /// NEW DEPENDENCY MANAGEMENT ENGINE METHOD:
        /// Drops all dependent records matching a specific target user constraint key from the transactional tracking sheet.
        /// Prevents SQLite Foreign Key verification faults during profile deletion workflows.
        /// </summary>
        public bool DeleteRequestsByStudent(string studentId)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = "DELETE FROM ClearanceRequests WHERE UserID = @id";
                return db.Execute(sql, new { id = studentId }) >= 0;
            }
        }

        // ───────────────────────────────────────────────────────────────
        // METHOD: GetStatusCountForOffice
        //
        // CALLED BY: BaseOfficeForm.cs → LoadDashboardStats()
        //
        // PURPOSE:
        //   Counts how many clearance rows match a specific status
        //   for a given office department.
        //   Used to populate CLEARED / PENDING / ON HOLD stat cards.
        //
        // PARAMETERS:
        //   officeDept → "SSG", "Treasurer", or "Technical"
        //   status     → "Approved", "Pending", or "On Hold"
        //
        // RETURNS:
        //   int → count of matching rows
        // ───────────────────────────────────────────────────────────────
        public int GetStatusCountForOffice(string officeDept, string status)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"SELECT COUNT(*) 
                       FROM ClearanceRequests 
                       WHERE Department = @dept 
                         AND Status = @status";

                return db.ExecuteScalar<int>(sql, new { dept = officeDept, status = status });
            }
        }

        // ───────────────────────────────────────────────────────────────
        // METHOD: GetTotalStudentsForOffice
        //
        // CALLED BY: BaseOfficeForm.cs → LoadDashboardStats()
        //
        // PURPOSE:
        //   Counts the total unique students who have ever submitted
        //   a clearance request to this office.
        //   Used for the progress bar label: "X out of Y students cleared"
        //
        // PARAMETERS:
        //   officeDept → "SSG", "Treasurer", or "Technical"
        //
        // RETURNS:
        //   int → total distinct students with a row for this office
        // ───────────────────────────────────────────────────────────────
        public int GetTotalStudentsForOffice(string officeDept)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"SELECT COUNT(DISTINCT UserID) 
                       FROM ClearanceRequests 
                       WHERE Department = @dept";

                return db.ExecuteScalar<int>(sql, new { dept = officeDept });
            }
        }
    }
}