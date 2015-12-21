namespace MyFitnesser.Droid.Views {
  using System;

  using Android;
  using Android.App;
  using Android.Content;
  using Android.Runtime;
  using Android.Views;
  using Android.Widget;
  using Android.OS;

  using Cirrious.CrossCore;
  using Cirrious.MvvmCross.Droid.Views;
  using Cirrious.MvvmCross.Droid.Fragging;
  using Cirrious.MvvmCross.ViewModels;

  using Core.ViewModels;

  [Activity]
  internal class MainActivityView : MvxFragmentActivity, IFragmentHost {

    protected override void OnCreate(Bundle bundle)	{
      base.OnCreate(bundle);
      SetContentView(Droid.Resource.Layout.Main);
    }

    protected override void OnStart() {
      base.OnStart();
      Core.DbInit.Start();

      (DataContext as MainActivityViewModel).InitViews();
    }

    protected override void OnStop() {
      base.OnStop();
      Core.DbInit.Stop();
    }

    public bool Show(MvxViewModelRequest request) {
      var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
      var viewModel = loaderService.LoadViewModel(request, null /* saved state */);

      if (viewModel is CalendarViewModel) {
        SupportFragmentManager.BeginTransaction()
          .Replace(Droid.Resource.Id.panel_left, new CalendarView())
          .AddToBackStack(null)
          .Commit();
        return true;
      }
      else if (viewModel is ClientViewModel) {
        var panelId = Droid.Resource.Id.panel_left;
        if (HasTwoPanels)
          panelId = Droid.Resource.Id.panel_right;
        SupportFragmentManager.BeginTransaction()
          .Replace(panelId, new ClientView())
          .AddToBackStack(null)
          .Commit();
        return true;
      }
      else if (viewModel is ClientsListViewModel) {
        // TODO
        return true;
      }
      return false;
    }

    private bool HasTwoPanels {
      get { return (FindViewById(Droid.Resource.Id.panel_right) != null); }
    }
  }
}