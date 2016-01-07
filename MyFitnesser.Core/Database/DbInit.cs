namespace MyFitnesser.Core.Database {
  using System;
  using System.Threading.Tasks;
  using System.Linq;


  public class DbInit {

    public static void Start() {
      var db = DbConnection.Get();
      db.CreateTable<DbVersionRecord>();
      var res = DbVersionRecord.Records().FirstOrDefault();
      if (res == null) {
        res = new DbVersionRecord() { Version = 0 };
        res.Write();
      }

      if (res.Version == _RequiredDbVersion)
        return;

      db.BeginTransaction();
      try {
        if (res.Version < 1) {
          ClientRecord.CreateTable();
          TrainRecord.CreateTable();
        }
        res.Version = _RequiredDbVersion;
        res.Write();
        db.Commit();
      }
      finally {
        db.Rollback();
      }
    }

    public static void Stop() {
      DbConnection.Get().Close();
    }

    private const int _RequiredDbVersion = 1;
  }
}

