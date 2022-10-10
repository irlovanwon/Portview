//Copyright(c) 2013,HIT All rights reserved.
//Des:RoundLamp
//Author:Irlovan   
//Date:2013-10-23
//modification :


Irlovan.Control.RoundLamp = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "RoundLamp", id, containerID, gridContainerID, controlClass, config ? config : ({
        value: { Attributes: "value", Value: "false" },
        color: { Attributes: "color", Value: "1" ,Description:"1:green2:red"},
        size: { Attributes: "size", Value: 60 }
    }), left, top, zIndex, isLock);
    this.LampID = this.ID + "_roundLamp";
    this.Init(id, containerID);
}
Irlovan.Control.RoundLamp.prototype = new Irlovan.Control.Classic();
Irlovan.Control.RoundLamp.prototype.constructor = Irlovan.Control.RoundLamp;
//init div element
Irlovan.Control.RoundLamp.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div>" +
       "<div id='" + this.LampID + "'  style='background-size:100% 100%;position: absolute;width:" + (this.Config.size.Value + "px") + ";height:" + (this.Config.size.Value + "px") + ";background-image: url(Images/ControlIcon/MasterController/" + this.GetImage(this.Config.value.Value) + ");'/>" +
       "</div>", this.ID, this.Pos);
}
Irlovan.Control.RoundLamp.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "value":
            document.getElementById(this.LampID).style.backgroundImage = "url('Images/ControlIcon/MasterController/" + this.GetImage(data) + "')";
            break;
        case "size":
            document.getElementById(this.LampID).style.width = data + 'px';
            document.getElementById(this.LampID).style.height = data + 'px';
            break;
        case "color":
            document.getElementById(this.LampID).style.backgroundImage = "url('Images/ControlIcon/MasterController/" + this.GetImage(this.Config.value.Value) + "')";
            break;
        default:
            break;
    }
}
Irlovan.Control.RoundLamp.prototype.GetImage = function (data) {
    switch (this.Config.color.Value) {
        case "1":
            return ((data == true)||(data=="true")) ? "LampGreenOn.png" : "LampGreenOff.png";
        case "2":
            return ((data == true) || (data == "true")) ? "LampRedOn.png" : "LampRedOff.png";
        default:
    }
}
Irlovan.Control.RoundLamp.prototype.Off = function () {
    Irlovan.Control.Classic.prototype.Off.apply(this);
    document.getElementById(this.LampID).style.backgroundImage = "";
    document.getElementById(this.LampID).style.backgroundColor = "black";
}
Irlovan.Control.RoundLamp.prototype.On = function () {
    Irlovan.Control.Classic.prototype.On.apply(this);
    document.getElementById(this.LampID).style.backgroundImage = "url('Images/ControlIcon/MasterController/" + this.GetImage(this.Config.value.Value) + "')";
    document.getElementById(this.LampID).style.backgroundColor = "transparent";
}