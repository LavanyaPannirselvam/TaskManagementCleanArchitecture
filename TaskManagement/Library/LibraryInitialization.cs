using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data;

namespace TaskManagementLibrary
{
    public class LibraryInitialization
    {
        CreateTables createTableInstance;
        private static LibraryInitialization _instance;

        private LibraryInitialization() { }

        public static LibraryInitialization GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LibraryInitialization();
            }
            return _instance;
        }
        public void InitializeDb()
        {
            CreateTables createTables = Domain.ServiceProvider.GetInstance().Services.GetService<CreateTables>();
            if (createTableInstance == null)
            {
                createTableInstance = createTables;
            }
        }
    }
}
