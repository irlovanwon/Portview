//Copyright(c) 2015,HIT All rights reserved.
//Des:VirtualUIReplay.js
//Author:Irlovan   
//Date:2015-08-31
//modification :

if (Irlovan.Global.Edition == Irlovan.Global.EditionMode.Admin) {
    Irlovan.Include.Using("Script/Layout/Layout.js");
    Irlovan.Include.Using("Script/Operator.js");
}
Irlovan.Include.Using("Script/Zoom.js");

Irlovan.VirtualUIReplay = function () {
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
    Irlovan.Global.VURMenu = new Irlovan.Menu.VURMenu();
}

/**Init Remote Handler**/
Irlovan.VirtualUIReplay.prototype.InitRemoteHandler = function () {
    this.GUIHandler = new Irlovan.Handler.GUIHandler(Irlovan.IrlovanHelper.Bind(this, function () {
        this.SubcribeGUI();
    }));
}

/**Init GUI Handler**/
Irlovan.VirtualUIReplay.prototype.SubcribeGUI = function () {
    this.GUIHandler.Loader.Subcribe(Irlovan.Global.GUICache, Irlovan.IrlovanHelper.Bind(this, this.InitGUIHandlerCallback));
}

/**Init GUI Handler Callback**/
Irlovan.VirtualUIReplay.prototype.InitGUIHandlerCallback = function () {
    this.InitGUI();
    this.CMDInit();
    this.Menu = new Irlovan.Menu.DockMenu();
}

/**Init CMD Handler**/
Irlovan.VirtualUIReplay.prototype.CMDInit = function () {
    for (var key in Irlovan.Global.CMD) {
        Irlovan.Global.CMD[key].Handler = new Irlovan.Handler.CMDHandler(Irlovan.Global.CMD[key].Domain);
    }
}

/**Init GUI**/
Irlovan.VirtualUIReplay.prototype.InitGUI = function () {
    Irlovan.Global.GUI = new Irlovan.GUI(this.GUIHandler);
    Irlovan.Global.GUI.LoadGUI(Irlovan.Global.CurrentPage);
}

/**Init Overflow State**/
Irlovan.VirtualUIReplay.prototype.SetOverflow = function () {
    document.getElementById(Irlovan.Global.BodyID).style.overflow = Irlovan.Global.Overflow;
}

/**Init Edition**/
Irlovan.VirtualUIReplay.prototype.InitEdition = function () {
    if (Irlovan.Global.Edition == Irlovan.Global.EditionMode.Admin) {
        Irlovan.Global.Layout = new Irlovan.Layout();
    }
    if (Irlovan.Global.Edition == Irlovan.Global.EditionMode.Operator) {
        new Irlovan.Zoom(Irlovan.Global.Resolution.Width, Irlovan.Global.Resolution.Height);
    }
}

/**Create Container Div For Controls**/
Irlovan.VirtualUIReplay.prototype.CreateControlContainer = function () {
    Irlovan.ControlHelper.CreateControlByStr(
           "<div id='" + Irlovan.Global.ControlContainerID + "' style='position: absolute;top:-" + Irlovan.Global.ControlMenuHeight + ";left:0px;'>" +
           "</div>", Irlovan.Global.BodyID, null);
}