namespace MyFitnesser.Core.Models {
  using System;
  using System.Collections.Generic;
  using System.Linq;


  internal class Client {
  
    public Client() {
      _Record = new Database.Client();
    }

    public Client(Guid id) {
      _Record = Database.Client.Records().Where(_ => _.Id == id).FirstOrDefault();
    }

    public string Name {
      get { 
        return _Record.Name;
      }
      set { 
        _Record.Name = value;
        _Record.Write();
        Changed();
      }
    }

    public string EMail {
      get { 
        return _Record.EMail;
      }
      set { 
        _Record.EMail = value;
        _Record.Write();
        Changed();
      }
    }

    public DateTime BirthDay {
      get { 
        return _Record.BirthDay;
      }
      set { 
        _Record.BirthDay = value;
        _Record.Write();
        Changed();
      }
    }

    protected void Changed() {
      var temp = OnChanged;
      if (temp != null)
        temp(this, EventArgs.Empty);
    }

    public event EventHandler OnChanged; 

    private Database.Client _Record;

    public static Client Get(Guid id) {
      if (_Clients.ContainsKey(id)) {
        return _Clients[id];
      }
      else {
        var client = new Models.Client(id);
        _Clients[id] = client;
        return client;
      }
    }

    public static Client New() {
      return new Client();
    }

    private static Dictionary<Guid, Client> _Clients = new Dictionary<Guid, Client>();
  }
}

