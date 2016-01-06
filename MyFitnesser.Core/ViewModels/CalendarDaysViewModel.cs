namespace MyFitnesser.Core.ViewModels {
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using System.Windows.Input;

  using Cirrious.MvvmCross.ViewModels;

  using MyFitnesser.Core.Utils;


  public class CalendarDaysViewModel : MvxViewModel {

    public CalendarDaysViewModel() : base() {
      CurrentDate = DateTime.Now.Date;
      var days = new SuspendableObservableCollection<CalendarDayViewModel>();
      for (int i = 0; i < ViewPagerCapacity; i++)
        days.Add(null);
      var middleIndex = ViewPagerCapacity / 2;
      days.Suspend();
      try {
        days[middleIndex - 1] = new CalendarDayViewModel(CurrentDate.AddDays(-1));
        days[middleIndex]     = new CalendarDayViewModel(CurrentDate);
        days[middleIndex + 1] = new CalendarDayViewModel(CurrentDate.AddDays(+1));
      }
      finally {
        days.Resume();
      }
      Days = days;
    }

    public SuspendableObservableCollection<CalendarDayViewModel> Days {
      get { return _Days; }
      set
      {
        _Days = value;
        RaisePropertyChanged(() => Days);
      }
    }
    private SuspendableObservableCollection<CalendarDayViewModel> _Days;

    public ICommand ItemPageChangedCommand {
      get { return new MvxCommand<CalendarDayViewModel>(ItemPageChanged); }
    }

    private void ItemPageChanged(CalendarDayViewModel toPage) {
      CurrentDate = toPage.Date;
      var index = Days.IndexOf(toPage);
      if (index == 0 || index == ViewPagerCapacity)
        return; // OMG!
      Days.Suspend();
      try {
        if (Days[index - 1] == null)
          Days[index - 1] = new CalendarDayViewModel(toPage.Date.AddDays(-1));
        if (Days[index + 1] == null)
          Days[index + 1] = new CalendarDayViewModel(toPage.Date.AddDays(+1));
      }
      finally {
        Days.Resume();
      }
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

    public readonly int ViewPagerCapacity = 400;
  }
}

