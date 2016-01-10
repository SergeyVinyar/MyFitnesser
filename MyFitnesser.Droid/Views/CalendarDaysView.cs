namespace MyFitnesser.Droid.Views {
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  using Android;
  using Android.App;
  using Android.Content;
  using Android.Content.Res;
  using Android.OS;
  using Android.Runtime;
  using Android.Util;
  using Android.Views;
  using Android.Support.V7.Widget;

  using Android.Graphics.Drawables;
  using Android.Graphics.Drawables.Shapes;
  using Android.Graphics;

  using Cirrious.MvvmCross.Droid.FullFragging.Fragments;
  using Cirrious.MvvmCross.Binding.Droid.BindingContext;

  using MvvmCross.Droid.Support.V7.AppCompat;

  using Cheesebaron.MvvmCross.Bindings.Droid;

  using Core.ViewModels;


  public class CalendarDaysView : MvxFragment {
  
    public override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
      if (this.ViewModel == null)
        this.ViewModel = new CalendarDaysViewModel();
    }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
      base.OnCreateView(inflater, container, savedInstanceState);
      var view = this.BindingInflate(MyFitnesser.Droid.Resource.Layout.CalendarDays, null);

      _Toolbar = view.FindViewById<Toolbar>(Droid.Resource.Id.toolbar);

      if (_Toolbar != null) {
        ((MainActivityView)Activity).SetSupportActionBar(_Toolbar);
        ((MainActivityView)Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        _Toolbar.Title = "План на день";

        _DrawerToggle = new MvxActionBarDrawerToggle(
          Activity,                              
          ((MainActivityView)Activity).DrawerLayout,
          _Toolbar,                               
          Droid.Resource.String.drawer_open,            
          Droid.Resource.String.drawer_close            
        );

        ((MainActivityView)Activity).DrawerLayout.SetDrawerListener(_DrawerToggle);
      }

      return view;
    }

    public override void OnStart() {
      base.OnStart();
      var viewPager = this.Activity.FindViewById<BindableViewPager>(Droid.Resource.Id.CalendarDaysPagerView);
      viewPager.SetCurrentItem((ViewModel as CalendarDaysViewModel).ViewPagerCapacity / 2, false);
    }

    public override void OnResume() {
      base.OnResume();
      _DrawerToggle.DrawerIndicatorEnabled = true;
    }

    public override void OnConfigurationChanged(Configuration newConfig) {
      base.OnConfigurationChanged(newConfig);
      if (_Toolbar != null) {
        _DrawerToggle.OnConfigurationChanged(newConfig);
      }
    }

    public override void OnActivityCreated(Bundle savedInstanceState) {
      base.OnActivityCreated(savedInstanceState);
      if (_Toolbar != null) {
        _DrawerToggle.SyncState();
      }
    }

    private Toolbar _Toolbar;
    private MvxActionBarDrawerToggle _DrawerToggle;
  }
}

