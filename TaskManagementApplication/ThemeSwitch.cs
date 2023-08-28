using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Notifications;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace TaskManagementCleanArchitecture
{
    public static class ThemeSwitch
    {
        static ThemeSwitch()
        {
            UIUpdation.ThemeSwitched += UIUpdation_ThemeSwitched;
        }

        public static void UIUpdation_ThemeSwitched()
        {
            ChangeTheme();
        }

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
                var check = ApplicationData.Current.LocalSettings.Values["themeSetting"];
                if (check == null)
                {
                    ApplicationData.Current.LocalSettings.Values["themeSetting"] = GetCurrentTheme();
                }
                CurrentTheme = (ElementTheme)(int)check;

            }
            catch (KeyNotFoundException)
            {
                ApplicationData.Current.LocalSettings.Values["themeSetting"] = (int)ElementTheme.Default;
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

        public async static void ChangeTheme()
        {
            ElementTheme theme = CurrentTheme;
            ApplicationData.Current.LocalSettings.Values["themeSetting"] = (int)theme;
            foreach (var rootCollection in XamlRootCollections)
            {
                await rootCollection.Value.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,() =>
                {
                    rootCollection.Value.RequestedTheme = CurrentTheme;
                });
            }
        }
    }
}
