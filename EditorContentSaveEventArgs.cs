using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreSharp.ContentToolsJs.Editor
{
    public class EditorContentSaveEventArgs : EventArgs
    {
        public EditorContentSaveEventArgs(string regionName, string htmlContent) : base()
        {
            this.RegionName = regionName;
            this.HtmlContent = htmlContent;
        }

        public string RegionName { get; private set; }
        public string HtmlContent { get; private set; }
    }
}
