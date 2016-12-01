using Android.Widget;
using Android.Content;
using Android.Graphics;
using Android.Util;

namespace JokeBox.UI.Views
{
    public class MainTextView : TextView
    {
        public Typeface typeface;

        public MainTextView (Context context) : base(context, null)
        {
            InitializeView(context);
        }

        public MainTextView (Context context, IAttributeSet attrs) : base(context, attrs)
        {
            InitializeView(context);
        }

        public MainTextView (Context context, IAttributeSet attrs, int defStyle) : base(context,attrs,defStyle)
        {
            InitializeView(context);
        }

        private void InitializeView(Context context)
        {
            this.Typeface = Typeface.CreateFromAsset(Context.Assets, "fonts/segoeui.ttf");
        }

        public void MakeBold()
        {
            this.Typeface = Typeface.CreateFromAsset(Context.Assets, "fonts/segoeui-bold.ttf");
        }

        public void MakeRegular()
        {
            this.Typeface = Typeface.CreateFromAsset(Context.Assets, "fonts/segoeui.ttf");
        }

        public void ChangeFontSize(float size)
        {
            this.SetTextSize(ComplexUnitType.Sp, size);
        }
    }
}

