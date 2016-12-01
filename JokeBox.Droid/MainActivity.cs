using System.Collections.Generic;
using System.Threading;
using Android.App;
using Android.OS;
using Android.Widget;
using Android.Views;
using API.Accessors;
using JokeBox.API.Models;
using JokeBox.Parsers;
using JokeBox.UI.Views;
using JokeBox.Core.Localization;
using Jokebox.Core.Localization;
using JokeBox.Droid.Activities;
using JokeBox.Core.Managers;
using JokeBox.Persistence;
using JokeBox.Core.Persistence.Models;

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
        private int _jokeIndex;
        private string _username;
        private ProgressBar _progBar;
        private ImageView _dots;
        private MainTextView _pointsText;
        private MainTextView _jokeContent;
        private MainTextView _upvotes;
        private MainTextView _downvotes;
        private RelativeLayout _upvoteBox;
        private RelativeLayout _downvoteBox;
        private List<Joke> _jokes;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            _jokeIndex = 0;
            DBManager.Static.Init(new DBConnection());

            setupViews();
            assignEvents();
            hideJokeUI();
            getJokes();
        }

        /// <summary>
        /// Will check wether a username is saved. If so, the current points
        /// of the user will be downloaded.
        /// </summary>
        private void checkUsername()
        {
            if (!DBManager.Static.DBAccessor.IsEmpty<SimpleItem>())
            {
                SimpleItem si = DBManager.Static.DBAccessor.Select<SimpleItem>()[0];
                if (si.Name.Equals("username"))
                {
                    _username = si.Value;
                    getPoints();
                }
            }
        }

        /// <summary>
        /// Will download and display the current points of the user.
        /// </summary>
        private void getPoints()
        {
            ThreadPool.QueueUserWorkItem(async o =>
            {
                string raw = await APIAccessor.Static.GetPoints("Areon",
                             Localization.Static.Raw(ResourceKeyNames.Static.CountryCode));
                int points = APIParser.Static.ParsePoints(raw);

                RunOnUiThread(() =>
                   {
                       _pointsText.Text = points.ToString();
                   });
            });
        }

        /// <summary>
        /// Will connect the widgets to the private fields.
        /// </summary>
        private void setupViews()
        {
            _progBar = FindViewById<ProgressBar>(Resource.Id.MainProgressBar);
            _dots = FindViewById<ImageView>(Resource.Id.MainDots);
            _pointsText = FindViewById<MainTextView>(Resource.Id.HeaderPointsValue);
            _jokeContent = FindViewById<MainTextView>(Resource.Id.JokeText);
            _jokeContent.SetTextIsSelectable(true);
            _upvotes = FindViewById<MainTextView>(Resource.Id.Upvotes);
            _downvotes = FindViewById<MainTextView>(Resource.Id.Downvotes);
            _upvoteBox = FindViewById<RelativeLayout>(Resource.Id.UpvoteBox);
            _downvoteBox = FindViewById<RelativeLayout>(Resource.Id.DownvoteBox);
        }

        /// <summary>
        /// Will assign events to the widgets.
        /// </summary>
        private void assignEvents()
        {
            _dots.Click += dotsClick;
            _upvoteBox.Click += upvoteClicked;
            _downvoteBox.Click += downvoteClicked;
        }

        /// <summary>
        /// Will show a context Menu once the menu dots are clicked.
        /// </summary>
        private void dotsClick(object sender, System.EventArgs e)
        {
            PopupMenu pm = new PopupMenu(this, _dots);
            pm.Menu.Add(Localization.Static.Raw(ResourceKeyNames.Static.Tell));
            pm.Menu.Add(Localization.Static.Raw(ResourceKeyNames.Static.About));
            pm.MenuItemClick += dotsClicked;
            pm.Show();
        }

        /// <summary>
        /// Will open an activity to set a username if none is set, otherwise the
        /// compose or about activity will be started.
        /// </summary>
        private void dotsClicked(object sender, PopupMenu.MenuItemClickEventArgs e)
        {
            if (e.Item.ToString().Equals(Localization.Static.Raw(ResourceKeyNames.Static.Tell)))
            {
                if (DBManager.Static.DBAccessor.IsEmpty<SimpleItem>())
                {
                    StartActivity(typeof(SetNameActivity));
                }
                else 
                {
                    StartActivity(typeof(ComposeActivity));    
                }
            }
            if (e.Item.ToString().Equals(Localization.Static.Raw(ResourceKeyNames.Static.About)))
            {
                StartActivity(typeof(AboutActivity));
            }
        }

        /// <summary>
        /// Will upvote the current joke.
        /// </summary>
        private void upvoteClicked(object sender, System.EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                APIAccessor.Static.Vote(true, _jokes[_jokeIndex].id, "DE");
            });
            _jokeIndex += 1;
            showJoke();
        }

        /// <summary>
        /// Will downvote the current joke.
        /// </summary>
        private void downvoteClicked(object sender, System.EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                APIAccessor.Static.Vote(false, _jokes[_jokeIndex].id, "DE");
            });
            _jokeIndex += 1;
            showJoke();
        }

        /// <summary>
        /// Will request the API to get a random set of jokes.
        /// </summary>
        private void getJokes()
        {
            ThreadPool.QueueUserWorkItem(async o =>
            {
                string raw = await APIAccessor.Static.Get("",
                                 Localization.Static.Raw(ResourceKeyNames.Static.CountryCode));
                _jokes = APIParser.Static.ParseGet(raw);

                showJoke();
                checkUsername();
            });
        }

        /// <summary>
        /// Will hide the joke UI (content, vote buttons).
        /// </summary>
        private void hideJokeUI()
        {
            _jokeContent.Visibility = Android.Views.ViewStates.Invisible;
            _upvoteBox.Visibility = Android.Views.ViewStates.Invisible;
            _downvoteBox.Visibility = Android.Views.ViewStates.Invisible;
        }

        private void showJokeUI()
        {
            _jokeContent.Visibility = Android.Views.ViewStates.Visible;
            _upvoteBox.Visibility = Android.Views.ViewStates.Visible;
            _downvoteBox.Visibility = Android.Views.ViewStates.Visible;
        }

        /// <summary>
        /// Will show the current joke.
        /// </summary>
        private void showJoke()
        {
            RunOnUiThread(() =>
            {
                showJokeUI();
                _progBar.Visibility = Android.Views.ViewStates.Invisible;

                _jokeContent.Text = _jokes[_jokeIndex].content;
                _upvotes.Text = _jokes[_jokeIndex].upvotes.ToString();
                _downvotes.Text = _jokes[_jokeIndex].downvotes.ToString();
            });
        }
    }
}

