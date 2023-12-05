using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomVision02
{
    public class Info
    {
        public string id { get; set; }
        public string project { get; set; }
        public string iteration { get; set; }
        public string created { get; set; }
        public List<Details> predictions { get; set; }
    }

    public class Details
    {
        public string probability { get; set; }
        public string tagId { get; set; }
        public string tagName { get; set; }
        public Rect boundingBox { get; set; }
    }

    public class Rect
    {
        public double left { get; set; }
        public double top { get; set; }
        public double width { get; set; }
        public double height { get; set; }
    }
}
