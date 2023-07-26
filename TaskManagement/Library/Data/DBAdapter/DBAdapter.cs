using Microsoft.Data.Sqlite;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data.DBAdapter;
using TaskManagementLibrary.Data;
using TaskManagementLibrary;

namespace TaskManagementLibrary.Data.DBAdapter
{
    public class DBAdapter : IDBAdapter
    {
        //private static SQLiteConnection connection = null;
        ////public DBAdapter(DatabaseConnection connection)
        //public DBAdapter(SQLiteConnection dbConn)
        //{
        //    //if (connection == null)
        //    //{
        //    //    var conn = dbConn;
        //        //connection = conn.GetDbConnection();
        //        lock (connection)
        //        {
        //            if (connection == null)
        //            {
        //                connection = dbConn;
        //                //LibraryInitialization libraryInitialization;
        //                //libraryInitialization = LibraryInitialization.GetInstance();
        //                //libraryInitialization.InitializeDb();
        //            }
        //        }
        //    //}
        //}
        private static SQLiteConnection connection = null;
        private static readonly object ConnectionLock = new object();
        public DBAdapter()
        {
            if (connection == null)
            {
                //var conn = dbConn;
                //connection = DatabaseConnection.GetDbConnection();
                lock (ConnectionLock)
                {
                    if (connection == null)
                    {
                        connection = DatabaseConnection.GetDBConnection();
                        //LibraryInitialization libraryInitialization;
                        //libraryInitialization = LibraryInitialization.GetInstance();
                        //libraryInitialization.InitializeDb();
                    }
                }
            }
        }

        public void Create<T>(T value) where T : new()
        {
            connection.CreateTable<T>();
        }

        public int Add<T>(T value) where T : new()
        {
            return connection.Insert(value);
        }

        public int Update<T>(T value) where T : new()
        {
            return connection.InsertOrReplace(value);
        }

        public int Delete<T>(T value) where T : new()
        {
            return connection.Delete(value);
        }

        public IEnumerable<T> GetList<T>() where T : new()
        {
            return connection.Table<T>();
        }

        public List<T> GetFromQuery<T>(string query, params object[] value) where T : new()
        {
            var a= connection.Query<T>(query, value);
            return a;
        }
    }
}



