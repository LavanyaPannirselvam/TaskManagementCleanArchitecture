using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.UI.Xaml.Controls;
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

namespace TaskManagementCleanArchitecture.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        SettingsViewModel viewModel;
        public static SolidColorBrush CurrentAccentColor;
  
        public SettingsPage()
        {
            this.InitializeComponent();
            viewModel = PresenterService.GetInstance().Services.GetService<SettingsViewModel>();
        }

        private void LanguageRButton_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var c = sender as ComboBox;
            var item = c.SelectedItem as Language;
            LanguageRButton.SelectedIndex = c.SelectedIndex;
            ApplicationLanguages.PrimaryLanguageOverride = item.LanguageTag.ToString();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AccentColorList.SelectedItem = viewModel.AccentColors.Where(c => c.Color.ToHex().Equals(CurrentAccentColor.Color.ToHex())).FirstOrDefault();
            var item = ApplicationLanguages.PrimaryLanguageOverride;
            var select = viewModel.Languages.Where(l => l.LanguageTag.Equals(item)).FirstOrDefault();//.DisplayName;
            LanguageRButton.SelectedItem = select;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        private void AccentColorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var b = sender as GridView;
            SolidColorBrush color = b.SelectedItem as SolidColorBrush;
            AccentChange.AppAccentColor = color.Color;
        }

    }
}
