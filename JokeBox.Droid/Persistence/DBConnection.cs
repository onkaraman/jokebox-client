using System;
using JokeBox.Core.Persistence.DB;
using SQLite;

namespace JokeBox.Persistence
{
    public class DBConnection : DBConnectionCore
    {
        public DBConnection () : base(){}

        protected override void setup()
        {
            dbPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            dbPath = System.IO.Path.Combine(dbPath, dbName);

            Connection = new SQLiteConnection(dbPath);
        }
    }
}

