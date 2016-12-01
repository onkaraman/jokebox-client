using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
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
    [Activity(Label = "SetNameActivity",
        WindowSoftInputMode = SoftInput.AdjustResize,
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class ComposeActivity : Activity
    {
        private int _max;
        private MainTextView _charsLeft;
        private MainTextView _composer;
        private MainEditText _editText;
        private Button _submitButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _max = 500;
            SetContentView(Resource.Layout.Compose);
            setupViews();
            assignEvents();
        }

        /// <summary>
        /// Will connect the views to the local fields of this class.
        /// </summary>
        private void setupViews()
        {
            _charsLeft = FindViewById<MainTextView>(Resource.Id.ComposeCharsLeft);
            _composer = FindViewById<MainTextView>(Resource.Id.ComposeComposer);
            _editText = FindViewById<MainEditText>(Resource.Id.ComposeEditText);
            _submitButton = FindViewById<Button>(Resource.Id.ComposeSubmitButton);

            _charsLeft.Text = _max.ToString();
            _composer.MakeBold();
            _composer.Text = DBManager.Static.DBAccessor.Select<SimpleItem>()[0].Value;
            _editText.ChangeFontSize(19);
            _submitButton.Text = Localization.Static.Raw(ResourceKeyNames.Static.Submit);
        }

        /// <summary>
        /// Will assign events to the views.
        /// </summary>
        private void assignEvents()
        {
            _editText.TextChanged += editTextTextChanged;
        }

        /// <summary>
        /// Will show how many characters are left.
        /// </summary>
        private void editTextTextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            int currentChars = _max - _editText.Text.Length;
            _charsLeft.Text = currentChars.ToString();

            if (_editText.Text.Length > 15 && !_submitButton.Enabled)
            {
                _submitButton.Enabled = true;
                if (_editText.Text.Length > _max) _editText.Text = _editText.Text.Substring(0, _max);
            }
            else if (_editText.Text.Length < 15 && _submitButton.Enabled) _submitButton.Enabled = false;
        }
    }
}
