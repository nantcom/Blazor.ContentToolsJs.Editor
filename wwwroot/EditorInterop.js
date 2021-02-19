// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

var editor;
var dotNetHelper;

export function setNewEditor() {
    editor = ContentTools.EditorApp.get();
    editor.addEventListener('saved', function (ev) {
        var name, regions;

        // Check that something changed
        regions = ev.detail().regions;
        if (Object.keys(regions).length == 0) {
            return;
        }

        // Set the editor as busy while we save our changes
        this.busy(true);

        // Collect the contents of each region into a FormData instance
        var dataNames = [];
        var updatedContents = [];
        for (name in regions) {
            if (regions.hasOwnProperty(name)) {
                dataNames.push(name);
                updatedContents.push(regions[name]);
            }
        }

        dotNetHelper.invokeMethodAsync('SaveChangesAsync', dataNames, updatedContents)
            .then(isSucess => {
                this.busy(false);
                if (isSucess) {
                    new ContentTools.FlashUI('ok');
                }
                else {
                    new ContentTools.FlashUI('no');
                }
            });;
    });
}

export function initialize(saveFuncHelper) {
    editor.init('*[data-editable]', 'data-name');
    dotNetHelper = saveFuncHelper;
}

export function addStylePalette(dotNetStyles) {
    var stylePalette = [];
    for (var i in dotNetStyles) {
        var style = dotNetStyles[i];
        stylePalette.push(
            new ContentTools.Style(style.name, style.cssClass, style.applicableTo)
        );
    }
    ContentTools.StylePalette.add(stylePalette);
}

export function destroy() {
    editor.destroy()
}