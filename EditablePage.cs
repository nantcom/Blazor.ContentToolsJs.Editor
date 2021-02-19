using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreSharp.ContentToolsJs.Editor
{
    public class EditablePageBase : ComponentBase, IDisposable
    {
        [Inject]
        public ContentToolsJsInterop contentToolsEditor { get; set; }

        public void Dispose()
        {
            contentToolsEditor.Reset();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
                await contentToolsEditor.Initialize();
        }
    }

    public class EditablePage : EditablePageBase
    {
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !contentToolsEditor.HasStylesBeenset)
            {
                var mediaTags = new List<HtmlTag>() { HtmlTag.img, HtmlTag.iframe, HtmlTag.video };
                var tableTags = new List<HtmlTag>() { HtmlTag.table };
                contentToolsEditor.Styles = new List<Style>()
                    {
                        new Style("Align Left", "align-left", mediaTags),
                        new Style("Align Right", "align-right", mediaTags),
                        new Style("Full Width", "table-full-width", tableTags),
                        new Style("Layout Fixed", "table-layout-fixed", tableTags),
                        new Style("Layout Auto", "table-layout-auto", tableTags),
                        new Style("Content Align Top", ".table-content-alight-top", tableTags),
                    };
            }

            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
