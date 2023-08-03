using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TaskManagementLibrary.Models;
using TaskManagementLibrary.Notifications;
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
    public sealed partial class GridViewItems : UserControl
    {
        public GridViewItems()
        {
            this.InitializeComponent();
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            var data = ((FrameworkElement)sender).DataContext as User;
            UIUpdation.OnUserSelectedToDelete(data.Email);
        }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            DeleteUserButton.Visibility = Visibility.Visible;
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            DeleteUserButton.Visibility= Visibility.Collapsed;
        }

        private void EmailTextblock_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.Content = EmailTextblock.Text.ToString();
            ToolTipService.SetToolTip(EmailTextblock, toolTip);
        }

        private void NameTextblock_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.Content = NameTextblock.Text.ToString();
            ToolTipService.SetToolTip(NameTextblock, toolTip);
        }
    }
}
