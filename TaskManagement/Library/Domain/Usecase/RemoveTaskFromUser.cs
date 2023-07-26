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
    public interface IRemoveTaskFromUserDataManager
    {
        void RemoveTaskFromUser(RemoveTaskRequest request, IUsecaseCallbackBasecase<RemoveTaskResponse> response);
    }


    public class RemoveTaskRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get; set; }
        public int taskId;
        public int userId;

        public RemoveTaskRequest(CancellationTokenSource ctsSource, int userId, int taskId)
        {
            CtsSource = ctsSource;
            this.taskId = taskId;
            this.userId = userId;
        }
    }


    public interface IPresenterRemoveTaskFromUserCallback : IPresenterCallbackBasecase<RemoveTaskResponse>
    { }


    public class RemoveTaskFromUser : UsecaseBase<RemoveTaskResponse>
    {
        private IRemoveTaskFromUserDataManager _datamanager;
        private RemoveTaskRequest _request;
        private IPresenterRemoveTaskFromUserCallback _response;

        public RemoveTaskFromUser(RemoveTaskRequest request, IPresenterRemoveTaskFromUserCallback response)
        {
            _datamanager = ServiceProvider.GetInstance().Services.GetService<IRemoveTaskFromUserDataManager>();
            _request = request;
            _response = response;
        }

        public override void Action()
        {
            this._datamanager.RemoveTaskFromUser(_request, new RemoveTaskCallback(this));
        }

        public class RemoveTaskCallback : IUsecaseCallbackBasecase<RemoveTaskResponse>
        {
            private RemoveTaskFromUser _taskRemovalFromUser;

            public RemoveTaskCallback(RemoveTaskFromUser projectRemovalFromUser)
            {
                _taskRemovalFromUser = projectRemovalFromUser;
            }

            public void OnResponseError(BaseException response)
            {
                _taskRemovalFromUser._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<RemoveTaskResponse> response)
            {
                _taskRemovalFromUser._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<RemoveTaskResponse> response)
            {
                _taskRemovalFromUser._response.OnSuccessAsync(response);
            }
        }
    }


    public class RemoveTaskResponse : ZResponse<ObservableCollection<User>>
    {
        //public ObservableCollection<User> users = new ObservableCollection<User>();
    }

}
