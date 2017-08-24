using Android.App;
using Android.Widget;
using Android.OS;
using Android.Media;
using Android.Content;

namespace FiveElements
{
    [Activity(Label = "FiveElements", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        MediaPlayer click;
        protected override void OnCreate(Bundle bundle)
        {
            Window.RequestFeature(Android.Views.WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);

            Vibrator vibrator = (Vibrator)this.ApplicationContext.GetSystemService(Context.VibratorService);
            click = MediaPlayer.Create(this, Resource.Raw.click3);
            Button start = FindViewById<Button>(Resource.Id.btnStart);

            start.Click += delegate{
               click.Start();
               vibrator.Vibrate(100); 
               StartActivity(typeof(Elements));
            };
        }
    }
}

