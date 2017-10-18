using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using WeClip.Core.Model;
using WeClip.Core.Common;
using Square.Picasso;

namespace WeClip.Droid.Adapters
{
    class MusicViewAdapter : BaseAdapter<Audio>
    {
        private MusicView activity;
        private List<Audio> audioList;
        private bool[] isPlaying;
        private int previousPlaying = -1, mPosition = -1;
        private bool isFirstTime = true;

        public MusicViewAdapter(List<Audio> audioList, MusicView activity)
        {
            this.audioList = audioList;
            this.activity = activity;
            isPlaying = new bool[audioList.Count];
            for (int i = 0; i < isPlaying.Length; i++)
            {
                isPlaying[i] = false;
            }
        }

        public override Audio this[int position]
        {
            get
            {
                return audioList[position];
            }
        }

        public override int Count
        {
            get
            {
                return audioList.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            AudioViewHolder holder;
            var item = audioList[position];
            if (convertView == null)
            {
                convertView = activity.LayoutInflater.Inflate(Resource.Layout.AudioViewItem, parent, false);
                holder = new AudioViewHolder();
                holder.Audioname = convertView.FindViewById<TextView>(Resource.Id.tvAudioViewName);
                holder.Audiopicture = convertView.FindViewById<ImageView>(Resource.Id.ivAduioViewItemPic);
                holder.PlayButton = convertView.FindViewById<ImageView>(Resource.Id.ivAduioViewPlay);
                holder.PlayButton.Click += delegate
                {
                    mPosition = position;
                    if (position != previousPlaying)
                    {

                        if (!isPlaying[position])
                        {
                            isPlaying[position] = true;
                            holder.PlayButton.SetImageResource(Resource.Drawable.pause);
                            activity.playAudioFile(audioList[position].FileUrl, true);
                        }

                        if (!isFirstTime)
                        {
                            isPlaying[previousPlaying] = false;
                        }
                        else
                        {
                            isFirstTime = false;
                        }
                    }
                    else
                    {
                        if (isPlaying[position])
                        {
                            isPlaying[position] = false;
                            holder.PlayButton.SetImageResource(Resource.Drawable.play_little);
                            activity.playAudioFile("", false);
                        }
                        else
                        {
                            isPlaying[position] = true;
                            holder.PlayButton.SetImageResource(Resource.Drawable.pause);
                            activity.playAudioFile(audioList[position].FileUrl, true);
                        }
                    }
                    previousPlaying = position;
                    NotifyDataSetChanged();
                };
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as AudioViewHolder;
            }

            if (!isPlaying[position])
            {
                holder.PlayButton.SetImageResource(Resource.Drawable.play_little);
            }
            else
            {
                holder.PlayButton.SetImageResource(Resource.Drawable.pause);
            }

            holder.Audioname.Text = item.Name;
            Picasso.With(activity).Load(item.Thumb).Placeholder(Resource.Drawable.default_event_back)
           .Resize(80, 80).Into(holder.Audiopicture);
            return convertView;
        }
    }
}

public class AudioViewHolder : Java.Lang.Object
{
    public ImageView Audiopicture { get; set; }
    public ImageView PlayButton { get; set; }
    public TextView Audioname { get; set; }
}
