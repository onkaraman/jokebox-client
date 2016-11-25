using System.Collections.Generic;
using System.Threading;
using Android.App;
using Android.OS;
using Android.Widget;
using API.Accessors;
using JokeBox.API.Models;
using JokeBox.Parsers;

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
        private ProgressBar _progBar;
        private TextView _pointsText;
        private TextView _jokeContent;
        private TextView _upvotes;
        private TextView _downvotes;
        private RelativeLayout _upvoteBox;
        private RelativeLayout _downvoteBox;
        private List<Joke> _jokes;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            setupViews();
            assignEvents();
            getJokes();
        }

        /// <summary>
        /// Will connect the widgets to the private fields.
        /// </summary>
        private void setupViews()
        {
            _progBar = FindViewById<ProgressBar>(Resource.Id.MainProgressBar);
            _pointsText = FindViewById<TextView>(Resource.Id.HeaderPointsValue);
            _jokeContent = FindViewById<TextView>(Resource.Id.JokeText);
            _upvotes = FindViewById<TextView>(Resource.Id.Upvotes);
            _downvotes = FindViewById<TextView>(Resource.Id.Downvotes);
            _upvoteBox = FindViewById<RelativeLayout>(Resource.Id.mainUpvoteBox);
            _downvoteBox = FindViewById<RelativeLayout>(Resource.Id.mainDownvoteBox);
        }

        /// <summary>
        /// Will assign events to the widgets.
        /// </summary>
        private void assignEvents()
        {
            _upvoteBox.Click += updateClicked;
            _downvoteBox.Click += downvoteClicked;
        }

        /// <summary>
        /// Will upvote the current joke.
        /// </summary>
        private void updateClicked (object sender, System.EventArgs e)
        {

        }

        /// <summary>
        /// Will downvote the current joke.
        /// </summary>
        private void downvoteClicked(object sender, System.EventArgs e)
        {

        }

        /// <summary>
        /// Will request the API to get a random set of jokes.
        /// </summary>
        private void getJokes()
        {
            ThreadPool.QueueUserWorkItem(async o =>
            {
                string raw = await APIAccessor.Static.Get("", "DE");
                _jokes = APIParser.Static.ParseGet(raw);
            });
        }
    }
}

