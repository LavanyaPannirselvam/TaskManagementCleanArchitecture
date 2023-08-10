using TaskManagementLibrary.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Enums;
using TaskManagementLibrary.Models;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface IChangeTaskStatusDataManager
    {
        void ChangeStatusOfTask(ChangeTaskStatusRequest request, IUsecaseCallbackBasecase<ChangeTaskStatusResponse> response);
    }


    public class ChangeTaskStatusRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get ; set; }
        public int taskId;
        public StatusType status;

        public ChangeTaskStatusRequest(int id,StatusType type,CancellationTokenSource cts)
        {
            taskId = id;
            status = type;
            CtsSource = cts;
        }
    }


    public interface IPresenterChangeTaskStatusCallback : IPresenterCallbackBasecase<ChangeTaskStatusResponse>
    { }


    public class ChangeTaskStatus : UsecaseBase<ChangeTaskStatusResponse>
    {
        private IChangeTaskStatusDataManager _dataManager;
        private ChangeTaskStatusRequest _request;
        private IPresenterChangeTaskStatusCallback _response;
        
        public ChangeTaskStatus(ChangeTaskStatusRequest request,IPresenterChangeTaskStatusCallback response)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IChangeTaskStatusDataManager>();
            _request = request;
            _response = response;
        }
        
        public override void Action()
        {
            this._dataManager.ChangeStatusOfTask(_request, new ChangeTaskStatusCallback(this));
        }
        
        
        public class ChangeTaskStatusCallback : IUsecaseCallbackBasecase<ChangeTaskStatusResponse>
        {
            private ChangeTaskStatus _projectStatus;
            
            public ChangeTaskStatusCallback(ChangeTaskStatus projectStatus)
            {
                _projectStatus = projectStatus;
            }
            
            public void OnResponseError(BaseException response)
            {
                _projectStatus._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<ChangeTaskStatusResponse> response)
            {
                _projectStatus._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<ChangeTaskStatusResponse> response)
            {
                _projectStatus._response.OnSuccessAsync(response);
            }
        }
    }


    public class ChangeTaskStatusResponse : ZResponse<Tasks>
    { }
}
