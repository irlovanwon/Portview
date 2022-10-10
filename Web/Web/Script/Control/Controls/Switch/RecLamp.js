//Copyright(c) 2013,HIT All rights reserved.
//Des:
//Author:Irlovan   
//Date:
//modification :


Irlovan.Control.RecLamp = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "RecLamp", id, containerID, gridContainerID, controlClass, config ? config : ({
        tagName: { Attributes: "tagName", Value: "Text" },
        value: { Attributes: "value", Value: false },
        fontSize: { Attributes: "fontSize", Value: "20" },
        borderColor: { Attributes: "borderColor", Value: 'transparent 1px solid' },
        size: { Attributes: "size", Value: "20px" },
        fontColor: { Attributes: "fontColor", Value: "black" },
        fontWeight: { Attributes: "fontWeight", Value: "normal", Description: "normal, bold, bolder,lighter,100-900,initial" },
        fontFamily: { Attributes: "fontFamily", Value: "font1", Description: "times, courier,arial,serif,sans-serif,cursive,fantasy,monospace" },
        textWrap: { Attributes: "textWrap", Value: "nowrap", Description: "normal, nowrap, pre,pre-line,pre-wrap,initial,inherit" },
        posFix: { Attributes: "posFix", Value: "80px" },
        onColor: { Attributes: "onColor", Value: 'lime' },
        offColor: { Attributes: "offColor", Value: 'white' }
    }), left, top, zIndex, isLock);
    this.EditionFix();
    this.LampID = this.ID + "_Lamp";
    this.TagID = this.ID + "_Tag";
    this.Init(id, containerID);
    this.SetFontSize();
}
Irlovan.Control.RecLamp.prototype = new Irlovan.Control.Classic();
Irlovan.Control.RecLamp.prototype.constructor = Irlovan.Control.RecLamp;
//init div element
Irlovan.Control.RecLamp.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div style='overflow: hidden;padding: 0px 0px 0px 0px, auto'>" +
       "<div id='" + this.TagID + "' style='font-size:" + this.Config.fontSize.Value + "px;font-weight:" + this.Config.fontWeight.Value + ";color:" + this.Config.fontColor.Value + ";text-align:left;position:absolute;height:" + this.Config.fontSize.Value + ";'>" + this.Config.tagName.Value + "</div>" +
       "<div id='" + this.LampID + "' style='background-color: " + this.Config.offColor.Value + ";position: absolute;border:thin solid #000000;padding: 2px 2px 2px 2px; width:" + this.Config.size.Value + ";height:" + this.Config.size.Value + ";left:" + this.Config.posFix.Value + "' />" +
       "</div>", this.ID, this.Pos);
    this.TextWrap(this.Config.textWrap.Value);
    this.FontFamily(this.Config.fontFamily.Value)
}
Irlovan.Control.RecLamp.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "value":
            document.getElementById(this.LampID).style.backgroundColor = (data.toString() == "true") ? this.Config.onColor.Value : this.Config.offColor.Value;
            break;
        case "fontSize":
            this.SetFontSize();
            break;
        case "fontColor":
            document.getElementById(this.TagID).style.color = data;
            break;
        case "tagName":
            document.getElementById(this.TagID).innerHTML = data;
            document.getElementById(this.TagID).style.width = (data.length * parseInt(this.Config.fontSize.Value) / 2 + 'px');
            break;
        case "borderColor":
            document.getElementById(this.TagID).style.border = data;
            break;
        case "posFix":
            document.getElementById(this.LampID).style.visibility = "visible";
            document.getElementById(this.LampID).style.left = data;
            if (data == "0px") {
                document.getElementById(this.LampID).style.visibility = "collapse";
            }
            break;
        case "size":
            document.getElementById(this.LampID).style.width = data;
            document.getElementById(this.LampID).style.height = data;
            document.getElementById(this.TagID).height = data;
            break;
        case "left":
            document.getElementById(this.ID).style.left = data;
            break;
        case "top":
            document.getElementById(this.ID).style.top = data;
            break;
        case "fontWeight":
            document.getElementById(this.TagID).style.fontWeight = data;
            break;
        case "fontFamily":
            this.FontFamily(data);
            break;
        case "textWrap":
            this.TextWrap(data);
            break;
        case "id":
            this.LampID = data + "_Lamp";
            this.TagID = data + "_Tag";
            break;
        default:
            break;
    }
}
Irlovan.Control.RecLamp.prototype.Off = function () {
    Irlovan.Control.Classic.prototype.Off.apply(this);
    document.getElementById(this.LampID).style.backgroundColor = "black";
    document.getElementById(this.TagID).style.textDecoration = "underline";

}
Irlovan.Control.RecLamp.prototype.On = function () {
    Irlovan.Control.Classic.prototype.On.apply(this);
    document.getElementById(this.LampID).style.backgroundColor = (this.Config.value.Value.toString() == "true") ? this.Config.onColor.Value : this.Config.offColor.Value;
    document.getElementById(this.TagID).style.textDecoration = "";
}

Irlovan.Control.RecLamp.prototype.SetFontSize = function () {
    document.getElementById(this.TagID).style.width = (this.Config.tagName.Value.length * parseInt(this.Config.fontSize.Value) / 2 + 'px');
    document.getElementById(this.TagID).style.height = parseInt(this.Config.fontSize.Value) + 'px';
    document.getElementById(this.TagID).style.fontSize = this.Config.fontSize.Value + "px";
}
Irlovan.Control.RecLamp.prototype.EditionFix = function () {
    if (!this.Config.fontWeight) {
        this.Config.fontWeight = { Attributes: "fontWeight", Value: "normal", Description: "normal, bold, bolder,lighter,100-900,initial" };
    }
    if (!this.Config.fontFamily) {
        this.Config.fontFamily = { Attributes: "fontFamily", Value: "font1", Description: "times, courier,arial,serif,sans-serif,cursive,fantasy,monospace" };
    }
    if (!this.Config.textWrap) {
        this.Config.textWrap = { Attributes: "textWrap", Value: "nowrap", Description: "normal, nowrap, pre,pre-line,pre-wrap,initial,inherit" };
    }
}
Irlovan.Control.RecLamp.prototype.TextWrap = function (wrap) {
    $('#' + this.TagID).css('white-space', wrap);
}
Irlovan.Control.RecLamp.prototype.FontFamily = function (fontFamily) {
    //document.getElementById(this.TagID).style.fontFamily = fontFamily;
    $('#' + this.TagID).css('font-family', fontFamily);
}