namespace MyFitnesser.Core.Database {
  using System;
  using System.Linq;
  using SQLite;


  internal class Client {

    [PrimaryKey]
    public Guid Id { get; set; }

    [MaxLength(256)]
    public string Name { get; set; }

    [MaxLength(50)]
    public string Phone { get; set; }

    [MaxLength(100)]
    public string EMail { get; set; }

    public DateTime BirthDay { get; set; }

    [MaxLength(2000)]
    public string Notes { get; set; }

    public static void CreateTable() {
      DbConnection.Get().CreateTable<Client>();
    }

    public static IQueryable<Client> Records() {
      return DbConnection.Get().Table<Client>().AsQueryable();
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

