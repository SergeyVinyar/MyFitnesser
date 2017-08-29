namespace MyFitnesser.Core.ViewModels {
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using System.Windows.Input;

  using Cirrious.MvvmCross.ViewModels;

  using MyFitnesser.Core.Utils;


  public class ClientsViewModel : MvxViewModel {
  
    public ClientsViewModel() {
      UpdateClients();
      Database.ClientRecord.OnDataChanged += (s, e) => UpdateClients();
    }

    public SuspendableObservableCollection<Client> Clients { 
      get { 
        return _Clients;
      } 
      private set { 
        _Clients = value; 
        RaisePropertyChanged(() => Clients); 
      } 
    }
    private SuspendableObservableCollection<Client> _Clients;

    private void UpdateClients() {
      Clients = new SuspendableObservableCollection<Client>();

      var clients  = Database.ClientRecord.Records();
      foreach (var client in clients)
        Clients.Add(new Client() { 
          Id = client.Id, 
          Name = client.Name, 
          Phone = client.Phone, 
          EMail = client.EMail, 
          BirthDay = client.BirthDay, 
          Notes = client.Notes 
        });
    }

    public IMvxCommand AddNewClientCommand {
      get { return new MvxCommand(() => AddNewClient()); }
    }

    private void AddNewClient() {
      ShowViewModel<ClientViewModel>();
    }

    public IMvxCommand EditClientCommand {
      get { return new MvxCommand<Client>(client => EditClient(client)); }
    }

    private void EditClient(Client client) {
      var parameters = new ClientViewModel.ViewParameters();
      parameters.ClientId = client.Id;
      ShowViewModel<ClientViewModel>(parameters);
    }

    /// <summary>Клиент</summary>
    public class Client {

      public Guid Id { get; set; }
      public string Name { get; set; }
      public string Phone { get; set; }
      public string EMail { get; set; }
      public DateTime BirthDay { get; set; }
      public string Notes { get; set; }
    }
  }
}