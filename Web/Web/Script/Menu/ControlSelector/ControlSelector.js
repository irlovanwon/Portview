//Copyright(c) 2013,HIT All rights reserved.
//Des:head.js
//Author:Irlovan   
//Date:2013-10-23
//modification :2015-04-01

Irlovan.Include.Using("Script/Menu/ControlSelector/UIConfig.js");

Irlovan.Menu.ControlSelector = function (containerID, id, pos) {
    this.ContainerID = containerID;
    this.ID = id;
    this.MenuID = id + "_controlSelectorMenu";
    this.Pos = pos;
    this.CreateControl();
    $("#" + this.MenuID).menu({
        select: function (event, ui) {
            var controlName = null;
            for (var i = 0; i < ui.item[0].attributes.length; i++) {
                if (ui.item[0].attributes[i].nodeName == "control") {
                    controlName = ui.item[0].attributes[i].nodeValue;
                }
            }
            if (!controlName) { return; }
            var index = 0;
            var itemId = "GridControl_" + controlName + "_" + index;
            while (document.getElementById(itemId)) { index++; itemId = "GridControl_" + controlName + "_" + index; }
            Irlovan.Global.ControlList.push(eval("new Irlovan.Control." + controlName + "(itemId, Irlovan.Global.ControlContainerID,Irlovan.Global.GridContainerID,null,null,'400px', '200px','auto')"));
            this._idNum++;
        }
    });
    document.getElementById(this.MenuID).style.zIndex = 1000;
}
Irlovan.Menu.ControlSelector.prototype.CreateControl = function () {
    Irlovan.ControlHelper.AppendElement(this.ContainerID, this.ID);
    var filter = Irlovan.ControlHelper.CreateControlByStr("<ul id='" + this.MenuID + "' style='position: absolute;'>" + Irlovan.Menu.ControlSelectorConfig + "</ul>", this.ID, this.Pos);
}