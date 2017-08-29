namespace MyFitnesser.Droid {
  using System.Collections.Generic;
  using System.Reflection;

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

      customPresenter.RegisterFragment(typeof(Core.ViewModels.MenuViewModel));
      customPresenter.RegisterFragment(typeof(Core.ViewModels.CalendarDaysViewModel));
      customPresenter.RegisterFragment(typeof(Core.ViewModels.CalendarYearsViewModel));
      customPresenter.RegisterFragment(typeof(Core.ViewModels.TrainViewModel));
      customPresenter.RegisterFragment(typeof(Core.ViewModels.ClientsViewModel));
      customPresenter.RegisterFragment(typeof(Core.ViewModels.ClientViewModel));

      return customPresenter;
    }

    protected override IEnumerable<Assembly> AndroidViewAssemblies {
      get {
        var assemblies = new List<Assembly>(base.AndroidViewAssemblies);
        assemblies.Add(typeof(Cheesebaron.MvvmCross.Bindings.Droid.BindableViewPager).Assembly);
        return assemblies;
      }
    }
  }
}