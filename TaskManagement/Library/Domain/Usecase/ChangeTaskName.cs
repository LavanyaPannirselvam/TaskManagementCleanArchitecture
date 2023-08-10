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
    public interface IChangeTaskNameDataManager
    {
        void ChangeName(ChangeTaskNameRequest request, IUsecaseCallbackBasecase<ChangeTaskNameResponse> response);
    }


    public class ChangeTaskNameRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get; set; }
        public string name;
        public int taskId;

        public ChangeTaskNameRequest(string name, int id, CancellationTokenSource ctsSource)
        {
            CtsSource = ctsSource;
            this.name = name;
            this.taskId = id;
        }
    }


    public interface IPresenterChangeTaskNameCallback : IPresenterCallbackBasecase<ChangeTaskNameResponse>
    { }


    public class ChangeTaskName : UsecaseBase<ChangeTaskNameResponse>
    {
        private IChangeTaskNameDataManager _dataManager;
        private ChangeTaskNameRequest _request;
        private IPresenterChangeTaskNameCallback _callback;

        public ChangeTaskName(ChangeTaskNameRequest request, IPresenterChangeTaskNameCallback callback)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IChangeTaskNameDataManager>();
            _request = request;
            _callback = callback;
        }

        public override void Action()
        {
            _dataManager.ChangeName(_request, new PresenterChangeTaskNameCallback(this));
        }


        public class PresenterChangeTaskNameCallback : IUsecaseCallbackBasecase<ChangeTaskNameResponse>
        {
            private ChangeTaskName changeTaskName;

            public PresenterChangeTaskNameCallback(ChangeTaskName changeTaskName)
            {
                this.changeTaskName = changeTaskName;
            }

            public void OnResponseError(BaseException response)
            {
                changeTaskName._callback.OnError(response);
            }

            public void OnResponseFailure(ZResponse<ChangeTaskNameResponse> response)
            {
                changeTaskName._callback.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<ChangeTaskNameResponse> response)
            {
                changeTaskName._callback.OnSuccessAsync(response);
            }
        }
    }


    public class ChangeTaskNameResponse : ZResponse<Tasks>
    { }
}
