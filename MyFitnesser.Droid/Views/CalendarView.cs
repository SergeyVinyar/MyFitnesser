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

  using Cirrious.MvvmCross.Droid.Fragging.Fragments;


  public class CalendarView : MvxFragment {
  
    public override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);

    }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
      //var ignored = inflater.Inflate(MyFitnesser.Droid.Resource.Layout.Calendar, container, false);
      base.OnCreateView(inflater, container, savedInstanceState);
      return this.BindingInflate(MyFitnesser.Droid.Resource.Layout.Calendar, null);
    }


    public override void OnResume() {
      base.OnResume();
    }

  }

}

