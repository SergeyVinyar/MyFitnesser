using Android.Content.Res;

namespace MyFitnesser.Droid.Views {
  using System;

  using Android;
  using Android.App;
  using Android.Content;
  using Android.Runtime;
  using Android.Views;
  using Android.Support.V4.Widget;
  using Android.Support.V7.Widget;
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

    protected override void OnCreate(Bundle bundle) {
      base.OnCreate(bundle);
      SetContentView(Droid.Resource.Layout.Main);
      DrawerLayout = FindViewById<DrawerLayout>(Droid.Resource.Id.drawer_layout);

      Toolbar = FindViewById<Toolbar>(Droid.Resource.Id.toolbar);
      SetSupportActionBar(Toolbar);
      SupportActionBar.SetDisplayHomeAsUpEnabled(true);

      DrawerToggle = new MvxActionBarDrawerToggle(
        this,
        DrawerLayout,
        //Toolbar,                               
        Droid.Resource.String.drawer_open,            
        Droid.Resource.String.drawer_close            
      );
      DrawerLayout.SetDrawerListener(DrawerToggle);
    }

    protected override void OnPostCreate(Bundle savedInstanceState) {
      base.OnPostCreate(savedInstanceState);
      DrawerToggle.SyncState();
    }

    protected override void OnStart() {
      base.OnStart();
      Core.Database.DbInit.Start();
      (DataContext as MainActivityViewModel).InitViews();
    }

    protected override void OnResume() {
      base.OnResume();
    }

    public override void OnConfigurationChanged(Configuration newConfig) {
      base.OnConfigurationChanged(newConfig);
      DrawerToggle.OnConfigurationChanged(newConfig);
    }

    protected override void OnStop() {
      base.OnStop();
      Core.Database.DbInit.Stop();
    }

    public override bool OnOptionsItemSelected(IMenuItem item) {
      switch (item.ItemId) {
        case Android.Resource.Id.Home:
          if (DrawerLayout.GetDrawerLockMode(GravityCompat.Start) != DrawerLayout.LockModeLockedClosed)
            DrawerLayout.OpenDrawer(GravityCompat.Start);
          else
            OnBackPressed();
          return true;
      }

      return base.OnOptionsItemSelected(item);
    }

    public override void OnBackPressed() {
      if (DrawerLayout.IsDrawerOpen(GravityCompat.Start)) {
        DrawerLayout.CloseDrawers();
      }
      else if (FragmentManager.BackStackEntryCount > 0)
        FragmentManager.PopBackStack();
      else {
        base.OnBackPressed();
      }
    }

    void IFragmentHost.GoBack() {
      OnBackPressed();
    }

    bool IFragmentHost.Show(MvxViewModelRequest request) {
      var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
      var viewModel = loaderService.LoadViewModel(request, null /* saved state */);

      if (viewModel is MenuViewModel) {
        MenuView v;
        FragmentManager.BeginTransaction()
          .Replace(Droid.Resource.Id.navigation_frame, v = new MenuView())
          .Commit();
        v.ViewModel = viewModel;
        return true;
      }
      if (viewModel is CalendarDaysViewModel) {
        CalendarDaysView v;
        FragmentManager.BeginTransaction()
          .Replace(Droid.Resource.Id.panel_left, v = new CalendarDaysView())
          .Commit();
        v.ViewModel = viewModel;
        return true;
      }
      if (viewModel is CalendarYearsViewModel) {
        CalendarYearsView v;
        FragmentManager.BeginTransaction()
          .Replace(Droid.Resource.Id.panel_left, v = new CalendarYearsView())
          .Commit();
        v.ViewModel = viewModel;
        return true;
      }
      else if (viewModel is TrainViewModel) {
        TrainView v;
        var panelId = Droid.Resource.Id.panel_left;
        //if (HasTwoPanels)
        //  panelId = Droid.Resource.Id.panel_right;
        FragmentManager.BeginTransaction()
          .Replace(panelId, v = new TrainView())
          .AddToBackStack(null)
          .Commit();
        v.ViewModel = viewModel;
        return true;
      }
      else if (viewModel is ClientsViewModel) {
        ClientsView v;
        var panelId = Droid.Resource.Id.panel_left;
        FragmentManager.BeginTransaction()
          .Replace(panelId, v = new ClientsView())
          .Commit();
        v.ViewModel = viewModel;
        return true;
      }
      else if (viewModel is ClientViewModel) {
        ClientView v;
        var panelId = Droid.Resource.Id.panel_left;
        //if (HasTwoPanels)
        //  panelId = Droid.Resource.Id.panel_right;
        FragmentManager.BeginTransaction()
          .Replace(panelId, v = new ClientView())
          .AddToBackStack(null)
          .Commit();
        v.ViewModel = viewModel;
        return true;
      }
      return false;
    }

    private bool HasTwoPanels {
      get { return false; } // (FindViewById(Droid.Resource.Id.panel_right) != null); }
    }

    public Toolbar Toolbar;
    public DrawerLayout DrawerLayout;
    public MvxActionBarDrawerToggle DrawerToggle;
  }
}