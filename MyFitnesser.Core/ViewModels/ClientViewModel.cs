﻿namespace MyFitnesser.Core.ViewModels {
  using System;
  using System.Linq;

  using Cirrious.MvvmCross.ViewModels;


  public class ClientViewModel : MvxViewModel {

    public ClientViewModel() : base() {
      _Record = new Database.ClientRecord();
    }

    public ClientViewModel(Guid id) : base() {
      _Record = Database.ClientRecord.Records().Where(_ => _.Id == id).FirstOrDefault();
      this.Name     = _Record.Name;
      this.Phone    = _Record.Phone;
      this.EMail    = _Record.EMail;
      this.Birthday = _Record.BirthDay;
      this.Notes    = _Record.Notes;
    }
    private Database.ClientRecord _Record;

    public string Name { 
      get { return _Name; }
      set { _Name = value; RaisePropertyChanged(() => Name); }
    }
    private string _Name;

    public string Phone { 
      get { return _Phone; }
      set { _Phone = value; RaisePropertyChanged(() => Phone); }
    }
    private string _Phone;

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

    public string Notes { 
      get { return _Notes; }
      set { _Notes = value; RaisePropertyChanged(() => Notes); }
    }
    private string _Notes;

    public IMvxCommand WriteAndCloseCommand { get { return new MvxCommand(() => WriteAndClose(), () => true); } }

    private void WriteAndClose() {
      _Record.Name     = this.Name;
      _Record.Phone    = this.Phone;
      _Record.EMail    = this.EMail;
      _Record.BirthDay = this.Birthday;
      _Record.Notes    = this.Notes;
      _Record.Write();
      Close(this);
    }

    public IMvxCommand CloseCommand { get { return new MvxCommand(() => Close(this), () => true); } }
  }
}

