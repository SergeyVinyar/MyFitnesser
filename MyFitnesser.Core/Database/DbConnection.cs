namespace MyFitnesser.Core.Database {
  using System;
  using System.Linq;
  using SQLite;
 

  internal class DbConnection {
  
    public static SQLiteConnection Get() {
      if (_Db == null) {
        string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        _Db = new SQLiteConnection(System.IO.Path.Combine(folder, "data.db"), true);
      }
      return _Db;
    }

    private static SQLiteConnection _Db;
  }
}

