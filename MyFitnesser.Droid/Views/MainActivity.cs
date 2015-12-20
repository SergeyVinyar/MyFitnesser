namespace MyFitnesser.Droid.Views {
  using System;

  using Android;
  using Android.App;
  using Android.Content;
  using Android.Runtime;
  using Android.Views;
  using Android.Widget;
  using Android.OS;

  using Cirrious.MvvmCross.Droid.Views;
  using Cirrious.MvvmCross.Droid.Fragging;

  using Core.ViewModels;


  [Activity(Label = "MyFitnesser", MainLauncher = true, Icon = "@mipmap/icon")]
  internal class MainActivity : MvxFragmentActivity {

    protected override void OnCreate(Bundle bundle)	{
      base.OnCreate(bundle);
      SetContentView(MyFitnesser.Droid.Resource.Layout.Main);
    }

    protected override void OnStart() {
      base.OnStart();
      Core.DbInit.Start();

      SetLeftPanel(ViewType.Calendar);
      SetRightPanel(ViewType.Client);
    }

    protected override void OnStop() {
      base.OnStop();
      Core.DbInit.Stop();
    }

    private void SetLeftPanel(Core.ViewModels.ViewType type) {
      SetPanel(MyFitnesser.Droid.Resource.Id.panel_left, type);
    }

    private void SetRightPanel(Core.ViewModels.ViewType type) {
      if (!HasTwoPanels)
        return;
      SetPanel(MyFitnesser.Droid.Resource.Id.panel_right, type);
    }

    private void SetPanel(int panelId, Core.ViewModels.ViewType type) {
      switch (type) {
        case ViewType.Calendar:
          SupportFragmentManager.BeginTransaction().Add(panelId, new CalendarView()).Commit();
          break;
        case ViewType.Client:
          SupportFragmentManager.BeginTransaction().Add(panelId, new ClientView()).Commit();
          break;
        case ViewType.ClientsList:
          // TODO
          break;
      }
    }

    private bool HasTwoPanels {
      get { return (FindViewById(MyFitnesser.Droid.Resource.Id.panel_right) != null); }
    }

	}
}