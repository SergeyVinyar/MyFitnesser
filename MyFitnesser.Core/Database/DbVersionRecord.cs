namespace MyFitnesser.Core.Database {
  using System;
  using SQLite;


  internal class DbVersionRecord : RecordBase<DbVersionRecord> {

    public int Version { get; set; }
  }

}

