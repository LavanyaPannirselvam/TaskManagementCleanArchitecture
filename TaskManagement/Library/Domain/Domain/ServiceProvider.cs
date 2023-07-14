using Microsoft.Extensions.DependencyInjection;
using System;
using TaskManagementLibrary.Data.DBAdapter;
using TaskManagementLibrary.Data.DBHandler;
using TaskManagementLibrary.Domain.Usecase;
using TaskManagementLibrary.Data.DBManager;
using TaskManagementLibrary.Data;

namespace TaskManagementLibrary.Domain
{
    public class ServiceProvider
    {
        public IServiceProvider Services { get; }
        private ServiceProvider()
        {
            Services = ConfigureServices();
        }

        private static ServiceProvider _instance;

        public static ServiceProvider GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ServiceProvider();
            }
            return _instance;
        }

        private static IServiceProvider ConfigureServices()//TODO
        {
            var services = new ServiceCollection();
            services.AddSingleton<IChangeProjectPriorityDataManager, ChangeProjectPriorityDataManager>();
            services.AddSingleton<IChangeProjectStatusDataManager,ChangeProjectStatusDataManager>();
            services.AddSingleton<ILoginDataManager, LoginDataManager>();
            services.AddSingleton<IAssignTaskToUserDataManager, AssignTaskToUserDataManager>();
            services.AddSingleton<IAssignIssueToUserDataManager, AssignIssueToUserDataManager>();
            services.AddSingleton<ICreateProjectDataManager, CreateProjectDataManager>();
            services.AddSingleton<ICreateTaskDataManager, CreateTaskDataManager>();
            services.AddSingleton<ICreateIssueDataManager, CreateIssueDataManager>();
            services.AddSingleton<IDeleteTaskDataManager, DeleteTaskDataManager>();
            services.AddSingleton<IDeleteIssueDataManager, DeleteIssueDataManager>();
            services.AddSingleton<IRemoveTaskFromUserDataManager, RemoveTaskFromUserDataManager>();
            services.AddSingleton<IRemoveIssueFromUserDataManager, RemoveIssueFromUserDataManager>();
            services.AddSingleton<ICreateUserAccountDataManager, CreateUserAccountDataManager>();
            services.AddSingleton<IGetProjectsListDataManager, GetProjectsListDataManager>();
            services.AddSingleton<IGetUsersListDataManager, GetUsersListDataManager>();
            services.AddSingleton<IGetIssuesListDataManager, GetIssuesListDataManager>();
            services.AddSingleton<IGetAIssueDataManager, GetAIssueDataManager>();
            services.AddSingleton<IGetATaskDataManager, GetATaskDataManager>();
            services.AddSingleton<IGetTasksListDataManager, GetTaskListDataManager>();
            services.AddSingleton<IDBHandler, DBHandler>();
            services.AddSingleton<DatabasePath>();
            services.AddSingleton<DatabaseConnection>();
            services.AddSingleton<CreateTables>();
            services.AddSingleton<DBHandler>();
            services.AddSingleton<IDBAdapter, DBAdapter>();

            return services.BuildServiceProvider();
        }
    }
}
