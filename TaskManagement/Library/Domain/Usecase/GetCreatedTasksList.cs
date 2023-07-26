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
    public interface IGetCreatedTasksListDataManager
    {
        void GetCreatedTasks(GetCreatedTasksListRequest request, IUsecaseCallbackBasecase<GetCreatedTasksListResponse> response);
    }


    public class GetCreatedTasksListRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get; set; }
        public string userName;
        public string userEmail;
        //public int userId;

        public GetCreatedTasksListRequest(string name, string email ,CancellationTokenSource cancellationTokenSource)
        {
            //this.userId = userId;
            this.userName = name;
            this.userEmail = email;
            CtsSource = cancellationTokenSource;
        }
    }

    public interface IPresenterGetCreatedTasksListCallback : IPresenterCallbackBasecase<GetCreatedTasksListResponse> { };


    public class GetCreatedTasksList : UsecaseBase<GetCreatedTasksListResponse>
    {
        private IGetCreatedTasksListDataManager _dataManager;
        private GetCreatedTasksListRequest _request;
        private IPresenterGetCreatedTasksListCallback _response;

        public GetCreatedTasksList(GetCreatedTasksListRequest request, IPresenterGetCreatedTasksListCallback response)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IGetCreatedTasksListDataManager>();
            _request = request;
            _response = response;
        }

        public override void Action()
        {
            _dataManager.GetCreatedTasks(_request, new GetCreatedTasksListCallback(this));
        }

        public class GetCreatedTasksListCallback : IUsecaseCallbackBasecase<GetCreatedTasksListResponse>
        {
            private GetCreatedTasksList _getTasksList;

            public GetCreatedTasksListCallback(GetCreatedTasksList getTasksList)
            {
                _getTasksList = getTasksList;
            }

            public void OnResponseError(BaseException response)
            {
                _getTasksList._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<GetCreatedTasksListResponse> response)
            {
                _getTasksList._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<GetCreatedTasksListResponse> response)
            {
                _getTasksList._response.OnSuccessAsync(response);
            }
        }
    }


    public class GetCreatedTasksListResponse : ZResponse<List<Tasks>>
    {
        //public List<Tasks> Tasks = new List<Tasks>();
    }
}
