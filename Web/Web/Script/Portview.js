//Copyright(c) 2013,HIT All rights reserved.
//Des:Portview.js
//Author:Irlovan   
//Date:2013-04-26
//modification :2015-03-26**Modify Everything
//modification :2015-04-01**Modify Namspace

if (Irlovan.Global.Edition == Irlovan.Global.EditionMode.Admin) {
    Irlovan.Include.Using("Script/Layout/Layout.js");
    Irlovan.Include.Using("Script/Operator.js");
}
Irlovan.Include.Using("Script/Zoom.js");

Irlovan.Portview = function (page) {
    this.InitPageSetting(page);
    this.SetOverflow();
    try {
        this.CreateControlContainer();
        this.InitEdition();
        Irlovan.Global.LoadBar = new Irlovan.Module.LoadBar();
    } catch (e) {
        location.reload();
    }
    this.GUIHandler;
    this.InitRemoteHandler();
}

/**set init page**/
Irlovan.Portview.prototype.InitPageSetting = function (page) {
    if (!page) { return; }
    Irlovan.Global.CurrentPage = page;
    Irlovan.Global.PageTo = page;
    if (!Irlovan.Global.GUICache[page]) { Irlovan.Global.GUICache[page] = false; }
}

/**Init Remote Handler**/
Irlovan.Portview.prototype.InitRemoteHandler = function () {
    this.GUIHandler = new Irlovan.Handler.GUIHandler(Irlovan.IrlovanHelper.Bind(this, function () {
        this.InitMessageHandler();
    }));
}

/**Init Message Handler**/
Irlovan.Portview.prototype.InitMessageHandler = function () {
    Irlovan.Global.MessageHandler = new Irlovan.Handler.MessageHandler(Irlovan.IrlovanHelper.Bind(this, function () {
        this.StartSubcribe();
    }));
}

/**StartSubcribe**/
Irlovan.Portview.prototype.StartSubcribe = function () {
    this.SubcribeGUI();
}

/**SubcribeMessage**/
Irlovan.Portview.prototype.SubcribeMessage = function () {
    Irlovan.Global.MessageHandler.DataMessage.Subcribe(Irlovan.Global.ControlList);
    Irlovan.Global.MessageHandler.DatabaseMessage.Subcribe();
}

/**Init GUI Handler**/
Irlovan.Portview.prototype.SubcribeGUI = function () {
    this.GUIHandler.Loader.Subcribe(Irlovan.Global.GUICache, Irlovan.IrlovanHelper.Bind(this, this.InitGUIHandlerCallback));
}

/**Init GUI Handler Callback**/
Irlovan.Portview.prototype.InitGUIHandlerCallback = function () {
    this.InitGUI();
    this.CMDInit();
    this.Menu = new Irlovan.Menu.DockMenu();
    this.SubcribeMessage();
}

/**Init CMD Handler**/
Irlovan.Portview.prototype.CMDInit = function () {
    for (var key in Irlovan.Global.CMD) {
        Irlovan.Global.CMD[key].Handler = new Irlovan.Handler.CMDHandler(Irlovan.Global.CMD[key].Domain);
    }
}

/**Init GUI**/
Irlovan.Portview.prototype.InitGUI = function () {
    Irlovan.Global.GUI = new Irlovan.GUI(this.GUIHandler);
    Irlovan.Global.GUI.LoadGUI(Irlovan.Global.CurrentPage);
}

/**Init Overflow State**/
Irlovan.Portview.prototype.SetOverflow = function () {
    document.getElementById(Irlovan.Global.BodyID).style.overflow = Irlovan.Global.Overflow;
}

/**Init Edition**/
Irlovan.Portview.prototype.InitEdition = function () {
    if (Irlovan.Global.Edition == Irlovan.Global.EditionMode.Admin) {
        Irlovan.Global.Layout = new Irlovan.Layout();
    }
    if (Irlovan.Global.Edition == Irlovan.Global.EditionMode.Operator) {
        new Irlovan.Zoom(Irlovan.Global.Resolution.Width, Irlovan.Global.Resolution.Height);
    }
}

/**Create Container Div For Controls**/
Irlovan.Portview.prototype.CreateControlContainer = function () {
    Irlovan.ControlHelper.CreateControlByStr(
           "<div id='" + Irlovan.Global.ControlContainerID + "' style='position: absolute;top:-" + Irlovan.Global.ControlMenuHeight + ";left:0px;'>" +
           "</div>", Irlovan.Global.BodyID, null);
}