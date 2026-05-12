using Dapper;
using System.Data;

namespace SchoolClearanceSystem.Repository
{
    public class SystemRepository : BaseRepository
    {
        public bool IsClearanceActive()
        {
            using (var conn = dbManager.GetConnection())
            {
                return conn.ExecuteScalar<int>("SELECT ClearanceIsActive FROM SystemSettings LIMIT 1") == 1;
            }
        }

        public void ToggleClearanceSeason(bool isActive)
        {
            using (var conn = dbManager.GetConnection())
            {
                conn.Execute("UPDATE SystemSettings SET ClearanceIsActive = @val", new { val = isActive ? 1 : 0 });
            }
        }
    }
}