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
    public interface ICreateTaskDataManager
    {
        void CreateTask(CreateTaskRequest request, IUsecaseCallbackBasecase<CreateTaskResponse> response);
    }


    public class CreateTaskRequest : IRequest
    {
        public Tasks task { get; set; }
        public CancellationTokenSource CtsSource { get; set; }

        public CreateTaskRequest(Tasks task, CancellationTokenSource Source)
        {
            this.task = task;
            CtsSource = Source;
        }
    }


    public interface IPresenterCreateTaskCallback : IPresenterCallbackBasecase<CreateTaskResponse>
    { }


    public class CreateTask : UsecaseBase<CreateTaskResponse>
    {
        private ICreateTaskDataManager _createTaskDataManager;
        private CreateTaskRequest _createTaskRequest;
        private IPresenterCreateTaskCallback _presenterCreateTaskResponse;

        public CreateTask(CreateTaskRequest request, IPresenterCreateTaskCallback response)
        {
            _createTaskDataManager = ServiceProvider.GetInstance().Services.GetService<ICreateTaskDataManager>(); ;
            _createTaskRequest = request;
            _presenterCreateTaskResponse = response;
        }

        public override void Action()
        {
            this._createTaskDataManager.CreateTask(_createTaskRequest, new CreateTaskCallback(this));
        }


        public class CreateTaskCallback : IUsecaseCallbackBasecase<CreateTaskResponse>
        {
            private CreateTask _createTask;

            public CreateTaskCallback(CreateTask createTask)
            {
                _createTask = createTask;
            }

            public void OnResponseError(BException error)
            {
                _createTask._presenterCreateTaskResponse?.OnError(error);
            }

            public void OnResponseFailure(ZResponse<CreateTaskResponse> response)
            {
                _createTask._presenterCreateTaskResponse?.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<CreateTaskResponse> response)
            {
                _createTask._presenterCreateTaskResponse?.OnSuccessAsync(response);
            }
        }
    }


    public class CreateTaskResponse : ZResponse<Tasks>
    {
        public Tasks NewTask { get; set; }
    }
}

