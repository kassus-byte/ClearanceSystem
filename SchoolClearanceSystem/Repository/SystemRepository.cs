using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace SchoolClearanceSystem.Repository
{
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
                string checkSql = @"SELECT COUNT(1) FROM ClearancePeriods 
                                    WHERE Semester = @sem AND AcademicYear = @ay";
                int exists = conn.ExecuteScalar<int>(checkSql, new { sem = semester, ay = academicYear });
                if (exists > 0) return false;

                conn.Execute("UPDATE ClearancePeriods SET IsActive = 0");

                string sql = @"INSERT INTO ClearancePeriods (Semester, AcademicYear, IsActive) 
                               VALUES (@sem, @ay, 1);";
                return conn.Execute(sql, new { sem = semester, ay = academicYear }) > 0;
            }
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