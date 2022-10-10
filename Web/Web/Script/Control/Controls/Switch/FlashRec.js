//Copyright(c) 2013,HIT All rights reserved.
//Des:
//Author:Irlovan   
//Date:2014-07-16
//modification :

Irlovan.Control.FlashRec = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "FlashRec", id, containerID, gridContainerID, controlClass, config ? config : ({
        value: { Attributes: "value", Value: "false" },
        offColor: { Attributes: "offColor", Value: 'white' },
        onColor: { Attributes: "onColor", Value: 'lime' },
        stopColor: { Attributes: "stopColor", Value: 'black' },
        thickness: { Attributes: "thickness", Value: '1px' },
        backGroundColor: { Attributes: "backGroundColor", Value: 'white' },
        interval: { Attributes: "interval", Value: 1000 },
        width: { Attributes: "width", Value: '25' },
        height: { Attributes: "height", Value: '25' }
    }), left, top, zIndex, isLock);
    this.Timer;
    this.IsOn = false;
    this.LampID = this.ID + "_FlashRec_border";
    this.Init(id, containerID);
}
Irlovan.Control.FlashRec.prototype = new Irlovan.Control.Classic();
Irlovan.Control.FlashRec.prototype.constructor = Irlovan.Control.FlashRec;
//init div element
Irlovan.Control.FlashRec.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.LampID + "' style='background-color: " + this.Config.backGroundColor.Value + ";border:" + this.Config.thickness.Value + " solid "+this.Config.stopColor.Value+";position:relative;width:" + this.Config.width.Value + "px;height:" + this.Config.height.Value + "px;'>" +
       "</div>", this.ID, this.Pos);
    this.SetColor();
}
Irlovan.Control.FlashRec.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "value":
            this.SetColor();
            break;
        case "interval":
            this.SetColor();
            break;
        case "backGroundColor":
            document.getElementById(this.LampID).style.backgroundColor = data;
            break;
        case "thickness":
            this.SetColor();
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
Irlovan.Control.FlashRec.prototype.SetColor = function () {
    this.TimerDispose();
    var result = (this.Config.value.Value == "true") || (this.Config.value.Value == true);
    if (result) {
        this.Timer = setInterval(Irlovan.IrlovanHelper.Bind(this, function () {
            document.getElementById(this.LampID).style.border = (this.IsOn) ? (this.Config.thickness.Value + " solid " + this.Config.onColor.Value) : (this.Config.thickness.Value + " solid " + this.Config.offColor.Value);
            this.IsOn = !this.IsOn;
        }), parseInt(this.Config.interval.Value));
    } else {
        document.getElementById(this.LampID).style.border = (this.Config.thickness.Value + " solid " + this.Config.stopColor.Value);
    }
}
Irlovan.Control.FlashRec.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    this.TimerDispose();
}

Irlovan.Control.FlashRec.prototype.TimerDispose = function () {
    clearInterval(this.Timer);
    this.Timer = null
}