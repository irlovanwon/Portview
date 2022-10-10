//Copyright(c) 2013,HIT All rights reserved.
//Des:
//Author:Irlovan   
//Date:2013-07-17
//modification :

Irlovan.Control.Label = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "Label", id, containerID, gridContainerID, controlClass, config ? config : ({
        tagName: { Attributes: "tagName", Value: "text" },
        fontSize: { Attributes: "fontSize", Value: "20" },
        color: { Attributes: "color", Value: 'black' },
        backgroundColor: { Attributes: "backgroundColor", Value: 'transparent' },
        borderColor: { Attributes: "borderColor", Value: 'transparent 1px solid' },
        fontWeight: { Attributes: "fontWeight", Value: "normal", Description: "normal, bold, bolder,lighter,100-900,initial" },
        fontFamily: { Attributes: "fontFamily", Value: "font1", Description: "times, courier,arial,serif,sans-serif,cursive,fantasy,monospace" },
        textWrap: { Attributes: "textWrap", Value: "nowrap", Description: "normal, nowrap, pre,pre-line,pre-wrap,initial,inherit" },
        height: { Attributes: "height", Value: '20' }
    }), left, top, zIndex, isLock);
    this.EditionFix();
    this.TextID = id + "_text";
    this.Init(id, containerID);
}
Irlovan.Control.Label.prototype = new Irlovan.Control.Classic();
Irlovan.Control.Label.prototype.constructor = Irlovan.Control.Label;
//init div element
Irlovan.Control.Label.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var Label = Irlovan.ControlHelper.CreateControlByStr(
       "<div nowrap='nowrap' id='" + this.TextID + "' style='font-weight:" + this.Config.fontWeight.Value + ";border:" + this.Config.borderColor.Value + ";font-size:" + this.Config.fontSize.Value + "px" + ";color:" + this.Config.color.Value + ";text-align:left;background-color:" + this.Config.backgroundColor.Value + ";position:relative;height:" + this.Config.height.Value + "px;'>" + this.Config.tagName.Value +
       "</div>", this.ID, this.Pos);
    this.TextWrap(this.Config.textWrap.Value);
    this.FontFamily(this.Config.fontFamily.Value)
}
Irlovan.Control.Label.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "fontSize":
            document.getElementById(this.TextID).style.width = (this.Config.tagName.Value.length * parseInt(this.Config.fontSize.Value) + 'px');
            document.getElementById(this.TextID).style.height = parseInt(this.Config.fontSize.Value) + 'px';
            document.getElementById(this.TextID).style.fontSize = data + "px";
            break;
        case "tagName":
            document.getElementById(this.TextID).innerHTML = data;
            break;
        case "color":
            document.getElementById(this.TextID).style.color = data;
            break;
        case "backgroundColor":
            document.getElementById(this.TextID).style.backgroundColor = data;
            break;
        case "borderColor":
            document.getElementById(this.TextID).style.border = data;
            break;
        case "fontWeight":
            document.getElementById(this.TextID).style.fontWeight = data;
            break;
        case "fontFamily":
            this.FontFamily(data);
            break;
        case "textWrap":
            this.TextWrap(data);
            break;
        case "height":
            document.getElementById(this.TextID).style.height = data + "px";
            break;
        case "id":
            this.TextID = data + "_text";
            break;
        default:
            break;
    }
}
Irlovan.Control.Label.prototype.Off = function () {
    Irlovan.Control.Classic.prototype.Off.apply(this);
    document.getElementById(this.TextID).style.backgroundColor = "grey";
    document.getElementById(this.TextID).value = "##";
}
Irlovan.Control.Label.prototype.On = function () {
    Irlovan.Control.Classic.prototype.On.apply(this);
    document.getElementById(this.TextID).style.backgroundColor = this.Config.backgroundColor.Value;
}
Irlovan.Control.Label.prototype.EditionFix = function () {
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
Irlovan.Control.Label.prototype.TextWrap = function (wrap) {
    $('#' + this.TextID).css('white-space', wrap);
}
Irlovan.Control.Label.prototype.FontFamily = function (fontFamily) {
    $('#' + this.TextID).css('font-family', fontFamily);
}