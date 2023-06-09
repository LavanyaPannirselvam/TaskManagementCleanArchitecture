using TaskManagementLibrary.Data.DBHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Data.DBHandler
{
    public interface IDBHandler : IProjectHandler, ITaskHandler, IUserHandler, IAssignmentHandler, IUserCredentialHandler
    {
    }
}
