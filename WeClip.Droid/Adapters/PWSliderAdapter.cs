using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using WeClip.Droid.Helper;
using Square.Picasso;
using System;
using Android.Content;
using WeClip.Core.Common;
using WeClip.Core.Model;
using Newtonsoft.Json;
using Android.Media;
using System.Diagnostics;
using WeClip.Droid.Services;

namespace WeClip.Droid.Adapters
{
    public class PWSliderAdapter : PagerAdapter
    {
        Activity context;
        private List<EventFiles> evFileInfo;


        public PWSliderAdapter(Activity context, List<EventFiles> evFileInfo)
        {
            this.evFileInfo = evFileInfo;
            this.context = context;
        }

        public override int Count
        {
            get { return evFileInfo.Count; }
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            LayoutInflater inflater = ((Activity)context).LayoutInflater;
            View viewItem = inflater.Inflate(Resource.Layout.PWSilderItem, container, false);

            ImageView imageView = (ImageView)viewItem.FindViewById(Resource.Id.imageView);
            ImageView imageViewOver = (ImageView)viewItem.FindViewById(Resource.Id.imageViewOver);

            Picasso.With(context).CancelRequest(imageView);
            float rotaiomangle = 0.0f;
            var orientation = MediaFileExtensions.GetRotation(evFileInfo[position].Thumb);
            switch (orientation)
            {
                case 6:
                    rotaiomangle = 90.0f;
                    break;
                case 3:
                    rotaiomangle = 180.0f;
                    break;
                   
                case 8:
                    rotaiomangle = 270.0f;
                    break;
                default:
                    rotaiomangle = 0.0f;
                    break;
            }

            

            Picasso.With(context).Load(evFileInfo[position].Thumb).Rotate(rotaiomangle).Fit().CenterInside().Placeholder(Resource.Drawable.default_event_back)
          .Into(imageView);

            if (evFileInfo[position].IsVideo)
            {
                imageViewOver.Visibility = ViewStates.Visible;
                imageViewOver.SetOnClickListener(new itemClickListner(evFileInfo[position], context, imageViewOver));

            }
            else
            {
                imageViewOver.Visibility = ViewStates.Gone;
            }
            ((ViewPager)container).AddView(viewItem);

            return viewItem;
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object objectValue)
        {
            return view == ((View)objectValue);
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object objectValue)
        {
            ((ViewPager)container).RemoveView((View)objectValue);
        }

        private class itemClickListner : Java.Lang.Object, View.IOnClickListener
        {
            private Activity context;
            private ImageView imageView;
            private EventFiles evFileInfo;

            public itemClickListner(EventFiles evFileInfo, Activity context, ImageView imageView)
            {
                this.evFileInfo = evFileInfo;
                this.context = context;
                this.imageView = imageView;
            }

            public void OnClick(View v)
            {
                Intent pwcActivity = new Intent(context, typeof(VideoPlayerActivity));
                pwcActivity.PutExtra("VideoUrlFromEvent", JsonConvert.SerializeObject(evFileInfo));
                context.StartActivity(pwcActivity);
            }
        }
    }
}