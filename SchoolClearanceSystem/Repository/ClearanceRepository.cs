using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace SchoolClearanceSystem.Repository
{
    public class ClearanceRepository: BaseRepository
    {
        public bool SubmitClearanceRequest(string studentId, string dept, string semester, string acadYear)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"INSERT INTO ClearanceRequests (StudentID, Department, Status, DateSubmitted, Semester, AcademicYear) 
                               VALUES (@id, @dept, 'Pending', @date, @sem, @ay)";

                return db.Execute(sql, new
                {
                    id = studentId,
                    dept,
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    sem = semester,
                    ay = acadYear
                }) > 0;
            }
        }

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
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    id = studentId,
                    dept = department
                }) > 0;
            }
        }
    }
}
