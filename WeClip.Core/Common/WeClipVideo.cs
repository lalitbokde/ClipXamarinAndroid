
using System;

namespace WeClip.Core.Common
{
    public class WeClipVideo
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public long EventID { get; set; }
        public string VideoFileName { get; set; }
        public string VideoFileUrl { get; set; }
        public string Thumb { get; set; }
        public string EventName { get; set; }
        public DateTime? EventDate { get; set; }
        public string WeClipTitle { get; set; }
        public string WeClipTag { get; set; }
    }
}
