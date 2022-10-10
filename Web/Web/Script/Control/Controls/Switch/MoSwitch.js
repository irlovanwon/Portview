//Copyright(c) 2013,HIT All rights reserved.
//Des:
//Author:Irlovan   
//Date:2013-10-22
//modification :

Irlovan.Control.MoSwitch = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "MoSwitch", id, containerID, gridContainerID, controlClass, config ? config : ({
        value: { Attributes: "value", Value: false },
        onPic: { Attributes: "onPic", Value: 1 },
        offPic: { Attributes: "offPic", Value: 2 },
        stopPic: { Attributes: "stopPic", Value: 3 },
        folderName: { Attributes: "folderName", Value: "PicSwitch" },
        interval: { Attributes: "interval", Value: 1000 },
        width: { Attributes: "width", Value: 100 },
        height: { Attributes: "height", Value: 100 }
    }), left, top, zIndex, isLock);
    this.SwitchID = this.ID + "_moSwitch";
    this.Timer;
    this.IsOn = false;
    this.Init(id, containerID);
}

Irlovan.Control.MoSwitch.prototype = new Irlovan.Control.Classic();
Irlovan.Control.MoSwitch.prototype.constructor = Irlovan.Control.MoSwitch;

//init div element
Irlovan.Control.MoSwitch.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div>" +
       "<div id='" + this.SwitchID + "'  style='background-size:100% 100%;position: absolute;width:" + (this.Config.width.Value + "px") + ";height:" + (this.Config.height.Value + "px") + ";background-image: url(Images/Mo/" + this.Config.folderName.Value+ "/Mo" + this.Config.stopPic.Value + ".png);'/>" +
       "</div>", this.ID, this.Pos);
}

Irlovan.Control.MoSwitch.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "value":
            this.SetBackgourdColor();
            break;
        case "interval":
            this.SetBackgourdColor();
            break;
        case "folderName":
            this.SetBackgourdColor();
            break;
        case "stopPic":
            this.SetBackgourdColor();
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

Irlovan.Control.MoSwitch.prototype.SetBackgourdColor = function () {
    this.TimerDispose();
    var result = (this.Config.value.Value == "true") || (this.Config.value.Value == true);
    if (result) {
        this.Timer = setInterval(Irlovan.IrlovanHelper.Bind(this, function () {
            document.getElementById(this.SwitchID).style.backgroundImage = "url('Images/Mo/"+this.Config.folderName.Value+"/Mo" + ((this.IsOn) ? this.Config.onPic.Value : this.Config.offPic.Value) + ".png')";
            this.IsOn = !this.IsOn;
        }), parseInt(this.Config.interval.Value));
    } else {
        document.getElementById(this.SwitchID).style.backgroundImage = "url('Images/Mo/" + this.Config.folderName.Value + "/Mo" + this.Config.stopPic.Value + ".png')";
    }
}

Irlovan.Control.MoSwitch.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    this.TimerDispose();
}

Irlovan.Control.MoSwitch.prototype.TimerDispose = function () {
    clearInterval(this.Timer);
    this.Timer = null
}