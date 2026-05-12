using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolClearanceSystem.Repository
{
    public abstract class BaseRepository
    {
        protected readonly DatabaseManager dbManager = new DatabaseManager();
    }
}
