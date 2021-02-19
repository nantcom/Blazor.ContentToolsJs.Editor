using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreSharp.ContentToolsJs.Editor
{
    public class Style
    {
        public Style(string name, string cssClass, IEnumerable<HtmlTag> applicableHtmlTags = null)
        {
            this.Name = name;
            this.CssClass = cssClass;
            if (applicableHtmlTags == null || applicableHtmlTags.Count() == 0)
                this.ApplicableHtmlTags = new List<HtmlTag>().AsReadOnly();
            else
                this.ApplicableHtmlTags = applicableHtmlTags.ToList().AsReadOnly();
        }

        public string Name { get; private set; }
        public string CssClass { get; private set; }
        public ReadOnlyCollection<HtmlTag> ApplicableHtmlTags { get; private set; }
        public string[] ApplicableTo
        {
            get
            {
                return this.ApplicableHtmlTags.Select(t => t.ToString()).ToArray();
            }
        }
    }

    public enum HtmlTag
    {
        p,
        b,
        pre,
        img,iframe,video,
        h1, h2, h3,
        ul, li,
        table, tr, td, th, tbody, thead, tfoot
    }
}
