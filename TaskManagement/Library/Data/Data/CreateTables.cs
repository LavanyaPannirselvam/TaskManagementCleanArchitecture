using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data;
using TaskManagementLibrary.Data.DBAdapter;
using TaskManagementLibrary.Models;

namespace TaskManagementLibrary.Data
{
    public class CreateTables
    {
        private readonly IDBAdapter _adapter;

        public CreateTables()
        {
            _adapter = Domain.ServiceProvider.GetInstance().Services.GetService<IDBAdapter>();
            InstantiateAllTables();
        }

        public void InstantiateAllTables()
        {
            _adapter.Create(new Project());
            _adapter.Create(new User());
            _adapter.Create(new Tasks());
            _adapter.Create(new Issue());
            _adapter.Create(new Assignment());
            _adapter.Create(new User());
            _adapter.Create(new UserCredential());
        }
    }
}
