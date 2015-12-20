namespace MyFitnesser.Core.ViewModels {
  using System;


  public class ClientViewModel : MvxViewModel {

    public string Name { 
      get { return _Name; }
      set { _Name = value; RaisePropertyChanged(() => Name); }
    }
    private string _Name;

    public string EMail { 
      get { return _EMail; }
      set { _EMail = value; RaisePropertyChanged(() => EMail); }
    }
    private string _EMail;
    
    public DateTime Birthday { 
      get { return _Birthday; }
      set { _Birthday = value; RaisePropertyChanged(() => Birthday); }
    }
    private DateTime _Birthday;

  }
}

