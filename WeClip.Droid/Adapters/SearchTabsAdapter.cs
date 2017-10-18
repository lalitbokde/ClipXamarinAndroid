using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace WeClip.Droid.Adapters
{
    public class SearchTabsAdapter : FragmentPagerAdapter
    {
        private readonly Fragment[] fragments;
        private readonly ICharSequence[] titles;

        public SearchTabsAdapter(FragmentManager fm, Fragment[] fragments, ICharSequence[] titles) : base(fm)
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

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return fragments[position];
        }
        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return titles[position];
        }
    }
}