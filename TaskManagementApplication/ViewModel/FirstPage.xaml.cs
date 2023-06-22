using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
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
using static TaskManagementLibrary.Models.User;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TaskManagementCleanArchitecture
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FirstPage : Page /*, INotifyPropertyChanged*/
    {
        public FirstPageViewModelBase _firstPageViewModel;
        private int projectId = 0;

        //public static readonly DependencyProperty UserProperty = DependencyProperty.Register("CurrentUser", typeof(User), typeof(FirstPage), new PropertyMetadata(null));

        //public User CurrentUser
        //{
        //    get { return (User)GetValue(UserProperty); }
        //    set { SetValue(UserProperty, value); }
        //}

        private LoggedInUserBO _currentUser;
        public LoggedInUserBO CUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                NotifyPropertyChanged(nameof(CUser));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public FirstPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            _firstPageViewModel = PresenterService.GetInstance().Services.GetService<FirstPageViewModelBase>();
            _firstPageViewModel.GetProjectsList();
        }

        private void ProjectListDetailsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Project project = (sender as ListDetailsView).SelectedItem as Project;
            projectId = project.Id;
            _firstPageViewModel.GetUsersList(projectId);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is LoggedInUserBO pg)
            {
                CUser = pg;
            }
        }

        
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
