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
            services.AddSingleton<IAssignProjectToUserDataManager, AssignProjectToUserDataManager>();
            services.AddSingleton<ICreateProjectDataManager, CreateProjectDataManager>();
            services.AddSingleton<IDeleteProjectDataManager, DeleteProjectDataManager>();
            services.AddSingleton<IRemoveProjectFromUserDataManager, RemoveProjectFromUserDataManager>();
            services.AddSingleton<ICreateUserAccountDataManager, CreateUserAccountDataManager>();
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
