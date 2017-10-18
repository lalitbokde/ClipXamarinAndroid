using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using WeClip.Core.Common;
using Square.Picasso;
using WeClip.Droid.Helper;
using System;
using WeClip.Droid.AsyncTask;

namespace WeClip.Droid.Adapters
{
    public class PhotosFragmentAdapter : BaseAdapter<EventFiles>, AbsListView.IMultiChoiceModeListener
    {
        private Activity context;
        private List<EventFiles> eventfile;
        private IMenuItem menuItem_;
        private ActionMode mode;
        private int NoOfFile;
        private List<EventFiles> tempFile;

        public PhotosFragmentAdapter(Activity context, List<EventFiles> eventfile) : base()
        {
            this.context = context;
            this.eventfile = eventfile;
            tempFile = new List<EventFiles>();
        }

        public override EventFiles this[int position]
        {
            get
            {
                return eventfile[position];
            }
        }

        public override int Count
        {
            get { return eventfile.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            PhotoragmentViewHolder holder;
            if (convertView == null)
            {
                convertView = context.LayoutInflater.Inflate(Resource.Layout.PhotosFragmentItem, parent, false);
                holder = new PhotoragmentViewHolder();
                holder.ivEventPhoto = convertView.FindViewById<ImageView>(Resource.Id.ivPhotosFragItemEventPhoto);
                holder.ivEventVideo = convertView.FindViewById<ImageView>(Resource.Id.ivPhotosFragItemvideo);
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as PhotoragmentViewHolder;
            }

            Picasso.With(context).CancelRequest(holder.ivEventPhoto);
            Picasso.With(context).Load(eventfile[position].Thumb).Placeholder(Resource.Drawable.default_event_back).Transform(new RoundedTransformation())
                .Resize(120, 120).CenterCrop().Into(holder.ivEventPhoto);

            if (eventfile[position].IsVideo)
            {
                holder.ivEventVideo.Visibility = ViewStates.Visible;
            }
            else
            {
                holder.ivEventVideo.Visibility = ViewStates.Gone;
            }
            return convertView;
        }


        public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
        {
            Android.App.AlertDialog.Builder alertDialog = new Android.App.AlertDialog.Builder(context);
            alertDialog.SetTitle("Are you sure ?");
            alertDialog.SetPositiveButton("Delete", (senderAlert, args) =>
            {
                new PostDeleteEventFiles(context, mode, tempFile, eventfile, this).Execute();
            });
            alertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();
            return true;
        }

        public bool OnCreateActionMode(ActionMode mode, IMenu menu)
        {
            mode.MenuInflater.Inflate(Resource.Menu.DeleteMenu, menu);
            this.mode = mode;
            menuItem_ = menu.FindItem(Resource.Id.action_delete);
            return true;
        }

        public void OnDestroyActionMode(ActionMode mode)
        {
        }

        public void OnItemCheckedStateChanged(ActionMode mode, int position, long id, bool @checked)
        {
            var evFile = eventfile[position];

            if (@checked == true)
            {
                tempFile.Add(evFile);
                NoOfFile++;
            }
            else
            {
                tempFile.Remove(evFile);
                NoOfFile--;
            }

            int selectCount = tempFile.Count;

            if (selectCount == 0)
            {
                mode.Finish();
                return;
            }
            mode.Title = NoOfFile + " item selected";
        }

        public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
        {
            return true;
        }


        public class PhotoragmentViewHolder : Java.Lang.Object
        {
            public ImageView ivEventPhoto { get; set; }
            public ImageView ivEventVideo { get; set; }
        }

    }
}
