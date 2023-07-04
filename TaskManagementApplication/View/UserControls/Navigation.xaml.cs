using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class Navigation : UserControl
    {
        public Navigation()
        {
            this.InitializeComponent();
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            DashboardContent.DataContext = this;
            if (args.SelectedItem == tasksTab)
                DashboardContent.Content = ((DataTemplate)this.Resources["TasksTab"]).LoadContent();
            else if (args.SelectedItem == issuesTab)
                DashboardContent.Content = ((DataTemplate)this.Resources["IssuesTab"]).LoadContent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TopNV.SelectedItem = tasksTab;
        }
    }
}
