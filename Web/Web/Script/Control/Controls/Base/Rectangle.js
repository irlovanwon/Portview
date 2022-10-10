//Copyright(c) 2013,HIT All rights reserved.
//Des:
//Author:Irlovan   
//Date:
//modification :

Irlovan.Control.Rectangle = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "Rectangle", id, containerID, gridContainerID, controlClass, config ? config : ({
        fillColor: { Attributes: "fillColor", Value: 'transparent' },
        strokeColor: { Attributes: "strokeColor", Value: 'black' },
        strokeType: { Attributes: "strokeType", Value: "solid", Description: "dotted or solid" },
        width: { Attributes: "width", Value: '25' },
        height: { Attributes: "height", Value: '25' },
        thickness: { Attributes: "thickness", Value: '2' },
        backgroundImage: { Attributes: "backgroundImage", Value: '' },
        stretch: { Attributes: "stretch", Value: true }
    }), left, top, zIndex, isLock);
    this.RecID = this.ID + "_rectangle";
    this.Init(id, containerID);
}
Irlovan.Control.Rectangle.prototype = new Irlovan.Control.Classic();
Irlovan.Control.Rectangle.prototype.constructor = Irlovan.Control.Rectangle;
//init div element
Irlovan.Control.Rectangle.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.RecID + "' style='background-size:100% 100%;background-color: " + this.Config.fillColor.Value + ";border:" + this.Config.thickness.Value + "px " + this.Config.strokeType.Value + " " + this.Config.strokeColor.Value + ";position:relative;width:" + this.Config.width.Value + "px;height:" + this.Config.height.Value + "px;'>" +
       "</div>", this.ID, this.Pos);
    this.SetBackgourdImage(this.Config.backgroundImage.Value);
    this.SetStretch(this.Config.stretch.Value);
}
Irlovan.Control.Rectangle.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "width":
            document.getElementById(this.RecID).style.width = data + "px";
            break;
        case "height":
            document.getElementById(this.RecID).style.height = data + "px";
            break;
        case "fillColor":
            document.getElementById(this.RecID).style.backgroundColor = this.Config.fillColor.Value;
            break;
        case "thickness":
        case "strokeType":
        case "strokeColor":
            document.getElementById(this.RecID).style.border = this.Config.thickness.Value + "px " + this.Config.strokeType.Value + " " + this.Config.strokeColor.Value;
            break;
        case "backgroundImage":
            this.SetBackgourdImage(data);
            break;
        case "stretch":
            this.SetStretch(data);
            break;
        default:
            break;
    }
}
Irlovan.Control.Rectangle.prototype.Off = function () {
    Irlovan.Control.Classic.prototype.Off.apply(this);
    document.getElementById(this.RecID).style.backgroundColor = "black";
    this.SetBackgourdImage();
}
Irlovan.Control.Rectangle.prototype.On = function () {
    Irlovan.Control.Classic.prototype.On.apply(this);
    document.getElementById(this.RecID).style.backgroundColor = this.Config.fillColor.Value;
    this.SetBackgourdImage(this.Config.backgroundImage.Value);
}
Irlovan.Control.Rectangle.prototype.SetBackgourdImage = function (image) {
    if ((image) && (image != "")) {
        document.getElementById(this.RecID).style.backgroundImage = "url('UserDefineImage/" + image + "')";
    } else {
        document.getElementById(this.RecID).style.backgroundImage = "";
    }
}
Irlovan.Control.Rectangle.prototype.SetStretch = function (isStretch) {
    if ((isStretch == true) || (isStretch == "true")) {
        document.getElementById(this.RecID).style.backgroundSize = "100% 100%";
    } else {
        document.getElementById(this.RecID).style.backgroundSize = "auto";
    }
}