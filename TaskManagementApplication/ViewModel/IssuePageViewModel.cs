﻿using System;
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

namespace TaskManagementCleanArchitecture.ViewModel
{
    internal class IssuePageViewModel : IssuesViewModelBase
    {
        private GetIssuesList _getIssuesList;

        public override void GetIssues(int projectId)
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

        public void OnError(BException errorMessage)
        {

        }

        public void OnFailure(ZResponse<GetIssuesListResponse> response)
        {

        }

        public async void OnSuccessAsync(ZResponse<GetIssuesListResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                if (response.Data.Issues.Count > 0)
                {
                    PopulateData(response.Data.Issues);
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
        public abstract void GetIssues(int projectId);
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

        public void OnError(BException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<bool> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<bool> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _deleteIssue.ResponseString = response.Response.ToString();
                _deleteIssue.NotificationVisibility = Visibility.Visible;
                _deleteIssue.Notification.NotificationMessage();//notification need to display
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
                //_viewModel.NewIssue = response.Data.NewIssue;
                _viewModel.ResponseString = response.Response.ToString();
                _viewModel.NotificationVisibility = Visibility.Visible;
                _viewModel.Notification.NotificationMessage();
                // _viewModel.AddedView.UpdateNewProject(_viewModel.NewProject);
            });
        }
    }


}
