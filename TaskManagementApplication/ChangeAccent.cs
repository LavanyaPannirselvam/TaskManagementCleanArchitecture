using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementCleanArchitecture.ViewModel;
using TaskManagementLibrary.Notifications;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace TaskManagementCleanArchitecture
{
    public static class ChangeAccent
    {
        static ChangeAccent()
        {
            UIUpdation.AccentColorChange += UIUpdation_AccentColorChange;
        }

        private static Color _appAccentColor;
        public static Color AppAccentColor
        {
            get => _appAccentColor;
            set
            {
                _appAccentColor = value;
                UIUpdation.OnAccentColorChanged(_appAccentColor);
                ApplicationData.Current.LocalSettings.Values["AppAccentColorHexStr"] = value.ToHex();
            }
        }

        private async static void UIUpdation_AccentColorChange(Color obj)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                SetRequestedAccentColor();
            });
        }

        private static void UpdateSystemAccentColor(Color color)
        {
            //Color customAccent = (Color)Application.Current.Resources["SystemAccentColor"];
            //customAccent = color;
            //Application.Current.Resources["SystemAccentColor"] = color;
            if (!new AccessibilitySettings().HighContrast)
            {
                SolidColorBrush customAccent = (SolidColorBrush)Application.Current.Resources["CustomAccentColor"];
                customAccent.Color = color;
            }
            var brushes = new string[]
            {
                "SystemControlBackgroundAccentBrush",
                "SystemControlDisabledAccentBrush",
                "SystemControlForegroundAccentBrush",
                "SystemControlHighlightAccentBrush",
                "SystemControlHighlightAltAccentBrush",
                "SystemControlHighlightAltListAccentHighBrush",
                "SystemControlHighlightAltListAccentLowBrush",
                "SystemControlHighlightAltListAccentMediumBrush",
                "SystemControlHighlightListAccentHighBrush",
                "SystemControlHighlightListAccentLowBrush",
                "SystemControlHighlightListAccentMediumBrush",
                "SystemControlHyperlinkTextBrush",
                "ContentDialogBorderThemeBrush",
                "JumpListDefaultEnabledBackground",
            };

            foreach (var brush in brushes)
            {
                try
                {
                    ((SolidColorBrush)Application.Current.Resources[brush]).Color = color;
                }
                catch { }
            }
            //try
            //{
            //    ((RevealBackgroundBrush)Application.Current.Resources["SystemControlHighlightAccent3RevealBackgroundBrush"]).Color = color;
            //}
            //catch { }
        }

        public static void SetRequestedAccentColor()
        {
            UpdateSystemAccentColor(AppAccentColor);
        }

    }
}
