using System;
using System.Threading;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using API.Accessors;
using HockeyApp.Android.Metrics;
using Jokebox.Core.Helpers;
using Jokebox.Core.Localization;
using JokeBox.Core.Localization;
using JokeBox.Core.Managers;
using JokeBox.Core.Persistence.Models;
using JokeBox.UI.Views;

namespace JokeBox.Droid.Activities
{
    /// <summary>
    /// Will let the user compose and submit a joke.
    /// </summary>
    [Activity(Label = "ComposeActivity",
        WindowSoftInputMode = SoftInput.AdjustResize,
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class ComposeActivity : Activity
    {
        private int _minLength;
        private int _maxLength;
        private ProgressBar _progBar;
        private MainTextView _charsLeft;
        private MainTextView _composer;
        private MainEditText _editText;
        private Button _submitButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _minLength = 30;
            _maxLength = 500;
            SetContentView(Resource.Layout.Compose);
            setupViews();
            assignEvents();
        }

        /// <summary>
        /// Will connect the views to the local fields of this class.
        /// </summary>
        private void setupViews()
        {
            _progBar = FindViewById<ProgressBar>(Resource.Id.ComposeProgressBar);
            _charsLeft = FindViewById<MainTextView>(Resource.Id.ComposeCharsLeft);
            _composer = FindViewById<MainTextView>(Resource.Id.ComposeComposer);
            _editText = FindViewById<MainEditText>(Resource.Id.ComposeEditText);
            _submitButton = FindViewById<Button>(Resource.Id.ComposeSubmitButton);

            _progBar.Visibility = ViewStates.Invisible;
            _charsLeft.Text = _maxLength.ToString();
            _composer.Text = DBManager.Static.DBAccessor.Select<SimpleItem>()[0].Value;
            _composer.MakeBold();
            _editText.ChangeFontSize(19);
            _submitButton.Text = Localization.Static.Raw(ResourceKeyNames.Static.Submit);
            _submitButton.Enabled = false;
        }

        /// <summary>
        /// Will assign events to the views.
        /// </summary>
        private void assignEvents()
        {
            _editText.TextChanged += editTextTextChanged;
            _submitButton.Click += submitButtonClick;
        }

        /// <summary>
        /// Will show how many characters are left.
        /// </summary>
        private void editTextTextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            int currentChars = _maxLength - _editText.Text.Length;
            _charsLeft.Text = currentChars.ToString();

            if (_editText.Text.Length > _minLength && !_submitButton.Enabled)
            {
                _submitButton.Enabled = true;
                if (_editText.Text.Length > _maxLength) _editText.Text = _editText.Text.Substring(0, _maxLength);
            }
            else if (_editText.Text.Length < _minLength && _submitButton.Enabled) _submitButton.Enabled = false;
        }

        /// <summary>
        /// Will submit the typed joke to the API, if it passes the textual tests.
        /// </summary>
        private void submitButtonClick(object sender, System.EventArgs e)
        {
            string text = ContentFilter.Static.CleanUp(_editText.Text);
            if (!ContentFilter.Static.IsGibberish(text))
            {
                _progBar.Visibility = ViewStates.Invisible;
                _submitButton.Enabled = false;

                ThreadPool.QueueUserWorkItem(async asynco =>
                {
                    try
                    {
                        string username = DBManager.Static.DBAccessor.Select<SimpleItem>()[0].Value;
                        await APIAccessor.Static.Submit(username, text,
                                         Localization.Static.Raw(ResourceKeyNames.Static.CountryCode));
                    }
                    catch(Exception ex)
                    {
                        MetricsManager.TrackEvent(ex.StackTrace);
                    }

                    RunOnUiThread(() =>
                    {
                        StartActivity(typeof(MainActivity));
                    });
                });
            }
        }
    }
}
