using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SchoolClearanceSystem.Models; // Added to map the DashboardMetrics model

namespace SchoolClearanceSystem.Repository
{
    // Strong model layout blueprint to enforce primitive variable types between SQLite and C#
    public class ClearancePeriodItem
    {
        public int PeriodID { get; set; }
        public string Semester { get; set; }
        public string AcademicYear { get; set; }
        public int IsActive { get; set; }
    }

    public class SystemRepository : BaseRepository
    {
        // ── ADDED CODE FOR STEP 2: Live Metrics Engine ───────────────────────────
        public DashboardMetrics GetLiveDashboardMetrics()
        {
            DashboardMetrics metrics = new DashboardMetrics();

            try
            {
                using (var conn = dbManager.GetConnection())
                {
                    // 1. Fetch total student counts
                    metrics.TotalStudents = conn.ExecuteScalar<int>(
                        "SELECT COUNT(*) FROM Users WHERE Role = 'Student';"
                    );

                    // 2. Fetch total office/staff account counts
                    metrics.TotalOfficeAccounts = conn.ExecuteScalar<int>(
                        "SELECT COUNT(*) FROM Users WHERE Role = 'Staff';"
                    );

                    // 3. Fetch new registrations count (Adjust table/column fields if needed)
                    // This queries users registered within the current calendar week
                    metrics.NewRegistrationsCount = conn.ExecuteScalar<int>(
                        @"SELECT COUNT(*) FROM Users 
                          WHERE Role = 'Student' 
                          AND strftime('%W', CreatedAt) = strftime('%W', 'now');"
                    );
                }
            }
            catch (Exception)
            {
                // Fallback graceful safety block to preserve app stability if schema diverges
            }

            return metrics;
        }
        // ─────────────────────────────────────────────────────────────────────────
        /// <summary>
        /// OOP Integration: Pulls contextual aggregated analytics numbers for a specific office workspace.
        /// </summary>
        public OfficeMetrics GetOfficeDashboardMetrics(string officeName)
        {
            OfficeMetrics metrics = new OfficeMetrics();
            try
            {
                using (var conn = dbManager.GetConnection())
                {
                    // Counts distinct clearance rows assigned to this specific office grouped by status values
                    metrics.ClearedCount = conn.ExecuteScalar<int>(
                        "SELECT COUNT(*) FROM ClearanceRequests WHERE OfficeName = @office AND Status = 'Approved';",
                        new { office = officeName });

                    metrics.PendingCount = conn.ExecuteScalar<int>(
                        "SELECT COUNT(*) FROM ClearanceRequests WHERE OfficeName = @office AND Status = 'Pending';",
                        new { office = officeName });

                    metrics.OnHoldCount = conn.ExecuteScalar<int>(
                        "SELECT COUNT(*) FROM ClearanceRequests WHERE OfficeName = @office AND Status = 'On Hold';",
                        new { office = officeName });

                    // General context metric to find total distinct students tracked under this system partition
                    metrics.TotalStudentsCount = conn.ExecuteScalar<int>(
                        "SELECT COUNT(*) FROM Users WHERE Role = 'Student';");
                }
            }
            catch (Exception)
            {
                // Fallback graceful safety block to preserve application stability if schema differs
            }
            return metrics;
        }
        public bool CreateNewPeriod(string semester, string academicYear)
        {
            using (var conn = dbManager.GetConnection())
            {
                // Block duplicate: same Semester + AcademicYear cannot exist more than once
                string checkSql = @"SELECT COUNT(1) FROM ClearancePeriods 
                            WHERE Semester = @sem AND AcademicYear = @ay";
                int exists = conn.ExecuteScalar<int>(checkSql, new { sem = semester, ay = academicYear });

                if (exists > 0) return false;

                // Deactivate all existing periods, then insert the new active one
                conn.Execute("UPDATE ClearancePeriods SET IsActive = 0");

                string sql = @"INSERT INTO ClearancePeriods (Semester, AcademicYear, IsActive) 
                       VALUES (@sem, @ay, 1);";

                return conn.Execute(sql, new { sem = semester, ay = academicYear }) > 0;
            }
        }

        /// <summary>
        /// FIXED: Now maps results explicitly to <ClearancePeriodItem> instead of loose dynamic structures
        /// </summary>
        public IEnumerable<ClearancePeriodItem> GetAllPeriods()
        {
            using (var conn = dbManager.GetConnection())
            {
                return conn.Query<ClearancePeriodItem>(@"SELECT PeriodID, Semester, AcademicYear, IsActive 
                                                        FROM ClearancePeriods 
                                                        ORDER BY PeriodID DESC").ToList();
            }
        }

        /// <summary>
        /// FIXED: Now maps the active profile safely to a strong object reference
        /// </summary>
        public ClearancePeriodItem GetActivePeriodSettings()
        {
            using (var conn = dbManager.GetConnection())
            {
                return conn.Query<ClearancePeriodItem>(@"SELECT PeriodID, Semester, AcademicYear, IsActive 
                                                        FROM ClearancePeriods 
                                                        WHERE IsActive = 1 
                                                        LIMIT 1").FirstOrDefault();
            }
        }
        // ───────────────────────────────────────────────────────────────
        // METHOD: CloseActivePeriod
        //
        // CALLED BY: AdminDashboard.cs → btnClosePeriod_Click
        //
        // PURPOSE:
        //   Sets ALL periods to IsActive = 0.
        //   This closes the clearance system without opening a new period.
        //   Students will see "No active period" and cannot submit.
        //
        // RETURNS:
        //   true  → at least one period was deactivated
        //   false → nothing was active to begin with
        // ───────────────────────────────────────────────────────────────
        public bool CloseActivePeriod()
        {
            using (var db = dbManager.GetConnection())
            {
                string sql = "UPDATE ClearancePeriods SET IsActive = 0 WHERE IsActive = 1";
                return db.Execute(sql) > 0;
            }
        }

        public bool DeletePeriod(string semester, string academicYear)
        {
            using (var conn = dbManager.GetConnection())
            {
                string sql = @"DELETE FROM ClearancePeriods 
                       WHERE Semester = @sem AND AcademicYear = @ay AND IsActive = 0";
                return conn.Execute(sql, new { sem = semester, ay = academicYear }) > 0;
            }
        }

    }
}