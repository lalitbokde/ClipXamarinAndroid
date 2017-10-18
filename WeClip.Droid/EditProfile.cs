using System;
using Android.App;
using Android.OS;
using Android.Widget;
using WeClip.Core.Model;
using WeClip.Core.Common;
using WeClip.Droid.Helper;
using Android.Content;
using Android.Provider;
using Android.Views;
using WeClip.Droid.AsyncTask;
using Square.Picasso;
using Android.Util;
using Fragment = Android.Support.V4.App.Fragment;
using WeClip.Droid.ImageResizer;

namespace WeClip.Droid
{
    public class EditProfile : Fragment
    {
        protected ImageButton imgButton;
        protected EditText etBio, userName, emailId;
        protected string strName, strUserName, strEmailId;
        protected UserProfile profile;
        protected ImageView ivProfilePic;
        protected ProgressDialog progressDialog;
        protected static int CAMERA_REQUEST = 500;
        protected static int GALLERY_PICTURE = 1443;
        protected static int RESIZE_IMAGE = 1001;
        private ImageToPath imageToPath;
        private View rootView;
        private TextView changeProfile;
        private string uploadImagePath;
        Android.Net.Uri selectedImagePath;
        private string strBio;
        private ImageView drawerProfile_pic;
        private TextView drawerUsername;
        private ISharedPreferences prefs;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            TextView toolbar_title = (TextView)Activity.FindViewById(Resource.Id.toolbar_title);

            rootView = inflater.Inflate(Resource.Layout.EditProfile, container, false);
            HasOptionsMenu = true;
            ivProfilePic = rootView.FindViewById<ImageView>(Resource.Id.ivProfilepic);
            changeProfile = rootView.FindViewById<TextView>(Resource.Id.tvChangeProfilePic);
            imgButton = rootView.FindViewById<ImageButton>(Resource.Id.btnSaveEditProfile);
            prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
            GlobalClass.UserName = prefs.GetString("UserName", "");
            GlobalClass.UserEmail = prefs.GetString("EmailID", "");
            GlobalClass.DOB = prefs.GetString("DOB", null);
            GlobalClass.ProfilePicture = prefs.GetString("ProfilePic", "");
            strBio = prefs.GetString("Bio", null);
            setProfilePic();
            imgButton.Click += ImgButton_Click;
            changeProfile.Click += ChangeProfile_Click;
            ivProfilePic.Click += IvProfilePic_Click;

            Activity.Title = "";
            toolbar_title.Text = "Edit your profile";
            return rootView;
        }

        private void IvProfilePic_Click(object sender, EventArgs e)
        {
            string[] items = { "Take photo", "Choose from gallery", "Cancel" };
            Android.App.AlertDialog.Builder alertDialog = new Android.App.AlertDialog.Builder(Activity);
            alertDialog.SetTitle("Upload profile picture");
            alertDialog.SetItems(items, new dialogeItemClickListner(this));
            alertDialog.Show();
        }

        public override void OnStart()
        {
            base.OnStart();
            imageToPath = new ImageToPath();
            etBio = rootView.FindViewById<EditText>(Resource.Id.etBio);
            userName = rootView.FindViewById<EditText>(Resource.Id.etUserName);
            emailId = rootView.FindViewById<EditText>(Resource.Id.etUserEmail);
            SetProfileInfo();
            drawerProfile_pic = (ImageView)Activity.FindViewById(Resource.Id.profile_pic);
            drawerUsername = (TextView)Activity.FindViewById(Resource.Id.tvUsername);
        }

        private void setProfilePic()
        {
            if (string.IsNullOrEmpty(GlobalClass.ProfilePicture))
            {
                ivProfilePic.SetImageResource(Resource.Drawable.contact_withoutphoto);
            }
            else
            {
                Picasso.With(Activity).Load(GlobalClass.ProfilePicture)
                             .Transform(new CircleTransformation())
                               .Resize(150, 150).Into(ivProfilePic);
            }
        }

        private void ChangeProfile_Click(object sender, EventArgs e)
        {
            string[] items = { "Take photo", "Choose from gallery", "Cancel" };
            Android.App.AlertDialog.Builder alertDialog = new Android.App.AlertDialog.Builder(Activity);
            alertDialog.SetTitle("Upload profile picture");
            alertDialog.SetItems(items, new dialogeItemClickListner(this));
            alertDialog.Show();
        }

        private void SetProfileInfo()
        {
            progressDialog = ProgressDialog.Show(Activity, "Please wait", "Loaing data");
            emailId.Text = GlobalClass.UserEmail;
            userName.Text = GlobalClass.UserName;
            etBio.Text = strBio;
            int pos = userName.Text.Length;
            userName.SetSelection(pos);
            progressDialog.Hide();
        }

        //public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        //{

        //    if ((requestCode == CAMERA_REQUEST) && (resultCode == (int)Result.Ok))
        //    {
        //        try
        //        {
        //            ivProfilePic.SetImageResource(0);
        //            int height = ivProfilePic.Height;
        //            int width = ivProfilePic.Width;
        //            BitmapHelper.bitmap = BitmapHelper._file.Path.LoadAndResizeBitmap(width, height);
        //            uploadImagePath = BitmapHelper._file.AbsolutePath;

        //            if (BitmapHelper.bitmap != null)
        //            {
        //                ivProfilePic.SetImageBitmap(BitmapHelper.bitmap);
        //                BitmapHelper.bitmap = null;
        //            }
        //            // Dispose of the Java side bitmap.
        //            GC.Collect();
        //        }
        //        catch (Java.Lang.Exception ex)
        //        {
        //            new CrashReportAsync("EditProfile", "OnActivityResult", ex.Message + ex.StackTrace).Execute();
        //        }
        //    }
        //    if ((requestCode == GALLERY_PICTURE) && (resultCode == (int)Result.Ok) && (data != null))
        //    {
        //        Log.Debug("OnActivityResult", "FromFragmentCalled");

        //        if (data.Data != null)
        //        {
        //            ivProfilePic.SetImageResource(0);
        //            selectedImagePath = data.Data;
        //            uploadImagePath = MediaHelper.GetMediaPath(Activity.ApplicationContext, selectedImagePath);

        //            Picasso.With(Activity).Load(selectedImagePath)
        //          .Transform(new CircleTransformation())
        //            .Resize(150, 150).Into(ivProfilePic);
        //        }
        //    }
        //}

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if ((requestCode == CAMERA_REQUEST) && (resultCode == (int)Result.Ok))
            {
                if (BitmapHelper._file != null && BitmapHelper._file.Path != null)
                {
                    OpenCropView(BitmapHelper._file.Path);
                }
            }
            else if ((requestCode == GALLERY_PICTURE) && (resultCode == (int)Result.Ok))
            {
                if (data != null && data.Data != null)
                {
                    OpenCropView(MediaHelper.GetMediaPath(this.Context, data.Data));
                }
            }
            else if (requestCode == RESIZE_IMAGE && resultCode == (int)Result.Ok)
            {
                if (data != null && data.Data != null)
                {
                    ivProfilePic.SetImageResource(0);
                    selectedImagePath = data.Data;
                    uploadImagePath = MediaHelper.GetMediaPath(this.Context, selectedImagePath);

                    Picasso.With(Activity)
                           .Load(selectedImagePath)
                           .Transform(new CircleTransformation())
                           .Resize(150, 150)
                           .Into(ivProfilePic);
                }
            }
        }

        private void OpenCropView(string imagePath)
        {
            if (imagePath == "") return;

            Intent intent = new Intent(this.Context, typeof(CropImageActivity));
            intent.PutExtra("image-path", imagePath);
            intent.PutExtra("scale", true);
            StartActivityForResult(intent, RESIZE_IMAGE);
        }

        private void ImgButton_Click(object sender, EventArgs e)
        {
            try
            {
                HideKeyBoard.hideSoftInput(Activity);
                UserProfile objUP = new UserProfile();
                objUP.UserName = userName.Text.Trim();
                objUP.Bio = etBio.Text.Trim();

                if (!string.IsNullOrEmpty(uploadImagePath))
                {
                    progressDialog.Show();
                    progressDialog.SetMessage("Profile updating...");
                    var uploadfile = new UploadProfilePic(Activity, uploadImagePath, progressDialog, objUP, drawerProfile_pic, drawerUsername);
                    uploadfile.Execute();
                }
                else
                {
                    progressDialog.Show();
                    progressDialog.SetMessage("Profile updating...");
                    new UploadProfileData(progressDialog, objUP, Activity, drawerUsername).Execute();
                }
            }
            catch (Java.Lang.Exception ex)
            {
                new CrashReportAsync("EditProfile", "ImgButton_Click", ex.Message + ex.StackTrace).Execute();
            }
        }

        private class dialogeItemClickListner : Java.Lang.Object, IDialogInterfaceOnClickListener
        {
            private EditProfile context;

            public dialogeItemClickListner(EditProfile context)
            {
                this.context = context;
            }

            public void OnClick(IDialogInterface dialog, int which)
            {
                try
                {
                    if (which == 0)
                    {
                        Intent intent = new Intent(MediaStore.ActionImageCapture);
                        BitmapHelper._file = ImageToPath.GetOutputMediaFile(ImageToPath.UploadFileType.image);
                        BitmapHelper.captureImageUri = Android.Net.Uri.FromFile(BitmapHelper._file);
                        intent.PutExtra(MediaStore.ExtraOutput, BitmapHelper.captureImageUri);
                        context.StartActivityForResult(intent, CAMERA_REQUEST);
                    }
                    else if (which == 1)
                    {
                        Intent pictureActionIntent = null;
                        pictureActionIntent = new Intent(Intent.ActionPick, Android.Provider.MediaStore.Images.Media.ExternalContentUri);
                        context.StartActivityForResult(pictureActionIntent, GALLERY_PICTURE);
                    }
                }
                catch (Java.Lang.Exception ex)
                {
                    new CrashReportAsync("dialogeItemClickListner", "OnClick", ex.Message + ex.StackTrace).Execute();
                }
            }
        }
    }
}