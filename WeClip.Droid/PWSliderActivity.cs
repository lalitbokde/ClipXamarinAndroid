using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.View;
using WeClip.Droid.Adapters;
using Newtonsoft.Json;
using WeClip.Core.Common;

namespace WeClip.Droid
{
    [Activity( ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class PWSliderActivity : Activity
    {
        ViewPager viewPager;
        string data;
        List<EventFiles> evFileInfo;
       
            protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PWSlider);

            data  = Intent.GetStringExtra("FromPhotoFragment");
            var val = Intent.GetIntExtra("FragmentPosition", 0);
            if(!string.IsNullOrEmpty(data))
            {
                evFileInfo =  JsonConvert.DeserializeObject<List<EventFiles>>(data);
            }

            viewPager = (ViewPager)FindViewById(Resource.Id.viewPager);
            PagerAdapter adapter = new PWSliderAdapter(this, evFileInfo);

            viewPager.Adapter = adapter;
            viewPager.CurrentItem = val;
        }
    }
}