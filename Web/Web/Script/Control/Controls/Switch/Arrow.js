//Copyright(c) 2013,HIT All rights reserved.
//Des:
//Author:Irlovan   
//Date:
//modification :

Irlovan.Control.Arrow = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "Arrow", id, containerID, gridContainerID, controlClass, config ? config : ({
        value: { Attributes: "value", Value: "false" },
        style: { Attributes: "style", Value: "Arrow" },
        direction: { Attributes: "direction", Value: 'right', Description: 'Left/Right/Up/Bottom' }
    }), left, top, zIndex, isLock);
    this.ArrowID = this.ID + "_arrowBase";
    this.Init(id, containerID);
}
Irlovan.Control.Arrow.prototype = new Irlovan.Control.Classic();
Irlovan.Control.Arrow.prototype.constructor = Irlovan.Control.Arrow;
//init div element
Irlovan.Control.Arrow.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.ArrowID + "' style='background-size:100% 100%;position: absolute;width:46px;height:46px;background-image: url(Images/Arrow/ArrowRightOff.png);'>" +
       "</div>", this.ID, this.Pos);
    this.Animate(this.Config.style.Value, this.Config.direction.Value, this.Config.value.Value);
}
Irlovan.Control.Arrow.prototype.Animate = function (style,direction,value) {
    document.getElementById(this.ArrowID).style.backgroundImage = "url('Images/Arrow/" + style + direction + (((value == "true")||(value==true)) ? "On" : "Off") + ".png')";
}
Irlovan.Control.Arrow.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "value":
            this.Animate(this.Config.style.Value, this.Config.direction.Value, data);
            break;
        case "style":
            this.Animate(data, this.Config.direction.Value, this.Config.value.Value);
            break;
        case "direction":
            this.Animate(this.Config.style.Value, data, this.Config.value.Value);
            break;
        default:
            break;
    }
}

Irlovan.Control.Arrow.prototype.Off = function () {
    Irlovan.Control.Classic.prototype.Off.apply(this);
    document.getElementById(this.ArrowID).style.backgroundColor = "black";
    document.getElementById(this.ArrowID).style.backgroundImage = "";
    document.getElementById(this.ArrowID).style.opacity = "0.8";
}
Irlovan.Control.Arrow.prototype.On = function () {
    Irlovan.Control.Classic.prototype.On.apply(this);
    document.getElementById(this.ArrowID).style.opacity = "1";
    document.getElementById(this.ArrowID).style.backgroundColor = "white";
    this.Animate(this.Config.style.Value, this.Config.direction.Value, this.Config.value.Value);
}