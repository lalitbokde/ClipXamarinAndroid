using System.Threading.Tasks;
using WeClip.Core.Common;
using WeClip.Core.Model;
using WeClip.Core.Repository;

namespace WeClip.Core.Service
{
    public class CrashReportService
    {

        private static CrashRepository repo = new CrashRepository();
        public Task<JsonResult> PostCrashReport(CrashReportModel ev)
        {
            return  repo.SendCrashReport(ev);
        }
    }
}
