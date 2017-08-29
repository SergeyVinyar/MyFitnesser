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
      ((MainActivityView)Activity).Toolbar.Title = "План на день";
      return this.BindingInflate(MyFitnesser.Droid.Resource.Layout.CalendarDays, null);
    }

    public override void OnStart() {
      base.OnStart();
      var viewPager = this.Activity.FindViewById<BindableViewPager>(Droid.Resource.Id.CalendarDaysPagerView);
      viewPager.SetCurrentItem((ViewModel as CalendarDaysViewModel).ViewPagerCapacity / 2, false);
    }

    public override void OnResume() {
      base.OnResume();
      var mainActivity = (MainActivityView)Activity;
      mainActivity.DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeUnlocked);
      mainActivity.DrawerToggle.DrawerIndicatorEnabled = true;
    }
  }
}

