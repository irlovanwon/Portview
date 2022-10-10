//Copyright(c) 2013,HIT All rights reserved.
//Des:
//Author:Irlovan   
//Date:2013-10-22
//modification :


Irlovan.Control.MasterSwitch = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "MasterSwitch", id, containerID, gridContainerID, controlClass, config ? config : ({
        gear: { Attributes: "gear", Value: 1 },
        size: { Attributes: "size", Value: 35 }
    }), left, top, zIndex, isLock);
    this.SwitchID = this.ID + "_masterSwitch";
    this.Init(id, containerID);
}
Irlovan.Control.MasterSwitch.prototype = new Irlovan.Control.Classic();
Irlovan.Control.MasterSwitch.prototype.constructor = Irlovan.Control.MasterSwitch;
//init div element
Irlovan.Control.MasterSwitch.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div>" +
       "<div id='" + this.SwitchID + "'  style='background-size:100% 100%;position: absolute;width:" + (this.Config.size.Value + "px") + ";height:" + (this.Config.size.Value + "px") + ";background-image: url(Images/MasterSwitch/MasterSwitch" + this.Config.gear.Value + ".png);'/>" +
       "</div>", this.ID, this.Pos);
}
Irlovan.Control.MasterSwitch.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "gear":
            var result;
            if (data >= 1 && data <= 15) { result = data; } else {result = 1;}
            document.getElementById(this.SwitchID).style.backgroundImage = "url('Images/MasterSwitch/MasterSwitch" + result + ".png')";
            break;
        case "size":
            document.getElementById(this.SwitchID).style.width = data + 'px';
            document.getElementById(this.SwitchID).style.height = data + 'px';
            break;
        default:
            break;
    }
}
Irlovan.Control.MasterSwitch.prototype.Off = function () {
    Irlovan.Control.Classic.prototype.Off.apply(this);
    document.getElementById(this.SwitchID).style.backgroundColor = "black";
    this.SetBackgourdImage();
}
Irlovan.Control.MasterSwitch.prototype.On = function () {
    Irlovan.Control.Classic.prototype.On.apply(this);
    document.getElementById(this.SwitchID).style.backgroundColor = "transparent";
    this.SetBackgourdImage(this.Config.gear.Value);
}
Irlovan.Control.MasterSwitch.prototype.SetBackgourdImage = function (image) {
    if ((image) && (image != "")) {
        document.getElementById(this.SwitchID).style.backgroundImage = "url('Images/MasterSwitch/MasterSwitch" + image + ".png')";
    } else {
        document.getElementById(this.SwitchID).style.backgroundImage = "";
    }
}