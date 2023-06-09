using Microsoft.Data.Sqlite;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Data
{
    public class DatabaseConnection
    {
        public static string DbPath;
        private static SQLiteConnection DbConnection;
        DatabasePath dbPathConnection;
        public DatabaseConnection(DatabasePath _dbpath)
        {
            dbPathConnection = _dbpath;
            EstablishConnection();
        }
        public void EstablishConnection()
        {
            DbPath = dbPathConnection.GetPath();
            DbConnection = new SQLiteConnection(DbPath);
        }

        public SQLiteConnection GetDbConnection()
        {
            return DbConnection;
        }
    }
}
