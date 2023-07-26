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
    public interface IGetATaskDataManager
    {
        void GetATask(GetATaskRequest request, IUsecaseCallbackBasecase<GetATaskResponse> response);
    }


    public class GetATaskRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get; set; }
        public int taskId;

        public GetATaskRequest(int id, CancellationTokenSource cancellationTokenSource)
        {
            taskId = id;
            CtsSource = cancellationTokenSource;
        }

    }

    public interface IPresenterGetATaskListCallback : IPresenterCallbackBasecase<GetATaskResponse> { };


    public class GetATask : UsecaseBase<GetATaskResponse>
    {
        private IGetATaskDataManager _dataManager;
        private GetATaskRequest _request;
        private IPresenterGetATaskListCallback _response;

        public GetATask(GetATaskRequest request, IPresenterGetATaskListCallback response)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IGetATaskDataManager>();
            _request = request;
            _response = response;
        }

        public override void Action()
        {
            _dataManager.GetATask(_request, new GetATaskCallback(this));
        }

        public class GetATaskCallback : IUsecaseCallbackBasecase<GetATaskResponse>
        {
            private GetATask _getATask;

            public GetATaskCallback(GetATask getTasksList)
            {
                _getATask = getTasksList;
            }

            public void OnResponseError(BaseException response)
            {
                _getATask._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<GetATaskResponse> response)
            {
                _getATask._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<GetATaskResponse> response)
            {
                _getATask._response.OnSuccessAsync(response);
            }
        }
    }


    public class GetATaskResponse : ZResponse<TaskBO>
    {
    }
}

