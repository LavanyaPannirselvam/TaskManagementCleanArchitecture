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
    public class GetProjectsListDataManager : TaskManagementDataManager, IGetProjectsListDataManager
    {
        public GetProjectsListDataManager(IDBHandler dbHandler) : base(dbHandler)
        {
        }

        public void GetProjects(GetProjectListRequest request, IUsecaseCallbackBasecase<GetProjectListResponse> response)
        {
            var list = DbHandler.ProjectsList(request.userName,request.userEmail);
            GetProjectListResponse projectsResponse = new GetProjectListResponse();
            ZResponse<GetProjectListResponse> zResponse = new ZResponse<GetProjectListResponse>();
            projectsResponse.Projects = list;
            zResponse.Response = "";
            zResponse.Data = projectsResponse;
            response.OnResponseSuccess(zResponse);
        }
    }
}
