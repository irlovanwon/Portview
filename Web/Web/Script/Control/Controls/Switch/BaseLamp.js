//Copyright(c) 2013,HIT All rights reserved.
//Des:
//Author:Irlovan   
//Date:
//modification :

Irlovan.Control.BaseLamp = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "BaseLamp", id, containerID, gridContainerID, controlClass, config ? config : ({
        value: { Attributes: "value", Value: "false" },
        offColor: { Attributes: "offColor", Value: 'white' },
        onColor: { Attributes: "onColor", Value: 'lime' },
        width: { Attributes: "width", Value: '25' },
        height: { Attributes: "height", Value: '25' }
    }), left, top, zIndex, isLock);
    this.LampID=this.ID+"_baseLamp_border";
    this.Init(id, containerID);
}
Irlovan.Control.BaseLamp.prototype = new Irlovan.Control.Classic();
Irlovan.Control.BaseLamp.prototype.constructor = Irlovan.Control.BaseLamp;
//init div element
Irlovan.Control.BaseLamp.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.LampID + "' style='background-color: " + this.Config.offColor.Value + ";border:thin solid #000000;position:relative;width:" + this.Config.width.Value + "px;height:" + this.Config.height.Value + "px;'>" +
       "</div>", this.ID, this.Pos);
    this.SetBackgourdColor();
}
Irlovan.Control.BaseLamp.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "value":
            this.SetBackgourdColor();
            break;
        case "width":
            document.getElementById(this.LampID).style.width = data + "px";
            break;
        case "height":
            document.getElementById(this.LampID).style.height = data + "px";
            break;
        default:
            break;
    }
}
Irlovan.Control.BaseLamp.prototype.SetBackgourdColor = function () {
    document.getElementById(this.LampID).style.backgroundColor = ((this.Config.value.Value == "true") || (this.Config.value.Value == true)) ? this.Config.onColor.Value : this.Config.offColor.Value;
}
Irlovan.Control.BaseLamp.prototype.Off = function () {
    Irlovan.Control.Classic.prototype.Off.apply(this);
    document.getElementById(this.LampID).style.backgroundColor = "black";
}
Irlovan.Control.BaseLamp.prototype.On = function () {
    Irlovan.Control.Classic.prototype.On.apply(this);
    this.SetBackgourdColor();
}