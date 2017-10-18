using System;
using Android.OS;
using WeClip.Core.Common;
using WeClip.Core.Model;
using WeClip.Droid.Helper;

namespace WeClip.Droid.AsyncTask
{
    public class CrashReportAsync : AsyncTask<Java.Lang.Object, Java.Lang.Object, JsonResult>
    {
        private string fileName;
        private string methodName;
        private string error;
        CrashReportModel cr;
        JsonResult jResult;

        public CrashReportAsync(string fileName, string methodName, string error)
        {
            cr = new CrashReportModel();
            this.fileName = fileName;
            this.methodName = methodName;
            this.error = error;
            jResult = new JsonResult();
        }

        protected override void OnPreExecute()
        {
            base.OnPreExecute();
            cr.Filename = fileName;
            cr.Eventname = methodName;
            cr.UserID = GlobalClass.UserID;
            cr.ErrorMsg = error;
            cr.CreateDate = DateTime.Now.ToLongDateString();
        }

        protected override JsonResult RunInBackground(params Java.Lang.Object[] @params)
        {
            jResult = RestSharpCall.Post<JsonResult>(cr, "Log/CrashReport");
            return jResult;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);
        }
    }
}