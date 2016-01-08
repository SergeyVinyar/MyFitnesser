namespace MyFitnesser.Core.ViewModels {
  using System;
  using System.Linq;

  using Cirrious.MvvmCross.ViewModels;


  public class TrainViewModel : MvxViewModel {

    public TrainViewModel() : base() {
      _Record = new Database.TrainRecord();
    }

    public TrainViewModel(Guid id) : base() {
      _Record = Database.TrainRecord.Records().Where(_ => _.Id == id).FirstOrDefault();
      this.ClientId  = _Record.ClientId;
      this.StartDate = _Record.StartDate;
      this.EndDate   = _Record.EndDate;
      this.Status    = _Record.Status;
      this.Notes     = _Record.Notes;
    }
    private Database.TrainRecord _Record;

    public Guid ClientId { 
      get { return _ClientId; }
      set { _ClientId = value; RaisePropertyChanged(() => ClientId); }
    }
    private Guid _ClientId;

    public DateTime StartDate { 
      get { return _StartDate; }
      set { _StartDate = value; RaisePropertyChanged(() => StartDate); }
    }
    private DateTime _StartDate;

    public DateTime EndDate { 
      get { return _EndDate; }
      set { _EndDate = value; RaisePropertyChanged(() => EndDate); }
    }
    private DateTime _EndDate;

    public Database.TrainRecord.TrainStatuses Status { 
      get { return _Status; }
      set { _Status = value; RaisePropertyChanged(() => Status); }
    }
    private Database.TrainRecord.TrainStatuses _Status;

    public string Notes { 
      get { return _Notes; }
      set { _Notes = value; RaisePropertyChanged(() => Notes); }
    }
    private string _Notes;

    public IMvxCommand WriteAndCloseCommand { get { return new MvxCommand(() => WriteAndClose()); } }

    private void WriteAndClose() {
      _Record.ClientId  = this.ClientId;
      _Record.StartDate = this.StartDate;
      _Record.EndDate   = this.EndDate;
      _Record.Status    = this.Status;
      _Record.Notes     = this.Notes;
      _Record.Write();
      Close(this);
    }

    public IMvxCommand CloseCommand { get { return new MvxCommand(() => Close(this), () => true); } }
  }
}


