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
    public class DatabaseConnection
    {
       // public static string DbPath;
        private static SQLiteConnection DbConnection;
        //DatabasePath dbPathConnection;
        
        //public DatabaseConnection(DatabasePath _dbpath)
        //{
        //    dbPathConnection = _dbpath;
        //    EstablishConnection();
        //}

        private DatabaseConnection()
        {
            //EstablishConnection();
        }

        //private void EstablishConnection()
        //{
        //    DbConnection = new SQLiteConnection(GetPath());
        //}

        private static readonly object Instancelock = new object();
        public static SQLiteConnection GetDBConnection()
        {
            lock (Instancelock)
            {
                if (DbConnection == null)
                {
                    DbConnection = new SQLiteConnection(GetPath());
                }
            }
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
