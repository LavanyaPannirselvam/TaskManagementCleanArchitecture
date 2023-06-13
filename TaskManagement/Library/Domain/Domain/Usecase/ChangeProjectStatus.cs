using TaskManagementLibrary.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Enums;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface IChangeProjectStatusDataManager
    {
        void ChangeStatusOfProject(ChangeProjectStatusRequest request, IUsecaseCallbackBasecase<ChangeProjectStatusResponse> response);
    }


    public class ChangeProjectStatusRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get ; set; }
        public int projectId;
        public StatusType status;

        public ChangeProjectStatusRequest(int id,StatusType type,CancellationTokenSource cts)
        {
            projectId = id;
            status = type;
            CtsSource = cts;
        }
    }


    public interface IPresenterChangeProjectStatusCallback : IPresenterCallbackBasecase<ChangeProjectStatusResponse>
    { }


    public class ChangeProjectStatus : UsecaseBase<ChangeProjectStatusResponse>
    {
        private IChangeProjectStatusDataManager _dataManager;
        private ChangeProjectStatusRequest _request;
        private IPresenterChangeProjectStatusCallback _response;
        
        public ChangeProjectStatus(ChangeProjectStatusRequest request,IPresenterChangeProjectStatusCallback response)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IChangeProjectStatusDataManager>();
            _request = request;
            _response = response;
        }
        
        public override void Action()
        {
            this._dataManager.ChangeStatusOfProject(_request, new ChangeProjectStatusCallback(this));
        }
        
        
        public class ChangeProjectStatusCallback : IUsecaseCallbackBasecase<ChangeProjectStatusResponse>
        {
            private ChangeProjectStatus _projectStatus;
            
            public ChangeProjectStatusCallback(ChangeProjectStatus projectStatus)
            {
                _projectStatus = projectStatus;
            }
            
            public void OnResponseError(BException response)
            {
                _projectStatus._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<ChangeProjectStatusResponse> response)
            {
                _projectStatus._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<ChangeProjectStatusResponse> response)
            {
                _projectStatus._response.OnSuccessAsync(response);
            }
        }
    }


    public class ChangeProjectStatusResponse : ZResponse<string>
    { }
}
