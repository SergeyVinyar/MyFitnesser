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
  using Android.Support.V4.Widget;
  using Android.Support.V7.Widget;

  using Cirrious.MvvmCross.Binding.Droid.BindingContext;
  using Cirrious.MvvmCross.Droid.FullFragging.Fragments;

  using Core.ViewModels;


  public class TrainView: MvxFragment {

    public override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
      if (this.ViewModel == null)
        this.ViewModel = new TrainViewModel();
    }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
      base.OnCreateView(inflater, container, savedInstanceState);
      ((MainActivityView)Activity).Toolbar.Title = "Тренировка";
      return this.BindingInflate(MyFitnesser.Droid.Resource.Layout.Train, null);
    }

    public override void OnResume() {
      base.OnResume();
      var mainActivity = (MainActivityView)Activity;
      mainActivity.DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);
      mainActivity.DrawerToggle.DrawerIndicatorEnabled = false;
    }
  }
}

