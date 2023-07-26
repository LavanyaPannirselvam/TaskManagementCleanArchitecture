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
    public interface IDeleteIssueDataManager
    {
        void DeleteIssue(DeleteIssueRequest request,IUsecaseCallbackBasecase<DeleteIssueResponse> response);
    }

    public class DeleteIssueRequest : IRequest
    {
        public int issueId;
        public CancellationTokenSource CtsSource { get ; set ; }

        public DeleteIssueRequest(int id,CancellationTokenSource source)
        {
            issueId = id;
            CtsSource = source;
        }
    }


    public interface IPresenterDeleteIssueCallback : IPresenterCallbackBasecase<DeleteIssueResponse> { }


    public class DeleteIssue : UsecaseBase<DeleteIssueResponse>
    {
        private IDeleteIssueDataManager _dataManager;
        private DeleteIssueRequest _request;
        private IPresenterDeleteIssueCallback _callback;

        public DeleteIssue(DeleteIssueRequest request,IPresenterDeleteIssueCallback callback)
        {
            _request = request;
            _callback = callback;
            _dataManager = ServiceProvider.GetInstance().Services.GetService<IDeleteIssueDataManager>();
        }

        public override void Action()
        {
            _dataManager.DeleteIssue(_request, new DeleteIssueCallback(this));
        }


        public class DeleteIssueCallback : IUsecaseCallbackBasecase<DeleteIssueResponse>
        {
            private DeleteIssue _deleteIssue;
            
            public DeleteIssueCallback(DeleteIssue deleteIssue)
            {
                _deleteIssue = deleteIssue;
            }

            public void OnResponseError(BaseException response)
            {
                _deleteIssue._callback.OnError(response);
            }

            public void OnResponseFailure(ZResponse<DeleteIssueResponse> response)
            {
                _deleteIssue._callback.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<DeleteIssueResponse> response)
            {
                _deleteIssue._callback.OnSuccessAsync(response);
            }
        }
    }


    public class DeleteIssueResponse : ZResponse<Issue>
    {
        //public Issue DeletedIssue { get; set; }
    } 
}
