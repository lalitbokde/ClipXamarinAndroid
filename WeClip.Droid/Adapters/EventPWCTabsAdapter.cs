using Android.Support.V4.App;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using Java.Lang;

namespace WeClip.Droid.Adapters
{
    class EventPWCTabsAdapter : FragmentPagerAdapter
    {

        private readonly Fragment[] fragments;
        private readonly ICharSequence[] titles;

        public EventPWCTabsAdapter(FragmentManager fm, Fragment[] fragments, ICharSequence[] titles) : base(fm)
        {
            this.fragments = fragments;
            this.titles = titles;
        }

        public override int Count
        {
            get
            {
                return fragments.Length;
            }
        }

        public override Fragment GetItem(int position)
        {
            return fragments[position];
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return titles[position];
        }

    }
}