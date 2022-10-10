//Copyright(c) 2013,HIT All rights reserved.
//Des:
//Author:Irlovan   
//Date:2014-01-17
//modification :

Irlovan.Control.TextBox = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "TextBox", id, containerID, gridContainerID, controlClass, config ? config : ({
        content: { Attributes: "content", Value: 0 },
        width: { Attributes: "width", Value: 100 },
        height: { Attributes: "height", Value: 20 },
        fillColor: { Attributes: "fillColor", Value: "white" },
        fontSize: { Attributes: "fontSize", Value: 20 },
        fontColor: { Attributes: "fontColor", Value: "black" },
        textAlign: { Attributes: "textAlign", Value: "left" },
        borderWidth: { Attributes: "borderWidth", Value: "1" },
        readonly: { Attributes: "readonly", Value: true }
    }), left, top, zIndex, isLock);
    this.EditionFix();
    this.BoxID = this.ID + "_textBox";
    this.Init(id, containerID);
}
Irlovan.Control.TextBox.prototype = new Irlovan.Control.Classic();
Irlovan.Control.TextBox.prototype.constructor = Irlovan.Control.TextBox;
//init div element
Irlovan.Control.TextBox.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    Irlovan.ControlHelper.CreateControlByStr(
   "<input type='text'  id='" + this.BoxID + "' value='" + this.Config.content.Value + "' readonly='" + this.Config.readonly.Value + "' style='font-size:" + this.Config.fontSize.Value + "px;text-align:" + this.Config.textAlign.Value+ ";background-color:" + this.Config.fillColor.Value + ";color:" + this.Config.fontColor.Value + ";width:" + this.Config.width.Value + "px;height:" + this.Config.height.Value + "px;' >" +
   "</input >", this.ID, null);
    this.SetReadonly();
    if (this.Config.borderWidth != "null") {
        this.SetBorder();
    }
}
Irlovan.Control.TextBox.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "content":
            document.getElementById(this.BoxID).value = data;
            break;
        case "width":
            document.getElementById(this.BoxID).style.width = data + "px";
            break;
        case "height":
            document.getElementById(this.BoxID).style.height = data + "px";
            break;
        case "fontSize":
            document.getElementById(this.BoxID).style.fontSize = data + "px";
            break;
        case "fontColor":
            document.getElementById(this.BoxID).style.color = data;
            break;
        case "fillColor":
            document.getElementById(this.BoxID).style.backgroundColor = data;
            break;
        case "textAlign":
            document.getElementById(this.BoxID).style.textAlign = data;
            break;
        case "borderWidth":
            this.SetBorder();
            break;
        case "readonly":
            this.SetReadonly();
            break;
        default:
            break;
    }
}
Irlovan.Control.TextBox.prototype.Off = function () {
    Irlovan.Control.Classic.prototype.Off.apply(this);
    document.getElementById(this.BoxID).style.backgroundColor = "grey";
    document.getElementById(this.BoxID).value = "##";
}
Irlovan.Control.TextBox.prototype.On = function () {
    Irlovan.Control.Classic.prototype.On.apply(this);
    document.getElementById(this.BoxID).style.backgroundColor = this.Config.fillColor.Value;
}
Irlovan.Control.TextBox.prototype.SetReadonly = function () {
    if ((this.Config.readonly.Value == true) || (this.Config.readonly.Value == "true")) {
        document.getElementById(this.BoxID).setAttribute("readonly", "readonly");
    } else {
        document.getElementById(this.BoxID).removeAttribute("readonly");
    }
}
Irlovan.Control.TextBox.prototype.SetBorder = function () {
    document.getElementById(this.BoxID).style.borderWidth = this.Config.borderWidth.Value + "px";
}
Irlovan.Control.TextBox.prototype.EditionFix = function () {
    if (!this.Config.fillColor) {
        this.Config.fillColor = { Attributes: "fillColor", Value: "white" };
    }
    if (!this.Config.textAlign) {
        this.Config.textAlign = { Attributes: "textAlign", Value: "left" };
    }
}