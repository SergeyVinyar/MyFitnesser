namespace MyFitnesser.Core.Database {
  using System;
  using System.Linq;


  public class DbInit {

    public static void CreateTables() {
      Client.CreateTable();

    }
  }
}

