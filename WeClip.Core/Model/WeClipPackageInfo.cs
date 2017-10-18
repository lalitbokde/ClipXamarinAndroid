using System;

namespace WeClip.Core.Model
{
    public class WeClipPackageInfo
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public Nullable<long> Days { get; set; }
    }
}
