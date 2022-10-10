//Copyright(c) 2013,HIT All rights reserved.
//Des:FrameCrane
//Author:Irlovan   
//Date:2013-05-23
//modification :

Irlovan.Control.FrameCrane = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "FrameCrane", id, containerID, gridContainerID, controlClass, config ? config : ({
        tagName: { Attributes: "tagName", Value: "11" },
        height: { Attributes: "height", Value: 428 },
        width: { Attributes: "width", Value: 602 },
        lock: { Attributes: "lock", Value: false },
        verticalPos: { Attributes: "verticalPos", Value: 0.5 },
        horizontalPos: { Attributes: "horizontalPos", Value: 0.5 },
        moveForward: { Attributes: "moveForward", Value: false },
        moveBackward: { Attributes: "moveBackward", Value: false },
    }), left, top, zIndex, isLock);
    this.FrameCraneID = id + "_frameCrane";
    this.BodyID = id + "_crane_body";
    this.RopeID = this.ID + "_Rope";
    this.TrolleyID = this.ID + "_Trolley";
    this.HoistID = this.ID + "_Hoist";
    this.TagID = this.ID + "_Tag";
    this.ForwardID = this.ID + "_Forward";
    this.BackwardID = this.ID + "_Backward";
    this.Init(id, containerID);

}

Irlovan.Control.FrameCrane.prototype = new Irlovan.Control.Classic();
Irlovan.Control.FrameCrane.prototype.constructor = Irlovan.Control.FrameCrane;
//init div element
Irlovan.Control.FrameCrane.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.FrameCraneID + "'  style='background-size:100% 100%;position: relative;width:" + this.Config.width.Value + "px;height:" + this.Config.height.Value + "px;' >" +
       "<div id='" + this.RopeID + "'  style='background-size:100% 100%;left:11%;position: absolute;top:1.5%;width:" + parseInt(this.Config.width.Value) * 0.083 + "px;height:0%;border:thin solid black;' />" +
       "<div id='" + this.ForwardID + "'  style='position: absolute;width:5px;height:5px;right:5%;top:35%;visibility: hidden;border:thick solid blue;' />" +
       "<div id='" + this.BackwardID + "'  style='position: absolute;width:5px;height:5px;left:5%;top:35%;visibility: hidden;border:thick solid blue;' />" +
       "<div id='" + this.HoistID + "' style='background-size:100% 100%;position: absolute;left:9%;width:" + 0.126 * parseInt(this.Config.width.Value) + "px;height:" + 0.17 * parseInt(this.Config.width.Value) + "px;background-image: url(Images/ControlIcon/Crane/FrameCraneHoistE.png)' />" +
       "<div id='" + this.BodyID + "' style='background-size:100% 100%;position: absolute;width:" + this.Config.width.Value + "px;height:" + this.Config.height.Value + "px;background-image: url(Images/ControlIcon/Crane/FrameCrane.png);' />" +
       "<div id='" + this.TrolleyID + "' style='background-size:100% 100%;position: absolute;width:" + 0.18 * parseInt(this.Config.width.Value) + "px;height:" + 0.06 * parseInt(this.Config.width.Value) + "px;top:-" + 0.03 * parseInt(this.Config.width.Value) + "px;background-image: url(Images/ControlIcon/Crane/FrameCraneTrolley.png)' />" +
       "<div id='" + this.TagID + "'  style='position: absolute;border:thin solid black;right:20%;top:5%;background-color: white;'>" + this.Config.tagName.Value + "</div>" +
       "</div>", this.ID, this.Pos);
    this.SetSize();
    this.SetHorizontal();
    this.SetVerticle();
    this.SetLock();
}
Irlovan.Control.FrameCrane.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "tagName":
            document.getElementById(this.TagID).innerHTML = data;
            break;
        case "height":
        case "width":
            this.SetSize();
            this.SetHorizontal();
            this.SetVerticle();
            this.SetLock();
            break;
        case "lock":
            this.SetLock();
            break;
        case "verticalPos":
            this.SetVerticle();
            break;
        case "horizontalPos":
            this.SetHorizontal();
            break;
        case "moveForward":
            document.getElementById(this.ForwardID).style.visibility = (data == "true") ? "visible" : "hidden";
            break;
        case "moveBackward":
            document.getElementById(this.BackwardID).style.visibility = (data == "true") ? "visible" : "hidden";
            break;
        default:
            break;
    }
}
Irlovan.Control.FrameCrane.prototype.SetHorizontal = function () {
    var result = this.Config.horizontalPos.Value * 0.7;
    document.getElementById(this.TrolleyID).style.left = (parseFloat(result) * 100 + 6) + "%";
    document.getElementById(this.RopeID).style.left = (parseFloat(result) * 100 + 11) + "%";
    document.getElementById(this.HoistID).style.left = (parseFloat(result) * 100 + 9) + "%";
}
Irlovan.Control.FrameCrane.prototype.SetVerticle = function () {
    var result = (this.Config.verticalPos.Value*1+0.11)*0.74;
    document.getElementById(this.HoistID).style.top = parseFloat(result) * 100 + "%";
    document.getElementById(this.RopeID).style.height = parseFloat(result) * parseInt(this.Config.height.Value) + "px";
}
Irlovan.Control.FrameCrane.prototype.SetSize = function () {
    var width = this.Config.width.Value;
    var height = this.Config.height.Value;
    document.getElementById(this.FrameCraneID).style.width = width + "px";
    document.getElementById(this.FrameCraneID).style.height = height + "px"; 
    document.getElementById(this.BodyID).style.width = width + "px";
    document.getElementById(this.BodyID).style.height = height + "px";
    document.getElementById(this.RopeID).style.width = parseInt(width) * 0.079 + "px";
    document.getElementById(this.HoistID).style.width = 0.126 * parseInt(width) + "px";
    document.getElementById(this.TrolleyID).style.top = -0.03 * parseInt(height) + "px";
    document.getElementById(this.TrolleyID).style.width = 0.18 * parseInt(width) + "px";
    document.getElementById(this.TrolleyID).style.height = 0.06 * parseInt(height) + "px";
}
Irlovan.Control.FrameCrane.prototype.SetLock = function () {
    var data = this.Config.lock.Value;
    if ((data == true) || (data == "true")) {
        document.getElementById(this.HoistID).style.backgroundImage = "url(Images/ControlIcon/Crane/FrameCraneHoist.png)";
        document.getElementById(this.HoistID).style.height = 0.34 * parseInt(this.Config.height.Value) + "px";
    } else {
        document.getElementById(this.HoistID).style.backgroundImage = "url(Images/ControlIcon/Crane/FrameCraneHoistE.png)";
        document.getElementById(this.HoistID).style.height = 0.17 * parseInt(this.Config.height.Value) + "px";
    }
}
Irlovan.Control.FrameCrane.prototype.Off = function () {
    Irlovan.Control.Classic.prototype.Off.apply(this);
    document.getElementById(this.HoistID).style.backgroundImage = "url(Images/ControlIcon/Crane/FrameCraneHoistE.png)";
    document.getElementById(this.HoistID).style.height = 0.17 * parseInt(this.Config.height.Value) + "px";
    document.getElementById(this.TrolleyID).style.left = (6) + "%";
    document.getElementById(this.RopeID).style.left = (11) + "%";
    document.getElementById(this.HoistID).style.left = (9) + "%";
    document.getElementById(this.HoistID).style.top ="0%";
    document.getElementById(this.RopeID).style.height ="0px";
}
Irlovan.Control.FrameCrane.prototype.On = function () {
    Irlovan.Control.Classic.prototype.On.apply(this);
    this.SetHorizontal();
    this.SetVerticle();
    this.SetLock();
}