using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementCleanArchitecture.ViewModel;

namespace TaskManagementCleanArchitecture
{
    public class PresenterService
    {
        public IServiceProvider Services { get; }
        //singleton so constructor called and ConfigureServices is called only once
        private PresenterService()
        {
            Services = ConfigureServices();
        }
        public static PresenterService _instance;
        public static PresenterService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PresenterService();
            }
            return _instance;
        }
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<FirstPageViewModelBase, FirstPageViewModel>();
            return services.BuildServiceProvider();
        }
    }
}
