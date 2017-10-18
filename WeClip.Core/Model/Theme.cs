using System;

namespace WeClip.Core.Model
{
    public class Theme
    {
        public long ID { get; set; }
        public string Filename { get; set; }
        public string FileUrl { get; set; }
        public string Thumb { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string Name { get; set; }
    }
}
