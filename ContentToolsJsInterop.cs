using CoreSharp.ContentToolsJs.Editor.DotNetHelpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.ContentToolsJs.Editor
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

    public class ContentToolsJsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;
        private EditorHelper editorHelper;
        private DotNetObjectReference<EditorHelper> helperRef;

        public ContentToolsJsInterop(IJSRuntime jsRuntime)
        {
            this.Reset();
            moduleTask = new(async () => {
                await jsRuntime.InvokeAsync<IJSObjectReference>(
                       "import", "./_content/CoreSharp.ContentToolsJs.Editor/content-tools.js");
                return await jsRuntime.InvokeAsync<IJSObjectReference>(
                       "import", "./_content/CoreSharp.ContentToolsJs.Editor/EditorInterop.js").AsTask();
                   });

        }

        private bool isInitialized = false;
        private bool needToSetEditor = true;
        public bool HasStylesBeenset { get; private set; }

        /// <summary>
        /// Styles is suppose to be for site not for some pages
        /// </summary>
        public List<Style> Styles { get; set; }

        /// <summary>
        /// Need to call AddSaveFunction Before Initialize
        /// Initialize will enable edit mode
        /// This function should be call only once per request
        /// </summary>
        /// <returns></returns>
        public async Task Initialize()
        {
            if (isInitialized)
                throw new ApplicationException("this ContentToolsJs is already been initialized.");

            isInitialized = true;
            helperRef = DotNetObjectReference.Create(editorHelper);
            var module = await moduleTask.Value;

            if (needToSetEditor)
            {
                needToSetEditor = false;
                await module.InvokeAsync<object>("setNewEditor");
            }

            if (!HasStylesBeenset && this.Styles != null && Styles.Count > 0)
            {
                HasStylesBeenset = true;
                await module.InvokeAsync<object>("addStylePalette", this.Styles);
            }

            await module.InvokeAsync<object>("initialize", helperRef);
        }

        public void AddCallBackSaveFunction(string regionName, EventCallback<EditorContentSaveEventArgs> callback)
        {
            editorHelper.DataNameToSaveFunctionDict.Add(regionName, callback);
        }

        /// <summary>
        /// after call DisableEditMode, Edit button will disappear
        /// and will need to call Initialize to re-enable edit mode again
        /// </summary>
        /// <returns></returns>
        public async Task DisableEditMode()
        {
            var module = await moduleTask.Value;
            await module.InvokeAsync<object>("destroy");
            editorHelper = new EditorHelper();
            if (helperRef != null)
                helperRef.Dispose();
            helperRef = null;
            needToSetEditor = true;
        }

        public void Reset()
        {
            if (helperRef != null)
                helperRef.Dispose();
            editorHelper = new EditorHelper();
            helperRef = null;
            isInitialized = false;
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
            if (helperRef != null)
                helperRef.Dispose();
            helperRef = null;
            editorHelper = null;
            needToSetEditor = true;
        }
    }
}
