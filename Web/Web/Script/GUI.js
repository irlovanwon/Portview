//Copyright(c) 2015,HIT All rights reserved.
//Des:GUI
//Author:Irlovan   
//Date:2015-03-24


Irlovan.GUI = function (guiHandler) {
    this.ControlTag = "Control";
    this.PageTag = "Page";
    this.PathAttr = "Path";
    this.NameAttr = "Name";
    this.GUISplitChar = ";";
    this.GUIInfoSplitChar = "|";
    this.GUIHandler = guiHandler;
    this.TempPage;
}

/**Load GUI**/
Irlovan.GUI.prototype.LoadGUI = function (gui, recorderName, interval) {
    var currentPage = Irlovan.Global.CurrentPage;
    if (currentPage==null) {return;}
    this.TempPage = currentPage;
    //Irlovan.Global.CurrentPage = null;
    if (recorderName) { Irlovan.Global.VURRecorderName = recorderName; }
    if (interval) { Irlovan.Global.VURInterval = interval; }
    var pages = gui.split(this.GUISplitChar);
    var page = pages[0];
    Irlovan.Global.PageTo = page;
    if (pages.length > 1) { this.RefreshCache(pages); return; }
    if (this.TryCache(page)) { return; }
    this.GUIHandler.Loader.Load(page, Irlovan.IrlovanHelper.Bind(this, this.ParseGUIXDoc));
}

/**Try loading the page from cache**/
Irlovan.GUI.prototype.TryCache = function (page) {
    var pageDoc = Irlovan.Global.GUICache[page.toLowerCase()];
    if (!pageDoc) { return false; }
    this.ParseGUIXDoc(pageDoc);
    return true;
}

/**Refresh cache**/
Irlovan.GUI.prototype.RefreshCache = function (pages) {
    this.ClearGUICache();
    for (var i = 0; i < pages.length; i++) {
        Irlovan.Global.GUICache[pages[i]] = false;
    }
    this.SubcribeGUI();
}

/**Clear GUI Cache**/
Irlovan.GUI.prototype.ClearGUICache = function (pages) {
    Irlovan.Global.GUICache = null;
    Irlovan.Global.GUICache = {};
}

/**Init GUI Handler**/
Irlovan.GUI.prototype.SubcribeGUI = function () {
    this.GUIHandler.Loader.Subcribe(Irlovan.Global.GUICache, Irlovan.IrlovanHelper.Bind(this, this.InitGUIHandlerCallback));
}

/**Init GUI Handler Callback**/
Irlovan.GUI.prototype.InitGUIHandlerCallback = function () {
    this.LoadGUI(Irlovan.Global.PageTo);
}


/**Record GUI to server**/
Irlovan.GUI.prototype.RecordGUI = function () {
    if (Irlovan.Global.CurrentPage == null) { Irlovan.Global.CurrentPage = this.TempPage; }
    this.XmlDocument = Irlovan.IrlovanHelper.CreateXDocument(this.PageTag);
    var root = this.XmlDocument.documentElement;
    root.setAttribute(this.PathAttr, Irlovan.Global.CurrentPage);
    root.setAttribute(this.NameAttr, Irlovan.Global.CurrentPage.split("/").GetLast());
    for (var i = 0; i < Irlovan.Global.ControlList.length; i++) {
        if (document.getElementById(Irlovan.Global.ControlList[i].ID) == null) { continue; }
        Irlovan.Global.ControlList[i].Config.left.Value = document.getElementById(Irlovan.Global.ControlList[i].ID).style.left;
        Irlovan.Global.ControlList[i].Config.top.Value = document.getElementById(Irlovan.Global.ControlList[i].ID).style.top;
        root.appendChild(Irlovan.IrlovanHelper.CreateXElementByArray(this.XmlDocument, ["Control",
            "Name", Irlovan.Global.ControlList[i].Name,
            "ID", Irlovan.Global.ControlList[i].ID,
            "ContainerID", Irlovan.Global.ControlList[i].ContainerID,
            "GridContainerID", Irlovan.Global.ControlList[i].GridContainerID,
            "Class", ((Irlovan.Global.ControlList[i].Class) ? (Irlovan.Global.ControlList[i].Class) : "null"),
            "Config", Irlovan.IrlovanHelper.JsonToString(Irlovan.Global.ControlList[i].Config),
            "Left", Irlovan.Global.ControlList[i].Config.left.Value,
            "Top", Irlovan.Global.ControlList[i].Config.top.Value,
            "ZIndex", Irlovan.Global.ControlList[i].Config.zIndex.Value,
            "IsLock", Irlovan.Global.ControlList[i].Config.isLock.Value
        ]
            ));
    }
    var resultList = [];
    resultList.push(root);
    this.GUIHandler.Recorder.Save(resultList);
    this.GUIHandler.Loader.Load(Irlovan.Global.CurrentPage, Irlovan.IrlovanHelper.Bind(this, this.ParseGUIXDoc));
}

/**Parse GUI Doc**/
Irlovan.GUI.prototype.ParseGUIXDoc = function (xDoc) {
    this.ParseGUIXDocLock(xDoc);
}
/**Parse GUI Doc Locked**/
Irlovan.GUI.prototype.ParseGUIXDocLock = function (xDoc) {
    if ((Irlovan.Global.MessageHandler) && (Irlovan.Global.MessageHandler.DataMessage)) { Irlovan.Global.MessageHandler.DataMessage.Stop(); }
    this.Dispose();
    var controlDoc = xDoc.getElementsByTagName(this.ControlTag);
    for (var i = 0; i < controlDoc.length; i++) {
        Irlovan.Global.ControlList.push(eval("new Irlovan.Control." + controlDoc[i].getAttribute("Name") + "(" +
            ("'" + controlDoc[i].getAttribute("ID") + "'") + "," +
            ("'" + Irlovan.Global.ControlContainerID + "'") + "," +
            ("'" + controlDoc[i].getAttribute("GridContainerID") + "'") + "," +
            controlDoc[i].getAttribute("Class") + "," +
            controlDoc[i].getAttribute("Config") + "," +
            ("'" + controlDoc[i].getAttribute("Left") + "'") + "," +
            ("'" + controlDoc[i].getAttribute("Top") + "'") + "," +
            ("'" + controlDoc[i].getAttribute("ZIndex") + "'") + "," +
            ("'" + controlDoc[i].getAttribute("IsLock") + "'") +
            ")"));
    }
    Irlovan.Global.CurrentPage = Irlovan.Global.PageTo;
    this.RunControl();
    if ((Irlovan.Global.MessageHandler) && (Irlovan.Global.MessageHandler.DataMessage)) { Irlovan.Global.MessageHandler.DataMessage.Subcribe(Irlovan.Global.ControlList); }
    if (Irlovan.Global.VURMenu) { Irlovan.Global.VURMenu.Keep(); }
    return true;
}

/**init control state**/
Irlovan.GUI.prototype.RunControl = function () {
    for (var i = 0; i < Irlovan.Global.ControlList.length; i++) {
        var control = Irlovan.Global.ControlList[i];
        var message = control.Config.tag.Value;
        var effect = control.Config.effect.Value;
        control.Tag(message);
        control.Effect(effect);
    }
}
/**Dispose**/
Irlovan.GUI.prototype.Dispose = function () {
    for (var i = 0; i < Irlovan.Global.ControlList.length; i++) {
        Irlovan.Global.ControlList[i].Dispose();
        Irlovan.Global.ControlList[i] = null;
    }
    Irlovan.ControlHelper.ClearControl(Irlovan.Global.ControlContainerID);
    Irlovan.Global.ControlList.length = 0;
    Irlovan.Global.ControlList = null;
    Irlovan.Global.ControlList = [];
}
















