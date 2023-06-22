using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        public FirstPageViewModelBase _firstPageViewModel;
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(LoginPage));
            LoginPage.OnLoginSuccess += NavigateToFirstPage;
        }

        private void NavigateToFirstPage(TaskManagementLibrary.Models.LoggedInUserBO currentUser)
        {
            //var firstPage = new FirstPage
            //{
            //    CurrentUser = currentUser
            //};
            //MainFrame.Navigate(firstPage.GetType(), firstPage);
             MainFrame.Navigate(typeof(FirstPage),currentUser);

        }


    }
}
//CONTROL TEMPLATE -> used to create custom template for ui elements
//RESOURCE DICTIONARY -> used when a particular customization should be used many times;definitions of some object that you expect to use more than once;
//                    -> only shareable object can be used inside 
