﻿using Microsoft.Data.Sqlite;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Data.DBAdapter
{
    public class DBAdapter : IDBAdapter
    {
        private static SQLiteConnection connection;
        private static DatabaseConnection dbConn;

        public DBAdapter(DatabaseConnection dbConn)
        {
            if (connection == null)
            {
                var conn = dbConn;
                connection = conn.GetDbConnection();
                lock (connection)
                {
                    LibraryInitialization libraryInitialization;
                    libraryInitialization = LibraryInitialization.GetInstance();
                    libraryInitialization.InitializeDb();
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
    }
}

