using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Dapper;

namespace SchoolClearanceSystem
{
    /// <summary>
    /// OOP CONCEPT: SINGLE RESPONSIBILITY PRINCIPLE (SRP)
    /// This class has one clear job: manage connections and execute data queries for SQLite.
    /// It knows nothing about UI dashboards, validation rules, or clearance processing.
    /// </summary>
    public class DatabaseManager
    {
        /// <summary>
        /// OOP CONCEPT: ENCAPSULATION (Data Hiding)
        /// By making this variable 'private readonly', we lock down the physical database path.
        /// No outside form or repository can view or modify this string.
        /// If the database filename or location changes later, we only change it here.
        /// 
        /// HOW IT WORKS: 
        /// AppDomain.CurrentDomain.BaseDirectory dynamically locates the 'bin\Debug' or 'bin\Release' 
        /// folder where the executable runs, ensuring the app can find 'ClearanceSystem.db' on any machine.
        /// </summary>
        private readonly string connectionString = $"Data Source={AppDomain.CurrentDomain.BaseDirectory}ClearanceSystem.db";

        /// <summary>
        /// OOP CONCEPT: POLYMORPHISM (via Interfaces)
        /// The method returns an 'IDbConnection' interface instead of a concrete 'SqliteConnection'.
        /// 'SqliteConnection' *is a type of* 'IDbConnection'. Returning the interface decouples the application. 
        /// If you migrate from SQLite to SQL Server later, you only change the constructor inside this method; 
        /// your repositories won't break because they depend on the universal 'IDbConnection' interface.
        /// 
        /// HOW IT WORKS:
        /// 1. Instantiates a new connection pipeline targeting our database file.
        /// 2. Opens the connection pipeline actively.
        /// 3. Hands the open pipeline back to whoever requested it (like a Repository).
        /// </summary>
        public IDbConnection GetConnection()
        {
            var conn = new SqliteConnection(connectionString);
            conn.Open(); // Establishes the real-time stream with the .db file
            return conn;
        }

        /// <summary>
        /// OOP CONCEPT: ABSTRACTION (Hiding System Complexity)
        /// Opening connections, managing object lifecycles, streaming data readers, and loading tables 
        /// involves messy system plumbing. This method abstracts that work away behind a simple, 
        /// clean method interface. Repositories just call: GetDataTable("SELECT...", parameters);
        /// 
        /// HOW IT WORKS:
        /// 1. Instantiates an empty DataTable in-memory to hold our eventual row/column results.
        /// 2. Open/Close Lifecycle Management: The 'using' block ensures 'var db' closes and releases
        ///    system memory immediately when it finishes, even if the query errors out.
        /// 3. Dapper Integration: 'db.ExecuteReader' passes the SQL script and safe parameters to SQLite.
        /// 4. 'dt.Load(reader)' reads the database stream and formats it into rows inside the DataTable.
        /// 5. Graceful Exception Handling: The try-catch blocks prevent the system from crashing if a table 
        ///    name is mistyped, displaying a clear message dialog box instead.
        /// </summary>
        public DataTable GetDataTable(string sql, object parameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var db = GetConnection()) // Connection safely closes when code exits this block
                {
                    // Executes raw query text safely using Dapper parameterization
                    var reader = db.ExecuteReader(sql, parameters);
                    dt.Load(reader); // Hydrates the empty table structure with structural data records
                }
            }
            catch (Exception ex)
            {
                // Catches software anomalies and warns developer/user elegantly
                MessageBox.Show("Query Error: " + ex.Message, "Database Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt; // Returns either populated records or an empty schema skeleton
        }
    }
}