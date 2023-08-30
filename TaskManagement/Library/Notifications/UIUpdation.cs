using Microsoft.Toolkit.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Enums;
using TaskManagementLibrary.Models;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TaskManagementLibrary.Notifications
{
    public static class UIUpdation
    {
        public static event Action<LoggedInUserBO> UserLoggedIn;
        public static void OnUserLogin(LoggedInUserBO userLoggedIn)
        {
            UserLoggedIn?.Invoke(userLoggedIn);
        }

        public static event Action UserLogout;
        public static void OnUserLogout()
        {
            UserLogout?.Invoke();
        }
        public static event Action<Project> ProjectCreated;
        public static void OnProjectCreated(Project project)
        {
            ProjectCreated?.Invoke(project);
        }
                
        public static event Action<ObservableCollection<UserBO>> UserAdded;
        public static void UserAddedUpdate(ObservableCollection<UserBO> taskBO)
        {
            UserAdded?.Invoke(taskBO);
        }

        public static event Action<ObservableCollection<UserBO>> UserRemoved;
        public static void UserRemovedUpdate(ObservableCollection<UserBO> taskBO)
        {
            UserRemoved?.Invoke(taskBO);
        }

        public static event Action<Tasks> TaskCreated;
        public static void OnTaskCreated(Tasks task)
        {
            TaskCreated?.Invoke(task);
        }

        public static event Action<Tasks> TaskDeleted;
        public static void OnTaskDeleted(Tasks task)
        {
            TaskDeleted?.Invoke(task);
        }
        public static event Action<Tasks> TaskUpdated;
        public static void OnTaskUpdate(Tasks task)
        {
            TaskUpdated.Invoke(task);
        }

        public static event Action<Issue> IssueCreated;
        public static void OnIssueCreated(Issue issue)
        {
            IssueCreated?.Invoke(issue);
        }

        public static event Action<Issue> IssueUpdated;
        public static void OnIssueUpdate(Issue issue)
        {
            IssueUpdated.Invoke(issue);
        }

        public static event Action<Issue> IssueDeleted;
        public static void OnIssueDeleted(Issue issue)
        {
            IssueDeleted?.Invoke(issue);
        }

        public static event Action PageNavigated;
        public static void OnBackNavigated()
        {
            PageNavigated?.Invoke();
        }

        public static event Action<User> NewUserCreated;
        public static void OnNewUserCreated(User newUser)
        {
            NewUserCreated?.Invoke(newUser);
        }

        public static event Action<User> UserDeleted;
        public static void OnUserDeleted(User user)
        {
            UserDeleted?.Invoke(user);
        }

        public static event Action<UserBO> UserSelectedToRemove;
        public static void OnUserSelectedRemove(UserBO user)
        {
            UserSelectedToRemove?.Invoke(user);
        }

        public static event Action<string> UserSelectedToDelete;
        public static void OnUserSelectedToDelete(string user)
        {
            UserSelectedToDelete?.Invoke(user);
        }

        public static event Action AccentColorChange;
        public static void OnAccentColorChanged()
        {
            AccentColorChange?.Invoke();
        }

        public static event Action ThemeSwitched;
        public static void OnThemeSwitched()
        {
            ThemeSwitched?.Invoke();
        }

    }
}
