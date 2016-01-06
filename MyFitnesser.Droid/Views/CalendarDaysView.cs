using Cirrious.MvvmCross.Binding.Droid.BindingContext;

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

  using Android.Graphics.Drawables;
  using Android.Graphics.Drawables.Shapes;
  using Android.Graphics;

  using Cirrious.MvvmCross.Droid.FullFragging.Fragments;

  using Core.ViewModels;

  public class CalendarDaysView : MvxFragment, ActionBar.IOnNavigationListener {
  
    public override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);

      this.Activity.ActionBar.SetDisplayShowTitleEnabled(false);
      this.Activity.ActionBar.NavigationMode = ActionBarNavigationMode.List;
      this.Activity.ActionBar.SetListNavigationCallbacks(new ArrayAdapter<string>(this.Activity, Android.Resource.Layout.SimpleListItem1, new[] { "День", "Год" } ), this);
      this.Activity.ActionBar.SetSelectedNavigationItem(0); // День
    }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
      base.OnCreateView(inflater, container, savedInstanceState);
      return this.BindingInflate(MyFitnesser.Droid.Resource.Layout.CalendarDays, null);
    }

    public override void OnResume() {
      base.OnResume();
    }

    bool ActionBar.IOnNavigationListener.OnNavigationItemSelected(int itemPosition, long itemId) {
      if (itemId == 1) {
        (ViewModel as CalendarDaysViewModel).ShowYear();
        return true;
      }
      return false;
    }

  }
}

