using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Square.Picasso;
using WeClip.Core.Common;
using WeClip.Core.Model;
using WeClip.Droid.Helper;

namespace WeClip.Droid.AsyncTask
{
    public class UploadProfilePic : AsyncTask<Java.Lang.Void, Java.Lang.Void, JsonResult>
    {
        private Activity activity;
        private string uploadImagePath;
        private string compressImage;
        JsonResult jsonResult;
        private ProgressDialog progressDialog;
        private UserProfile objUP;
        private ImageView drawerProfile_pic;
        private TextView drawerUsername;

        public UploadProfilePic(Activity activity, string uploadImagePath, ProgressDialog progressDialog, UserProfile objUP, ImageView drawerProfile_pic, TextView drawerUsername)
        {
            this.drawerProfile_pic = drawerProfile_pic;
            this.drawerUsername = drawerUsername;
            this.activity = activity;
            this.uploadImagePath = uploadImagePath;
            jsonResult = new JsonResult();
            this.progressDialog = progressDialog;
            this.objUP = objUP;
        }

        protected override JsonResult RunInBackground(params Java.Lang.Void[] @params)
        {

            Java.IO.File file = new Java.IO.File(uploadImagePath);
            long length = file.Length() / 1024; // Size in KB

            if (length > 500)
            {
                compressImage = CompressFile.compressImage(uploadImagePath);
            }
            else
            {
                compressImage = uploadImagePath;
            }

            jsonResult = RestSharpCall.PostFile<JsonResult>(compressImage, "User/SetPicture?userid=" + GlobalClass.UserID);
            CompressFile.deleteTempFile();
            return jsonResult;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);
            if (result != null && jsonResult != null)
            {
                if (jsonResult.Success == true)
                {
                    var prefs = Application.Context.GetSharedPreferences("WeClip", FileCreationMode.Private);
                    var prefEditor = prefs.Edit();
                    prefEditor.PutString("ProfilePic", jsonResult.ImagePath);
                    prefEditor.Commit();
                    objUP.ProfilePic = jsonResult.ImageName;
                    if (jsonResult.ImageName.Contains(".jpg") || jsonResult.ImageName.Contains(".png"))
                    {
                        Picasso.With(activity).Load(jsonResult.ImagePath).Placeholder(Resource.Drawable.contact_withoutphoto)
             .Transform(new CircleTransformation())
               .Resize(150, 150).Into(drawerProfile_pic);
                    }
                    new UploadProfileData(progressDialog, objUP, activity, drawerUsername).Execute();
                }
            }
            else
            {
                progressDialog.Hide();
            }
        }
    }
}