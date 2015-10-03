namespace MyFitnesser.Core.Database {
  using System;
  using System.Linq;
  using SQLite;
 

  internal class DbConnection {
  
    public static SQLiteConnection Get() {
      if (_Db == null)
        _Db = new SQLiteConnection("data.db");
      return _Db;
    }

    private static SQLiteConnection _Db;
  }
}

