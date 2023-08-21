using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Domain;
using TaskManagementLibrary.Models;
using TaskManagementLibrary;
using Windows.UI.Xaml;
using TaskManagementLibrary.Notifications;

namespace TaskManagementCleanArchitecture.ViewModel
{
    internal class IssuePageViewModel : IssuesViewModelBase
    {
        private GetIssuesList _getIssuesList;

        public override void GetIssues(int projectId)//, int count, int skipCount)
        {
            _getIssuesList = new GetIssuesList(new GetIssuesListRequest(projectId, new CancellationTokenSource()), new PresenterGetIssuesList(this));
            _getIssuesList.Execute();
        }
    }


    public class PresenterGetIssuesList : IPresenterGetIssuesListCallback
    {
        private IssuesViewModelBase _viewModel;

        public PresenterGetIssuesList(IssuesViewModelBase viewModel)
        {
            _viewModel = viewModel;
        }

        public void OnError(BaseException errorMessage)
        {

        }

        public void OnFailure(ZResponse<GetIssuesListResponse> response)
        {

        }

        public async void OnSuccessAsync(ZResponse<GetIssuesListResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                if (response.Data.Data.Count!=0)
                {
                    PopulateData(response.Data.Data);
                    if(_viewModel.IssuesList.Count >=20)
                    {
                        _viewModel.DataGridHeight = new GridLength(750,GridUnitType.Pixel);
                    }
                    else
                    {
                        _viewModel.DataGridHeight = new GridLength(0, GridUnitType.Auto);
                    }
                    _viewModel.TextVisibility = Visibility.Collapsed;
                    _viewModel.DataGridVisibility = Visibility.Visible;
                }
                else
                {
                    _viewModel.TextVisibility = Visibility.Visible;
                    _viewModel.DataGridVisibility = Visibility.Collapsed;
                    _viewModel.ResponseString = response.Response;
                }
            });
        }

        private void PopulateData(List<Issue> data)
        {
            foreach (var p in data)
                _viewModel.IssuesList.Add(p);
        }
    }
    public abstract class IssuesViewModelBase : NotifyPropertyBase
    {
        public ObservableCollection<Issue> IssuesList = new ObservableCollection<Issue>();
        public int projectId { get; set; }
        public abstract void GetIssues(int projectId);//, int count, int skipCount);
        public IIssuePageUpdateNotification Notification { get; set; }

        public void CreateIssue(Issue issue)
        {
            CreateIssue _createIssue;
            _createIssue = new CreateIssue(new CreateIssueRequest(issue, new CancellationTokenSource()), new PresenterCreateIssueCallback(this));
            _createIssue.Execute();
        }

        public void DeleteIssue(int issueId)
        {
            DeleteIssue _deleteIssue;
            _deleteIssue = new DeleteIssue(new DeleteIssueRequest(issueId, new CancellationTokenSource()), new PresenterDeleteIssueCallback(this));
            _deleteIssue.Execute();
        }

        private Visibility _textVisibility = Visibility.Collapsed;
        public Visibility TextVisibility
        {
            get { return _textVisibility; }
            set
            {
                _textVisibility = value;
                OnPropertyChanged(nameof(TextVisibility));
            }
        }

        private string _responseString = string.Empty;
        public string ResponseString
        {
            get { return _responseString; }
            set
            {
                _responseString = value;
                OnPropertyChanged(nameof(ResponseString));
            }
        }

        private Visibility _dataGridVisibility = Visibility.Collapsed;
        public Visibility DataGridVisibility
        {
            get { return _dataGridVisibility; }
            set
            {
                _dataGridVisibility = value;
                OnPropertyChanged(nameof(DataGridVisibility));
            }

        }

        private Visibility _notificationVisibility;
        public Visibility NotificationVisibility
        {
            get { return _notificationVisibility; }
            set
            {
                _notificationVisibility = value;
                OnPropertyChanged(nameof(NotificationVisibility));
            }
        }

        private GridLength _dataGridHeight;//= 700;
        public GridLength DataGridHeight
        {
            get { return _dataGridHeight; }
            set
            {
                _dataGridHeight = value;
                OnPropertyChanged(nameof(DataGridHeight));
            }
        }
    }


    public interface IIssuePageUpdateNotification
    {
        void NotificationMessage();
    }


    public class PresenterDeleteIssueCallback : IPresenterDeleteIssueCallback
    {
        IssuesViewModelBase _deleteIssue;
        public PresenterDeleteIssueCallback(IssuesViewModelBase deleteTask)
        {
            _deleteIssue = deleteTask;
        }

        public void OnError(BaseException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<DeleteIssueResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<DeleteIssueResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _deleteIssue.ResponseString = response.Response.ToString();
                _deleteIssue.NotificationVisibility = Visibility.Visible;
                _deleteIssue.Notification.NotificationMessage();//notification need to display
                UIUpdation.OnIssueDeleted(response.Data.Data);
                if (_deleteIssue.IssuesList.Count == 0)
                {
                    _deleteIssue.DataGridVisibility = Visibility.Collapsed;
                    _deleteIssue.TextVisibility = Visibility.Visible;
                    _deleteIssue.ResponseString = "Issue not created yet:)";
                }
                else
                {
                    _deleteIssue.DataGridVisibility = Visibility.Visible;
                    _deleteIssue.TextVisibility = Visibility.Collapsed;
                }
            });
        }
    }

    public class PresenterCreateIssueCallback : IPresenterCreateIssueCallback
    {
        private IssuesViewModelBase _viewModel;

        public PresenterCreateIssueCallback(IssuesViewModelBase viewModel)
        {
            _viewModel = viewModel;
        }

        public void OnError(BaseException response)
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
                _viewModel.ResponseString = response.Response.ToString();
                _viewModel.NotificationVisibility = Visibility.Visible;
                _viewModel.Notification.NotificationMessage();
                UIUpdation.OnIssueCreated(response.Data.Data);
                if (_viewModel.IssuesList.Count != 0)
                {
                    _viewModel.DataGridVisibility = Visibility.Visible;
                    _viewModel.TextVisibility = Visibility.Collapsed;
                }
                else
                {
                    _viewModel.DataGridVisibility = Visibility.Collapsed;
                    _viewModel.TextVisibility = Visibility.Visible;
                    _viewModel.ResponseString = "Issues not created yet :)";
                }
            });
        }
    }
}
