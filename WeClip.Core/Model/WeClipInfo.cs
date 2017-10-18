using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeClip.Core.Model
{
    public class WeClipInfo
    {
        public List<ImageInfo> MediaFile { get; set; }
        public int AudioId { get; set; }
        public string Title { get; set; }
        public string Tag { get; set; }
        public long EventID { get; set; }
        public bool IsDefault { get; set; }
        public long ThemeID { get; set; }

    }
}
