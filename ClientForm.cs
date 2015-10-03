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

  using P = Core.Presenters;


  public class ClientForm : Fragment, P.IClientFormView {

    public ClientForm(Guid id) {
      _Presenter = new P.ClientForm(this, id);
    }

    public override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
    }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
      return inflater.Inflate(Resource.Layout.Client, container, false);
    }

    public override void OnStart() {
      base.OnStart();
      View.FindViewById<EditText>(Resource.Id.client_name).TextChanged += (sender, e) => { _Presenter.SetName(e.Text.ToString()); };
      View.FindViewById<EditText>(Resource.Id.client_email).TextChanged += (sender, e) => { _Presenter.SetEmail(e.Text.ToString()); };
    }

    void P.IClientFormView.SetName(string value) {
      View.FindViewById<EditText>(Resource.Id.client_name).Text = value;
    }

    void P.IClientFormView.SetEmail(string value) {
      View.FindViewById<EditText>(Resource.Id.client_email).Text = value;
    }

    void P.IClientFormView.SetBirthDay(DateTime value) {

    }

    private P.IClientFormPresenter _Presenter;

  }
}

