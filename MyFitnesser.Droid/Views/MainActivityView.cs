namespace MyFitnesser.Droid.Views {
  using System;

  using Android;
  using Android.App;
  using Android.Content;
  using Android.Runtime;
  using Android.Views;
  using Android.Support.V4.Widget;
  using Android.Support.V4.View;
  using Android.OS;

  using Cirrious.CrossCore;
  using Cirrious.MvvmCross.Droid.Views;
  using Cirrious.MvvmCross.Droid.FullFragging;
  using Cirrious.MvvmCross.ViewModels;

  using MvvmCross.Droid.Support.V7.AppCompat;

  using Core.ViewModels;

  [Activity]
  internal class MainActivityView : MvxAppCompatActivity, IFragmentHost {

    protected override void OnCreate(Bundle bundle)	{
      base.OnCreate(bundle);
      SetContentView(Droid.Resource.Layout.Main);
      DrawerLayout = FindViewById<DrawerLayout>(Droid.Resource.Id.drawer_layout);
    }

    protected override void OnStart() {
      base.OnStart();
      Core.Database.DbInit.Start();

      (DataContext as MainActivityViewModel).InitViews();
    }

    protected override void OnStop() {
      base.OnStop();
      Core.Database.DbInit.Stop();
    }

    public override bool OnOptionsItemSelected(IMenuItem item) {
      switch (item.ItemId) {
        case Android.Resource.Id.Home:
          DrawerLayout.OpenDrawer(GravityCompat.Start);
          return true;
      }

      return base.OnOptionsItemSelected(item);
    }

    private void ShowBackButton()
    {
      //TODO Tell the toggle to set the indicator off
      //this.DrawerToggle.DrawerIndicatorEnabled = false;

      //Block the menu slide gesture
      DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);
    }

    private void ShowHamburguerMenu()
    {
      //TODO set toggle indicator as enabled 
      //this.DrawerToggle.DrawerIndicatorEnabled = true;

      //Unlock the menu sliding gesture
      DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeUnlocked);
    }

    public override void OnBackPressed() {
      if (DrawerLayout != null && DrawerLayout.IsDrawerOpen(GravityCompat.Start)) {
        DrawerLayout.CloseDrawers();
      }
      else {
        base.OnBackPressed();
      }
    }

    public bool Show(MvxViewModelRequest request) {
      var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
      var viewModel = loaderService.LoadViewModel(request, null /* saved state */);

      if (viewModel is MenuViewModel) {
        MenuView v;
        FragmentManager.BeginTransaction()
          .Replace(Droid.Resource.Id.navigation_frame, v = new MenuView())
          .AddToBackStack(null)
          .Commit();
        v.ViewModel = viewModel;
        return true;
      }
      if (viewModel is CalendarDaysViewModel) {
        CalendarDaysView v;
        FragmentManager.BeginTransaction()
          .Replace(Droid.Resource.Id.panel_left, v = new CalendarDaysView())
          .AddToBackStack(null)
          .Commit();
        v.ViewModel = viewModel;
        ShowHamburguerMenu();
        return true;
      }
      if (viewModel is CalendarYearsViewModel) {
        CalendarYearsView v;
        FragmentManager.BeginTransaction()
          .Replace(Droid.Resource.Id.panel_left, v = new CalendarYearsView())
          .AddToBackStack(null)
          .Commit();
        v.ViewModel = viewModel;
        ShowHamburguerMenu();
        return true;
      }
      else if (viewModel is ClientViewModel) {
        ClientView v;
        var panelId = Droid.Resource.Id.panel_left;
        if (HasTwoPanels)
          panelId = Droid.Resource.Id.panel_right;
        FragmentManager.BeginTransaction()
          .Replace(panelId, v = new ClientView())
          .AddToBackStack(null)
          .Commit();
        v.ViewModel = viewModel;
        ShowBackButton();
        return true;
      }
      else if (viewModel is TrainViewModel) {
        TrainView v;
        var panelId = Droid.Resource.Id.panel_left;
        if (HasTwoPanels)
          panelId = Droid.Resource.Id.panel_right;
        FragmentManager.BeginTransaction()
          .Replace(panelId, v = new TrainView())
          .AddToBackStack(null)
          .Commit();
        v.ViewModel = viewModel;
        ShowBackButton();
        return true;
      }
      else if (viewModel is ClientsListViewModel) {
        // TODO
        ShowHamburguerMenu();
        return true;
      }
      return false;
    }

    private bool HasTwoPanels {
      get { return (FindViewById(Droid.Resource.Id.panel_right) != null); }
    }

    public DrawerLayout DrawerLayout;

  }
}