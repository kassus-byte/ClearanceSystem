using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolClearanceSystem.Repository
{
    public abstract class BaseRepository
    {

        // protected- only this class and children can see
        // readonly- can never be reassigned
        //abstract- exists to be inherited not used on its own
        protected readonly DatabaseManager dbManager = new DatabaseManager();
    }
}
