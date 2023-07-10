﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
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
    public sealed partial class TaskDetailsPage : UserControl
    {
        private ATaskViewModelBase _aTaskViewModel;
        private AssignTaskToUserViewModelBase _assignTaskToUserViewModel;
        private RemoveTaskFromUserViewModelBase _removeTaskFromUserViewModel;
        public TaskBO task;
        public TaskDetailsPage()
        {
            this.InitializeComponent();
            _aTaskViewModel = PresenterService.GetInstance().Services.GetService<ATaskViewModelBase>();
            _assignTaskToUserViewModel = PresenterService.GetInstance().Services.GetService<AssignTaskToUserViewModelBase>();
            _removeTaskFromUserViewModel = PresenterService.GetInstance().Services.GetService<RemoveTaskFromUserViewModelBase>();
            //task = _aTaskViewModel.ATask.FirstOrDefault();
            //if (task!=null && task.AssignedUsers.Count!= 0)
            //{
            //    TextVisibility = Visibility.Collapsed;
            //    ListVisibility = Visibility.Visible;
            //}
            
        }

        private void assignButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListView).SelectedItem is User selected)
            {
                // _assignTaskToUserViewModel.AssignUserToTask(selected.UserId, _aTaskViewModel.SelectedTask.Tasks.Id);
                _removeTaskFromUserViewModel.RemoveTask(selected.UserId, _aTaskViewModel.SelectedTask.Tasks.Id);
                //NotificationGrid.Visibility = Visibility.Visible;
                //string Content = _removeTaskFromUserViewModel.ResponseString;
                NotificationControl.Show(1000);
            }
        }

        //private void UserControl_Loaded(object sender, RoutedEventArgs e)
        //{
        //    TaskBO taskBO = 
        //    if (taskBO != null)
        //        SelectedTask = taskBO;
        //    if (taskBO!=null && taskBO.AssignedUsers.Count != 0)
        //    {
        //        TextVisibility = Visibility.Collapsed;
        //        ListVisibility = Visibility.Visible;
        //    }
        //}

        //private void Tasks_DataChanged(FrameworkElement element,DataContextChangedEventArgs args)
        //{
        //    if(this.DataContext != null) 
        //    {
        //        this.task = (TaskBO)this.DataContext;
        //        Bindings.Update();
        //    }
        //}

        //private void Grid_ContextRequested(UIElement sender, ContextRequestedEventArgs args)
        //{
        //    TaskBO taskBO = _aTaskViewModel.ATask.FirstOrDefault();
        //    if (taskBO != null)
        //        SelectedTask = taskBO;
        //    if (taskBO != null && taskBO.AssignedUsers.Count != 0)
        //    {
        //        TextVisibility = Visibility.Collapsed;
        //        ListVisibility = Visibility.Visible;
        //    }
        //}




    }
}
