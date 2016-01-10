namespace MyFitnesser.Droid {
  using Android.App;
  using Android.OS;
  using Android.Content.PM;

  using Cirrious.MvvmCross.Droid.Views;


  [Activity(Label = "MyFitnesser", MainLauncher = true, Theme = "@style/AppTheme.Splash", Icon = "@mipmap/icon", NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
  public class SplashView : MvxSplashScreenActivity {

    public SplashView(): base(Droid.Resource.Layout.Splash) {
    }
  }
}

