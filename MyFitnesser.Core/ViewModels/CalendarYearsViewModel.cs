namespace MyFitnesser.Core.ViewModels {
  using System;
  using Cirrious.MvvmCross.ViewModels;


  public class CalendarYearsViewModel : MvxViewModel {

    public CalendarYearsViewModel() : base() {
      ModelProp = 666;
      sModelProp = "Hello from CalendarViewModel";

    }

    public void ShowDay() {
      ShowViewModel<CalendarDaysViewModel>();
    }













    public int ModelProp { 
      get { 
        return _ModelProp;
      } 
      set { 
        _ModelProp = value; 
        RaisePropertyChanged(() => ModelProp); 
      } 
    }
    private int _ModelProp = 666;

    public string sModelProp { 
      get { 
        return _sModelProp;
      } 
      set { 
        _sModelProp = value; 
        RaisePropertyChanged(() => sModelProp); 
      } 
    }
    private string _sModelProp = "Hello from CalendarViewModel";
  }

}

