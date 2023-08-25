using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementCleanArchitecture.Converter;
using TaskManagementCleanArchitecture.View.UserControls;
using TaskManagementCleanArchitecture.ViewModel;
using TaskManagementLibrary.Notifications;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Color = Windows.UI.Color;

namespace TaskManagementCleanArchitecture
{
    public static class AccentChange
    {
        static AccentChange()
        {
            UIUpdation.AccentColorChange += UIUpdation_AccentColorChange;
        }

        private static Color _appAccentColor;//= rgba;
        public static Color AppAccentColor
        {
            get => _appAccentColor;
            set
            {
                _appAccentColor = value;
                ApplicationData.Current.LocalSettings.Values["accentColor"] = AppAccentColor.ToString();
                UIUpdation.OnAccentColorChanged();
            }
        }

        public async static void UIUpdation_AccentColorChange()
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                UpdateSystemAccentColor(AppAccentColor);
            });
        }

        public static void UpdateSystemAccentColor(Color color)
        {
            if (!new AccessibilitySettings().HighContrast)
            {
                if (ThemeSwitch.CurrentTheme == ElementTheme.Light)
                {
                    UpdateAccentBasedOnTheme(ElementTheme.Light);
                }

                else if (ThemeSwitch.CurrentTheme == ElementTheme.Dark)
                {
                    UpdateAccentBasedOnTheme(ElementTheme.Dark);
                }
            }
        }

        public static void SetAccentColor()
        {
            object c = ApplicationData.Current.LocalSettings.Values["accentColor"];
            if(c == null)
            {
                var uiSettings = new UISettings();
                var rgba = uiSettings.GetColorValue(UIColorType.Accent);
                ApplicationData.Current.LocalSettings.Values["accentColor"] = rgba.ToString();
                AppAccentColor = rgba;
            }
            else
            {
                SolidColorBrush solidColor = new SolidColorBrush();
                solidColor = ColorsHelper.GetSolidColorBrush(c.ToString());
                AppAccentColor = solidColor.Color;
            }
            SettingsPage.CurrentAccentColor = new SolidColorBrush(AppAccentColor);
        }

        public static void UpdateAccentBasedOnTheme(ElementTheme currentTheme)
        {
            if (currentTheme == ElementTheme.Light)
            {
                SolidColorBrush customAccent = (SolidColorBrush)Application.Current.Resources["CustomAccentColor"];
                customAccent.Color = AppAccentColor;
            }

            else if(currentTheme == ElementTheme.Dark)
            {
                SolidColorBrush customAccent = (SolidColorBrush)Application.Current.Resources["CustomAccentColor"];
                if (AppAccentColor == ColorsHelper.GetSolidColorBrush("#FFBA4273").Color)
                {
                    customAccent.Color = ColorsHelper.GetSolidColorBrush("#AABA4273").Color;
                }
                else if (AppAccentColor == ColorsHelper.GetSolidColorBrush("FF6A3F73").Color)
                {
                    customAccent.Color = ColorsHelper.GetSolidColorBrush("AA6A3F73").Color;
                }
                else if (AppAccentColor == ColorsHelper.GetSolidColorBrush("FF0078D4").Color)
                {
                    customAccent.Color = ColorsHelper.GetSolidColorBrush("AA0078D4").Color;
                }
                else if (AppAccentColor == ColorsHelper.GetSolidColorBrush("FF3666CD").Color)
                {
                    customAccent.Color = ColorsHelper.GetSolidColorBrush("AA3666CD").Color;
                }
                else if (AppAccentColor == ColorsHelper.GetSolidColorBrush("FF6E6FD8").Color)
                {
                    customAccent.Color = ColorsHelper.GetSolidColorBrush("AA6E6FD8").Color;
                }
                else if (AppAccentColor == ColorsHelper.GetSolidColorBrush("FF488FA5").Color)
                {
                    customAccent.Color = ColorsHelper.GetSolidColorBrush("AA488FA5").Color;
                }
            }
        }
    }
}
