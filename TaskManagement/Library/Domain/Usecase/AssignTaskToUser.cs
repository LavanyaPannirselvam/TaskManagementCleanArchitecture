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
    public interface IAssignTaskToUserDataManager
    {
        void AssignTask(AssignTaskRequest request, IUsecaseCallbackBasecase<AssignTaskResponse> response);
    }


    public class AssignTaskRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get ; set ; }
        public int taskId;
        public int userId;
        
        public AssignTaskRequest(int taskId,int userId,CancellationTokenSource cts) 
        {
            this.taskId = taskId;
            this.userId = userId;
            CtsSource = cts;
        }
    }

    public interface IPresenterAssignTaskCallback : IPresenterCallbackBasecase<AssignTaskResponse>
    { }


    public class AssignTaskToUser : UsecaseBase<AssignTaskResponse>
    {
        private IAssignTaskToUserDataManager _assignTaskDataManager;
        private AssignTaskRequest _request;
        private IPresenterAssignTaskCallback _response;
    
        public AssignTaskToUser(AssignTaskRequest request, IPresenterAssignTaskCallback response)
        {
            _assignTaskDataManager = ServiceProvider.GetInstance().Services.GetService<IAssignTaskToUserDataManager>();
            this._request = request;
            this._response = response;
        }
        
        public override void Action()
        {
            this._assignTaskDataManager.AssignTask(_request, new AssignTaskCallback(this));
        }


        public class AssignTaskCallback : IUsecaseCallbackBasecase<AssignTaskResponse>
        {
            private AssignTaskToUser _taskAssignment;
            
            public AssignTaskCallback(AssignTaskToUser obj)
            {
                _taskAssignment = obj;
            }

            public void OnResponseError(BaseException response)
            {
                _taskAssignment._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<AssignTaskResponse> response)
            {
                _taskAssignment._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<AssignTaskResponse> response)
            {
                _taskAssignment._response.OnSuccessAsync(response);
            }
        }
    }


    public class AssignTaskResponse : ZResponse<ObservableCollection<User>>
    {
        //public TaskBO taskBO { get; set; }
        //public ObservableCollection<User> users = new ObservableCollection<User>();
    }
    
}
