namespace MyFitnesser.Droid {
  using Android.App;
  using Android.OS;
  using Cirrious.MvvmCross.Droid.Views;


  [Activity(Label = "MyFitnesser", MainLauncher = true, Icon = "@mipmap/icon")]
  public class SplashView : MvxSplashScreenActivity {

    public SplashView(): base(Droid.Resource.Layout.Splash) {
    
    }

    protected override void OnStart() {
      base.OnStart();

    }

  }
}

