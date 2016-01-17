namespace MyFitnesser.Core.ViewModels {
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using System.Windows.Input;

  using Cirrious.MvvmCross.ViewModels;

  using MyFitnesser.Core.Utils;


  public class CalendarDayViewModel : MvxNavigatingObject {

    public CalendarDayViewModel() {
      Date = DateTime.Now.Date;      
      UpdateTrains();
    }

    public CalendarDayViewModel(DateTime date) {
      Date = date.Date;      
      UpdateTrains();
    }

    /// <summary>Дата данной страницы</summary>
    public DateTime Date { 
      get { 
        return _Date;
      } 
      private set { 
        _Date = value.Date; 
        RaisePropertyChanged(() => Date); 
        UpdateTrains();
      } 
    }
    private DateTime _Date;

    /// <summary>Тренировки на дату страницы</summary>
    public SuspendableObservableCollection<Train> Trains { 
      get { 
        return _Trains;
      } 
      private set { 
        _Trains = value; 
        RaisePropertyChanged(() => Trains); 
      } 
    }
    private SuspendableObservableCollection<Train> _Trains;

    private void UpdateTrains() {
      Trains = new SuspendableObservableCollection<Train>();

      var trains  = Database.TrainRecord.Records().Where(_ => _.StartDate >= Date && _.StartDate <= Date.AddDays(1));
      var clients = Database.ClientRecord.Records().ToDictionary(_ => _.Id, _ => _);
      foreach (var train in trains)
        Trains.Add(new Train() { StartDate = train.StartDate, EndDate = train.EndDate, ClientName = clients[train.ClientId].Name });
    }

    public IMvxCommand EditTrainCommand {
      get { return new MvxCommand<Guid>(trainId => EditTrain(trainId)); }
    }

    private void EditTrain(Guid trainId) {
      var parameters = new TrainViewModel.ViewParameters();
      parameters.TrainId = trainId;
      ShowViewModel<TrainViewModel>(parameters);
    }

    public IMvxCommand DeleteTrainCommand {
      get { return new MvxCommand<Guid>(trainId => DeleteTrain(trainId)); }
    }

    private void DeleteTrain(Guid trainId) {
      // TODO
    }

    /// <summary>Тренировка-событие в календаре</summary>
    public class Train {

      public Guid Id { get; set; }
      public DateTime StartDate { get; set; }
      public DateTime EndDate { get; set; }
      public string ClientName { get; set; }
    }
  }
}