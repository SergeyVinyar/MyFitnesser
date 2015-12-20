namespace MyFitnesser.Core {
  using System;
  using SQLite;


  internal class ClientRecord : RecordBase<ClientRecord> {

    [MaxLength(256)]
    public string Name { get; set; }

    [MaxLength(50)]
    public string Phone { get; set; }

    [MaxLength(100)]
    public string EMail { get; set; }

    public DateTime BirthDay { get; set; }

    [MaxLength(2000)]
    public string Notes { get; set; }
  }

}

