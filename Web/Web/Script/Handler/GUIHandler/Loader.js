//Copyright(c) 2015,HIT All rights reserved.
//Des:Loader for Loading GUI
//Author:Irlovan   
//Date:2015-03-24

/**Construction**/
Irlovan.Handler.Loader = function (session) {
    this.Name = "Loader";
    this.SubcriptionTag = "SBC";
    this.PageTag = "Page";
    this.PathAttr = "Path";
    this.Session = session;
    this.SBCCallback = null;
    this.LoadCallback = null;
}
/**Handle message from server**/
Irlovan.Handler.Loader.prototype.Handle = function (xDocument) {
    var element = xDocument.documentElement;
    if (element.nodeName != this.Name) { return; }
    this.HandleSubcription(element);

}

/**Subcribe from server**/
Irlovan.Handler.Loader.prototype.Subcribe = function (guiList, callback) {
    this.SBCCallback = callback;
    this.Session.Send(this.CreateLoaderMessage(guiList));
}

/**Load GUI from server**/
Irlovan.Handler.Loader.prototype.Load = function (page, loadCallback) {
    this.LoadCallback = loadCallback;
    var result = {};
    result[page] = false;
    this.Session.Send(this.CreateLoaderMessage(result));
}

/**Handle subcription from server**/
Irlovan.Handler.Loader.prototype.HandleSubcription = function (element) {
    var subcriptions = element.getElementsByTagName(this.SubcriptionTag);
    if ((!subcriptions) || (subcriptions.length == 0)) { return; }
    var pages = subcriptions[0].getElementsByTagName(this.PageTag);
    if ((!pages) || (pages.length == 0)) { return; }
    for (var i = 0; i < pages.length; i++) {
        this.HandleGUIPage(pages[i]);
    }
}

/**Handle gui pages from server by element**/
Irlovan.Handler.Loader.prototype.HandleGUIPage = function (pageElement) {
    var path = pageElement.getAttribute(this.PathAttr);
    if (!path) { return; }
    if (Irlovan.Global.GUICache[path] != null) {
        Irlovan.Global.GUICache[path.toLowerCase()] = pageElement;
    } else {
        if (this.LoadCallback) { this.LoadCallback(pageElement); }
    }
    if (path == Irlovan.Global.PageTo) {
        Irlovan.Global.LoadBar.Off();
        if (this.SBCCallback) {
            this.SBCCallback();
            this.SBCCallback = null;
        }
    }
}

/**Create Loader Init Info**/
Irlovan.Handler.Loader.prototype.CreateLoaderMessage = function (guiList) {
    var loadDoc = Irlovan.Lib.XML.CreateXDocument(this.Name);
    var sbc = Irlovan.Lib.XML.CreateXElement(this.SubcriptionTag);
    loadDoc.documentElement.appendChild(sbc);
    if (!guiList) { return Irlovan.Lib.XML.ToString(loadDoc); }
    for (var gui in guiList) {
        var page = Irlovan.Lib.XML.CreateXElement(this.PageTag);
        page.setAttribute(this.PathAttr, gui);
        sbc.appendChild(page);
    }
    return Irlovan.Lib.XML.ToString(loadDoc);
}

/**Dispose**/
Irlovan.Handler.Loader.prototype.Dispose = function () {
}