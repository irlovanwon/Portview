//Copyright(c) 2013,HIT All rights reserved.
//Des:Alarm.js
//Author:Irlovan   
//Date:2013-11-11

Irlovan.Include.Using("Script/Communication/Websocket.js");
Irlovan.Include.Using("Script/RealtimeData/RealtimeData.js");

Irlovan.AlarmLight = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "AlarmLight", id, containerID, gridContainerID, controlClass, config ? config : ({
        tagName: { Attributes: "tagName", Value: "RealtimeEvent" },
        size: { Attributes: "size", Value: "128" },
        recorderName: { Attributes: "recorderName", Value: "" }
    }), left, top, zIndex, isLock);
    this.Timer;
    this.IsRed = false;
    this.AlarmLightID = id + "_alarmLight";
    this.Init(id, containerID);
    this.StartWebSocket();
}
Irlovan.AlarmLight.prototype = new Irlovan.Control();
Irlovan.AlarmLight.prototype.constructor = Irlovan.AlarmLight;
//init div element
Irlovan.AlarmLight.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.AlarmLightID + "' style='background-size:100% 100%;position: absolute;width:" + (this.Config.size.Value + "px") + ";height:" + (this.Config.size.Value + "px") + ";background-image: url(Images/ControlIcon/Alarm/AlarmLightGreen.png);'>" +
       "</div>", this.ID, this.Pos);
}
Irlovan.AlarmLight.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    clearInterval(this.Timer);
    this.Timer = null;
    this.WebSocket.Close();
    this.WebSocket = null;
}
Irlovan.AlarmLight.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "size":
            document.getElementById(this.AlarmLightID).style.width = data + 'px';
            document.getElementById(this.AlarmLightID).style.height = data + 'px';
            //document.getElementById(this.AlarmLightID).style.backgroundImage = "url('Images/ControlIcon/Alarm/.png')";
            break;
        case "recorderName":
            this.WebSocket.Send(this._requestMessage());
            break;
        default:
            break;
    }
}
Irlovan.AlarmLight.prototype._requestMessage = function () {
    return Irlovan.IrlovanHelper.JsonToString({ RealtimeEvent: this.Config.recorderName.Value });
}

Irlovan.AlarmLight.prototype._handler = function (data) {
    if (data.Name != "RealtimeEvent") { return false; }
    clearInterval(this.Timer);
    this.Timer = null;
    if (data.Data.length > 0) {
        document.getElementById(this.AlarmLightID).style.backgroundImage = "url('Images/ControlIcon/Alarm/AlarmLightRed.png')";
        this.IsRed = false;
        this.Timer = setInterval(Irlovan.IrlovanHelper.Bind(this, function () {
            if (this.IsRed) {
                document.getElementById(this.AlarmLightID).style.backgroundImage = "url('Images/ControlIcon/Alarm/AlarmLightRed.png')";
                this.IsRed = false;
            } else {
                document.getElementById(this.AlarmLightID).style.backgroundImage = "url('Images/ControlIcon/Alarm/AlarmLightYellow.png')";
                this.IsRed = true;
            }
        }), 500);
    } else {
        document.getElementById(this.AlarmLightID).style.backgroundImage = "url('Images/ControlIcon/Alarm/AlarmLightGreen.png')";
    }
}
Irlovan.AlarmLight.prototype.StartWebSocket = function () {
    this.WebSocket = new Irlovan.Communication.Websocket(
      'ws://' + Irlovan.Global.Domain + '/SQLHandler', '', null,
        Irlovan.IrlovanHelper.Bind(this, function (evt) {
            this.WebSocket.Send(this._requestMessage());
        }),
       Irlovan.IrlovanHelper.Bind(this, function (evt) {
           this._handler(Irlovan.RealtimeData.ReadDataByXML(evt.data));
       }), null);
}