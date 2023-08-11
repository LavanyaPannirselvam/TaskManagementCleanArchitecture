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
    public interface IGetTasksListDataManager
    {
        void GetTasks(GetTasksListRequest request, IUsecaseCallbackBasecase<GetTasksListResponse> response);
    }


    public class GetTasksListRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get; set; }
        public int projectId;

        public GetTasksListRequest(int projectId,CancellationTokenSource cancellationTokenSource)
        {
            this.projectId = projectId;
        }
    }

    public interface IPresenterGetTasksListCallback : IPresenterCallbackBasecase<GetTasksListResponse> { };


    public class GetTasksList : UsecaseBase<GetTasksListResponse>
    {
        private IGetTasksListDataManager _dataManager;
        private GetTasksListRequest _request;
        private IPresenterGetTasksListCallback _response;

        public GetTasksList(GetTasksListRequest request, IPresenterGetTasksListCallback response)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IGetTasksListDataManager>();
            _request = request;
            _response = response;
        }

        public override void Action()
        {
            _dataManager.GetTasks(_request, new GetTasksListCallback(this));
        }

        public class GetTasksListCallback : IUsecaseCallbackBasecase<GetTasksListResponse>
        {
            private GetTasksList _getTasksList;

            public GetTasksListCallback(GetTasksList getTasksList)
            {
                _getTasksList = getTasksList;
            }

            public void OnResponseError(BaseException response)
            {
                _getTasksList._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<GetTasksListResponse> response)
            {
                _getTasksList._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<GetTasksListResponse> response)
            {
                _getTasksList._response.OnSuccessAsync(response);
            }
        }
    }


    public class GetTasksListResponse : ZResponse<List<Tasks>>
    {
        //public List<Tasks> Tasks;
    }
}

