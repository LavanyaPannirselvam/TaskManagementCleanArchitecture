using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public static class SwitchToMainUIThread
    {
        public static async Task SwitchToMainThread(Action action)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
            Windows.UI.Core.CoreDispatcherPriority.Normal, () => action());
        }
    }
}
