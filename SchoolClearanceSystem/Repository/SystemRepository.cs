using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
        public bool CreateNewPeriod(string semester, string academicYear)
        {
            using (var conn = dbManager.GetConnection())
            {
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
    }
}