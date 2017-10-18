using Android.Views;
using Android.Support.V7.Widget;
using Android.Graphics;

namespace WeClip.Droid.Helper
{
    public class VerticalSpaceItemDecoration : RecyclerView.ItemDecoration
    {

        private int mVerticalSpaceHeight;

        public VerticalSpaceItemDecoration(int mVerticalSpaceHeight)
        {
            this.mVerticalSpaceHeight = mVerticalSpaceHeight;
        }

        public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
        {
            if (parent.GetChildAdapterPosition(view) != parent.GetAdapter().ItemCount - 1)
            {
                outRect.Bottom = mVerticalSpaceHeight;
            }

            if (parent.GetChildAdapterPosition(view) == parent.GetAdapter().ItemCount - 1)
            {
                outRect.Bottom = 70;
            }
        }
    }
}