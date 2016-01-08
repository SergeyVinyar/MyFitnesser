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
  using Cirrious.MvvmCross.Droid.FullFragging;
  using Cirrious.MvvmCross.ViewModels;

  using Core.ViewModels;

  [Activity]
  internal class MainActivityView : MvxActivity, IFragmentHost {

    protected override void OnCreate(Bundle bundle)	{
      base.OnCreate(bundle);
      SetContentView(Droid.Resource.Layout.Main);
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

    public bool Show(MvxViewModelRequest request) {
      var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
      var viewModel = loaderService.LoadViewModel(request, null /* saved state */);

      if (viewModel is CalendarDaysViewModel) {
        CalendarDaysView v;
        FragmentManager.BeginTransaction()
          .Replace(Droid.Resource.Id.panel_left, v = new CalendarDaysView())
          .AddToBackStack(null)
          .Commit();
        v.ViewModel = viewModel;
        return true;
      }
      if (viewModel is CalendarYearsViewModel) {
        CalendarYearsView v;
        FragmentManager.BeginTransaction()
          .Replace(Droid.Resource.Id.panel_left, v = new CalendarYearsView())
          .AddToBackStack(null)
          .Commit();
        v.ViewModel = viewModel;
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