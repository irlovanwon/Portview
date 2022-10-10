//Copyright(c) 2013,HIT All rights reserved.
//Des:
//Author:Irlovan   
//Date:
//modification :

Irlovan.Control.HTMLIF = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "HTMLIF", id, containerID, gridContainerID, controlClass, config ? config : ({
        fillColor: { Attributes: "fillColor", Value: 'transparent' },
        strokeColor: { Attributes: "strokeColor", Value: 'black' },
        strokeType: { Attributes: "strokeType", Value: "solid", Description: "dotted or solid" },
        width: { Attributes: "width", Value: '250' },
        height: { Attributes: "height", Value: '250' },
        thickness: { Attributes: "thickness", Value: '2' },
        uri: { Attributes: "uri", Value: 'Empty' },
        stretch: { Attributes: "stretch", Value: false }
    }), left, top, zIndex, isLock);
    this.RecID = this.ID + "_HTMLIF";
    this.Init(id, containerID);
}
Irlovan.Control.HTMLIF.prototype = new Irlovan.Control.Classic();
Irlovan.Control.HTMLIF.prototype.constructor = Irlovan.Control.HTMLIF;
//init div element
Irlovan.Control.HTMLIF.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.RecID + "' style='background-size:100% 100%;background-color: " + this.Config.fillColor.Value + ";border:" + this.Config.thickness.Value + "px " + this.Config.strokeType.Value + " " + this.Config.strokeColor.Value + ";position:relative;width:" + this.Config.width.Value + "px;height:" + this.Config.height.Value + "px;'>" +
       "</div>", this.ID, this.Pos);
    this.SetStretch(this.Config.stretch.Value);
    this.SetInnerHtml(this.Config.uri.Value);
}
Irlovan.Control.HTMLIF.prototype.SetValue = function (name, colName, data) {
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
        case "uri":
            this.SetInnerHtml(data);
            break;
        case "stretch":
            this.SetStretch(data);
            break;
        default:
            break;
    }
}
Irlovan.Control.HTMLIF.prototype.SetInnerHtml = function (uri) {
    var div=document.getElementById(this.RecID);
    div.innerHTML = "<object width='100%' height='100%' type='text/html' data='" + uri + "' ></object>";
}
Irlovan.Control.HTMLIF.prototype.SetStretch = function (isStretch) {
    if ((isStretch == true) || (isStretch == "true")) {
        document.getElementById(this.RecID).style.backgroundSize = "100% 100%";
    } else {
        document.getElementById(this.RecID).style.backgroundSize = "auto";
    }
}