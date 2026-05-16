using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

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
        /// 2. Defines a clean structural INSERT query string mapping data parameters safely.
        /// 3. OOP CONCEPT: POLYMORPHISM / DATA ENCAPSULATION (Anonymous Types)
        ///    'new { id = studentId, dept ... }' creates an anonymous object on the fly. 
        ///    Dapper reads this dynamic structure to pass parameters safely to the SQL statement,
        ///    completely eliminating SQL Injection threats.
        /// </summary>
        public bool SubmitClearanceRequest(string studentId, string dept, string semester, string acadYear)
        {
            using (var db = dbManager.GetConnection()) // Automatic scoping: Closes database connection when leaving this block
            {
                // Hardcoding 'Pending' status here enforces consistent structural application data rules
                string sql = @"INSERT INTO ClearanceRequests (StudentID, Department, Status, DateSubmitted, Semester, AcademicYear) 
                               VALUES (@id, @dept, 'Pending', @date, @sem, @ay)";

                // db.Execute returns an integer representing rows modified in storage. 
                // If the return count is greater than 0, the operation was a true success.
                return db.Execute(sql, new
                {
                    id = studentId,
                    dept, // Implicit naming shortcut: C# assigns property key matching variable title automatically
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), // Generates localized persistent timestamps
                    sem = semester,
                    ay = acadYear
                }) > 0;
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