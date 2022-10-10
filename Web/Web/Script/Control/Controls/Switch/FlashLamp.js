//Copyright(c) 2013,HIT All rights reserved.
//Des:
//Author:Irlovan   
//Date:2014-07-16
//modification :

Irlovan.Control.FlashLamp = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "FlashLamp", id, containerID, gridContainerID, controlClass, config ? config : ({
        value: { Attributes: "value", Value: "false" },
        offColor: { Attributes: "offColor", Value: 'white' },
        onColor: { Attributes: "onColor", Value: 'lime' },
        stopColor: { Attributes: "stopColor", Value: 'white' },
        border: { Attributes: "border", Value: '1px solid #000000' },
        interval: { Attributes: "interval", Value: 1000 },
        width: { Attributes: "width", Value: '25' },
        height: { Attributes: "height", Value: '25' }
    }), left, top, zIndex, isLock);
    this.Timer;
    this.IsOn = false;
    this.LampID = this.ID + "_flashLamp_border";
    this.Init(id, containerID);
}
Irlovan.Control.FlashLamp.prototype = new Irlovan.Control.Classic();
Irlovan.Control.FlashLamp.prototype.constructor = Irlovan.Control.FlashLamp;
//init div element
Irlovan.Control.FlashLamp.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.LampID + "' style='background-color: " + this.Config.offColor.Value + ";border:"+this.Config.border.Value+";position:relative;width:" + this.Config.width.Value + "px;height:" + this.Config.height.Value + "px;'>" +
       "</div>", this.ID, this.Pos);
    this.SetBackgourdColor();
}
Irlovan.Control.FlashLamp.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "value":
            this.SetBackgourdColor();
            break;
        case "interval":
            this.SetBackgourdColor();
            break;
        case "border":
            document.getElementById(this.LampID).style.border = data;
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
Irlovan.Control.FlashLamp.prototype.SetBackgourdColor = function () {
    this.TimerDispose();
    var result = (this.Config.value.Value == "true") || (this.Config.value.Value == true);
    if (result) {
        this.Timer = setInterval(Irlovan.IrlovanHelper.Bind(this, function () {
            document.getElementById(this.LampID).style.backgroundColor = (this.IsOn) ? this.Config.onColor.Value : this.Config.offColor.Value;
            this.IsOn = !this.IsOn;
        }), parseInt(this.Config.interval.Value));
    } else {
        document.getElementById(this.LampID).style.backgroundColor = this.Config.stopColor.Value;
    }
    
    
}
Irlovan.Control.FlashLamp.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    this.TimerDispose();
}

Irlovan.Control.FlashLamp.prototype.TimerDispose = function () {
    clearInterval(this.Timer);
    this.Timer = null
}