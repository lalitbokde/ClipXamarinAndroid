using Android.Content;
using Android.Runtime;
using Android.Text;
using Android.Text.Method;
using Android.Text.Style;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.ComponentModel;

namespace WeClip.Droid.Controls
{
    [DesignTimeVisible(true)]
    public class ExpandableTextView : TextView, ViewTreeObserver.IOnGlobalLayoutListener
    {
        private static readonly string ELLIPSIZE = "...";
        private static readonly string MORE = "more";
        private static readonly string LESS = "less";

        private string mFullText;
        private int mMaxLines;

        public ExpandableTextView(Context context) : base(context)
        { }

        public ExpandableTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        { }

        protected ExpandableTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        { }

        public void MakeExpandable(int maxLines)
        {
            MakeExpandable(this.Text, maxLines);
        }

        public void MakeExpandable(string fullText, int maxLines)
        {
            mFullText = fullText;
            mMaxLines = maxLines;

            ViewTreeObserver vto = ViewTreeObserver;
            vto.AddOnGlobalLayoutListener(this);
        }

        public void OnGlobalLayout()
        {
            ViewTreeObserver vto = ViewTreeObserver;
            vto.RemoveOnGlobalLayoutListener(this);

            if (this.LineCount <= mMaxLines)
            {
                SetText(mFullText, BufferType.Normal);
            }
            else
            {
                MovementMethod = LinkMovementMethod.Instance;
                ShowLess();
            }
        }

        private void ShowLess()
        {
            int lineEndIndex = Layout.GetLineEnd(mMaxLines - 1);
            string newText = mFullText.Substring(0, lineEndIndex - (ELLIPSIZE.Length + MORE.Length + 1)) + ELLIPSIZE + " " + MORE;

            SpannableStringBuilder builder = new SpannableStringBuilder(newText);
            builder.SetSpan(new MoreClickableSpan(this), newText.Length - MORE.Length, newText.Length, 0);
            SetText(builder, BufferType.Spannable);
        }

        private void ShowMore()
        {
            SpannableStringBuilder builder = new SpannableStringBuilder(mFullText + " " + LESS);
            builder.SetSpan(new LessClickableSpan(this), builder.Length() - LESS.Length, builder.Length(), 0);
            SetText(builder, BufferType.Spannable);
        }

        private class MoreClickableSpan : ClickableSpan
        {
            ExpandableTextView _expandableTextView;

            public MoreClickableSpan(ExpandableTextView expandableTextView)
            {
                _expandableTextView = expandableTextView;
            }

            public override void OnClick(View widget)
            {
                _expandableTextView.ShowMore();
            }
        }

        private class LessClickableSpan : ClickableSpan
        {
            ExpandableTextView _expandableTextView;

            public LessClickableSpan(ExpandableTextView expandableTextView)
            {
                _expandableTextView = expandableTextView;
            }

            public override void OnClick(View widget)
            {
                _expandableTextView.ShowLess();
            }
        }

    }
}