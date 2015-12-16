namespace MyFitnesser {
  using System;
  using Android.App;
  using Android.Content;
  using Android.Runtime;
  using Android.Views;
  using Android.Widget;
  using Android.OS;


  [Activity(Label = "MyFitnesser", MainLauncher = true, Icon = "@drawable/icon")]
  internal class MainActivity : Activity {

    protected override void OnCreate(Bundle bundle)	{
      base.OnCreate(bundle);
      SetContentView(Resource.Layout.Main);
    }

    protected override void OnStart() {
      base.OnStart();
      Core.DbInit.Start();

      SetLeftPanel(Core.FragmentType.ClientForm, Guid.Empty);
      SetRightPanel(Core.FragmentType.ClientForm, Guid.Empty);
    }

    protected override void OnStop() {
      base.OnStop();
      Core.DbInit.Stop();
    }

    public void SetLeftPanel(Core.FragmentType type, Guid id) {
      switch (type) {
        case Core.FragmentType.ClientForm:
          FragmentManager.BeginTransaction().Add(Resource.Id.panel_left, new ClientForm(id)).Commit();
          break;
        case Core.FragmentType.ClientsList:
          // TODO
          break;
      }
    }

    public void SetRightPanel(Core.FragmentType type, Guid id) {
      if (!HasTwoPanels)
        return;
      switch (type) {
        case Core.FragmentType.ClientForm:
          FragmentManager.BeginTransaction().Add(Resource.Id.panel_right, new ClientForm(id)).Commit();
          break;
        case Core.FragmentType.ClientsList:
          // TODO
          break;
      }
    }

    public bool HasTwoPanels {
      get { return (FindViewById(Resource.Id.panel_right) != null); }
    }

	}
}