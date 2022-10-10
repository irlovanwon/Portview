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
        values: { Attributes: "values", Value: "", Description: "#dataName#+';'+#dataName#" },
        drawPoints: { Attributes: "drawPoints", Value: "circle", Description: "square, circle" },
        shaded: { Attributes: "shaded", Value: "top", Description: "top, bottom" },
        timeLength: { Attributes: "timeLength", Value: -60, Description: "default:-60 (means 1 minute)" },
        width: { Attributes: "width", Value: '250' },
        height: { Attributes: "height", Value: '250' },
        thickness: { Attributes: "thickness", Value: '2' },
        interval: { Attributes: "interval", Value: 1000 },
        renderTimeout: { Attributes: "renderTimeout", Value: 500 }
    }), left, top, zIndex, isLock);
    this.EditionFix();
    this.RecID = this.ID + "_DataViewer";
    this.ContainerID = this.ID + "_trendContainer";
    this.ModeSelectID = this.ID + "_strategy";
    this.Mode;
    this.Graph2d;
    this.DataList = [];
    this.Container;
    this.Timer_RenderStep;
    this.Timer_AddDataPoint;
    this.Dataset;
    this.Options;
    this.Init(id, containerID);
    this.RenderStep();
}
Irlovan.Control.DataViewer.prototype = new Irlovan.Control.Classic();
Irlovan.Control.DataViewer.prototype.constructor = Irlovan.Control.DataViewer;
//init div element
Irlovan.Control.DataViewer.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.RecID + "'>" +
       "<p>" +
       "<label for='" + this.ModeSelectID + "'>Strategy:</label>" +
       "<select id='" + this.ModeSelectID + "'>" +
       //"<option value='continuous' selected=''>Continuous (CPU intensive)</option>" +
       "<option value='static'>Static</option>" +
       "<option value='discrete'>Discrete</option>" +
       "<option value='stop'>Stop</option>" +
       "</select>" +
       "<div id='" + this.ContainerID + "' style='background-size:100% 100%;background-color: " + this.Config.fillColor.Value + ";border:" + this.Config.thickness.Value + "px " + this.Config.strokeType.Value + " " + this.Config.strokeColor.Value + ";position:relative;width:" + this.Config.width.Value + "px;height:" + this.Config.height.Value + "px;'/>" +
       "</p>" +
       "</div>", this.ID, this.Pos);
    this.Start();
}
Irlovan.Control.DataViewer.prototype.GridInit = function () {
    this.Mode = document.getElementById(this.ModeSelectID);
    // create a graph2d with an (currently empty) dataset
    this.Container = document.getElementById(this.ContainerID);
    this.Dataset = new vis.DataSet();
    this.Options = {
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
    this.Graph2d = new vis.Graph2d(this.Container, this.Dataset, this.Options);
    this.Graph2d.fit();
}
Irlovan.Control.DataViewer.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "values":
            this.UpdateTrend(data);
            break;
        case "width":
            document.getElementById(this.ContainerID).style.width = data + "px";
            this.Start();
            break;
        case "height":
            document.getElementById(this.ContainerID).style.height = data + "px";
            this.Start();
            break;
        case "fillColor":
            document.getElementById(this.ContainerID).style.backgroundColor = this.Config.fillColor.Value;
            break;
        case "drawPoints":
        case "shaded":
        case "timeLength":
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
    try { var range = this.Graph2d.getWindow(); } catch (e) { return; }
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
    var result = this.DataList[0];
    if ((result) || (result == 0)) {
        this.Dataset.add({
            x: now,
            y: result,
        });
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
    this.DataList = [];
}
Irlovan.Control.DataViewer.prototype.Start = function () {
    this.TimerDispose();
    this.LibDestroy();
    this.ObjectDestroy();
    this.InitDataList();
    this.GridInit();
    this.AddDataPoint();
}

Irlovan.Control.DataViewer.prototype.UpdateTrend = function (values) {
    var data = values.toString();
    var value;
    if (data == "true") { value = 1; }
    else if (data == "false") { value = 0; }
    else { value = parseFloat(data); }
    this.DataList[0] = value;
}
Irlovan.Control.DataViewer.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    this.TimerDispose();
    this.LibDestroy();
    this.ObjectDestroy();
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
}
Irlovan.Control.DataViewer.prototype.ObjectDestroy = function () {
    this.DataList = null;
    this.DataList = [];
}
/**solution for new version compatible**/
Irlovan.Control.DataViewer.prototype.EditionFix = function () {
    if (!this.Config.values) {
        this.Config.values = { Attributes: "values", Value: "", Description: "#dataName#+';'+#dataName#" };
    }
}