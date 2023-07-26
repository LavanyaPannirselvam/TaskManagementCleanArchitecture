using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Models;

namespace TaskManagementLibrary.Domain.Usecase
{
    public interface IRemoveIssueFromUserDataManager
    {
        void RemoveIssueFromUser(RemoveIssueRequest request, IUsecaseCallbackBasecase<RemoveIssueResponse> response);
    }


    public class RemoveIssueRequest : IRequest
    {
        public CancellationTokenSource CtsSource { get; set; }
        public int issueId;
        public int userId;

        public RemoveIssueRequest(CancellationTokenSource ctsSource, int userId, int issueId)
        {
            CtsSource = ctsSource;
            this.issueId = issueId;
            this.userId = userId;
        }
    }


    public interface IPresenterRemoveIssueFromUserCallback : IPresenterCallbackBasecase<RemoveIssueResponse>
    { }

    
    public class RemoveIssueFromUser : UsecaseBase<RemoveIssueResponse>
    {
        private IRemoveIssueFromUserDataManager _datamanager;
        private RemoveIssueRequest _request;
        private IPresenterRemoveIssueFromUserCallback _response;

        public RemoveIssueFromUser(RemoveIssueRequest request, IPresenterRemoveIssueFromUserCallback response)
        {
            _datamanager = ServiceProvider.GetInstance().Services.GetService<IRemoveIssueFromUserDataManager>();
            _request = request;
            _response = response;
        }

        public override void Action()
        {
            this._datamanager.RemoveIssueFromUser(_request, new RemoveIssueCallback(this));
        }

        public class RemoveIssueCallback : IUsecaseCallbackBasecase<RemoveIssueResponse>
        {
            private RemoveIssueFromUser _taskRemovalFromUser;

            public RemoveIssueCallback(RemoveIssueFromUser projectRemovalFromUser)
            {
                _taskRemovalFromUser = projectRemovalFromUser;
            }

            public void OnResponseError(BaseException response)
            {
                _taskRemovalFromUser._response.OnError(response);
            }

            public void OnResponseFailure(ZResponse<RemoveIssueResponse> response)
            {
                _taskRemovalFromUser._response.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<RemoveIssueResponse> response)
            {
                _taskRemovalFromUser._response.OnSuccessAsync(response);
            }
        }
    }


    public class RemoveIssueResponse : ZResponse<ObservableCollection<User>>
    {
        //public ObservableCollection<User> users = new ObservableCollection<User>();
    }

}
