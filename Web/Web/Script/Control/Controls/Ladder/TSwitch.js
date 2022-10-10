//Copyright(c) 2013,HIT All rights reserved.
//Des:
//Author:Irlovan   
//Date:2014-04-16
//modification :


Irlovan.Control.TSwitch = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "TSwitch", id, containerID, gridContainerID, controlClass, config ? config : ({
        type: { Attributes: "type", Value: 1 },
        width: { Attributes: "width", Value: 40 },
        height: { Attributes: "height", Value: 60 }
    }), left, top, zIndex, isLock);
    this.SwitchID = this.ID + "_tSwitch";
    this.Init(id, containerID);
}
Irlovan.Control.TSwitch.prototype = new Irlovan.Control.Classic();
Irlovan.Control.TSwitch.prototype.constructor = Irlovan.Control.TSwitch;
//init div element
Irlovan.Control.TSwitch.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div>" +
       "<div id='" + this.SwitchID + "'  style='background-size:100% 100%;position: absolute;width:" + (this.Config.width.Value + "px") + ";height:" + (this.Config.height.Value + "px") + ";background-image: url(Images/Ladder/T" + this.Config.type.Value + ".png);'/>" +
       "</div>", this.ID, this.Pos);
}
Irlovan.Control.TSwitch.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "type":
            document.getElementById(this.SwitchID).style.backgroundImage = "url('Images/Ladder/T" + data + ".png')";
            break;
        case "width":
            document.getElementById(this.SwitchID).style.width = data + 'px';
            break;
        case "height":
            document.getElementById(this.SwitchID).style.height = data + 'px';
            break;
        default:
            break;
    }
}
Irlovan.Control.TSwitch.prototype.Off = function () {
    Irlovan.Control.Classic.prototype.Off.apply(this);
    document.getElementById(this.SwitchID).style.backgroundImage = "";
    document.getElementById(this.SwitchID).style.backgroundColor = "black";
}
Irlovan.Control.TSwitch.prototype.On = function () {
    Irlovan.Control.Classic.prototype.On.apply(this);
    document.getElementById(this.SwitchID).style.backgroundImage = "url('Images/Ladder/T" + this.Config.type.Value + ".png')";
    document.getElementById(this.SwitchID).style.backgroundColor = "transparent";
}