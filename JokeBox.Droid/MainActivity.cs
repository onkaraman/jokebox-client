using Android.App;
using Android.OS;

namespace JokeBox.Droid
{
    /// <summary>
    /// This activity contains the main screen of the app and thereby shows the joke
    /// and its upvote controls.
    /// </summary>
    [Activity(Label = "JokeBox",
          MainLauncher = true,
          Icon = "@drawable/icon",
          ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
        }
    }
}

