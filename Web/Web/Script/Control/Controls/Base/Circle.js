//Copyright(c) 2013,HIT All rights reserved.
//Des:Circle Control
//Author:Irlovan   
//Date:2014-01-17
//modification :2015-03-31

Irlovan.Control.Circle = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "Circle", id, containerID, gridContainerID, controlClass, config ? config : ({
        r: { Attributes: "r", Value: 50 },
        fillColor: { Attributes: "fillColor", Value: "transparent" },
        strokeColor: { Attributes: "strokeColor", Value: "black" },
        thickness: { Attributes: "thickness", Value: "2" }
    }), left, top, zIndex, isLock);
    this.CircleID = this.ID + "_circle";
    this.Init(id, containerID);
}
Irlovan.Control.Circle.prototype = new Irlovan.Control.Classic();
Irlovan.Control.Circle.prototype.constructor = Irlovan.Control.Circle;
//init div element
Irlovan.Control.Circle.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    Irlovan.ControlHelper.CreateControlByStr(
   "<canvas  id='" + this.CircleID + "' width='" + (parseFloat(this.Config.r.Value) + parseFloat(this.Config.thickness.Value)) * 2 + "' height='" + (parseFloat(this.Config.r.Value) + parseFloat(this.Config.thickness.Value)) * 2 + "' >" +
   "</canvas >", this.ID, null);
    this.SetCircle(this.Config.fillColor.Value, this.Config.strokeColor.Value, parseFloat(this.Config.r.Value), parseFloat(this.Config.thickness.Value));
}
Irlovan.Control.Circle.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "r":
        case "fillColor":
        case "strokeColor":
        case "thickness":
            this.SetCircle(this.Config.fillColor.Value, this.Config.strokeColor.Value, parseFloat(this.Config.r.Value), parseFloat(this.Config.thickness.Value));
            break;
        default:
            break;
    }
}
Irlovan.Control.Circle.prototype.SetCircle = function (fillStyle, strokeStyle, r, thickness) {
    var c = document.getElementById(this.CircleID);
    c.width = (parseFloat(this.Config.r.Value) + parseFloat(this.Config.thickness.Value)) * 2;
    c.height = (parseFloat(this.Config.r.Value) + parseFloat(this.Config.thickness.Value)) * 2;
    var ctx = c.getContext("2d");
    ctx.fillStyle = fillStyle;
    ctx.strokeStyle = strokeStyle;
    ctx.beginPath();
    ctx.arc(r + thickness, r + thickness, r, 0, 2 * Math.PI);
    ctx.lineWidth = thickness;
    ctx.stroke();
    ctx.fill();
}

Irlovan.Control.Circle.prototype.Off = function () {
    Irlovan.Control.Classic.prototype.Off.apply(this);
    this.SetCircle("black", "black", parseFloat(this.Config.r.Value), parseFloat(this.Config.thickness.Value));
}
Irlovan.Control.Circle.prototype.On = function () {
    Irlovan.Control.Classic.prototype.On.apply(this);
    this.SetCircle(this.Config.fillColor.Value, this.Config.strokeColor.Value, parseFloat(this.Config.r.Value), parseFloat(this.Config.thickness.Value));
}