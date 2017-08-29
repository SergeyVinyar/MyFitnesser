namespace MyFitnesser.Core.Database {
  using System;
  using SQLite;


  public class DbVersionRecord : RecordBase<DbVersionRecord> {

    public int Version { get; set; }
  }

}

