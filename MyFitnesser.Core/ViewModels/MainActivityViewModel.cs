namespace MyFitnesser.Core.ViewModels {
  using System;
  using Cirrious.MvvmCross.ViewModels;


  public class MainActivityViewModel: MvxViewModel {

    public void InitViews() {
      ShowViewModel<MenuViewModel>();
      ShowViewModel<CalendarDaysViewModel>();
      //////////ShowViewModel<TrainViewModel>();
    }

  }
}

