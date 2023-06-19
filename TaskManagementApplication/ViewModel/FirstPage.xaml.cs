﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TaskManagementCleanArchitecture
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FirstPage : Page
    {
        public FirstPageViewModelBase _firstPageViewModel;
        private int projectId = 0;
        public FirstPage()
        {
            this.InitializeComponent();
            _firstPageViewModel = PresenterService.GetInstance().Services.GetService<FirstPageViewModelBase>();
            _firstPageViewModel.GetProjectsList();
        }

        private void ProjectListDetailsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Project project = (sender as ListDetailsView).SelectedItem as Project;
            projectId = project.Id;
            _firstPageViewModel.GetUsersList(projectId);
        }
    }
}
