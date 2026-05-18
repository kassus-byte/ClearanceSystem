using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SchoolClearanceSystem.Models; // 

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
            using (var db = dbManager.GetConnection()) // Automatic scoping: Closes database connection when leaving this block
            {
                string sql = @"INSERT INTO ClearanceRequests (StudentID, Department, Status, DateSubmitted, Semester, AcademicYear, FilePath) 
                               VALUES (@id, @dept, 'Pending', @date, @sem, @ay, @path)";

                // db.Execute returns an integer representing rows modified in storage. 
                // If the return count is greater than 0, the operation was a true success.
                return db.Execute(sql, new
                {
                    id = studentId,
                    dept, // In StudentPortal, this passes "Technical", "SSG", or "Treasurer"
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), // Generates localized persistent timestamps
                    sem = semester,
                    ay = acadYear,
                    path = filePath // Passes the string location pointer to the DB engine safely
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
                                c.StudentID AS UserID, 
                                u.FullName AS FullName, 
                                u.Program AS Program, 
                                u.Year AS Year, 
                                c.Semester AS Semester,
                                c.Status AS Status,      -- Maps 'Pending' safely into your STATUS column!
                                '' AS Action,            -- Keeps the ACTION column completely empty for now
                                c.Remarks AS Remarks,
                                c.FilePath AS FilePath
                               FROM ClearanceRequests c
                               INNER JOIN Users u ON c.StudentID = u.UserID
                               WHERE c.Department = @dept AND c.Status = 'Pending'";

                return db.Query(sql, new { dept = officeDept }).ToList();
            }
        }

        /// <summary>
        /// HOW IT WORKS (Update Pipeline):
        /// 1. Triggered exclusively when an Office Staff user (Treasurer, Technical, SSG) clicks Approve/Reject.
        /// 2. Binds the dynamic variables parsed from the UI into a secure query map context.
        /// 3. Updates persistent records matching specific compound conditions (StudentID + Department).
        /// </summary>
        public bool UpdateRequestStatus(string studentId, string department, string newStatus, string remarks)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"UPDATE ClearanceRequests 
                               SET Status = @status, Remarks = @remarks, DateProcessed = @date 
                               WHERE StudentID = @id AND Department = @dept";

                return db.Execute(sql, new
                {
                    status = newStatus,
                    remarks,
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), // Automatically logs processing time
                    id = studentId,
                    dept = department
                }) > 0;
            }
        }
    }
}