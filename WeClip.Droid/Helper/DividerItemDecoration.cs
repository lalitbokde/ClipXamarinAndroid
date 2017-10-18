using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Graphics.Drawables;
using Android.Content.Res;
using Android.Support.V4.Content;
using Android.Graphics;

namespace WeClip.Droid.Helper
{
    class DividerItemDecoration : RecyclerView.ItemDecoration
    {
        private static int[] ATTRS = new int[] { Android.Resource.Attribute.ListDivider };
        private Drawable mDivider;

        /**
         * Default divider will be used
         */
        public DividerItemDecoration(Context context)
        {
            TypedArray styledAttributes = context.ObtainStyledAttributes(ATTRS);
            mDivider = styledAttributes.GetDrawable(0);
            styledAttributes.Recycle();
        }

        public override void OnDraw(Canvas cValue, RecyclerView parent, RecyclerView.State state)
        {
            int left = parent.PaddingLeft;
            int right = parent.Width - parent.PaddingRight;

            int childCount = (parent.ChildCount -1);
            for (int i = 0; i < childCount; i++)
            {
                View child = parent.GetChildAt(i);

                RecyclerView.LayoutParams p = (RecyclerView.LayoutParams)child.LayoutParameters;

                int top = child.Bottom + p.BottomMargin;
                int bottom = top + mDivider.IntrinsicHeight;
                mDivider.SetBounds(left, top, right, bottom);
                mDivider.Draw(cValue);
            }
        }
    }
}
