using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementCleanArchitecture.ViewModel;
using TaskManagementCleanArchitecture.ViewModel.TaskManagementCleanArchitecture.ViewModel;
using TaskManagementLibrary.Domain.Usecase;

namespace TaskManagementCleanArchitecture
{
    public sealed class PresenterService
    {
        public IServiceProvider Services { get; }
        private PresenterService()
        {
            Services = ConfigureServices();
        }
        public static PresenterService _instance = new PresenterService();
        
        public static PresenterService GetInstance()
        {
            return _instance;
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ProjectsViewModelBase, ProjectsViewModel>();
            services.AddSingleton<IssueViewModelBase, IssueViewModel>();
            services.AddSingleton<LoginViewModelBase,LoginViewModel>();
            services.AddSingleton<UserViewModelBase,UserViewModel>();
            services.AddSingleton<IssueDetailsViewModelBase, IssueDetailsViewModel>();
            services.AddSingleton<TasksPageViewModelBase,TaskViewModel>();
            services.AddSingleton<TaskDetailsViewModelBase, TaskDetailsViewModel>();
            services.AddSingleton<AssignedTasksPageViewModelBase, AssignedTaskPageViewModel>();
            services.AddSingleton<CreatedTasksPageViewModelBase,CreatedTasksPageViewModel>();
            services.AddSingleton<CreateUserViewModelBase, CreateUserViewModel>();
            services.AddSingleton<SettingsViewModel>();
            return services.BuildServiceProvider();
        }
    }
}
