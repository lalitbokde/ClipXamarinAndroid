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
using Android.Support.V7.App;
using Calligraphy;

namespace WeClip.Droid.Helper
{
    public class AppComCustomeActivty : AppCompatActivity
    {
        protected override void AttachBaseContext(Context newBase)
        {
            base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newBase));
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            CalligraphyConfig.InitDefault(new CalligraphyConfig.Builder()
             .SetDefaultFontPath("Fonts/Avenir.ttc")
             .SetFontAttrId(Resource.Attribute.fontPath)
             .Build());
        }
    }
}