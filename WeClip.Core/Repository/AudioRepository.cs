using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeClip.Core.Common;
using WeClip.Core.Model;
using WeClip.Core.Service;

namespace WeClip.Core.Repository
{

    public class AudioRepository
    {
        HttpCall call;
        CrashReportService crashReport = new CrashReportService();
        CrashReportModel CR = new CrashReportModel();

        public AudioRepository()
        {
            call = new HttpCall();
        }

        public async Task<List<Audio>> GetAudio(string url)
        {
            List<Audio> lst = null;
            try
            {
                lst = await call.GetAll<Audio>(url);
                return lst;
            }
            catch (Exception ex)
            {
                CR.Filename = "AudioRepository";
                CR.Eventname = "GetAudio";
                CR.UserID = GlobalClass.UserID == null ? "0" : GlobalClass.UserID;
                CR.ErrorMsg = ex.Message + ex.StackTrace;
                await crashReport.PostCrashReport(CR);
                return lst;
            }
        }
    }
}
