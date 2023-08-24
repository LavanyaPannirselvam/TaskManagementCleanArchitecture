using Microsoft.Data.Sqlite;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace TaskManagementLibrary.Data
{
    public sealed class DatabaseConnection
    {
        private static SQLiteConnection DbConnection;

        private DatabaseConnection() { }
        
        static DatabaseConnection()
        {
            DbConnection = new SQLiteConnection(GetPath());
        }

        public static SQLiteConnection GetDBConnection()
        {
             return DbConnection;
        }

        private static string GetPath()
        {
            string databasename = "TaskManagement.db";
            string databasePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, databasename);
            return databasePath;
        }
    }
}
