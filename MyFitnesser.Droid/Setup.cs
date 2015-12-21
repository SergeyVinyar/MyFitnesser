namespace MyFitnesser.Droid {
  using Android.Content;
  using Cirrious.CrossCore;
  using Cirrious.CrossCore.Platform;
  using Cirrious.MvvmCross.Droid.Platform;
  using Cirrious.MvvmCross.Droid.Views;
  using Cirrious.MvvmCross.ViewModels;


  public class Setup : MvxAndroidSetup {

    public Setup(Context applicationContext) : base(applicationContext) {
    }

    protected override IMvxApplication CreateApp() {
      return new MyFitnesser.Core.App();
    }

    protected override IMvxAndroidViewPresenter CreateViewPresenter() {
      var customPresenter = Mvx.IocConstruct<CustomPresenter>();
      Mvx.RegisterSingleton<ICustomPresenter>(customPresenter);

      customPresenter.RegisterFragment(typeof(Core.ViewModels.CalendarViewModel));
      customPresenter.RegisterFragment(typeof(Core.ViewModels.ClientViewModel));
      customPresenter.RegisterFragment(typeof(Core.ViewModels.ClientsListViewModel));

      return customPresenter;
    }

//    protected override IMvxTrace CreateDebugTrace() {
//      return new DebugTrace();
//    }

  }
}