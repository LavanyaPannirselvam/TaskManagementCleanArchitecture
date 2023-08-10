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
    public interface IChangeTaskDescriptionDataManager
    {
        void ChangeDescription(ChangeTaskDescriptionRequest request, IUsecaseCallbackBasecase<ChangeTaskDescriptionResponse> callback);
    }


    public class ChangeTaskDescriptionRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get ; set ; }
        public string description;
        public int taskId;

        public ChangeTaskDescriptionRequest(string description, int id,CancellationTokenSource ctsSource)
        {
            CtsSource = ctsSource;
            this.description = description;
            this.taskId = id;
        }
    }


    public interface IPresenterChangeTaskDescriptionCallback : IPresenterCallbackBasecase<ChangeTaskDescriptionResponse>
    {}


    public class ChangeTaskDescription : UsecaseBase<ChangeTaskDescriptionResponse>
    {
        private IChangeTaskDescriptionDataManager _dataManager;
        private ChangeTaskDescriptionRequest _request;
        private IPresenterChangeTaskDescriptionCallback _callback;

        public ChangeTaskDescription(ChangeTaskDescriptionRequest request, IPresenterChangeTaskDescriptionCallback callback)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IChangeTaskDescriptionDataManager>();
            _request = request;
            _callback = callback;
        }

        public override void Action()
        {
            _dataManager.ChangeDescription(_request, new PresenterChangeTaskDescriptionCallback(this));
        }


        public class PresenterChangeTaskDescriptionCallback : IUsecaseCallbackBasecase<ChangeTaskDescriptionResponse>
        {
            private ChangeTaskDescription changeTaskDescription;

            public PresenterChangeTaskDescriptionCallback(ChangeTaskDescription changetaskDesc)
            {
                this.changeTaskDescription = changetaskDesc;
            }

            public void OnResponseError(BaseException response)
            {
                changeTaskDescription._callback.OnError(response);
            }

            public void OnResponseFailure(ZResponse<ChangeTaskDescriptionResponse> response)
            {
                changeTaskDescription._callback.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<ChangeTaskDescriptionResponse> response)
            {
                changeTaskDescription._callback.OnSuccessAsync(response);
            }
        }
    }


    public class ChangeTaskDescriptionResponse : ZResponse<Tasks>
    { }
}
