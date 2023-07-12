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

        public void CreateProject(CreateProjectRequest request, IUsecaseCallbackBasecase<CreateProjectResponse> response)
        {
            var newProject = request.project;
            var id = DbHandler.AddProject(newProject);
            newProject = DbHandler.GetProject(id);
            ZResponse<CreateProjectResponse> zResponse = new ZResponse<CreateProjectResponse>();
            CreateProjectResponse createProjectResponse = new CreateProjectResponse();
            createProjectResponse.NewProject = newProject;
            zResponse.Response = "Project added successfully";
            zResponse.Data = createProjectResponse;
            response.OnResponseSuccess(zResponse);
        } 
        //event to be created and handled
    }
}
