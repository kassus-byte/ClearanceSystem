using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolClearanceSystem.Models;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
using System.Data;

namespace SchoolClearanceSystem.Repository
{
    /// <summary>
    /// OOP CONCEPT: INHERITANCE (Is-A Relationship)
    /// 'UserRepository' inherits from 'BaseRepository'. This means it automatically gains access 
    /// to the protected 'dbManager' instance defined in the base class without redeclaring it.
    /// It implements the Single Responsibility Principle by encapsulating all data transactions 
    /// related specifically to users and structural requests.
    /// </summary>
    public class UserRepository : BaseRepository
    {
        /// <summary>
        /// HOW IT WORKS & CONNECTS TO DATABASE MANAGER:
        /// 1. Calls 'dbManager.GetConnection()' to obtain an active polymorphic 'IDbConnection' pipeline.
        /// 2. Executes conditional ternary checks to build raw SQL based on filter requirements.
        /// 3. OOP CONCEPT: OBJECT-RELATIONAL MAPPING (Dapper ORM)
        ///    'db.Query<User>' is a generic method. Dapper maps database rows to C# 'User' 
        ///    objects by matching column names directly to class property names.
        /// </summary>
        public List<User> GetUsersByRole(string role, bool isStudent = true)
        {
            using (var db = dbManager.GetConnection()) // Ensures connection resource closure
            {
                string sql = isStudent
                ? "SELECT * FROM Users WHERE Role = 'Student' ORDER BY FullName ASC"
                : "SELECT * FROM Users WHERE Role != 'Student' AND Role != 'Admin' ORDER BY Role ASC";

                // Null-coalescing fallback: Returns an empty list instantiation if query comes up empty
                return db.Query<User>(sql).ToList() ?? new List<User>();
            }
        }

        public User GetUserDetails(string userId)
        {
            using (var db = dbManager.GetConnection())
            {
                // Dapper Parameterization: Using '@id' prevents SQL Injection security vulnerabilities.
                // An anonymous parameter object 'new { id = userId }' binds data types securely.
                return db.QueryFirstOrDefault<User>("SELECT * FROM Users WHERE UserID = @id", new { id = userId });
            }
        }

        /// <summary>
        /// HOW IT WORKS (Insert Pipeline):
        /// 1. Modifies the state of the passed 'User' object by calculating a timestamp string.
        /// 2. Passes the entire structured object directly to Dapper.
        /// 3. 'db.Execute' returns the number of database rows affected. If > 0, operation succeeded.
        /// </summary>
        public bool AddUser(User user)
        {
            try
            {
                user.DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string sql = @"INSERT INTO Users (UserID, FullName, Program, Year, Role, Password, UploadPath, DateCreated) 
                               VALUES (@UserID, @FullName, @Program, @Year, @Role, @Password, @UploadPath, @DateCreated)";

                using (var db = dbManager.GetConnection())
                {
                    return db.Execute(sql, user) > 0;
                }
            }
            // Error Handling: SQLite Error Code 19 explicitly identifies a Primary Key violation (Duplicate ID)
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
            {
                MessageBox.Show($"The User ID '{user.UserID}' is already registered!", "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
                return false;
            }
        }

        public bool EditUser(User user)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"UPDATE Users 
                               SET FullName = @FullName, Program = @Program, Year = @Year, Role = @Role 
                               WHERE UserID = @UserID";

                return db.Execute(sql, user) > 0;
            }
        }

        public bool ValidateLogin(string userId, string password)
        {
            using (var db = dbManager.GetConnection())
            {
                // COLLATE NOCASE ensures that student ID evaluations ignore text casing discrepancies during login verification
                string sql = "SELECT COUNT(*) FROM Users WHERE UserID = @id COLLATE NOCASE AND Password = @pass";

                // db.ExecuteScalar returns a single scalar value from the first column of the first row
                return db.ExecuteScalar<int>(sql, new { id = userId.Trim(), pass = password.Trim() }) > 0;
            }
        }

        public bool DeleteUser(string userId)
        {
            using (var db = dbManager.GetConnection())
            {
                return db.Execute("DELETE FROM Users WHERE UserID = @id", new { id = userId }) > 0;
            }
        }

        public int GetClearedCount(string studentId)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"SELECT COUNT(*) FROM ClearanceRequests 
                               WHERE StudentID = @id AND Status = 'Approved'";
                return db.ExecuteScalar<int>(sql, new { id = studentId });
            }
        }

        /// <summary>
        /// OOP CONCEPT: ABSTRACTION VIA DYNAMIC/ANONYMOUS TYPES
        /// 'IEnumerable<dynamic>' lets you stream back a collection of objects without declaring a formal model class.
        /// This is ideal for quick, custom read-only data transformations that only require specific columns.
        /// </summary>
        public IEnumerable<dynamic> GetStudentStatus(string studentId)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"SELECT Department, Status, Remarks FROM ClearanceRequests 
                               WHERE StudentID = @studentId";

                return db.Query(sql, new { studentId = studentId });
            }
        }

        /// <summary>
        /// HOW IT WORKS (Relational SQL Processing):
        /// Uses an SQL 'JOIN' clause. This connects the data fields inside the 'ClearanceRequests' table
        /// to the 'Users' table based on matching ID keys, pulling the student's 'FullName' on the fly.
        /// </summary>
        public IEnumerable<dynamic> GetDepartmentRequests(string department)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"SELECT r.StudentID, u.FullName, r.Status, r.DateSubmitted, r.Remarks 
                               FROM ClearanceRequests r
                               JOIN Users u ON r.StudentID = u.UserID
                               WHERE r.Department = @dept AND r.Status = 'Pending'";

                return db.Query(sql, new { dept = department });
            }
        }

        public bool UpdateRequestStatus(string studentId, string department, string status, string remarks)
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = @"UPDATE ClearanceRequests 
                               SET Status = @status, Remarks = @remarks 
                               WHERE StudentID = @id AND Department = @dept";
                return db.Execute(sql, new { id = studentId, dept = department, status = status, remarks = remarks }) > 0;
            }
        }
    }
}