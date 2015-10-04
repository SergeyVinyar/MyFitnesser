namespace MyFitnesser.Core {
  using System;
  using System.Collections.Generic;
  using System.Linq;


  internal class ClientModel {

    public static ClientModel New() {
      return new ClientModel(Guid.Empty);
    }

    public static ClientModel Get(Guid id) {
      if (_Models.ContainsKey(id)) {
        return _Models[id];
      }
      else {
        var model = new ClientModel(id);
        _Models[id] = model;
        return model;
      }
    }

    private ClientModel(Guid id) : base() { 
      if (id == Guid.Empty)
        _Record = new ClientRecord();
      else
        _Record = ClientRecord.Records().Where(_ => _.Id == id).FirstOrDefault();
    }

    public string Name {
      get { 
        return _Record.Name;
      }
      set { 
        if (_Record.Name == value)
          return;
        _Record.Name = value;
        Changed();
        _Record.Write();
      }
    }

    public string EMail {
      get { 
        return _Record.EMail;
      }
      set { 
        if (_Record.EMail == value)
          return;
        _Record.EMail = value;
        Changed();
        _Record.Write();
      }
    }

    public DateTime BirthDay {
      get { 
        return _Record.BirthDay;
      }
      set { 
        if (_Record.BirthDay == value)
          return;
        _Record.BirthDay = value;
        Changed();
        _Record.Write();
      }
    }

    protected void Changed() {
      var temp = OnChanged;
      if (temp != null)
        temp(this, EventArgs.Empty);
    }

    public event EventHandler OnChanged; 
    private ClientRecord _Record;

    private static Dictionary<Guid, ClientModel> _Models = new Dictionary<Guid, ClientModel>();
  }
}

