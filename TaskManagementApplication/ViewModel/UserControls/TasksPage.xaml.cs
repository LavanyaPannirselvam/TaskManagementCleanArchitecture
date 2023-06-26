using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using TaskManagementCleanArchitecture.ViewModel;
using TaskManagementLibrary.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace TaskManagementCleanArchitecture.View.UserControls
{
    public sealed partial class TasksPage : UserControl
    {
        public UserViewModelBase _usersViewModel;
        public static event Action<ObservableCollection<User>> OnLoadingSuccess;
        //public event PropertyChangedEventHandler PropertyChanged;

        public TasksPage()
        {
            this.InitializeComponent();
            _usersViewModel = PresenterService.GetInstance().Services.GetService<UserViewModelBase>();
            //_usersViewModel.LoadUsers = this;
        }
        
        public static readonly DependencyProperty TaskProperty = DependencyProperty.Register("Tasks",typeof(List<Tasks>),typeof(TasksPage),new PropertyMetadata(null));

        public ObservableCollection<Tasks> Tasks
        {
            get { return (ObservableCollection<Tasks>)GetValue(TaskProperty); }
            set
            {
                SetValue(TaskProperty, value);
            }
        }
        
        //public void LoadUsers(ObservableCollection<Tasks> usersList)
        //{
        //    OnLoadingSuccess.Invoke(usersList);
        //}

        //protected override void OnTo(NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);
        //    if (e.Parameter is List<User> pg)
        //    {
        //        Users = pg;
        //    }
        //}

        //private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
