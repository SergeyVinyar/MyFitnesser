namespace MyFitnesser.Droid.Views {
  using System;
  using System.Threading.Tasks;

  using Android.OS;
  using Android.Runtime;
  using Android.Views;
  using Android.Support.Design.Widget;
  using Android.Support.V7.Widget;

  using Cirrious.MvvmCross.Binding.Droid.BindingContext;
  using Cirrious.MvvmCross.Droid.FullFragging.Fragments;

  using MvvmCross.Droid.Support.V7.AppCompat;

  using Core.ViewModels;


  public class MenuView : MvxFragment, NavigationView.IOnNavigationItemSelectedListener {

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
      base.OnCreateView(inflater, container, savedInstanceState);

      var view = this.BindingInflate(Droid.Resource.Layout.Menu, null);

      _NavigationView = view.FindViewById<NavigationView>(Droid.Resource.Id.menu);
      _NavigationView.SetNavigationItemSelectedListener(this);

      return view;
    }

    public bool OnNavigationItemSelected(IMenuItem item) {
      Navigate(item.ItemId).ContinueWith(t => {});
      return true;
    }

    private async Task Navigate(int itemId) {
      ((MainActivityView)Activity).DrawerLayout.CloseDrawers();

      // add a small delay to prevent any UI issues
      await Task.Delay(TimeSpan.FromMilliseconds (250));

      var model = (MenuViewModel)ViewModel;
      switch (itemId) {
        case Resource.Id.nav_days:
          model.ShowCalendarDaysCommand.Execute();
          break;
        case Resource.Id.nav_years:
          model.ShowCalendarYearsCommand.Execute();
          break;
        case Resource.Id.nav_clients:
          model.ShowClientsCommand.Execute();
          break;
      }
    }

    private NavigationView _NavigationView;
   }
}