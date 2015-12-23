namespace MyFitnesser.Core.ViewModels {
  using System;
  using Cirrious.MvvmCross.ViewModels;


  public class MainActivityViewModel: MvxViewModel {

    public void InitViews() {
      ShowViewModel<CalendarViewModel>();
    }

    public string sModelProp { 
      get { 
        return _sModelProp;
      } 
      set { 
        _sModelProp = value; 
        RaisePropertyChanged(() => sModelProp); 
      } 
    }
    private string _sModelProp = "Hello from MainActivityViewModel";

  }
}

