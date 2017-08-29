namespace MyFitnesser.Core.ViewModels {
  using System;
  using System.Linq;

  using Cirrious.MvvmCross.ViewModels;

  using MyFitnesser.Core.Utils;


  public class TrainViewModel : MvxViewModel {

    public TrainViewModel() : base() {
      _Record = new Database.TrainRecord();
    }

    public void Init(ViewParameters parameters) {
      if (parameters.TrainId == Guid.Empty) {
        _Record = new Database.TrainRecord();
        StartDate = parameters.Date.Date.AddHours(13);
        EndDate   = parameters.Date.Date.AddHours(14);
      }
      else {
        _Record = Database.TrainRecord.Records().Where(_ => _.Id == parameters.TrainId).FirstOrDefault();
        this.ClientId  = _Record.ClientId;
        this.StartDate = _Record.StartDate;
        this.EndDate   = _Record.EndDate;
        this.Status    = _Record.Status;
        this.Notes     = _Record.Notes;
      }
      Clients = new SuspendableObservableCollection<ClientsViewModel.Client>();
      var clients = Database.ClientRecord.Records().ToList();
      clients.ForEach(client => {
        ClientsViewModel.Client c;
        Clients.Add(c = new ClientsViewModel.Client() {
          Id   = client.Id,
          Name = client.Name,
        });
        if (c.Id == this.ClientId)
          SelectedClient = c;
      });
      if (SelectedClient == null && Clients.Count > 0)
        SelectedClient = Clients[0];
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

    public SuspendableObservableCollection<ClientsViewModel.Client> Clients { 
      get { 
        return _Clients;
      } 
      private set { 
        _Clients = value; 
        RaisePropertyChanged(() => Clients); 
      } 
    }
    private SuspendableObservableCollection<ClientsViewModel.Client> _Clients;

    public ClientsViewModel.Client SelectedClient { 
      get { return _SelectedClient; }
      set { _SelectedClient = value; _ClientId = value.Id; RaisePropertyChanged(() => SelectedClient); }
    }
    private ClientsViewModel.Client _SelectedClient;

    public IMvxCommand WriteCommand { get { return new MvxCommand(() => Write(false), () => true); } }

    public IMvxCommand WriteAndCloseCommand { get { return new MvxCommand(() => Write(true), () => true); } }

    private void Write(bool thenClose) {
      _Record.ClientId  = this.ClientId;
      _Record.StartDate = this.StartDate;
      _Record.EndDate   = this.EndDate;
      _Record.Status    = this.Status;
      _Record.Notes     = this.Notes;
      _Record.Write();
      if (thenClose)
        Close(this);
    }

    public IMvxCommand CloseCommand { get { return new MvxCommand(() => Close(this), () => true); } }

    public class ViewParameters {
      public DateTime Date { get; set; }
      public Guid TrainId { get; set; }
    }
  }
}


