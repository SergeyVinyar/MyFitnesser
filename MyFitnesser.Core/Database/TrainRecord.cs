namespace MyFitnesser.Core.Database {
  using System;
  using SQLite;


  public class TrainRecord : RecordBase<TrainRecord> {

    public Guid ClientId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public TrainStatuses Status { get; set; }

    [MaxLength(2000)]
    public string Notes { get; set; }

    public enum TrainStatuses { NotCompleted, Completed, Skipped }
  }
}
