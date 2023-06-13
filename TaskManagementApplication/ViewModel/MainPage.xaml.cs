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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using controls = Microsoft.Toolkit.Uwp.UI.Controls;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TaskManagementCleanArchitecture
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public FirstPageViewModelBase _firstPageViewModel;
        private int projectId;
        public MainPage()
        {
            this.InitializeComponent();
            _firstPageViewModel = PresenterService.GetInstance().Services.GetService<FirstPageViewModelBase>();
            _firstPageViewModel.GetProjectsList();
            _firstPageViewModel.GetUsersList(projectId);
        }

        private void ProjectListDetailsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = sender as ListDetailsView;
            //projectId = e.AddedItems;   
        }
    }
}
