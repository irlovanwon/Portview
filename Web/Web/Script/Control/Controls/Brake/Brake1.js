//Copyright(c) 2013,HIT All rights reserved.
//Des:
//Author:Irlovan   
//Date:2013-10-22
//modification :


Irlovan.Control.Brake1 = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "Brake1", id, containerID, gridContainerID, controlClass, config ? config : ({
        gear: { Attributes: "gear", Value: 1 },
        width: { Attributes: "width", Value: 52 },
        height: { Attributes: "height", Value: 101 },
    }), left, top, zIndex, isLock);
    this.SwitchID = this.ID + "_Brake1";
    this.Init(id, containerID);
}
Irlovan.Control.Brake1.prototype = new Irlovan.Control.Classic();
Irlovan.Control.Brake1.prototype.constructor = Irlovan.Control.Brake1;
//init div element
Irlovan.Control.Brake1.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div>" +
       "<div id='" + this.SwitchID + "'  style='background-size:100% 100%;position: absolute;width:" + (this.Config.width.Value + "px") + ";height:" + (this.Config.height.Value + "px") + ";background-image: url(Images/Brake/Brake1/Brake" + this.Config.gear.Value + ".png);'/>" +
       "</div>", this.ID, this.Pos);
}
Irlovan.Control.Brake1.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "gear":
            var result;
            if (data >= 0 && data <= 5) { result = data; } else {result = 0;}
            document.getElementById(this.SwitchID).style.backgroundImage = "url('Images/Brake/Brake1/Brake" + result + ".png')";
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