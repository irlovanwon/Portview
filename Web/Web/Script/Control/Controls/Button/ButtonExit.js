//Copyright(c) 2014,HIT All rights reserved.
//Des:
//Author:Irlovan   
//Date:2014-10-05
//modification :

Irlovan.Control.ButtonExit = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "ButtonClassic", id, containerID, gridContainerID, controlClass, config ? config : ({
        name: { Attributes: "name", Value: 'button' },
        fontSize: { Attributes: "fontSize", Value: '20' },
        fontColor: { Attributes: "fontColor", Value: 'black' },
        backgroundColor: { Attributes: "backgroundColor", Value: null },
        width: { Attributes: "width", Value: '140' },
        height: { Attributes: "height", Value: '35' },
        thickness: { Attributes: "thickness", Value: '0' }
    }), left, top, zIndex, isLock);
    this.ButtonContainerID = id + "_buttonContainer";
    this.ButtonID = id + "_buttonClassic";
    this.Init(id, containerID);
}
Irlovan.Control.ButtonExit.prototype = new Irlovan.Control.Classic();
Irlovan.Control.ButtonExit.prototype.constructor = Irlovan.ButtonClassic;
//init div element
Irlovan.Control.ButtonExit.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.ButtonContainerID + "' style='position: relative;width:" + ((parseFloat(this.Config.width.Value)+20)+"px") + ";height:" + this.Config.height.Value + ";'>" +
       "<button type='button' id='" + this.ButtonID + "' style='border-width:"+this.Config.thickness.Value+"px;font-size:" + this.Config.fontSize.Value + "px;color:" + this.Config.fontColor.Value + ";line-height:" + this.Config.height.Value + "px;text-align:center;position: absolute;width:" + this.Config.width.Value + "px;height:" + this.Config.height.Value + "'>" + this.Config.name.Value + "</button>" +
       "</div>", this.ID, this.Pos);
    document.getElementById(this.ButtonID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, this.TriggerHandler), false);
    this.SetThickness();
}
Irlovan.Control.ButtonExit.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "name":
            document.getElementById(this.ButtonID).innerHTML = data;
            break;
        case "fontSize":
        case "fontColor":   
        case "width":
        case "height":
        case "backgroundColor":
            this.Refresh();
            break;
        default:
            break;
    }
}

/**Refresh**/
Irlovan.Control.ButtonExit.prototype.Refresh = function () {
    document.getElementById(this.ButtonID).removeAttribute("style");
    document.getElementById(this.ButtonID).style.fontSize = this.Config.fontSize.Value + "px";
    document.getElementById(this.ButtonID).style.color = this.Config.fontColor.Value;
    document.getElementById(this.ButtonID).style.backgroundColor = this.Config.backgroundColor.Value;
    document.getElementById(this.ButtonContainerID).style.width = (parseFloat(this.Config.width.Value) + 20) + "px";
    document.getElementById(this.ButtonID).style.width = this.Config.width.Value + "px";
    document.getElementById(this.ButtonContainerID).style.height = this.Config.height.Value + "px";
    document.getElementById(this.ButtonID).style.height = this.Config.height.Value + "px";
    document.getElementById(this.ButtonID).style.lineHeight = this.Config.height.Value + "px";
    document.getElementById(this.ButtonID).style.borderWidth = this.Config.thickness.Value + "px";
}

/**Handler for triggering event**/
Irlovan.Control.ButtonExit.prototype.TriggerHandler = function () {
    if ((this.Config.uri.Value == "") || (this.Config.uri.Value == null)) { return; }
    Irlovan.Global.GUI.LoadGUI(this.Config.uri.Value);
}

/**Dispose**/
Irlovan.Control.ButtonExit.prototype.Dispose = function () {
    document.getElementById(this.ButtonID).removeEventListener("click", Irlovan.IrlovanHelper.Bind(this, this.TriggerHandler), false);
    this.DisposeTimer();
}