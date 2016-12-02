using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Views;
using Android.Widget;
using Jokebox.Core.Localization;
using JokeBox.Core.Localization;
using JokeBox.UI.Views;

namespace JokeBox.Droid.Activities
{
    /// <summary>
    /// Will show information about this app.
    /// </summary>
    [Activity(Label = "AboutActivity",
        WindowSoftInputMode = SoftInput.AdjustResize,
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AboutActivity : Activity
    {
        private MainTextView _developedBy;
        private MainTextView _description;
        private Button _reviewButton;
        private Button _contactButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.About);
            setupViews();
            assignEvents();
        }

        /// <summary>
        /// Will connect the views of the layout to the fields of this class.
        /// </summary>
        private void setupViews()
        {
            _developedBy = FindViewById<MainTextView>(Resource.Id.AboutDevelopedBy);
            _description = FindViewById<MainTextView>(Resource.Id.AboutDescription);
            _reviewButton = FindViewById<Button>(Resource.Id.AboutReviewButton);
            _contactButton = FindViewById<Button>(Resource.Id.AboutContactButton);

            _developedBy.MakeBold();
            _developedBy.Text = Localization.Static.Raw(ResourceKeyNames.Static.DevelopedBy);
            _description.Text = Localization.Static.Raw(ResourceKeyNames.Static.AboutText);
            _description.ChangeFontSize(16);
            _reviewButton.Text = Localization.Static.Raw(ResourceKeyNames.Static.Review);
            _contactButton.Text = Localization.Static.Raw(ResourceKeyNames.Static.Mail);
        }

        /// <summary>
        /// Will assign events to the controls of this activity.
        /// </summary>
        private void assignEvents()
        {
            _reviewButton.Click += reviewButtonClick;
            _contactButton.Click += contactButtonClick; 
        }

        /// <summary>
        /// Will open the app store review page of this app.
        /// </summary>
        private void reviewButtonClick(object sender, System.EventArgs e)
        {
            string appPackageName = Application.Context.PackageName;
            try
            {
                var intent = new Intent(Intent.ActionView, Uri.Parse("market://details?id=" + appPackageName));
                intent.AddFlags(ActivityFlags.NewTask);

                Application.Context.StartActivity(intent);
            }
            catch (ActivityNotFoundException)
            {
                var intent = new Intent(Intent.ActionView, Uri.Parse("http://play.google.com/store/apps/details?id=" + appPackageName));
                intent.AddFlags(ActivityFlags.NewTask);
                Application.Context.StartActivity(intent);
            }
        }

        /// <summary>
        /// Will open the local email client of the app of the user
        /// to write us an email.
        /// </summary>
        private void contactButtonClick(object sender, System.EventArgs e)
        {
            Intent email = new Intent(Intent.ActionSend);
            email.PutExtra(Intent.ExtraEmail, new string[] { "service@areondev.de" });
            email.SetType("message/rfc822");
            StartActivity(email);
        }
    }
}
