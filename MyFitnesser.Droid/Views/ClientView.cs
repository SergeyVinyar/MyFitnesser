namespace MyFitnesser.Droid.Views {
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  using Android;
  using Android.App;
  using Android.Content;
  using Android.OS;
  using Android.Runtime;
  using Android.Util;
  using Android.Views;
  using Android.Widget;

  using Cirrious.MvvmCross.Droid.FullFragging.Fragments;


  public class ClientView: MvxFragment {

    public override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
    }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
      return inflater.Inflate(MyFitnesser.Droid.Resource.Layout.Client, container, false);
    }

    public override void OnStart() {
      base.OnStart();
    

    }

    public override void OnStop() {
      base.OnStop();
    
    }

  }
}

