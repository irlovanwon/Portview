//Copyright(c) 2013,HIT All rights reserved.
//Des:BridgeCrane
//Author:Irlovan   
//Date:2013-09-18
//modification :

Irlovan.Control.BridgeCrane = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "BridgeCrane", id, containerID, gridContainerID, controlClass, config ? config : ({
        tagName: { Attributes: "tagName", Value: "##" },
        boom: { Attributes: "boom", Value: 0 },
        height: { Attributes: "height", Value: 500 },
        width: { Attributes: "width", Value: 720 },
        lock: { Attributes: "lock", Value: false },
        moveForward: { Attributes: "moveForward", Value: false },
        moveBackward: { Attributes: "moveBackward", Value: false },
        verticalPos: { Attributes: "verticalPos", Value: 0.2 },
        horizontalPos: { Attributes: "horizontalPos", Value: 0.57 }
    }), left, top, zIndex, isLock);
    this.BridgeID = this.ID + "_bridge";
    this.BodyID = this.ID + "_body";
    this.HandID = this.ID + "_hand";
    this.CabID = this.ID + "_cab";
    this.RopeID = this.ID + "_rope";
    this.SpreaderID = this.ID + "_spreader";
    this.BridgeContainerID = this.ID + "_container";
    this.TagID = this.ID + "_tagID";
    this.Init(id, containerID);
}

Irlovan.Control.BridgeCrane.prototype = new Irlovan.Control.Classic();
Irlovan.Control.BridgeCrane.prototype.constructor = Irlovan.Control.BridgeCrane;

//init div element
Irlovan.Control.BridgeCrane.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.BridgeID + "' style='position: relative;background-size:100% 100%;width:" + this.Config.width.Value + "px;height:" + this.Config.height.Value + "px'>" +
       "<div id='" + this.HandID + "' style='position: absolute;background-size:100% 100%;top:27.2%;width:44.72%;height:7.2%;background-image: url(Images/ControlIcon/BridgeCrane/BridgeHand.png);' />" +
        "<div id='" + this.RopeID + "' style='position: absolute;background-size:100% 100%;top:34.8%;left:41.4%;width:1.8%;height:14%;border:thin solid black;' />" +
       "<div id='" + this.CabID + "' style='position: absolute;background-size:100% 100%;left:40%;top:33.6%;width:3.9%;height:6.8%;background-image: url(Images/ControlIcon/BridgeCrane/Cab.png);' />" +
       "<div id='" + this.BridgeContainerID + "' style='position: absolute;background-size:100% 100%;visibility:" + (((this.Config.lock.Value == "true") || ((this.Config.lock.Value == true))) ? "visible" : "hidden") + ";left:40.35%;top:53.6%;width:4.3%;height:6.2%;background-image: url(Images/ControlIcon/BridgeCrane/Container.png);' />" +
       "<div id='" + this.SpreaderID + "' style='position: absolute;background-size:100% 100%;left:39.7%;top:48%;width:5.28%;height:7%;background-image: url(Images/ControlIcon/BridgeCrane/Spreader.png);' />" +
       "<div id='" + this.BodyID + "' style='position: absolute;background-size:100% 100%;left:44.22%;width:55.28%;height:100%;background-image: url(Images/ControlIcon/BridgeCrane/BridgeCraneBody.png);' />" +
       "<div id='" + this.TagID + "'  style='position: absolute;border:thin solid black;left:75%;top:28.1%;background-color: white;'>" + this.Config.tagName.Value + "</div>" +
       "</div>", this.ID, this.Pos);
}
Irlovan.Control.BridgeCrane.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "horizontalPos":
            document.getElementById(this.CabID).style.left = (1-parseFloat(data)) * 100 - 5 + 0.56 + "%";// + 4
            document.getElementById(this.RopeID).style.left = (1 - parseFloat(data)) * 100 - 5 + 1.67 + "%";// + 12
            document.getElementById(this.SpreaderID).style.left = (1 - parseFloat(data)) * 100 - 5 + "%";//// + 0
            document.getElementById(this.BridgeContainerID).style.left = (1 - parseFloat(data)) * 100 - 5 + 0.694 + "%";// + 5
            break;
        case "verticalPos":
            document.getElementById(this.RopeID).style.height = 0.652 *parseFloat(this.Config.height.Value) *Math.abs(parseFloat(data) - 0.1) + "px";
            document.getElementById(this.SpreaderID).style.top = (34.8+(Math.abs(parseFloat(data) - 0.1)*100*0.62)) + "%";//170
            document.getElementById(this.BridgeContainerID).style.top = (1.8+34.8 + (Math.abs(parseFloat(data) - 0.1) * 100 + 6) * 0.62) + "%";//201
            break;
        case "lock":
            document.getElementById(this.BridgeContainerID).style.visibility = (((this.Config.lock.Value == "true") || ((this.Config.lock.Value == true))) ? "visible" : "hidden");
            break;
        case "height":
            document.getElementById(this.BridgeID).style.height = data + "px";
            //document.getElementById(this.BodyID).style.height = data + "px";
            //document.getElementById(this.HandID).style.height = 0.072 * parseFloat(this.Config.height.Value) + "px";
            //document.getElementById(this.CabID).style.height = 0.068 * parseFloat(this.Config.height.Value) + "px";
            break;
        case "width":
            document.getElementById(this.BridgeID).style.width = data + "px";
            //document.getElementById(this.BodyID).style.width = 0.5528 * parseFloat(this.Config.width.Value) + "px";
            //document.getElementById(this.HandID).style.width = 0.4472 * parseFloat(this.Config.width.Value) + "px";
            //document.getElementById(this.CabID).style.width = 0.039 * parseFloat(this.Config.width.Value) + "px";
            //document.getElementById(this.SpreaderID).style.height = 0.08 * parseInt(data) + "px";
            //document.getElementById(this.BridgeContainerID).style.height = 0.08 * parseInt(data) + "px";
            break;
        case "boom":
            $("#" + this.HandID).rotate({
                angle: this.Config.boom.Value,
                center: ["100%", "50%"],
                animateTo: parseFloat(data)
            });
            break;
        case "tagName":
            document.getElementById(this.TagID).innerHTML =data;
            break;
        default:
            break;
    }
}