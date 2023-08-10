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
    public interface IChangeEndDateofTaskDataManager
    {
        void ChangeEndDate(ChangeEndDateofTaskRequest request, IUsecaseCallbackBasecase<ChangeEndDateofTaskResponse> response);
    }

    public class ChangeEndDateofTaskRequest : IRequest
    {
        public int taskId;
        public DateTimeOffset date;
        public CancellationTokenSource CtsSource { get; set; }

        public ChangeEndDateofTaskRequest(int id, DateTimeOffset date, CancellationTokenSource ctsSource)
        {
            this.taskId = id;
            this.date = date;
            CtsSource = ctsSource;
        }
    }

    public interface IPresenterChangeEndDateOfTaskCallback : IPresenterCallbackBasecase<ChangeEndDateofTaskResponse> { }


    public class ChangeEndDateofTask : UsecaseBase<ChangeEndDateofTaskResponse>
    {
        private IChangeEndDateofTaskDataManager _dataManager;
        private ChangeEndDateofTaskRequest _request;
        private IPresenterChangeEndDateOfTaskCallback _callback;

        public ChangeEndDateofTask(ChangeEndDateofTaskRequest request, IPresenterChangeEndDateOfTaskCallback callback)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IChangeEndDateofTaskDataManager>();
            _request = request;
            _callback = callback;
        }

        public override void Action()
        {
            _dataManager.ChangeEndDate(_request, new ChangeEndDateofTaskCallback(this));
        }


        public class ChangeEndDateofTaskCallback : IUsecaseCallbackBasecase<ChangeEndDateofTaskResponse>
        {
            private ChangeEndDateofTask _changeEndDateofTask;

            public ChangeEndDateofTaskCallback(ChangeEndDateofTask obj)
            {
                _changeEndDateofTask = obj;
            }
            public void OnResponseError(BaseException response)
            {
                _changeEndDateofTask._callback.OnError(response);
            }

            public void OnResponseFailure(ZResponse<ChangeEndDateofTaskResponse> response)
            {
                _changeEndDateofTask._callback.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<ChangeEndDateofTaskResponse> response)
            {
                _changeEndDateofTask._callback.OnSuccessAsync(response);
            }
        }
    }


    public class ChangeEndDateofTaskResponse : ZResponse<Tasks>
    {

    }
}


