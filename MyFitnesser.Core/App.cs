using Cirrious.MvvmCross.ViewModels;

namespace MyFitnesser.Core {
  using Cirrious.CrossCore;
  using Cirrious.CrossCore.IoC;


  public class App : Cirrious.MvvmCross.ViewModels.MvxApplication {
        
    public override void Initialize() {
      CreatableTypes()
        .EndingWith("Service")
        .AsInterfaces()
        .RegisterAsLazySingleton();

      Mvx.RegisterSingleton<IMvxAppStart>(new MvxAppStart<ViewModels.MainActivityViewModel>());
    }
  }
}