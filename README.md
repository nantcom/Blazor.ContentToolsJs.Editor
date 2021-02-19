# CoreSharp.ContentToolsJs.Editor
WYSIWYG Editor for Blazor applications - Uses [CotentTools Js](https://getcontenttools.com/ "CotentTools Js.com")

### Why using ContentTools Editor?

Before creating this library, I used to integrate [Blazored.TextEditor](https://github.com/Blazored/TextEditor "Blazored.TextEditor on Github")
nuget (v1.0) in my company web engine which uses [Quill JS](https://quilljs.com/ "Quill JS.com") for editing content. 
I was working on website project call [CoreSharp.Net](https://www.coresharp.net/), 
When found out the following issues:

- `<table>` does not support in Quill Js (v1.3.6)
- Quill Js (or Blazored.TextEditor) insert unnecessary `<p>` tag on loading HTML

Quill Js is a great library but my project is a blog website which needs to be able to insert a table. 
Also, Needing to remove `<p>` tag every time when content become editable is not suitable with a very long content website.


**NOTE** Quill Js will support `<table>` when version 2.0 has been release

### Installation

Install from NuGet using the following command:

`Install-Package CoreSharp.ContentToolsJs.Editor`

Or via the Visual Studio package manger.

### Setup
In Blazor Server applications, Include the following CSS files in `Pages\_Host.cshtml`.

In the `head` tag add the following CSS.

```html
    <link href="/_content/CoreSharp.ContentToolsJs.Editor/content-tools.min.css" rel="stylesheet" />
    <link href="/_content/CoreSharp.ContentToolsJs.Editor/default.min.css" rel="stylesheet" />
```

**NOTE** If style in default.min.css does not work for your website read: 
*Setup standard style for HTML in editable region (Optional)*

Add the following using statement to your main `_Imports.razor`

```cs
@using CoreSharp.ContentToolsJs.Editor
```

Open `StartUp.cs` in your project and add service of `ContentToolsJsInterop` as Scoped
```csharp
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            ...
            services.AddScoped<ContentToolsJsInterop>();
        }
```



## Usage

These are available on the `EditableContent` component.

**Parameters**
- `DataName` (Required) - Name of editable region, Every region name have to be unique.
- `ChildContent` (optional) - initial content of editable region.

**Event**
- `OnSave` (Required) - callback when content in the region has been updated.

Use `EditableContent` component to creat editable region.
```html
<EditableContent DataName="main-content" OnSave="this.Save">
    <blockquote>
        1 Always code as if the guy who ends up maintaining your code will be a violent psychopath who knows where you live.
    </blockquote>
    <p>John F. Woods</p>
</EditableContent>
```

**Sidenote:** ContentTools editor is like page editor rather than rich text editor
- When Edit button has been clicked, all `EditableContent` component become editable content
- When Save button has been clicked, all updated `EditableContent` component will call `OnSave`

Inherit `EditablePage` in every page (component) which using `EditableContent` to enable **edit mode**
```html
@inherits EditablePage
```

To manually **enable/disable edit mode** see *ContentToolsJsInterop*

**NOTE** only one page (component) should be inherit `EditablePage` per HTML web page request otherwise 
inlitilize code will be call more than once.

#### ContentToolsJsInterop

These are available on the `ContentToolsJsInterop` class.

**Methods**
- `Initialize` - Check all Editable region in HTML dom and enable edit button on top left corner.
- `DisableEditMode` - Disable edit button from `Initialize` function.
- `Reset` - Reset editor data, Use when exit `EditablePage` or need to re-`Initialize`



## Configure
This library has been created to matched ContentTools Js feature as much as posible. For setting ControlTools Js configuration can be view below  

### Setup standard style for HTML in editable region (Optional)

ContentTools uses CSS classes to align text, imagery, videos, and iframes (typically used by services
such as YouTube and Vimeo to embed their players). You'll need to define styles for these alignment 
classes in your own CSS.

**NOTE** skip this if `default.min.css` is working out for your website 

In your project, Create `wwwroot\custom-style-editable.css` add the follow class and change style to suit your website

for example look at `wwwroot\default.less` in library for the reference

```css
/* Alignment styles for images, videos and iframes in editable regions */

/* Center (default) */
[data-editable] iframe,
[data-editable] image,
[data-editable] [data-ce-tag=img],
[data-editable] img,
[data-editable] video {
    clear: both;
    display: block;
    margin-left: auto;
    margin-right: auto;
    max-width: 100%;
}

/* Left align */
[data-editable] .align-left {
    clear: initial;
    float: left;
    margin-right: 0.5em;
}

/* Right align */
[data-editable].align-right {
    clear: initial;
    float: right;
    margin-left: 0.5em;
}

/* Alignment styles for text in editable regions */
[data-editable] .text-center {
    text-align: center;
}

[data-editable] .text-left {
    text-align: left;
}

[data-editable] .text-right {
    text-align: right;
}
```

In the `head` tag of `Pages\_Host.cshtml` update as following CSS

```html
    <!- Remove this ->
    <link href="/_content/CoreSharp.ContentToolsJs.Editor/default.min.css" rel="stylesheet" />
    
    <!- And add your own style ->
    <link href="/wwwroot/custom-style-editable.css" rel="stylesheet" />
```

**NOTE** If you're using Blazor WebAssembly then these need to be added to your `wwwroot\index.html`.

### Add CSS class in EditableContent (Optional)

See [reference](https://getcontenttools.com/getting-started#configure-styles)
in ContentTools Js

For example of how to use, see: `EditablePage.cs`

```csharp
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
```