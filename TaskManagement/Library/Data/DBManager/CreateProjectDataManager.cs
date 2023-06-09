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
    public class CreateProjectDataManager : TaskManagementDataManager,ICreateProjectDataManager
    {
        public CreateProjectDataManager(IDBHandler handler) : base(handler) { }

        public void CreateProject(CreateProjectRequest request, IUsecaseCallbackBasecase<bool> response)
        {
            var newProject = request.project;
            DbHandler.AddProject(newProject);
            ZResponse<bool> zResponse = new ZResponse<bool>();
            zResponse.Response = "Project added successfully";
            zResponse.Data = true;
            response.OnResponseSuccess(zResponse);
        }
        //event to be created and handled
    }
}
