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
    public sealed partial class AdminNavigation : UserControl
    {
        public AdminNavigation()
        {
            this.InitializeComponent();
        }

        private void TopNV_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            DashboardContent.DataContext = this;
            if (args.SelectedItem == createTab)
                DashboardContent.Content = ((DataTemplate)this.Resources["CreateUserTab"]).LoadContent();
            else if (args.SelectedItem == deleteTab)
                DashboardContent.Content = ((DataTemplate)this.Resources["DeleteUserTab"]).LoadContent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TopNV.SelectedItem = createTab;
        }
    }
}
