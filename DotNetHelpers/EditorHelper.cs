using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NUglify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreSharp.ContentToolsJs.Editor.DotNetHelpers
{
    public class EditorHelper
    {
        public EditorHelper()
        {
            this.DataNameToSaveFunctionDict = new Dictionary<string, EventCallback<EditorContentSaveEventArgs>>();
        }

        public Dictionary<string, EventCallback<EditorContentSaveEventArgs>> DataNameToSaveFunctionDict { get; private set; }

        [JSInvokable]
        public async Task<bool> SaveChangesAsync(List<string> dataNames, List<string> updatedHtmlContents)
        {
            for (int i = 0; i < dataNames.Count; i++)
            {
                await this.SaveAsync(dataNames[i], updatedHtmlContents[i]);
            }

            return true;
        }

        public async Task SaveAsync(string dataName, string updatedHtmlContent)
        {
            if (!this.DataNameToSaveFunctionDict.ContainsKey(dataName))
                throw new ApplicationException($"Save function for region: {dataName} has not been set.");

            var saveFunction = this.DataNameToSaveFunctionDict[dataName];

            var result = Uglify.Html(updatedHtmlContent);
            await saveFunction.InvokeAsync(
                new EditorContentSaveEventArgs(dataName, result.Code));
        }
    }
}
