using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TaskManagementCleanArchitecture.View.UserControls;
using TaskManagementCleanArchitecture.ViewModel;
using TaskManagementLibrary.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using controls = Microsoft.Toolkit.Uwp.UI.Controls;
using User = TaskManagementLibrary.Models;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TaskManagementCleanArchitecture
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(CurrentUserClass.CurrentUser == null)
                MainFrame.Navigate(typeof(LoginPage));
            else MainFrame.Navigate(typeof(FirstPage));
            LoginPage.OnLoginSuccess += NavigateToFirstPage;
            FirstPage.LogoutEvent += LogoutUser;
            // TasksPage.OnLoadingSuccess += NavigateToTasksPage;
            //MainFrame.Navigate(typeof(FirstPage));
        }

        private void LogoutUser()
        {
            CurrentUserClass.CurrentUser= null;
            MainFrame.Navigate(typeof(LoginPage));
        }

        private void NavigateToFirstPage(LoggedInUserBO currentUser)
        {
            var firstpage = new FirstPage();
            firstpage.CurrentUser = currentUser;
            CurrentUserClass.CurrentUser = currentUser;
            MainFrame.Navigate(firstpage.GetType(), firstpage);
        }
    }
}
//CONTROL TEMPLATE -> used to create custom template for ui elements
//RESOURCE DICTIONARY -> used when a particular customization should be used many times;definitions of some object that you expect to use more than once;
//                    -> only shareable object can be used inside 
//login VM (interface)-> login pg(interface implementation = event calling)-> Mainpage.cs (event implementation = updates and moves to new page)