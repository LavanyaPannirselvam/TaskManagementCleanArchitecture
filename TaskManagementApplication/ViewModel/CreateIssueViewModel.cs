using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Models;
using TaskManagementLibrary;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class CreateIssueViewModel : CreateIssueViewModelBase
    {
        private CreateIssue _createIssue;

        public override void CreateIssue(Issue issue)
        {
            _createIssue = new CreateIssue(new CreateIssueRequest(issue, new CancellationTokenSource()), new PresenterCreateIssueCallback(this));
            _createIssue.Execute();
        }
    }


    public class PresenterCreateIssueCallback : IPresenterCreateIssueCallback
    {
        private CreateIssueViewModel _viewModel;

        public PresenterCreateIssueCallback(CreateIssueViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void OnError(BException response)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<CreateIssueResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<CreateIssueResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _viewModel.NewIssue = response.Data.NewIssue;
                // _viewModel.AddedView.UpdateNewProject(_viewModel.NewProject);
            });
        }
    }



    public abstract class CreateIssueViewModelBase : NotifyPropertyBase
    {
        public abstract void CreateIssue(Issue issue);
        private Issue _newIssue;
        public Issue NewIssue
        {
            get { return this._newIssue; }
            set
            {
                _newIssue = value;
                OnPropertyChanged(nameof(NewIssue));
            }
        }

        public IIssueAddedView AddedView { get; set; }
    }

    public interface IIssueAddedView
    {
        void UpdateNewIssue(Issue newIssue);
    }

}
