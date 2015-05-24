using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Model
{
    public class Nav
    {
        public string Url { get; set; }
        public string StyleCurrent { get; set; }
        public string Style { get; set; }
        public string Title { get; set; }

        public Nav(string url, string styleCurrent, string style, string title)
        {
            this.Url = url;
            this.StyleCurrent = styleCurrent;
            this.Style = style;
            this.Title = title;
        }
    }
}
