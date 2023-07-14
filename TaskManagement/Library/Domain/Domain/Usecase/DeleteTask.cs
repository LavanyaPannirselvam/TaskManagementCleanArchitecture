using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface IDeleteTaskDataManager
    {
        void DeleteTask(DeleteTaskRequest request, IUsecaseCallbackBasecase<bool> response);
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


    public interface IPresenterDeleteTaskCallback : IPresenterCallbackBasecase<bool>
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
        
        
        public class DeleteTaskCallback : IUsecaseCallbackBasecase<bool>
        {
            private DeleteTask _taskDeletion;
            
            public DeleteTaskCallback(DeleteTask projectDeletion)
            {
                _taskDeletion = projectDeletion;
            }

            public void OnResponseError(BException response)
            {
                _taskDeletion._callback.OnError(response);
            }

            public void OnResponseFailure(ZResponse<bool> response)
            {
                _taskDeletion._callback.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<bool> response)
            {
                _taskDeletion._callback.OnSuccessAsync(response);
            }
        }
        
        
        public class DeleteTaskResponse : ZResponse<bool>
        {

        }
    }
}
