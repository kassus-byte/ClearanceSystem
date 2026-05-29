using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using SchoolClearanceSystem.Models;



namespace SchoolClearanceSystem.Repository
{
    
    public class SystemRepository : BaseRepository
    {
        public bool CreateNewPeriod(string semester, string academicYear)
        {
            using (var conn = dbManager.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Check if the specific period already exists
                        string checkSql = @"SELECT COUNT(1) FROM ClearancePeriods 
                                            WHERE Semester = @sem AND AcademicYear = @ay";
                        int exists = conn.ExecuteScalar<int>(checkSql, new { sem = semester, ay = academicYear }, transaction);
                        if (exists > 0) return false;

                        // 2. Deactivate all existing clearance periods
                        conn.Execute("UPDATE ClearancePeriods SET IsActive = 0", null, transaction);

                        // 3. Create the new period and mark it as active
                        string sql = @"INSERT INTO ClearancePeriods (Semester, AcademicYear, IsActive) 
                                       VALUES (@sem, @ay, 1);";
                        int inserted = conn.Execute(sql, new { sem = semester, ay = academicYear }, transaction);

                        if (inserted == 0)
                        {
                            transaction.Rollback();
                            return false;
                        }

                        // 4. AUTOMATION LOGIC: Roll over existing students to their next year level
                        if (semester.Trim().Equals("1st Semester", StringComparison.OrdinalIgnoreCase))
                        {
                            PromoteStudentsLogic(conn, transaction, academicYear);
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool ForceExecutePromotion(string academicYear)
        {
            using (var conn = dbManager.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        bool outcome = PromoteStudentsLogic(conn, transaction, academicYear);
                        transaction.Commit();
                        return outcome;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private bool PromoteStudentsLogic(System.Data.IDbConnection conn, System.Data.IDbTransaction transaction, string academicYear)
        {
            // This ONLY updates students who carried over from previous school years (where tracking field is old or null).
            // New students created DURING this academic year already match @ay, so this script safely ignores them!
            string promoteSql = @"
                UPDATE Users 
                SET 
                    IsActive = CASE Year
                        WHEN '4th Year' THEN 0 
                        ELSE IsActive 
                    END,
                    Year = CASE Year
                        WHEN '1st Year' THEN '2nd Year'
                        WHEN '2nd Year' THEN '3rd Year'
                        WHEN '3rd Year' THEN '4th Year'
                        WHEN '4th Year' THEN 'Graduated'
                        ELSE Year 
                    END,
                    CurrentPeriodPromotionYear = @ay
                WHERE Role = 'Student' 
                  AND Year IS NOT NULL 
                  AND IsActive = 1
                  AND (CurrentPeriodPromotionYear IS NULL OR CurrentPeriodPromotionYear != @ay);";

            int affectedRows = conn.Execute(promoteSql, new { ay = academicYear }, transaction);
            return affectedRows > 0;
        }

        public IEnumerable<ClearancePeriodItem> GetAllPeriods()
        {
            using (var conn = dbManager.GetConnection())
            {
                return conn.Query<ClearancePeriodItem>(@"SELECT PeriodID, Semester, AcademicYear, IsActive 
                                                         FROM ClearancePeriods 
                                                         ORDER BY PeriodID DESC").ToList();
            }
        }

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

        public bool CloseActivePeriod()
        {
            using (var db = dbManager.GetConnection())
            {
                return db.Execute("UPDATE ClearancePeriods SET IsActive = 0 WHERE IsActive = 1") > 0;
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