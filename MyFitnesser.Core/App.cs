namespace MyFitnesser.Core {
  using Cirrious.CrossCore.IoC;


  public class App : Cirrious.MvvmCross.ViewModels.MvxApplication {
        
    public override void Initialize() {
      CreatableTypes()
        .EndingWith("Service")
        .AsInterfaces()
        .RegisterAsLazySingleton();
			
      //RegisterAppStart<ViewModels.TestActViewModel>();
    }
  }
}