using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data;

namespace TaskManagementLibrary
{
    public sealed class LibraryInitialization
    {
        private static LibraryInitialization _instance ;
        
        private LibraryInitialization() { }

        static LibraryInitialization ()
        {
            _instance = new LibraryInitialization();
        }

        public static LibraryInitialization GetInstance()
        {
            return _instance;
        }

        public void InitializeDb()
        {
            CreateTables createTables = Domain.ServiceProvider.GetInstance().Services.GetService<CreateTables>();
        }
    }


   
}
