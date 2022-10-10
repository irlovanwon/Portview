//Copyright(c) 2013,HIT All rights reserved.
//Des:Alarm.js
//Author:Irlovan   
//Date:2013-05-13
//modification :2013-07-18 by Irlovan
//modification :2013-10-23 by Irlovan
//modification :2015-03-31 by Irlovan Add history event
//modification :2015-04-20 by Irlovan Add SQL Event handler for server
//modification :2015-08-21 Remove SQL handler and add notification handler instead

Irlovan.Control.AlarmViewer = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "AlarmViewer", id, containerID, gridContainerID, controlClass, config ? config : ({
        tagName: { Attributes: "tagName", Value: "RealtimeEvent" },
        height: { Attributes: "height", Value: "768px" },
        width: { Attributes: "width", Value: "1024px" },
        history: { Attributes: "history", Value: true },
        realtime: { Attributes: "realtime", Value: true },
        interval: { Attributes: "interval", Value: 100 },
        notificationID: { Attributes: "notificationID", Value: "" }
    }), left, top, zIndex, isLock);
    this.EditionFix();
    this.Timer;
    this.GridID = this.ID + "_container_alarmGrid";
    this.NotificationSelectID = this.ID + "_notification_select";
    this.AlarmViewerContainerID = this.ID + "_container";
    this.Init(id, containerID);
    this.Interval = parseInt(this.Config.interval.Value);
    this.NotificationHandler = new Irlovan.Handler.NotificationHandler(Irlovan.IrlovanHelper.Bind(this, this.StartSBC));
}
Irlovan.Control.AlarmViewer.prototype = new Irlovan.Control.Classic();
Irlovan.Control.AlarmViewer.prototype.constructor = Irlovan.Control.AlarmViewer;
//init div element
Irlovan.Control.AlarmViewer.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.AlarmViewerContainerID + "' style='position:relative;width:" + this.Config.width.Value + ";height:" + this.Config.height.Value + ";'>" +
       "<select id='" + this.NotificationSelectID + "' style='position:absolute;z-index:1001;top:0px;left:0px;'>" +
       "</select>" +
       "</div>", this.ID, this.Pos);
    this.PropertyGridInit();
    this.NotificationChange();
    this.SetNotification();
}

/**Dispose**/
Irlovan.Control.AlarmViewer.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    if (this.NotificationHandler) {
        this.NotificationHandler.Dispose();
        this.NotificationHandler = null
    }
    if (this.Timer) {
        clearTimeout(this.Timer);
        this.Timer = null;
    }
}

/**Set value**/
Irlovan.Control.AlarmViewer.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "tagName":
            $('#' + this.AlarmGrid.ID).jqGrid('setCaption', this.Config.tagName.Value);
            break;
        case "height":
            document.getElementById(this.AlarmViewerContainerID).style.height = data;
            $('#' + this.AlarmGrid.ID).jqGrid('setGridHeight', parseFloat(data));
            break;
        case "width":
            document.getElementById(this.AlarmViewerContainerID).style.width = data;
            $('#' + this.AlarmGrid.ID).jqGrid('setGridWidth', parseFloat(data));
            break;
        case "notificationID":
            this.SetNotification();
            this.StartSBC();
            break;
        case "history":
        case "realtime":
            this.StartSBC();
            break;
        default:
            break;
    }
}

/**PropertyGridInit**/
Irlovan.Control.AlarmViewer.prototype.PropertyGridInit = function () {
    if (!this.AlarmGrid) {
        this.AlarmGrid = new Irlovan.PropertyGrid(this.AlarmViewerContainerID, this.GridID, this.GridID + "_PropertyGrid_Table", this.GridID + "_PropertyGrid_Page", Irlovan.IrlovanHelper.Bind(this, Irlovan.RealtimeData.RealtimeEventDataConfig), "RealtimeEvent", this.Pos, null);
        $('#' + this.AlarmGrid.ID).jqGrid('setGridWidth', parseFloat(this.Config.width.Value));
        $('#' + this.AlarmGrid.ID).jqGrid('setGridHeight', parseFloat(this.Config.height.Value));
        $('#' + this.AlarmGrid.ID).jqGrid('setCaption', this.Config.tagName.Value);
    }
}
/**Start subcribe data**/
Irlovan.Control.AlarmViewer.prototype.StartSBC = function () {
    this.NotificationHandler.EventNotification.Subcribe($("#" + this.NotificationSelectID).val(), this.SelectType(), Irlovan.IrlovanHelper.Bind(this, this.Handle));
}

/**select event type (history only)(realtime only)( history and realtime)**/
Irlovan.Control.AlarmViewer.prototype.SelectType = function () {
    var isHistory = Irlovan.Lib.Help.Boolean(this.Config.history.Value);
    var isRealtime = Irlovan.Lib.Help.Boolean(this.Config.realtime.Value);
    if (isHistory && isRealtime) {
        return this.NotificationHandler.EventNotification.RegisterTypeBoth;
    } else if (isHistory) {
        return this.NotificationHandler.EventNotification.RegisterTypeHistory;
    } else if (isRealtime) {
        return this.NotificationHandler.EventNotification.RegisterTypeRealtime;
    }
}

/**Handle Event Message**/
Irlovan.Control.AlarmViewer.prototype.Handle = function (data) {
    if (this.Timer) { return; }
    this.Timer = setTimeout(Irlovan.IrlovanHelper.Bind(this, function () {
        this.FillChart(data);
        clearTimeout(this.Timer);
        this.Timer = null;
    }), this.Interval);
}

/**fill chart with received data from server**/
Irlovan.Control.AlarmViewer.prototype.FillChart = function (data) {
    this.PropertyGridInit();
    this.AlarmGrid.LoadData(data);
}

/**Set Notification**/
Irlovan.Control.AlarmViewer.prototype.SetNotification = function () {
    Irlovan.ControlHelper.ClearControl(this.NotificationSelectID);
    var notifications = this.Config.notificationID.Value.split(",");
    for (var i = 0; i < notifications.length; i++) {
        Irlovan.ControlHelper.CreateControlByStr(
        "<option value='" + notifications[i] + "' selected='selected'>" + notifications[i] +
        "</option>", this.NotificationSelectID, null);
    }
}

/**Notification Change Event**/
Irlovan.Control.AlarmViewer.prototype.NotificationChange = function () {
    $("#" + this.NotificationSelectID).change(Irlovan.IrlovanHelper.Bind(this, function () {
        this.FillChart({});
        this.StartSBC();
    }));
}

Irlovan.Control.AlarmViewer.prototype.EditionFix = function () {
    if (!this.Config.interval) {
        this.Config.interval = { Attributes: "interval", Value: 100 };
    }
    if (!this.Config.history) {
        this.Config.history = { Attributes: "history", Value: true };
    }
    if (!this.Config.realtime) {
        this.Config.realtime = { Attributes: "realtime", Value: true };
    }
}