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
  using Cirrious.MvvmCross.Binding.Droid.Views;

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
      SetHasOptionsMenu(true);
      return this.BindingInflate(MyFitnesser.Droid.Resource.Layout.Train, null);
    }

    public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater) {
      inflater.Inflate(Droid.Resource.Menu.SaveCancel, menu);
    }

    public override void OnPause() {
      base.OnPause();
      // Выход из фрагмента или приложения не основание терять изменения данных
      var model = (TrainViewModel)ViewModel;
      model.WriteCommand.Execute();
    }

    public override void OnResume() {
      base.OnResume();
      var mainActivity = (MainActivityView)Activity;
      mainActivity.DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);
      mainActivity.DrawerToggle.DrawerIndicatorEnabled = false;
    }

    public override bool OnOptionsItemSelected(IMenuItem item) {
      var model = (TrainViewModel)ViewModel;
      switch (item.ItemId) {
        case Droid.Resource.Id.action_save:
          model.WriteAndCloseCommand.Execute();
          return true;
        case Droid.Resource.Id.action_cancel:
          model.CloseCommand.Execute();
          return true;
      }
      return base.OnOptionsItemSelected(item);
    }

  }
}

