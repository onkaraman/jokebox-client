using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Jokebox.Core.Localization;
using JokeBox.Core.Localization;
using JokeBox.UI.Views;

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
        private bool _canSave;
        private MainEditText _editText;
        private MainTextView _charsLeft;
        private MainTextView _nameDescription;
        private Button _saveButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SetName);
            setupViews();
            assignEvents();
        }

        /// <summary>
        /// Will connect the controls to the fields of this class.
        /// </summary>
        private void setupViews()
        {
            _editText = FindViewById<MainEditText>(Resource.Id.NameEditText);
            _charsLeft = FindViewById<MainTextView>(Resource.Id.NameCharsLeft);
            _nameDescription = FindViewById<MainTextView>(Resource.Id.SetNameDescription);
            _saveButton = FindViewById<Button>(Resource.Id.SetNameSaveButton);

            _nameDescription.Text = Localization.Static.Raw(ResourceKeyNames.Static.NameDescription);
            _saveButton.Text = Localization.Static.Raw(ResourceKeyNames.Static.Save);

            _editText.Text = "";
            _charsLeft.Text = "";
            _saveButton.Enabled = false;
        }

        /// <summary>
        /// Will assign events to the controls of this class.
        /// </summary>
        private void assignEvents()
        {
            _editText.TextChanged += editTextTextChanged;
        }

        /// <summary>
        /// Will show how many characters are left to fill the minimum name length.
        /// </summary>
        private void editTextTextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (_editText.Text.Length < 5)
            {
                _canSave = false;
                _saveButton.Enabled = false;
                _charsLeft.Text = string.Format("{0} {1} {2}", Localization.Static.Raw(ResourceKeyNames.Static.CharsLeft),
                                               (5 - _editText.Text.Length), Localization.Static.Raw(ResourceKeyNames.Static.Letters));
            }
            else
            {
                _saveButton.Enabled = true;
                _canSave = true;
                _charsLeft.Text = "";
            }
        }
    }
}
