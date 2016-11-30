using Android.Widget;
using Android.App;
using Android.Content;
using Android.Util;
using JokeBox.Droid;

namespace JokeBox.UI.Views
{
    public class MainEditText : EditText
    {
        private Activity _activity;

        public MainEditText(Context context) : base(context)
        {
            InitializeView(context);
        }

        public MainEditText (Context context, IAttributeSet attrs) 
            : base(context, attrs)
        {
            InitializeView(context);
        }

        public MainEditText (Context context, IAttributeSet attrs, int defStyle) 
            : base(context, attrs, defStyle)
        {
            InitializeView(context);
        }

        private void InitializeView(Context context)
        {
            _activity = (Activity)context;
            this.Typeface = Android.Graphics.Typeface.CreateFromAsset(Context.Assets, "fonts/segoeui.ttf");
        }
    }
}

