namespace MyFitnesser.Droid.Views {
  using System;
  using System.Threading.Tasks;

  using Android.OS;
  using Android.Runtime;
  using Android.Support.Design.Widget;
  using Android.Views;

  using Cirrious.MvvmCross.Binding.Droid.BindingContext;
  using Cirrious.MvvmCross.Droid.FullFragging.Fragments;


  public class MenuView : MvxFragment, NavigationView.IOnNavigationItemSelectedListener {

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
      base.OnCreateView(inflater, container, savedInstanceState);

      var view = this.BindingInflate(Droid.Resource.Layout.Menu, null);

      _NavigationView = view.FindViewById<NavigationView>(Droid.Resource.Id.menu);
      _NavigationView.SetNavigationItemSelectedListener(this);
      _NavigationView.Menu.FindItem(Resource.Id.nav_home).SetChecked(true);
      
      return view;
    }

    public bool OnNavigationItemSelected(IMenuItem item) {
      item.SetCheckable(true);
      item.SetChecked(true);
      if (_PreviousMenuItem != null) {
          _PreviousMenuItem.SetChecked(false);
      }
      _PreviousMenuItem = item;

      Navigate(item.ItemId).ContinueWith(t => {});
      return true;
    }

    private async Task Navigate(int itemId) {
      ((MainActivityView)Activity).DrawerLayout.CloseDrawers();

      // add a small delay to prevent any UI issues
      await Task.Delay(TimeSpan.FromMilliseconds (250));
      
      switch (itemId) {
          case Resource.Id.nav_home:
//                  ViewModel.ShowHomeCommand.Execute();
              break;
      }
    }

    private NavigationView _NavigationView;
    private IMenuItem _PreviousMenuItem;
  }
}