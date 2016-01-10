namespace MyFitnesser.Core.Database {
  using System;
  using System.Threading.Tasks;
  using System.Linq;
  using SQLite;


  public class RecordBase<T> where T: new() {
    
    [PrimaryKey, Unique]
    public Guid Id { get; set; }

    public static void CreateTable() {
      DbConnection.Get().CreateTable<T>();
    }

    public static IQueryable<T> Records() {
      return DbConnection.Get().Table<T>().AsQueryable();
    }

    public void Write() {
      bool isNew = (Id == Guid.Empty);
      if (isNew) {
        Id = Guid.NewGuid();
        DbConnection.Get().Insert(this);
      }
      else {
        DbConnection.Get().Update(this);
      }
    }

  }
}

