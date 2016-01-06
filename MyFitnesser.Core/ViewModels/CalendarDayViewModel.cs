namespace MyFitnesser.Core.ViewModels {
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using System.Windows.Input;
  using Cirrious.MvvmCross.ViewModels;

  
  public class CalendarDayViewModel : MvxNotifyPropertyChanged {

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
        _Date = value; 
        RaisePropertyChanged(() => Date); 
        UpdateTrains();
      } 
    }
    private DateTime _Date;

    /// <summary>Тренировки на дату страницы</summary>
    public ObservableCollection<Train> Trains { 
      get { 
        return _Trains;
      } 
      private set { 
        _Trains = value; 
        RaisePropertyChanged(() => Trains); 
      } 
    }
    private ObservableCollection<Train> _Trains;

    private void UpdateTrains() {
      // TODO Request db
      var trains = new Train[3];
      trains[0] = new Train() { StartDate = Date.AddHours(5),  EndDate = Date.AddHours(6),  ClientName = "Date: " + Date.ToShortDateString() };
      trains[1] = new Train() { StartDate = Date.AddHours(10), EndDate = Date.AddHours(12), ClientName = "Гумнов Петр Иванович" };
      trains[2] = new Train() { StartDate = Date.AddHours(20), EndDate = Date.AddHours(21).AddMinutes(30), ClientName = "Заваляй Мовлюд" };
      Trains = new ObservableCollection<Train>(trains);
    }

    /// <summary>Тренировка-событие в календаре</summary>
    public class Train {

      public DateTime StartDate;

      public DateTime EndDate;

      public string ClientName;
    }
  }
}