//Copyright(c) 2013,HIT All rights reserved.
//Des:
//Author:Irlovan   
//Date:2013-10-22
//modification :


Irlovan.Control.GearSwitch = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "GearSwitch", id, containerID, gridContainerID, controlClass, config ? config : ({
        gear: { Attributes: "gear", Value: 1 },
        size: { Attributes: "size", Value: 70 }
    }), left, top, zIndex, isLock);
    this.SwitchID = this.ID + "_gearSwitch";
    this.Init(id, containerID);
}
Irlovan.Control.GearSwitch.prototype = new Irlovan.Control.Classic();
Irlovan.Control.GearSwitch.prototype.constructor = Irlovan.Control.GearSwitch;
//init div element
Irlovan.Control.GearSwitch.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div>" +
       "<div id='" + this.SwitchID + "'  style='background-size:100% 100%;position: absolute;width:" + (this.Config.size.Value + "px") + ";height:" + (this.Config.size.Value + "px") + ";background-image: url(Images/ControlIcon/MasterController/Switch"+this.Config.gear.Value+".png);'/>" +
       "</div>", this.ID, this.Pos);
}
Irlovan.Control.GearSwitch.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "gear":
            var result;
            if (data >= 0 && data <= 50) { result = data; } else {result = 1;}
            document.getElementById(this.SwitchID).style.backgroundImage = "url('Images/ControlIcon/MasterController/Switch" + result + ".png')";
            break;
        case "size":
            document.getElementById(this.SwitchID).style.width = data + 'px';
            document.getElementById(this.SwitchID).style.height = data + 'px';
            break;
        default:
            break;
    }
}
Irlovan.Control.GearSwitch.prototype.Off = function () {
    Irlovan.Control.Classic.prototype.Off.apply(this);
    document.getElementById(this.SwitchID).style.backgroundColor = "black";
    this.SetBackgourdImage();
}
Irlovan.Control.GearSwitch.prototype.On = function () {
    Irlovan.Control.Classic.prototype.On.apply(this);
    document.getElementById(this.SwitchID).style.backgroundColor = "transparent";
    this.SetBackgourdImage(this.Config.gear.Value);
}
Irlovan.Control.GearSwitch.prototype.SetBackgourdImage = function (image) {
    if ((image) && (image != "")) {
        document.getElementById(this.SwitchID).style.backgroundImage = "url(Images/ControlIcon/MasterController/Switch" + image + ".png)";
    } else {
        document.getElementById(this.SwitchID).style.backgroundImage = "";
    }
}