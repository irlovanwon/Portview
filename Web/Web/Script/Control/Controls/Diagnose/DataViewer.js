//Copyright(c) 2015,HIT All rights reserved.
//Des:
//Author:Irlovan   
//Date:2015-01-07
//modification :

Irlovan.Include.Using("Script/Lib/VIS/vis.css");
Irlovan.Include.Using("Script/Lib/VIS/vis.js");

Irlovan.Control.DataViewer = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "DataViewer", id, containerID, gridContainerID, controlClass, config ? config : ({
        fillColor: { Attributes: "fillColor", Value: 'white' },
        strokeColor: { Attributes: "strokeColor", Value: 'black' },
        strokeType: { Attributes: "strokeType", Value: "solid", Description: "dotted, solid" },
        dataList: { Attributes: "dataList", Value: "1,2" },
        labels: { Attributes: "labels", Value: "1,2" },
        drawPoints: { Attributes: "drawPoints", Value: "circle", Description: "square, circle" },
        shaded: { Attributes: "shaded", Value: "bottom", Description: "top, bottom" },
        timeLength: { Attributes: "timeLength", Value: -60, Description: "default:-60 (means 1 minute)" },
        minValue: { Attributes: "minValue", Value: -50 },
        maxValue: { Attributes: "maxValue", Value: 50 },
        iniSelected: { Attributes: "iniSelected", Value: true },
        width: { Attributes: "width", Value: '250' },
        height: { Attributes: "height", Value: '250' },
        thickness: { Attributes: "thickness", Value: '2' },
        interval: { Attributes: "interval", Value: 1000 },
        renderTimeout: { Attributes: "renderTimeout", Value: 500 }
    }), left, top, zIndex, isLock);
    this.RecID = this.ID + "_DataViewer";
    this.ContainerID = this.ID + "_trendContainer";
    this.ModeSelectID = this.ID + "_strategy";
    this.DataSelectID = this.ID + "_select";
    this.DataViewerSocket;
    this.Mode;
    this.Graph2d;
    this.DataList = {};
    this.Container;
    this.Timer_RenderStep;
    this.Timer_AddDataPoint;
    this.Dataset;
    this.Groups;
    this.Options;
    this.Init(id, containerID);
    this.RenderStep();
    this.DataSelectInit();
    this.MultipleSelectInit();
    this.SelectAll();
}
Irlovan.Control.DataViewer.prototype = new Irlovan.Control.Classic();
Irlovan.Control.DataViewer.prototype.constructor = Irlovan.Control.DataViewer;
//init div element
Irlovan.Control.DataViewer.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.RecID + "' >" +
       "<p>" +
       "<label for='" + this.ModeSelectID + "'>Strategy:</label>" +
       "<select id='" + this.ModeSelectID + "'>" +
       "<option value='static'>Static</option>" +
       "<option value='discrete'>Discrete</option>" +
       "<option value='stop'>Stop</option>" +
       "</select>" +
       "<label for='" + this.DataSelectID + "'>DataSelect:</label>" +
       "<select id='" + this.DataSelectID + "' multiple='multiple'>" +
       "</select>" +
       "<div id='" + this.ContainerID + "' style='background-size:100% 100%;background-color: " + this.Config.fillColor.Value + ";border:" + this.Config.thickness.Value + "px " + this.Config.strokeType.Value + " " + this.Config.strokeColor.Value + ";position:relative;width:" + this.Config.width.Value + "px;height:" + this.Config.height.Value + "px;'/>" +
       "</p>" +
       "</div>", this.ID, this.Pos);
    this.Start();
}
Irlovan.Control.DataViewer.prototype.GroupInit = function () {
    this.Groups = new vis.DataSet();
    var labels = [];
    if (this.Config.labels.Value) {
        labels = this.Config.labels.Value.split(",");
    }
    var index = 0;
    for (var key in this.DataList) {
        this.Groups.add({
            id: index,
            content: labels[index],
            options: {
                drawPoints: {
                    style: this.Config.drawPoints.Value, // square, circle
                    enabled: true
                },
                shaded: {
                    orientation: this.Config.shaded.Value// top, bottom
                }
            }
        });
        index++;
    }
}
Irlovan.Control.DataViewer.prototype.GridInit = function () {
    this.Mode = document.getElementById(this.ModeSelectID);
    // create a graph2d with an (currently empty) dataset
    this.Container = document.getElementById(this.ContainerID);
    this.Dataset = new vis.DataSet();
    var range = {};
    if (this.Config.minValue.Value != "null") {
        range.min = parseFloat(this.Config.minValue.Value);
    }
    if (this.Config.maxValue.Value != "null") {
        range.max = parseFloat(this.Config.maxValue.Value);
    }

    this.Options = {
        legend: true,
        start: vis.moment().add(parseInt(this.Config.timeLength.Value), 'seconds'), // changed so its faster
        end: vis.moment(),
        width: '100%',
        //graphHeight: (parseFloat(this.Config.height.Value) - 50) + "px",
        minHeight: "50px",
        height: (parseFloat(this.Config.height.Value)) + "px",
        drawPoints: {
            style: 'circle' // square, circle
        },
        shaded: {
            orientation: 'bottom' // top, bottom
        }
    }
    this.Graph2d = new vis.Graph2d(this.Container, this.Dataset, this.Groups, this.Options);
    this.Graph2d.fit();
}
Irlovan.Control.DataViewer.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "width":
            document.getElementById(this.ContainerID).style.width = data + "px";
            break;
        case "height":
            document.getElementById(this.ContainerID).style.height = data + "px";
            break;
        case "fillColor":
            document.getElementById(this.ContainerID).style.backgroundColor = this.Config.fillColor.Value;
            break;
        case "iniSelected":
            this.SelectAll();
            break;
        case "labels":
            this.DataSelectInit();
            $("#" + this.DataSelectID).multiselect("refresh");
        case "dataList":
        case "drawPoints":
        case "shaded":
        case "timeLength":
        case "minValue":
        case "maxValue":
            this.Start();
            break;
        case "thickness":
        case "strokeType":
        case "strokeColor":
            document.getElementById(this.ContainerID).style.border = this.Config.thickness.Value + "px " + this.Config.strokeType.Value + " " + this.Config.strokeColor.Value;
            break;
        default:
            break;
    }
}
Irlovan.Control.DataViewer.prototype.RenderStep = function (name, colName, data) {
    // move the window (you can think of different strategies).
    var now = vis.moment();
    var range;
    try {
        range = this.Graph2d.getWindow();
    }
    catch (e) {
        clearTimeout(this.Timer_RenderStep);
        return;
    }
    var interval = range.end - range.start;
    switch (this.Mode.value) {
        case 'continuous':
            // continuously move the window
            this.Graph2d.setWindow(now - interval, now, { animate: false });
            requestAnimationFrame(Irlovan.IrlovanHelper.Bind(this, this.RenderStep));
            break;

        case 'discrete':
            this.Graph2d.setWindow(now - interval, now, { animate: false });
            this.Timer_RenderStep = setTimeout(Irlovan.IrlovanHelper.Bind(this, this.RenderStep), this.Config.renderTimeout.Value);
            break;
        case 'stop':
            this.Timer_RenderStep = setTimeout(Irlovan.IrlovanHelper.Bind(this, this.RenderStep), this.Config.renderTimeout.Value);
            break;

        default: // 'static'
            // move the window 90% to the left when now is larger than the end of the window
            if (now > range.end) {
                this.Graph2d.setWindow(now - 0.1 * interval, now + 0.9 * interval);
            }
            this.Timer_RenderStep = setTimeout(Irlovan.IrlovanHelper.Bind(this, this.RenderStep), this.Config.renderTimeout.Value);
            break;
    }
}
Irlovan.Control.DataViewer.prototype.AddDataPoint = function (name, colName, data) {
    // add a new data point to the dataset
    var now = vis.moment();
    var index = 0;
    for (var key in this.DataList) {
        var result = this.DataList[key];
        if ((result) || (result == 0)) {
            this.Dataset.add({
                x: now,
                y: result,
                group: index
            });
        }
        index++;
    }
    // remove all data points which are no longer visible
    var range = this.Graph2d.getWindow();
    var interval = range.end - range.start;
    var oldIds = this.Dataset.getIds({
        filter: function (item) {
            return item.x < range.start - interval;
        }
    });
    this.Dataset.remove(oldIds);
    this.Timer_AddDataPoint = setTimeout(Irlovan.IrlovanHelper.Bind(this, this.AddDataPoint), this.Config.renderTimeout.Value);
}
Irlovan.Control.DataViewer.prototype.InitDataList = function () {
    var dataList = this.Config.dataList.Value.split(",");
    for (var i = 0; i < dataList.length; i++) {
        var name = dataList[i];
        if (!name) { continue; }
        this.DataList[name] = null;
    }
}
Irlovan.Control.DataViewer.prototype.Start = function () {
    this.SocketDispose();
    this.TimerDispose();
    this.LibDestroy();
    this.ObjectDestroy();
    this.InitDataList();
    this.GroupInit();
    this.GridInit();
    this.StartWebSocket(Irlovan.IrlovanHelper.Bind(this, this.SendRequest));
    this.AddDataPoint();
}
Irlovan.Control.DataViewer.prototype.StartWebSocket = function (onSend) {
    this.TrendDataStack = [];
    this.TrendLabelList = [];
    this.DataViewerSocket = new Irlovan.Communication.Websocket(
        'ws://' + Irlovan.Global.Domain_Message + '/MessageHandler', '', null, onSend,
     Irlovan.IrlovanHelper.Bind(this, function (evt) {
         this.UpdateTrend(evt.data);
     }), null);
}
Irlovan.Control.DataViewer.prototype.UpdateTrend = function (Doc) {
    var xDocument = Irlovan.Lib.XML.ParseFromString(Doc);
    var element = xDocument.documentElement;
    if (element.nodeName != "Data") { return; }
    var subcriptions = element.getElementsByTagName("SBC");
    if ((!subcriptions) || (subcriptions.length == 0)) { return; }
    var groups = element.getElementsByTagName("Group");
    if ((!groups) || (groups.length == 0)) { return; }
    for (var i = 0; i < groups.length; i++) {
        var dataElements = groups[i].getElementsByTagName("InDataMessage");
        for (var i = 0; i < dataElements.length; i++) {
            var name = dataElements[i].getAttribute("Name");
            var value = dataElements[i].getAttribute("Value");
            this.DataList[name] = value;
        }
    }
}
Irlovan.Control.DataViewer.prototype.SendRequest = function () {
    if (this.DataList.length == 0) { return; }
    var doc = Irlovan.Lib.XML.CreateXDocument("Data");
    var subcription = Irlovan.Lib.XML.CreateXElement("SBC");
    var group = Irlovan.Lib.XML.CreateXElement("Group");
    doc.documentElement.appendChild(subcription);
    subcription.appendChild(group);
    group.setAttribute("Mode", this.Config.interval.Value);
    group.setAttribute("Name", "GUI");
    for (var key in this.DataList) {
        var name = key;
        var data = Irlovan.Lib.XML.CreateXElement("Item");
        data.setAttribute("Name", name);
        group.appendChild(data);
    }
    this.DataViewerSocket.Send(Irlovan.Lib.XML.ToString(doc));
}

Irlovan.Control.DataViewer.prototype.DataSelectInit = function () {
    Irlovan.ControlHelper.ClearControl(this.DataSelectID);
    var labels = this.Config.labels.Value.split(",");
    for (var i = 0; i < labels.length; i++) {
        Irlovan.ControlHelper.CreateControlByStr(
        "<option value='" + labels[i] + "' selected='selected'>" + labels[i] +
        "</option>", this.DataSelectID, null);
    }
    $("#" + this.DataSelectID).on("multiselectclick", Irlovan.IrlovanHelper.Bind(this, function (event, ui) {
        /* event: the original event object ui.value: value of the checkbox ui.text: text of the checkbox ui.checked: whether or not the input was checked or unchecked (boolean) */
        var index = 0;
        var labels = this.Config.labels.Value.split(",");
        for (var i = 0; i < labels.length; i++) {
            if (labels[i] == ui.text) { index = i; }
        }
        var groupsData = this.Groups.get();
        // if visible, hide
        if (ui.checked == true) {
            this.Groups.update({ id: groupsData[index].id, visible: true });
        }
        else {
            this.Groups.update({ id: groupsData[index].id, visible: false });
        }

    }));
    $("#" + this.DataSelectID).on("multiselectcheckall", Irlovan.IrlovanHelper.Bind(this, function (event, ui) {
        var groupsData = this.Groups.get();
        for (var i = 0; i < groupsData.length; i++) {
            this.Groups.update({ id: groupsData[i].id, visible: true });
        }
    }));
    $("#" + this.DataSelectID).on("multiselectuncheckall", Irlovan.IrlovanHelper.Bind(this, function (event, ui) {
        var groupsData = this.Groups.get();
        for (var i = 0; i < groupsData.length; i++) {
            this.Groups.update({ id: groupsData[i].id, visible: false });
        }
    }));
}
Irlovan.Control.DataViewer.prototype.MultipleSelectInit = function () {
    $("#" + this.DataSelectID).multiselect();
}
Irlovan.Control.DataViewer.prototype.SelectAll = function () {
    if ((this.Config.iniSelected.Value == true) || (this.Config.iniSelected.Value == "true")) {
        $("#" + this.DataSelectID).multiselect("checkAll");
    } else {
        $("#" + this.DataSelectID).multiselect("uncheckAll");
    }
}
Irlovan.Control.DataViewer.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    this.SocketDispose();
    this.TimerDispose();
    this.LibDestroy();
    this.ObjectDestroy();
}
Irlovan.Control.DataViewer.prototype.SocketDispose = function () {
    if (this.DataViewerSocket) {
        this.DataViewerSocket.Close();
        this.DataViewerSocket = null;
    }
}
Irlovan.Control.DataViewer.prototype.TimerDispose = function () {
    if (this.Timer_AddDataPoint) {
        clearTimeout(this.Timer_AddDataPoint);
        this.Timer_AddDataPoint = null;
    }
    if (this.Timer_RenderStep) {
        clearTimeout(this.Timer_RenderStep);
        this.Timer_RenderStep = null;
    }
}
Irlovan.Control.DataViewer.prototype.LibDestroy = function () {
    if (this.Graph2d) {
        this.Graph2d.destroy();
        this.Graph2d = null;
    }
    $("#" + this.DataSelectID).off();
}
Irlovan.Control.DataViewer.prototype.ObjectDestroy = function () {
    this.DataList = null;
    this.DataList = {};
}