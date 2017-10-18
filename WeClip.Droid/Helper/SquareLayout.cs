using Android.Content;
using Android.Widget;
using Android.Util;

namespace WeClip.Droid.Helper
{
    public class SquareLayout : ImageView
    {
        public SquareLayout(Context context) : base(context)
        {
        }

        public SquareLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public SquareLayout(Context context, IAttributeSet attrs, int defStyleAttr)

            : base(context, attrs, defStyleAttr)
        {
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, widthMeasureSpec);
            SetMeasuredDimension(MeasuredWidth, MeasuredWidth);
        }
    }
}