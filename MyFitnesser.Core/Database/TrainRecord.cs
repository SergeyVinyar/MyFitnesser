namespace MyFitnesser.Core {
  using System;
  using SQLite;


  internal class TrainRecord : RecordBase<TrainRecord> {

    public int ClientId { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan Duration { get; set; }

    public int Status { get; set; }

    [MaxLength(2000)]
    public string Notes { get; set; }

    public enum TrainStatuses { NotCompleted, Completed, Skipped }
  }
}
