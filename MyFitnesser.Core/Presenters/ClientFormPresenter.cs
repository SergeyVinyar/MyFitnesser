namespace MyFitnesser.Core {
  using System;


  public class ClientFormPresenter : IClientFormPresenter {

    public ClientFormPresenter(IClientFormView view) : this(view, Guid.Empty) {

    }

    public ClientFormPresenter(IClientFormView view, Guid id) {
      if(id == Guid.Empty)
        _Client = ClientModel.New();
      else
        _Client = ClientModel.Get(id);
      _View = view;
      _OnClientChanged = (sender, e) => {
        SetViewData();
      };
    }

    void IClientFormPresenter.Start() {
      _Client.OnChanged += _OnClientChanged;
      SetViewData();
    }

    void IClientFormPresenter.Stop() {
      _Client.OnChanged -= _OnClientChanged;
    }

    void IClientFormPresenter.SetName(string value) {
      _Client.Name = value;
    }

    void IClientFormPresenter.SetEmail(string value) {
      _Client.EMail = value;
    }

    void IClientFormPresenter.SetBirthDay(DateTime value) {
      _Client.BirthDay = value;
    }

    private Core.ModelEventHandler _OnClientChanged;

    private void SetViewData() {
      _View.SetName(_Client.Name);
      _View.SetEmail(_Client.EMail);
      _View.SetBirthDay(_Client.BirthDay);
    }

    private ClientModel _Client;
    private IClientFormView _View;
  }

  public interface IClientFormPresenter {

    void Start();

    void Stop();

    void SetName(string value);

    void SetEmail(string value);

    void SetBirthDay(DateTime value);
  }

  public interface IClientFormView {

    void SetName(string value);

    void SetEmail(string value);

    void SetBirthDay(DateTime value);
  }
}

