namespace MyFitnesser {
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  using Android.App;
  using Android.Content;
  using Android.OS;
  using Android.Runtime;
  using Android.Util;
  using Android.Views;
  using Android.Widget;


  public class ClientForm : Fragment, Core.IClientFormView {

    public ClientForm() : base() {
      _Presenter = new Core.ClientFormPresenter(this);
    }

    public ClientForm(Guid id) : base() {
      _Presenter = new Core.ClientFormPresenter(this, id);
    }

    public override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
    }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
      return inflater.Inflate(Resource.Layout.Client, container, false);
    }

    public override void OnStart() {
      base.OnStart();
      _Presenter.Start();
      View.FindViewById<EditText>(Resource.Id.client_name).TextChanged += (sender, e) => { _Presenter.SetName(e.Text.ToString()); };
      View.FindViewById<EditText>(Resource.Id.client_email).TextChanged += (sender, e) => { _Presenter.SetEmail(e.Text.ToString()); };
    }

    public override void OnStop() {
      base.OnStop();
      _Presenter.Stop();
    }

    void Core.IClientFormView.SetName(string value) {
      View.FindViewById<EditText>(Resource.Id.client_name).Text = value;
    }

    void Core.IClientFormView.SetEmail(string value) {
      View.FindViewById<EditText>(Resource.Id.client_email).Text = value;
    }

    void Core.IClientFormView.SetBirthDay(DateTime value) {

    }

    private Core.IClientFormPresenter _Presenter;
  }
}

