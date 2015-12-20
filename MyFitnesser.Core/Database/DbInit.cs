namespace MyFitnesser.Core {
  using System;
  using System.Threading.Tasks;
  using System.Linq;


  public class DbInit {

    public static void Start() {
      ClientRecord.CreateTable();
      TrainRecord.CreateTable();
    }

    public static void Stop() {
      DbConnection.Get().Close();
    }

  }
}

