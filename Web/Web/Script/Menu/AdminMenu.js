//Copyright(c) 2013,HIT All rights reserved.
//Des:DockMenu
//Author:Irlovan   
//Date:2013-04-18
//modification :2014-07-17

Irlovan.Include.Using("Script/Menu/Lib/EUDockMenu/euDock.2.0.js");
Irlovan.Include.Using("Script/Menu/Lib/EUDockMenu/euDock.Image.js");


Irlovan.Menu.DockMenu = function () {
    this._idNum = 0;
    this._createMenu();
    this.CreatePropertyGridContainer();
    this._selectors();
}
Irlovan.Menu.DockMenu.prototype._createMenu = function () {
    Irlovan.ControlHelper.DeleteControl("euDock_0");
    var downDock = new euDock();
    downDock.setScreenAlign(euDOWN, 3);
    downDock.setBar({
        left: { euImage: { image: "Script/Menu/Lib/EUDockMenu/BarImages/dockBg-l.png" } },
        horizontal: { euImage: { image: "Script/Menu/Lib/EUDockMenu/BarImages/dockBg-c-o.gif" } },
        right: { euImage: { image: "Script/Menu/Lib/EUDockMenu/BarImages/dockBg-r.png" } }
    });
    downDock.setIconsOffset(2);
    downDock.addIcon(new Array({ euImage: { image: "Images/NewPage.png" } }), {
        mouseInsideClick: function (x, y) {}
    });
    downDock.addIcon(new Array({ euImage: { image: "Images/DockMenu/Save.png" } }), {
        mouseInsideClick: function (x, y) {
            Irlovan.Global.GUI.RecordGUI(Irlovan.Global.CurrentPage);
        }
    });
    downDock.addIcon(new Array({ euImage: { image: "Images/Load.png" } }), { mouseInsideClick: function (x, y) { Irlovan.Global.GUI.LoadGUI(e.data); } });
    downDock.addIcon(new Array({ euImage: { image: "Images/PropertyGrid.png" } }), {
        mouseInsideClick: function (x, y) {
            if (Irlovan.Global.PropertyGrid) {
                for (var i = 0; i < Irlovan.Global.ControlList.length; i++) {
                    Irlovan.Global.ControlList[i].Grid = null;
                }
                Irlovan.ControlHelper.DeleteControl('controlgrid');
                Irlovan.Global.PropertyGrid = null;
            } else {
                Irlovan.Global.PropertyGrid = new Irlovan.PropertyGrid(Irlovan.Global.PropertyGridContainerID, Irlovan.Global.GridContainerID, "controlgrid_table", 'controlgrid_pager', Irlovan.RealtimeData.PropertyGridConfig, "PropertyGrid", null, null);
                Irlovan.Global.PropertyGrid.MakeDragable();
            }
        }
    });
    downDock.addIcon(new Array({ euImage: { image: "Images/ToolBox.png" } }), {
        mouseInsideClick: function (x, y) {
            if ($('#' + Irlovan.Global.ControlSelectorID) != []) {
                $('#' + Irlovan.Global.ControlSelectorID).toggle('slide', {}, 500);
                document.getElementById(Irlovan.Global.ControlSelectorID).style.zIndex = 1000;
            }
        }
    });
    downDock.addIcon(new Array({ euImage: { image: "Images/DockMenu/eye.png" } }), {
        mouseInsideClick: function (x, y) {
            for (var i = 0; i <Irlovan.Global.ControlList.length; i++) {
                Irlovan.Global.ControlList[i].Visible(true);
            }
        }
    });
}
Irlovan.Menu.DockMenu.prototype._selectors = function () {
    var controlSelector = new Irlovan.Menu.ControlSelector(Irlovan.Global.BodyID, Irlovan.Global.ControlSelectorID, { left: "0px", top: "20px" });
    $('#' + Irlovan.Global.ControlSelectorID).toggle('slide', {}, 500);
}
Irlovan.Menu.DockMenu.prototype.CreatePropertyGridContainer = function () {
    Irlovan.ControlHelper.CreateControlByStr(
    "<div id='" + Irlovan.Global.PropertyGridContainerID + "' style='z-index : 1;position: absolute;width:100px; top: 10px' >" +
    "</div>", Irlovan.Global.BodyID, null);
}
