using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Models;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface IGetAssignedTasksListDataManager
    {
        void GetAssignedTasks(GetAssignedTasksListRequest request, IUsecaseCallbackBasecase<GetAssignedTasksListResponse> response);
    }


    public class GetAssignedTasksListRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get; set; }
        //public string userName;
        public string userEmail;
        //public int userId;

        public GetAssignedTasksListRequest(string userEmail, CancellationTokenSource cancellationTokenSource)
        {
            //this.userId = userId;
            //this.userName = name;
            this.userEmail = userEmail;
            CtsSource = cancellationTokenSource;
        }
    }

    public interface IPresenterGetAssignedTasksListCallback : IPresenterCallbackBasecase<GetAssignedTasksListResponse> { };


    public class GetAssignedTasksList : UsecaseBase<GetAssignedTasksListResponse>
    {
        private IGetAssignedTasksListDataManager _dataManager;
        private GetAssignedTasksListRequest _request;
        private IPresenterGetAssignedTasksListCallback _response;

        public GetAssignedTasksList(GetAssignedTasksListRequest request, IPresenterGetAssignedTasksListCallback response)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IGetAssignedTasksListDataManager>();
            _request = request;
            _response = response;
        }

        public override void Action()
        {
            _dataManager.GetAssignedTasks(_request, new GetAssignedTasksListCallback(this));
        }


        public class GetAssignedTasksListCallback : IUsecaseCallbackBasecase<GetAssignedTasksListResponse>
        {
            private GetAssignedTasksList _getTasksList;

            public GetAssignedTasksListCallback(GetAssignedTasksList getTasksList)
            {
                _getTasksList = getTasksList;
            }

            public void OnResponseError(BaseException response)
            {
                _getTasksList._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<GetAssignedTasksListResponse> response)
            {
                _getTasksList._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<GetAssignedTasksListResponse> response)
            {
                _getTasksList._response.OnSuccessAsync(response);
            }
        }
    }


    public class GetAssignedTasksListResponse : ZResponse<ObservableCollection<Tasks>>
    {
        //public List<Tasks> Tasks = new List<Tasks>();
    }
}
