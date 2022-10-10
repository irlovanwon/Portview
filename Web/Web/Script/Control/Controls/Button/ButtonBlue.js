//Copyright(c) 2013,HIT All rights reserved.
//Des:Button Blue
//Author:Irlovan   
//Date:2013-07-17
//modification :2015-04-01

Irlovan.Control.ButtonBlue = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "ButtonBlue", id, containerID, gridContainerID, controlClass, config ? config : ({
        uri: { Attributes: "uri", Value: '' },
        name: { Attributes: "name", Value: 'button' },
        fontSize: { Attributes: "fontSize", Value: '20px' },
        fontColor: { Attributes: "fontColor", Value: 'white' },
        width: { Attributes: "width", Value: '140px' },
        height: { Attributes: "height", Value: '35px' },
        flash: { Attributes: "flash", Value: false },
        flashInfo: { Attributes: "flashInfo", Value: 'red;white|false|600', Description: "fontOnColor;fontOffColor|false or true to indicate if the background flashing|interval" }
    }), left, top, zIndex, isLock);
    this.EditionFix();
    this.FlashTimer;
    this.FlashOn = false;
    this.ButtonImage = "Button.png";
    this.ButtonPressImage = "ButtonPress.png";
    this.ButtonAlarmImage = "ButtonAlarm.png";
    this.FlashSplitChar = "|";
    this.VURSplitChar = "|";
    this.LinkFlag = "link:";
    this.FlashColorSplitChar = ";";
    this.ButtonContainerID = id + "_buttonContainer";
    this.ButtonID = id + "_button";
    this.Init(id, containerID);
}
Irlovan.Control.ButtonBlue.prototype = new Irlovan.Control.Classic();
Irlovan.Control.ButtonBlue.prototype.constructor = Irlovan.Control.ButtonBlue;
//init div element
Irlovan.Control.ButtonBlue.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.ButtonContainerID + "' style='position: relative;width:" + ((parseFloat(this.Config.width.Value) + 20) + "px") + ";height:" + this.Config.height.Value + ";'>" +
       "<div id='" + this.ButtonID + "' style='background-size:100% 100%;font-size:" + this.Config.fontSize.Value + ";color:" + this.Config.fontColor.Value + ";line-height:" + this.Config.height.Value + ";text-align:center;position: absolute;width:" + this.Config.width.Value + ";height:" + this.Config.height.Value + ";background-image: url(Images/Template/Blue/" + this.ButtonImage + ");'>" + this.Config.name.Value + "</div>" +
       "</div>", this.ID, this.Pos);
    document.getElementById(this.ButtonID).addEventListener("mouseover", Irlovan.IrlovanHelper.Bind(this, this.MouseOver), false);
    document.getElementById(this.ButtonID).addEventListener("mouseleave", Irlovan.IrlovanHelper.Bind(this, this.MouseLeave), false);
    document.getElementById(this.ButtonID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, this.TriggerHandler), false);
    this.Refresh();
}
Irlovan.Control.ButtonBlue.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "name":
            document.getElementById(this.ButtonID).innerHTML = data;
            break;
        case "fontSize":
        case "fontColor":
        case "width":
        case "height":
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
Irlovan.Control.ButtonBlue.prototype.Refresh = function () {
    document.getElementById(this.ButtonID).style.fontSize = this.Config.fontSize.Value;
    document.getElementById(this.ButtonID).style.color = this.Config.fontColor.Value;
    document.getElementById(this.ButtonContainerID).style.width = (parseFloat(this.Config.width.Value) + 20) + "px";
    document.getElementById(this.ButtonID).style.width = this.Config.width.Value;
    document.getElementById(this.ButtonContainerID).style.height = this.Config.height.Value;
    document.getElementById(this.ButtonID).style.height = this.Config.height.Value;
    document.getElementById(this.ButtonID).style.lineHeight = this.Config.height.Value;
}

/**Handler for triggering event**/
Irlovan.Control.ButtonBlue.prototype.TriggerHandler = function () {
    if ((this.Config.uri.Value == "") || (this.Config.uri.Value == null)) { return; }
    if (this.Config.uri.Value.indexOf(this.LinkFlag) != -1) {
        this.TriggerLinkHandler(this.Config.uri.Value);
    } else {
        this.TriggerPageHandler(this.Config.uri.Value);
    }
}

Irlovan.Control.ButtonBlue.prototype.TriggerPageHandler = function (page) {
    var pageInfos = page.split(this.VURSplitChar);
    Irlovan.Global.GUI.LoadGUI(pageInfos[0], pageInfos[1], pageInfos[2]);
}

Irlovan.Control.ButtonBlue.prototype.TriggerLinkHandler = function (link) {
    var link = link.replace(this.LinkFlag, '');
    window.location.href = link;
}

/**Flash the button**/
Irlovan.Control.ButtonBlue.prototype.Flash = function () {
    var isFlash = this.Config.flash.Value;
    if (Irlovan.Lib.Help.Boolean(isFlash)) {
        this.StopFlash();
        this.StartFlash();
    } else {
        this.StopFlash();
    }
}

/**Handler for Flashing event**/
Irlovan.Control.ButtonBlue.prototype.StartFlash = function () {
    var flashInfo = this.Config.flashInfo.Value;
    if (!flashInfo) { return; }
    var flashInfoList = flashInfo.split(this.FlashSplitChar);
    if (flashInfoList.length != 3) { return; }
    var interval = flashInfoList[2];
    var fontColorInfo = this.InitColorInfo(flashInfoList[0]);
    var isBGFlash = Irlovan.Lib.Help.Boolean(flashInfoList[1]);
    this.FlashTimer = setInterval(Irlovan.IrlovanHelper.Bind(this, function () {
        var result = Irlovan.Lib.Help.Boolean(this.FlashOn);
        var button = document.getElementById(this.ButtonID);
        if (fontColorInfo != null) {
            button.style.color = result ? fontColorInfo[0] : fontColorInfo[1];
        }
        if (isBGFlash) {
            this.SetImage(result ? this.ButtonPressImage : this.ButtonAlarmImage);
        }
        this.FlashOn = !result;
    }), interval);
}

/**Stop Flashing**/
Irlovan.Control.ButtonBlue.prototype.StopFlash = function () {
    this.DisposeTimer();
    document.getElementById(this.ButtonID).style.color = this.Config.fontColor.Value;
    this.SetImage(this.ButtonImage);
}

/**Init on/off color**/
Irlovan.Control.ButtonBlue.prototype.InitColorInfo = function (colorInfo) {
    if (!colorInfo) { return null; }
    var colorInfoList = colorInfo.split(this.FlashColorSplitChar);
    if (colorInfoList.length != 2) { return null; }
    return colorInfoList;
}

/**Handler for MouseOver event**/
Irlovan.Control.ButtonBlue.prototype.MouseOver = function () {
    this.SetImage(this.ButtonPressImage);
}

/**Handler for MouseLeave event**/
Irlovan.Control.ButtonBlue.prototype.MouseLeave = function () {
    this.SetImage(this.ButtonImage);
}

/**Set Background Image for button**/
Irlovan.Control.ButtonBlue.prototype.SetImage = function (name) {
    try { document.getElementById(this.ButtonID).style.backgroundImage = "url('Images/Template/Blue/" + name + "')"; } catch (e) { }
}

/**Edition Fixed for the release before 2015-04-01**/
Irlovan.Control.ButtonBlue.prototype.EditionFix = function () {
    if (!this.Config.flash) {
        this.Config.flash = { Attributes: "flash", Value: false };
    }
    if (!this.Config.flashInfo) {
        this.Config.flashInfo = { Attributes: "flashInfo", Value: 'red;white|false|600', Description: "fontOnColor;fontOffColor|false or true to indicate if the background flashing|interval" };
    }
}

/**Dispose**/
Irlovan.Control.ButtonBlue.prototype.Dispose = function () {
    document.getElementById(this.ButtonID).removeEventListener("mouseover", Irlovan.IrlovanHelper.Bind(this, this.MouseOver), false);
    document.getElementById(this.ButtonID).removeEventListener("mouseleave", Irlovan.IrlovanHelper.Bind(this, this.MouseLeave), false);
    document.getElementById(this.ButtonID).removeEventListener("click", Irlovan.IrlovanHelper.Bind(this, this.TriggerHandler), false);
    this.DisposeTimer();
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
}

/**Dispose Timer**/
Irlovan.Control.ButtonBlue.prototype.DisposeTimer = function () {
    if (this.FlashTimer != null) { clearInterval(this.FlashTimer); }
    this.FlashTimer = null;
}
