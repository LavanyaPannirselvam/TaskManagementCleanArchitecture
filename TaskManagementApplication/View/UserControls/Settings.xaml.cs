using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TaskManagementCleanArchitecture.Converter;
using TaskManagementCleanArchitecture.ViewModel;
using TaskManagementLibrary.Enums;
using TaskManagementLibrary.Notifications;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TaskManagementCleanArchitecture.View.UserControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        SettingsViewModel viewModel;
        public Settings()
        {
            this.InitializeComponent();
            viewModel= PresenterService.GetInstance().Services.GetService<SettingsViewModel>();
            var item = viewModel.Languages.FirstOrDefault().NativeName;
        }

        private void AccentButton_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush selectedBrush = ((FrameworkElement)sender).DataContext as SolidColorBrush;
            var color = selectedBrush.Color;
            ChangeAccent.AppAccentColor = color;
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SolidColorBrush selectedBrush = (sender as Grid).DataContext as SolidColorBrush;
            var color = selectedBrush.Color;
            ChangeAccent.AppAccentColor = color;
        }

        private void LanguageRButton_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadioButton item = (RadioButton)e.AddedItems[0];
            ApplicationLanguages.PrimaryLanguageOverride = item.Tag.ToString();
            e.AddedItems.Clear();
        }
    }
}
