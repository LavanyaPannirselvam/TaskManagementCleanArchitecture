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
    public interface IChangeStartDateofTaskDataManager
    {
        void ChangeStartDate(ChangeStartDateofTaskRequest request, IUsecaseCallbackBasecase<ChangeStartDateofTaskResponse> response);
    }

    public class ChangeStartDateofTaskRequest : IRequest
    {
        public int taskId;
        public DateTimeOffset date;
        public CancellationTokenSource CtsSource { get; set; }

        public ChangeStartDateofTaskRequest(int id, DateTimeOffset date, CancellationTokenSource ctsSource)
        {
            this.taskId = id;
            this.date = date;
            CtsSource = ctsSource;
        }
    }

    public interface IPresenterChangeStartDateOfTaskCallback : IPresenterCallbackBasecase<ChangeStartDateofTaskResponse> { }


    public class ChangeStartDateofTask : UsecaseBase<ChangeStartDateofTaskResponse>
    {
        private IChangeStartDateofTaskDataManager _dataManager;
        private ChangeStartDateofTaskRequest _request;
        private IPresenterChangeStartDateOfTaskCallback _callback;

        public ChangeStartDateofTask(ChangeStartDateofTaskRequest request, IPresenterChangeStartDateOfTaskCallback callback)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IChangeStartDateofTaskDataManager>();
            _request = request;
            _callback = callback;
        }

        public override void Action()
        {
            _dataManager.ChangeStartDate(_request, new ChangeStartDateofTaskCallback(this));
        }


        public class ChangeStartDateofTaskCallback : IUsecaseCallbackBasecase<ChangeStartDateofTaskResponse>
        {
            private ChangeStartDateofTask _changeStartDateofTask;

            public ChangeStartDateofTaskCallback(ChangeStartDateofTask obj)
            {
                _changeStartDateofTask = obj;
            }
            public void OnResponseError(BaseException response)
            {
                _changeStartDateofTask._callback.OnError(response);
            }

            public void OnResponseFailure(ZResponse<ChangeStartDateofTaskResponse> response)
            {
                _changeStartDateofTask._callback.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<ChangeStartDateofTaskResponse> response)
            {
                _changeStartDateofTask._callback.OnSuccessAsync(response);
            }
        }
    }


    public class ChangeStartDateofTaskResponse : ZResponse<Tasks>
    {

    }
}
