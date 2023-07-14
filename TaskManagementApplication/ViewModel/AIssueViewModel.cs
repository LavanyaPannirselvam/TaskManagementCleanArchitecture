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

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class AIssueViewModel : AIssueViewModelBase
    {
        private GetAIssue _getAIssue;
        public override void GetAIssue(int issueId)
        {
            _getAIssue = new GetAIssue(new GetAIssueRequest(issueId, new CancellationTokenSource()), new PresenterGetAIssue(this));
            _getAIssue.Execute();
        }
    }


    public class PresenterGetAIssue : IPresenterGetAIssueListCallback
    {
        private readonly AIssueViewModel _aTaskViewModel;

        public PresenterGetAIssue(AIssueViewModel viewModel)
        {
            _aTaskViewModel = viewModel;
        }

        public void OnError(BException errorMessage)
        {

        }

        public void OnFailure(ZResponse<GetAIssueResponse> response)
        {

        }

        public async void OnSuccessAsync(ZResponse<GetAIssueResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                PopulateData(response.Data.issue);
                //_aTaskViewModel.CanAssignUsersList.Clear();
                //_aTaskViewModel.CanRemoveUsersList.Clear();
                //if(response.Data.task.AssignedUsers != null)//if users available
                //{
                //    _aTaskViewModel.ListVisibility = Visibility.Visible;
                //    _aTaskViewModel.TextVisibility = Visibility.Collapsed;
                //    _aTaskViewModel.AssignButtonVisibility = Visibility.Visible;
                //    _aTaskViewModel.RemoveButtonVisibility = Visibility.Visible;
                //}
                //else//no users
                //{
                //    _aTaskViewModel.TextVisibility = Visibility.Visible;
                //    _aTaskViewModel.ListVisibility = Visibility.Collapsed;
                //    _aTaskViewModel.ResponseString = response.Response.ToString();
                //    _aTaskViewModel.AssignButtonVisibility = Visibility.Visible;
                //    _aTaskViewModel.RemoveButtonVisibility = Visibility.Collapsed;
                //}
                foreach (var u in response.Data.issue.UsersList)
                {
                    if (u.Value == true)
                        _aTaskViewModel.CanRemoveUsersList.Add(u.Key);
                    else
                        _aTaskViewModel.CanAssignUsersList.Add(u.Key);
                }
                if (_aTaskViewModel.CanRemoveUsersList.Count == 0)
                {
                    _aTaskViewModel.TextVisibility = Visibility.Visible;
                    _aTaskViewModel.ListVisibility = Visibility.Collapsed;
                    _aTaskViewModel.ResponseString = response.Response.ToString();
                    _aTaskViewModel.AssignButtonVisibility = Visibility.Visible;
                    _aTaskViewModel.RemoveButtonVisibility = Visibility.Collapsed;
                }
                else
                {
                    _aTaskViewModel.ListVisibility = Visibility.Visible;
                    _aTaskViewModel.TextVisibility = Visibility.Collapsed;
                    _aTaskViewModel.AssignButtonVisibility = Visibility.Visible;
                    _aTaskViewModel.RemoveButtonVisibility = Visibility.Visible;
                }
            });
        }

        private void PopulateData(IssueBO data)
        {
            _aTaskViewModel.SelectedIssue = data;
            _aTaskViewModel.AIssue.Add(data);
        }
    }


    public abstract class AIssueViewModelBase : NotifyPropertyBase
    {
        public ObservableCollection<IssueBO> AIssue = new ObservableCollection<IssueBO>();
        public abstract void GetAIssue(int issueId);

        private IssueBO _selectedIssue;
        public IssueBO SelectedIssue
        {
            get { return _selectedIssue; }
            set
            {
                _selectedIssue = value;
                OnPropertyChanged(nameof(SelectedIssue));
            }
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

        private Visibility _listVisibility = Visibility.Collapsed;
        public Visibility ListVisibility
        {
            get { return _listVisibility; }
            set
            {
                _listVisibility = value;
                OnPropertyChanged(nameof(ListVisibility));
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

        private Visibility _assignButtonVisibility = Visibility.Collapsed;
        public Visibility AssignButtonVisibility
        {
            get { return _assignButtonVisibility; }
            set
            {
                _assignButtonVisibility = value;
                OnPropertyChanged(nameof(AssignButtonVisibility));
            }
        }

        private Visibility _removeButtonVisibility = Visibility.Collapsed;
        public Visibility RemoveButtonVisibility
        {
            get { return _removeButtonVisibility; }
            set
            {
                _removeButtonVisibility = value;
                OnPropertyChanged(nameof(RemoveButtonVisibility));
            }
        }

        private ObservableCollection<User> _canAssignUsersList = new ObservableCollection<User>();
        public ObservableCollection<User> CanAssignUsersList
        {
            get { return _canAssignUsersList; }
            set
            {
                _canAssignUsersList = value;
                OnPropertyChanged(nameof(CanAssignUsersList));
            }
        }

        private ObservableCollection<User> _canRemoveUsersList = new ObservableCollection<User>();
        public ObservableCollection<User> CanRemoveUsersList
        {
            get { return _canRemoveUsersList; }
            set
            {
                _canRemoveUsersList = value;
                OnPropertyChanged(nameof(CanRemoveUsersList));
            }
        }
    }
}
