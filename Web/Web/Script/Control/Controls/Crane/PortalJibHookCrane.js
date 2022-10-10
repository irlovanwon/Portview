//Copyright(c) 2014,HIT All rights reserved.
//Des:PortalJibGrabCrane
//Author:xiaobai   
//Date:2013-07-18
//modification :2014-11-22

Irlovan.Control.PortalJibHookCrane = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "PortalJibHookCrane", id, containerID, gridContainerID, controlClass, config ? config : ({
        width: { Attributes: "width", Value: 525 },
        height: { Attributes: "height", Value: 450 },
        luff: { Attributes: "luff", Value: 60, Description: "Actived when luff encoder is bypass" },
        luff2: { Attributes: "luff2", Value: 80, Description: "Actived when luff encoder is bypass" },
        luffEncoder: { Attributes: "luffEncoder", Value: 0.5, Description: "range:0-1,bypass by setting to more than 1" },
        hoist: { Attributes: "hoist", Value: 0.5, Description: "range:0-1" }
    }), left, top, zIndex, isLock);
    this.CraneID = this.ID + "_jbCrane";
    this.RopeID = this.ID + "_Rope";
    this.HandID = this.ID + "_Hand";
    this.BodyID = this.ID + "_Body";
    this.LuffID = this.ID + "_Luff1";
    this.LuffID2 = this.ID + "_Luff2";
    this.HoistID = this.ID + "_Hoist";
    this.RopeDegree = 0;
    this.OffsetDegreeLuff = 1;
    this.OffsetDegreeLuff2 = 0.5;
    this.OffsetDegreeHoist = 3.7;
    this.OffsetTopLuff2 = 2;
    this.OffsetHeightHoist = 3;
    this.OffsetHeightFix = 8;
    this.OffsetHoistFix = 1;
    this.Init(id, containerID);
}

Irlovan.Control.PortalJibHookCrane.prototype = new Irlovan.Control.Classic();
Irlovan.Control.PortalJibHookCrane.prototype.constructor = Irlovan.Control.PortalJibHookCrane;
//init div element
Irlovan.Control.PortalJibHookCrane.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.CraneID + "' style='position: relative;background-size:100% 100%;width:" + this.Config.width.Value + "px;height:" + this.Config.height.Value + "px;'>" +
       "<div id='" + this.RopeID + "'  style='position: absolute;top:-25px;left:-93px;width:0.5%;height:30%;border:2px solid #353535;' />" +
       "<div id='" + this.LuffID + "' style='position: absolute;background-size:100% 100%; -webkit-transform: rotate(29.25deg); -webkit-transform-origin: 0% 50%;left:28%;top:10%;width:57.3%;height:5%;background-image: url(Images/ControlIcon/PortalJibHookCrane/PortalJibHookCraneLuff2.png)' />" +
       "<div id='" + this.LuffID2 + "' style='position: absolute;background-size:100% 100%; -webkit-transform: rotate(29.25deg); -webkit-transform-origin: 0% 50%;left:28%;top:10%;width:25%;height:6%;background-image: url(Images/ControlIcon/PortalJibHookCrane/PortalJibHookCraneLuff_b.png)' />" +
       "<div id='" + this.HandID + "' style='background-size:100% 100%;position: absolute;left:28%;top:38%;width:50%;height:5%;background-image: url(Images/ControlIcon/PortalJibHookCrane/PortalJibHookCraneLuff.png)' />" +
       "<div id='" + this.BodyID + "' style='background-size:100% 100%;position: absolute;width:50%;height:100%;background-image: url(Images/ControlIcon/PortalJibHookCrane/PortalJibCrane.png);' />" +
       "<div id='" + this.HoistID + "' style='background-size:100% 100%;position: absolute;right:-397px;top:210px;width:8%;height:12%;background-image: url(Images/ControlIcon/PortalJibHookCrane/PortalJibHookCraneHoist.png)' />" +
       "</div>", this.ID, this.Pos);
    this.Animate();
}
Irlovan.Control.PortalJibHookCrane.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "luff":
            this.Animate();
            break;
        case "luff2":
            this.Animate();
            break;
        case "hoist":
            this.Animate();
            break;
        case "luffEncoder":
            this.Animate();
            break;
        case "width":
            document.getElementById(this.CraneID).style.width = parseFloat(data) + "px";
            this.Animate();
            break;
        case "height":
            document.getElementById(this.CraneID).style.height = parseFloat(data) + "px";
            this.Animate();
            break;
        default:
            break;
    }
}
Irlovan.Control.PortalJibHookCrane.prototype.Animate = function () {
    if ((parseFloat(this.Config.luffEncoder.Value) <= 1) && (parseFloat(this.Config.luffEncoder.Value)>=0)) {
        Irlovan.Control.Classic.prototype.SetValue.apply(this, ["luff", "Value", 35 * parseFloat(this.Config.luffEncoder.Value) + 25]);
        Irlovan.Control.Classic.prototype.SetValue.apply(this, ["luff2", "Value", 80 - 40 * parseFloat(this.Config.luffEncoder.Value)]);
    }


    var radinHand = parseFloat(this.Config.luff.Value) * (Math.PI / 180)
    $("#" + this.HandID).rotate({
        angle: parseFloat(this.Config.luff.Value) - 90,
        center: ["0%", "50%"],
        animateTo: parseFloat(this.Config.luff.Value) - 90
    });
    //COS(A)=(a^2+b^2-c^2)/2ab
    var handWidth = 0.5 * parseFloat(this.Config.width.Value);
    var gapHeight = 0.28 * parseFloat(this.Config.height.Value)
    var ropeWidth = Math.sqrt(Math.pow(gapHeight, 2) + Math.pow(handWidth, 2) - Math.cos(radinHand) * 2 * gapHeight * handWidth);
    var radianRope = Math.acos((Math.pow(ropeWidth, 2) + Math.pow(gapHeight, 2) - Math.pow(handWidth, 2)) / (2 * ropeWidth * gapHeight));
    var degreeRope = 180 / Math.PI * radianRope;
    $("#" + this.LuffID).rotate({
        angle: 90 - degreeRope + this.OffsetDegreeLuff,
        center: ["0%", "50%"],
        animateTo: 90 - degreeRope + this.OffsetDegreeLuff
    });
    this.RopeDegree = degreeRope;
    document.getElementById(this.LuffID).style.width = ropeWidth / parseFloat(this.Config.width.Value) * 100 + "%";
    var handXOffset = Math.sin(radinHand) * handWidth;
    var handYOffset = Math.cos(radinHand) * handWidth;
    document.getElementById(this.LuffID2).style.left = (28 + handXOffset / parseFloat(this.Config.width.Value) * 100 - this.OffsetDegreeLuff2) + "%";
    document.getElementById(this.LuffID2).style.top = (38 - handYOffset / parseFloat(this.Config.height.Value) * 100) + "%";
    $("#" + this.LuffID2).rotate({
        angle: parseFloat(this.Config.luff2.Value),
        center: ["0%", "50%"],
        animateTo: parseFloat(this.Config.luff2.Value)
    });
    var luff2Width = 0.25 * parseFloat(this.Config.width.Value);
    var luff2Radin = parseFloat(this.Config.luff2.Value) * Math.PI / 180;
    var luff2XOffset = Math.cos(luff2Radin) * luff2Width;
    var luff2YOffset = Math.sin(luff2Radin) * luff2Width;
    var luff2Left = document.getElementById(this.LuffID2).style.left;
    var luff2Top = document.getElementById(this.LuffID2).style.top;
    document.getElementById(this.RopeID).style.left = (parseFloat(luff2Left) + luff2XOffset / parseFloat(this.Config.width.Value) * 100 - this.OffsetDegreeLuff2) + "%";
    document.getElementById(this.RopeID).style.top = (parseFloat(luff2Top) + luff2YOffset / parseFloat(this.Config.height.Value) * 100 + this.OffsetTopLuff2) + "%";
    var ropeHeight = parseFloat(this.Config.hoist.Value) * 100 - parseFloat(document.getElementById(this.RopeID).style.top);
    if (ropeHeight <= 0) { ropeHeight = "5%" }
    document.getElementById(this.RopeID).style.height = (ropeHeight - parseFloat(this.OffsetHeightFix)) + "%";
    document.getElementById(this.HoistID).style.left = (parseFloat(luff2Left) + luff2XOffset / parseFloat(this.Config.width.Value) * 100 - this.OffsetDegreeHoist) + "%";
    document.getElementById(this.HoistID).style.top = (parseFloat(luff2Top) + luff2YOffset / parseFloat(this.Config.height.Value) * 100 + parseFloat(document.getElementById(this.RopeID).style.height) - this.OffsetHeightHoist - parseFloat(this.OffsetHoistFix)) + "%";

}