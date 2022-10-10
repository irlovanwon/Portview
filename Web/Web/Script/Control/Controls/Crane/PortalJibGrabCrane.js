//Copyright(c) 2013,HIT All rights reserved.
//Des:PortalJibHookCrane
//Author:Irlovan   
//Date:2013-05-23
//modification :

Irlovan.Control.PortalJibHookCrane = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "PortalJibHookCrane", id, containerID, gridContainerID, controlClass, config ? config : ({
        tagName: { Attributes: "tagName", Value: "11" },
        height: { Attributes: "height", Value: 0 },
        width: { Attributes: "width", Value: 0 },
        moveForward: { Attributes: "moveForward", Value: false },
        moveBackward: { Attributes: "moveBackward", Value: false }
    }), left, top, zIndex, isLock);
    this.RopeID = this.ID + "_Rope";
    this.TrolleyID = this.ID + "_Trolley";
    this.HoistID = this.ID + "_Hoist";
    this.TagID = this.ID + "_Tag";
    this.ForwardID = this.ID + "_Forward";
    this.BackwardID = this.ID + "_Backward";
    this.Init(id, containerID);

}

Irlovan.Control.PortalJibHookCrane.prototype = new Irlovan.Control.Classic();
Irlovan.Control.PortalJibHookCrane.prototype.constructor = Irlovan.Control.PortalJibHookCrane;
//init div element
Irlovan.Control.PortalJibHookCrane.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div style='position: relative;width:602px;height:428px;background-image: url(Images/ControlIcon/Crane/FrameCrane.png);' >" +
       "<div id='" + this.TagID + "'  style='position: absolute;border:thin solid black;right:10%;top:11.5%;background-color: white;'>" + this.Config.tagName.Value + "</div>" +
       "<div id='" + this.RopeID + "'  style='position: absolute;top:1%;width:50px;height:80px;border:thin solid black;' />" +
       "<div id='" + this.ForwardID + "'  style='position: absolute;width:5px;height:5px;right:5%;top:35%;visibility: hidden;border:thick solid blue;' />" +
       "<div id='" + this.BackwardID + "'  style='position: absolute;width:5px;height:5px;left:5%;top:35%;visibility: hidden;border:thick solid blue;' />" +
       "<div id='" + this.HoistID + "' style='position: absolute;width:76px;height:106px;background-image: url(Images/ControlIcon/Crane/FrameCraneHoist.png)' />" +
       "<div id='" + this.TrolleyID + "' style='position: absolute;width:191px;height:47px;top:0%;background-image: url(Images/ControlIcon/Crane/FrameCraneTrolley.png)' />" +
       "</div>", this.ID, this.Pos);
}
Irlovan.Control.PortalJibHookCrane.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "tagName":
            document.getElementById(this.TagID).innerHTML=data;
            break;
        case "height":
            var result = data * 0.7;
            document.getElementById(this.HoistID).style.top = parseFloat(result) * 100 + "%";
            document.getElementById(this.RopeID).style.height = parseFloat(result) * 428 + "px";
            break;
        case "width":
            var result = data * 0.7;
            document.getElementById(this.TrolleyID).style.left = parseFloat(result) * 100 + "%";
            document.getElementById(this.RopeID).style.left = (parseFloat(result) * 100+ 11) + "%";
            document.getElementById(this.HoistID).style.left = (parseFloat(result) * 100 + 9) + "%";
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