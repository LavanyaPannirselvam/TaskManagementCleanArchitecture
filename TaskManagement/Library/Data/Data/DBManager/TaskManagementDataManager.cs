using TaskManagementLibrary.Data.DBHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data;

namespace TaskManagementLibrary.Data.DBManager
{
    public class TaskManagementDataManager
    {
        protected static IDBHandler DbHandler;
        public TaskManagementDataManager(IDBHandler dbHandler)
        {
            DbHandler = dbHandler;
        }

        
    }

}