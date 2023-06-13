using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Models;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface IGetProjectsListDataManager
    {
        void GetProjects(GetProjectListRequest request, IUsecaseCallbackBasecase<GetProjectListResponse> response);
    }


    public class GetProjectListRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get; set ; }
        
        public GetProjectListRequest(CancellationTokenSource ctsSource)
        {
            CtsSource = ctsSource;
        }
    }


    public interface IPresenterGetProjectsListCallback : IPresenterCallbackBasecase<GetProjectListResponse> { }


    public class GetProjectsList : UsecaseBase<GetProjectListResponse>
    {
        private IPresenterGetProjectsListCallback _response;
        private GetProjectListRequest _request;
        private IGetProjectsListDataManager _dataManager;
        
        public GetProjectsList(IPresenterGetProjectsListCallback response, GetProjectListRequest request)
        {
            _response = response;
            _request = request;
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IGetProjectsListDataManager>();
        }

        
        public override void Action()
        {
            _dataManager.GetProjects(_request, new GetProjectsListCallback(this));
        }

        public class GetProjectsListCallback : IUsecaseCallbackBasecase<GetProjectListResponse>
        {
            private GetProjectsList _getProjects;

            public GetProjectsListCallback(GetProjectsList getProjects)
            {
                _getProjects = getProjects;
            }

            public void OnResponseError(BException response)
            {
                _getProjects._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<GetProjectListResponse> response)
            {
                _getProjects._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<GetProjectListResponse> response)
            {
                _getProjects._response.OnSuccessAsync(response);
            }
        }
    }


    public class GetProjectListResponse : ZResponse<List<Project>>
    {
        public List<Project> Projects;
    }
}
