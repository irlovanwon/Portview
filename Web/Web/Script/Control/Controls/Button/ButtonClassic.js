//Copyright(c) 2013,HIT All rights reserved.
//Des:Button with click and flash function
//Author:Irlovan   
//Date:2013-07-17
//modification :2015-04-01

Irlovan.Control.ButtonClassic = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "ButtonClassic", id, containerID, gridContainerID, controlClass, config ? config : ({
        uri: { Attributes: "uri", Value: '' },
        name: { Attributes: "name", Value: 'button' },
        fontSize: { Attributes: "fontSize", Value: '20' },
        fontColor: { Attributes: "fontColor", Value: 'black' },
        width: { Attributes: "width", Value: '140' },
        height: { Attributes: "height", Value: '35' },
        thickness: { Attributes: "thickness", Value: '0' },
        backgroundColor: { Attributes: "backgroundColor", Value: null },
        flash: { Attributes: "flash", Value: false },
        flashInfo: { Attributes: "flashInfo", Value: 'red;white|grey;white|600', Description: "fontOnColor;fontOffColor|BackgroundOn;BackgroundOff|interval" }
    }), left, top, zIndex, isLock);
    this.EditionFix();
    this.FlashTimer;
    this.FlashOn = false;
    this.FlashSplitChar = "|";
    this.VURSplitChar = "|";
    this.FlashColorSplitChar = ";";
    this.ButtonContainerID = id + "_buttonContainer";
    this.ButtonID = id + "_buttonClassic";
    this.Init(id, containerID);
}
Irlovan.Control.ButtonClassic.prototype = new Irlovan.Control.Classic();
Irlovan.Control.ButtonClassic.prototype.constructor = Irlovan.Control.ButtonClassic;
//init div element
Irlovan.Control.ButtonClassic.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.ButtonContainerID + "' style='position: relative;width:" + ((parseFloat(this.Config.width.Value) + 20) + "px") + ";height:" + this.Config.height.Value + "px;'>" +
       "<button type='button' id='" + this.ButtonID + "' style='background-color:redborder-width:" + this.Config.thickness.Value + "px;font-size:" + this.Config.fontSize.Value + "px;color:" + this.Config.fontColor.Value + ";line-height:" + this.Config.height.Value + "px;text-align:center;position: absolute;width:" + this.Config.width.Value + "px;height:" + this.Config.height.Value + "px'>" + this.Config.name.Value + "</button>" +
       "</div>", this.ID, this.Pos);
    document.getElementById(this.ButtonID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, this.TriggerHandler), false);
    this.Refresh();
}
Irlovan.Control.ButtonClassic.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "name":
            document.getElementById(this.ButtonID).innerHTML = data;
            break;
        case "fontSize":
        case "fontColor":
        case "width":
        case "height":
        case "thickness":
        case "backgroundColor":
            this.Refresh();
            break;
        case "flash":
            this.Flash();
            break;
        default:
            break;
    }
}

/**Refresh**/
Irlovan.Control.ButtonClassic.prototype.Refresh = function () {
    document.getElementById(this.ButtonID).removeAttribute("style");
    document.getElementById(this.ButtonID).style.fontSize = this.Config.fontSize.Value + "px";
    document.getElementById(this.ButtonID).style.color = this.Config.fontColor.Value;
    if (this.Config.backgroundColor) {
        document.getElementById(this.ButtonID).style.backgroundColor = this.Config.backgroundColor.Value;
    }
    document.getElementById(this.ButtonContainerID).style.width = (parseFloat(this.Config.width.Value) + 20) + "px";
    document.getElementById(this.ButtonID).style.width = this.Config.width.Value + "px";
    document.getElementById(this.ButtonContainerID).style.height = this.Config.height.Value + "px";
    document.getElementById(this.ButtonID).style.height = this.Config.height.Value + "px";
    document.getElementById(this.ButtonID).style.lineHeight = this.Config.height.Value + "px";
    document.getElementById(this.ButtonID).style.borderWidth = this.Config.thickness.Value + "px";
}

/**Flash the button**/
Irlovan.Control.ButtonClassic.prototype.Flash = function () {
    var isFlash = this.Config.flash.Value;
    if (Irlovan.Lib.Help.Boolean(isFlash)) {
        this.StopFlash();
        this.StartFlash();
    } else {
        this.StopFlash();
    }
}

/**Handler for Flashing event**/
Irlovan.Control.ButtonClassic.prototype.StartFlash = function () {
    var flashInfo = this.Config.flashInfo.Value;
    if (!flashInfo) { return; }
    var flashInfoList = flashInfo.split(this.FlashSplitChar);
    if (flashInfoList.length != 3) { return; }
    var interval = flashInfoList[2];
    var fontColorInfo = this.InitColorInfo(flashInfoList[0]);
    var backgroundInfo = this.InitColorInfo(flashInfoList[1]);
    this.FlashTimer = setInterval(Irlovan.IrlovanHelper.Bind(this, function () {
        var result = Irlovan.Lib.Help.Boolean(this.FlashOn);
        var button = document.getElementById(this.ButtonID);
        if (fontColorInfo != null) {
            button.style.color = result ? fontColorInfo[0] : fontColorInfo[1];
        }
        if (backgroundInfo != null) {
            button.style.backgroundColor=result ? backgroundInfo[0] : backgroundInfo[1];
        }
        this.FlashOn = !result;
    }), interval);
}

/**Stop Flashing**/
Irlovan.Control.ButtonClassic.prototype.StopFlash = function () {
    this.DisposeTimer();
    this.Refresh();
}

/**Init on/off color**/
Irlovan.Control.ButtonClassic.prototype.InitColorInfo = function (colorInfo) {
    if (!colorInfo) { return null; }
    var colorInfoList = colorInfo.split(this.FlashColorSplitChar);
    if (colorInfoList.length != 2) { return null; }
    return colorInfoList;
}

/**Handler for triggering event**/
Irlovan.Control.ButtonClassic.prototype.TriggerHandler = function () {
    if ((this.Config.uri.Value == "") || (this.Config.uri.Value == null)) { return; }
    var pageInfos=this.Config.uri.Value.split(this.VURSplitChar);
    Irlovan.Global.GUI.LoadGUI(pageInfos[0], pageInfos[1], pageInfos[2]);
}

/**Dispose**/
Irlovan.Control.ButtonClassic.prototype.Dispose = function () {
    document.getElementById(this.ButtonID).removeEventListener("click", Irlovan.IrlovanHelper.Bind(this, this.TriggerHandler), false);
    this.DisposeTimer();
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
}

/**Dispose Timer**/
Irlovan.Control.ButtonClassic.prototype.DisposeTimer = function () {
    if (this.FlashTimer != null) { clearInterval(this.FlashTimer); }
    this.FlashTimer = null;
}

/**Edition Fixed for the release before 2015-04-01**/
Irlovan.Control.ButtonClassic.prototype.EditionFix = function () {
    if (!this.Config.backgroundColor) {
        this.Config.backgroundColor = { Attributes: "backgroundColor", Value: null };
    }
    if (!this.Config.flash) {
        this.Config.flash = { Attributes: "flash", Value: false };
    }
    if (!this.Config.flashInfo) {
        this.Config.flashInfo = { Attributes: "flashInfo", Value: 'red;white|grey;white|600', Description: "fontOnColor;fontOffColor|BackgroundOn;BackgroundOff|interval" };
    }
}