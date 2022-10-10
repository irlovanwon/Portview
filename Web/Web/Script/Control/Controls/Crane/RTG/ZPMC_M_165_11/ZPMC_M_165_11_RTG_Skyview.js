//Copyright(c) 2013,HIT All rights reserved.
//Des:ZPMC_M_165_11_RTG_Skyview Model
//Author:Irlovan   
//Date:2015-05-27
//modification :

Irlovan.Control.ZPMC_M_165_11_RTG_Skyview = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "ZPMC_M_165_11_RTG_Skyview", id, containerID, gridContainerID, controlClass, config ? config : ({
        tagName: { Attributes: "tagName", Value: "11" },
        scale: { Attributes: "scale", Value: 1, Description: "if set to 0, manual mode " },
        width: { Attributes: "width", Value: 300 },
        height: { Attributes: "height", Value: 565 },
        pos: { Attributes: "pos", Value: 0.5 },
    }), left, top, zIndex, isLock);
    this.CraneID = id + "_ZPMC_M_165_11_RTG_Skyview_Crane";
    this.BodyID = id + "_ZPMC_M_165_11_RTG_Skyview_Crane_Body";
    this.TrolleyID = this.ID + "_ZPMC_M_165_11_RTG_Skyview_Crane_Trolley";
    this.TagID = this.ID + "_Tag";
    this.CraneScale = 1.8843;
    this.CraneWidth = 300;
    this.TrolleyScale = 1.0125;
    this.TrolleyWidth = 143.3;
    this.Init(id, containerID);
    this.SetScale();

}
Irlovan.Control.ZPMC_M_165_11_RTG_Skyview.prototype = new Irlovan.Control.Classic();
Irlovan.Control.ZPMC_M_165_11_RTG_Skyview.prototype.constructor = Irlovan.Control.ZPMC_M_165_11_RTG_Skyview;
/**Init**/
Irlovan.Control.ZPMC_M_165_11_RTG_Skyview.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.CraneID + "'  style='background-size:100% 100%;position: relative;width:" + this.Config.width.Value + "px;height:" + this.Config.height.Value + "px;' >" +
       "<div id='" + this.BodyID + "'  style='background-size:100% 100%;position: absolute;width:" + this.Config.width.Value + "px;height:" + this.Config.height.Value + "px;background-image: url(Images/Crane/RTG/ZPMC_M_165_11/GantrySkyview.png);' />" +
       "<div id='" + this.TrolleyID + "'   style='background-size:100% 100%;position: absolute;width:" + this.Config.width.Value + "px;height:" + this.Config.height.Value + "px;background-image: url(Images/Crane/RTG/ZPMC_M_165_11/TrolleySkyview.png);' />" +
       "<div id='" + this.TagID + "'  style='position: absolute;border:thin solid black;right:100%;background-color: white;'>" + this.Config.tagName.Value + "</div>" +
       "</div>", this.ID, this.Pos);
}
/**SetValue**/
Irlovan.Control.ZPMC_M_165_11_RTG_Skyview.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "tagName":
            this.SetTag();
            break;
        case "pos":
            this.SetPos();
            break;
        case "height":
        case "width":
            this.SetSize();
            break;
        case "scale":
            this.SetScale();
            break;
        default:
            break;
    }
}

/**Set Scale**/
Irlovan.Control.ZPMC_M_165_11_RTG_Skyview.prototype.SetScale = function () {
    try { var scale = parseFloat(this.Config.scale.Value); } catch (e) { scale = 1; }
    if (scale != 0) {
        this.SetValue("width", "Value", this.CraneWidth * scale);
        this.SetValue("height", "Value", this.CraneWidth * this.CraneScale * scale);
    } else {
        this.SetValue("width", "Value", this.Config.width.Value);
        this.SetValue("height", "Value", this.Config.height.Value);
    }
}
/**Set Trolley**/
Irlovan.Control.ZPMC_M_165_11_RTG_Skyview.prototype.SetTrolley = function () {
    document.getElementById(this.TrolleyID).style.width = 0.4776 * parseFloat(this.Config.width.Value) + "px";
    document.getElementById(this.TrolleyID).style.height = 0.25675 * parseFloat(this.Config.height.Value) + "px";
    document.getElementById(this.TrolleyID).style.left = "26.119%";

}
/**Set Pos**/
Irlovan.Control.ZPMC_M_165_11_RTG_Skyview.prototype.SetPos = function () {
    var scale = parseFloat(this.Config.pos.Value);
    var top = 73 * scale + "%";
    document.getElementById(this.TrolleyID).style.top = top;
}
/**Set Pos**/
Irlovan.Control.ZPMC_M_165_11_RTG_Skyview.prototype.SetSize = function () {
    document.getElementById(this.BodyID).style.width = this.Config.width.Value + "px";
    document.getElementById(this.BodyID).style.height = this.Config.height.Value + "px";
    document.getElementById(this.CraneID).style.width = this.Config.width.Value + "px";
    document.getElementById(this.CraneID).style.height = this.Config.height.Value + "px";
    this.SetTrolley();
    this.SetPos();
}
/**Set Tag**/
Irlovan.Control.ZPMC_M_165_11_RTG_Skyview.prototype.SetTag = function () {
    document.getElementById(this.TagID).innerHTML = this.Config.tagName.Value;
}

Irlovan.Control.ZPMC_M_165_11_RTG_Skyview.prototype.Off = function () {
    Irlovan.Control.Classic.prototype.Off.apply(this);
}
Irlovan.Control.ZPMC_M_165_11_RTG_Skyview.prototype.On = function () {
    Irlovan.Control.Classic.prototype.On.apply(this);
}