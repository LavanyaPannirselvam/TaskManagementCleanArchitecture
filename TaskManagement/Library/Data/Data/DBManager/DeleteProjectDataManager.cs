
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Domain.Usecase;

namespace TaskManagementLibrary.Data.DBManager
{
    public class DeleteProjectDataManager : TaskManagementDataManager , IDeleteProjectDataManager
    {
        public DeleteProjectDataManager(IDBHandler dbHandler) : base(dbHandler) {}

        public void DeleteProject(DeleteProjectRequest request, IUsecaseCallbackBasecase<bool> response)
        {
            var id = request.projectId;
            DbHandler.DeleteProject(id);
            ZResponse<bool> zResponse = new ZResponse<bool>();
            zResponse.Response = "Project deleted successfully";
            zResponse.Data = true;
            response.OnResponseSuccess(zResponse);
        }
    }
    //event to be created and handled
}
