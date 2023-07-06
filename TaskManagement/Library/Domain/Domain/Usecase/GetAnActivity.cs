using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Models;
using TaskManagementLibrary.Models.Enums;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface IGetAnActivityDataManager
    {
        void GetAnActivity(GetAnActivityRequest request, IUsecaseCallbackBasecase<GetAnActivityResponse> response);
    }


    public class GetAnActivityRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get ; set ; }
        public int id;
        public ActivityType activityType;

        public GetAnActivityRequest(int id,ActivityType activityType,CancellationTokenSource cancellationTokenSource)
        {
            this.id = id;
            this.activityType = activityType;  
            CtsSource = cancellationTokenSource;
        }

    }

    public interface IPresenterGetAnActivityCallback : IPresenterCallbackBasecase<GetAnActivityResponse> { };


    public class GetAnActivity : UsecaseBase<GetAnActivityResponse>
    {
        private IGetAnActivityDataManager _dataManager;
        private GetAnActivityRequest _request;
        private IPresenterGetAnActivityCallback _response;

        public GetAnActivity(GetAnActivityRequest request, IPresenterGetAnActivityCallback response)
        {
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IGetAnActivityDataManager>();
            _request = request;
            _response = response;
        }

        public override void Action()
        {
            _dataManager.GetAnActivity(_request, new GetAnActivityCallback(this));
        }

        public class GetAnActivityCallback : IUsecaseCallbackBasecase<GetAnActivityResponse>
        {
            private GetAnActivity _getAnActivity;

            public GetAnActivityCallback(GetAnActivity getAnActivity)
            {
                _getAnActivity = getAnActivity;
            }

            public void OnResponseError(BException response)
            {
                _getAnActivity._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<GetAnActivityResponse> response)
            {
                _getAnActivity._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<GetAnActivityResponse> response)
            {
                _getAnActivity._response.OnSuccessAsync(response);
            }
        }
    }


    public class GetAnActivityResponse : ZResponse<ActivityType>
    {
        public ActivityType AnActivity;
    }
}
