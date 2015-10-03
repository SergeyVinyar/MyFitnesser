namespace MyFitnesser.Core.Presenters {
  using System;


  public class ClientForm : IClientFormPresenter {

    public ClientForm(IClientFormView view) : this(view, Guid.Empty) {

    }

    public ClientForm(IClientFormView view, Guid id) {
      if(id == Guid.Empty)
        _Client = Models.Client.New();
      else
        _Client = Models.Client.Get(id);
      _View = view;
      //_View.SetName(_Client.Name);
      //_View.SetEmail(_Client.EMail);
      //_View.SetBirthDay(_Client.BirthDay);
      _Client.OnChanged += (sender, e) => {
        _View.SetName(_Client.Name);
        _View.SetEmail(_Client.EMail);
        _View.SetBirthDay(_Client.BirthDay);
      };
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

    private Models.Client _Client;
    private IClientFormView _View;
  }

  public interface IClientFormPresenter {

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

