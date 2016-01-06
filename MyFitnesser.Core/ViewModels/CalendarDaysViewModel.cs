using System.Windows.Input;

namespace MyFitnesser.Core.ViewModels {
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;

  using Cirrious.MvvmCross.ViewModels;


  public class CalendarDaysViewModel : MvxViewModel {

    public CalendarDaysViewModel() : base() {
      CurrentDate = DateTime.Now.Date;
      var days = new ObservableCollection<CalendarDayViewModel>();
      days.Add(new CalendarDayViewModel(CurrentDate.AddDays(-1)));
      days.Add(new CalendarDayViewModel(CurrentDate));
      days.Add(new CalendarDayViewModel(CurrentDate.AddDays(+1)));
      Days = days;
    }

    public ObservableCollection<CalendarDayViewModel> Days {
      get { return _Days; }
      set
      {
        _Days = value;
        RaisePropertyChanged(() => Days);
      }
    }
    private ObservableCollection<CalendarDayViewModel> _Days;

    public ICommand ItemPageChangedCommand {
      get { return new MvxCommand<CalendarDayViewModel>(ItemPageChanged); }
    }

    private void ItemPageChanged(CalendarDayViewModel toPage) {
      toPage


      CurrentDate = toPage.Date;



//      if (toPage == Days.Last())
//        Days.Add(new CalendarDayViewModel(toPage.Date.AddDays(+1)));
//      else if (toPage == Days.First()) {
//        var days = new ObservableCollection<CalendarDayViewModel>();
//        days.Add(new CalendarDayViewModel(toPage.Date.AddDays(-1)));
//        Days = new ObservableCollection<CalendarDayViewModel>(days.Concat(Days));
//      }
    }

    public void ShowYear() {
      var parameters = new Dictionary<string, string>();
      parameters.Add("CurrentDate", CurrentDate.ToShortDateString());
      ShowViewModel<CalendarYearsViewModel>(parameters);
    }

    /// <summary>Текущая дата на календаре</summary>
    public DateTime CurrentDate { 
      get { 
        return _CurrentDate;
      } 
      private set { 
        _CurrentDate = value; 
        RaisePropertyChanged(() => CurrentDate); 
      } 
    }
    private DateTime _CurrentDate;


  }
}

