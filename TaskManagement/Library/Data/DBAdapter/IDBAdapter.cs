using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Data.DBAdapter
{
    public interface IDBAdapter
    {
        void Create<T>(T value) where T : new();
        int Add<T>(T value) where T : new();
        int Delete<T>(T value) where T : new();
        int Update<T>(T value) where T : new();
        IEnumerable<T> GetList<T>() where T:new();
        List<T> GetFromQuery<T>(string query,params object[] values) where T:new();
    }
}
