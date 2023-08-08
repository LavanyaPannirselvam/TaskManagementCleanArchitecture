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
            var projectsList = DBhandler.ProjectsList(request.userName,request.userEmail,request.count,request.skipCount);
            GetProjectListResponse projectsResponse = new GetProjectListResponse();
            ZResponse<GetProjectListResponse> zResponse = new ZResponse<GetProjectListResponse>();
            if (projectsList.Count > 0)
            {
                projectsResponse.Data = projectsList;
                zResponse.Response = "";
            }
            else
            {
                zResponse.Response = "You have not created any project yet :)";
            }
            zResponse.Data = projectsResponse;
            response.OnResponseSuccess(zResponse);
        }
    }
}
