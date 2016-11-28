using Android.App;
using Android.OS;
using Android.Views;

namespace JokeBox.Droid.Activies
{
    /// <summary>
    /// This activity enables the user to set a username for himself.
    /// </summary>
    [Activity(Label = "SetNameActivity",
        WindowSoftInputMode = SoftInput.AdjustResize,
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SetNameActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SetName);
        }
    }
}
