using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TaskManagementCleanArchitecture
{
    public static class SwitchTheme
    {
        public static ElementTheme CurrentTheme { get; set; }
        private static Dictionary<UIContext, FrameworkElement> XamlRootCollections { get; } = new Dictionary<UIContext, FrameworkElement>();

        public static bool AddUIRootElement(FrameworkElement rootElement)
        {
            ThemeSetting();
            try
            {
                XamlRootCollections.Add(rootElement.UIContext, rootElement);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        public static bool RemoveUIRootElement(FrameworkElement rootElement)
        {
            try
            {
                XamlRootCollections.Remove(rootElement.UIContext);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        public static void ThemeSetting()
        {
            try
            {
                var check = ApplicationData.Current.LocalSettings.Values["ThemeSetting"];
                if (check == null)
                {
                    ApplicationData.Current.LocalSettings.Values["ThemeSetting"] = GetCurrentTheme();
                }
                CurrentTheme = (ElementTheme)((int)check);

            }
            catch (KeyNotFoundException)
            {
                ApplicationData.Current.LocalSettings.Values["ThemeSetting"] = (int)ElementTheme.Default;
            }
        }

        private static ElementTheme GetCurrentTheme()
        {
            if (Window.Current.Content is Frame rootFrame)
            {
                XamlRootCollections.Add(rootFrame.UIContext, rootFrame);
                return rootFrame.RequestedTheme;
            }
            return ElementTheme.Default;
        }

        public async static Task ChangeTheme(ElementTheme theme)
        {
            ApplicationData.Current.LocalSettings.Values["ThemeSetting"] = (int)theme;
            CurrentTheme = theme;
            foreach (var rootCollection in XamlRootCollections)
            {
                await rootCollection.Value.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,() =>
                {
                    rootCollection.Value.RequestedTheme = CurrentTheme;
                });
            }
        }

        public static void ChangeAccentColor(Color color)
        {
            if (!new AccessibilitySettings().HighContrast)
            {
                Application.Current.Resources["SystemAccentColor"] = color;
            }
        }
    }
}
