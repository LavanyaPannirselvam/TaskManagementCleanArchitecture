using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Models;
using static TaskManagementLibrary.Domain.Usecase.DeleteTask;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface IDeleteTaskDataManager
    {
        void DeleteTask(DeleteTaskRequest request, IUsecaseCallbackBasecase<DeleteTaskResponse> response);
    }


    public class DeleteTaskRequest : IRequest
    {
        public int taskId;
        public CancellationTokenSource CtsSource { get ; set ; }

        public DeleteTaskRequest(int id,CancellationTokenSource cts) 
        {
            taskId = id;
            CtsSource = cts;
        }
    }


    public interface IPresenterDeleteTaskCallback : IPresenterCallbackBasecase<DeleteTaskResponse>
    { }


    public class DeleteTask : UsecaseBase<bool>
    {
        private IDeleteTaskDataManager _taskDeletionDataManager;
        private DeleteTaskRequest _taskProjectRequest;
        private IPresenterDeleteTaskCallback _callback;
        
        public DeleteTask( DeleteTaskRequest deleteProjectRequest, IPresenterDeleteTaskCallback callback)
        {
            _taskDeletionDataManager = ServiceProvider.GetInstance().Services.GetService<IDeleteTaskDataManager>();
            _taskProjectRequest = deleteProjectRequest;
            _callback = callback;
        }
        
        public override void Action()
        {
            this._taskDeletionDataManager.DeleteTask(_taskProjectRequest, new DeleteTaskCallback(this));
        }
        
        
        public class DeleteTaskCallback : IUsecaseCallbackBasecase<DeleteTaskResponse>
        {
            private DeleteTask _taskDeletion;
            
            public DeleteTaskCallback(DeleteTask projectDeletion)
            {
                _taskDeletion = projectDeletion;
            }

            public void OnResponseError(BaseException response)
            {
                _taskDeletion._callback.OnError(response);
            }

            public void OnResponseFailure(ZResponse<DeleteTaskResponse> response)
            {
                _taskDeletion._callback.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<DeleteTaskResponse> response)
            {
                _taskDeletion._callback.OnSuccessAsync(response);
            }
        }
        
        
        public class DeleteTaskResponse : ZResponse<Tasks>
        {
            //public Tasks taskDeleted { get; set; }
        }
    }
}
