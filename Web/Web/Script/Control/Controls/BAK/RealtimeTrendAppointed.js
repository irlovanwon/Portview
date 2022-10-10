//Copyright(c) 2014,HIT All rights reserved.
//Des:RealtimeTrendAppointed
//Author:Irlovan   
//Date:2014-10-09
//modification :

Irlovan.Include.Using("Script/Communication/Websocket.js");
Irlovan.Include.Using("Script/Control/Lib/JqPlot/Head.js");

Irlovan.RealtimeTrendAppointed = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "RealtimeTrendAppointed", id, containerID, gridContainerID, controlClass, config ? config : ({
        tagName: { Attributes: "tagName", Value: "Trend" },
        width: { Attributes: "width", Value: "1024" },
        gridHeight: { Attributes: "gridHeight", Value: "100" },
        plotHeight: { Attributes: "plotHeight", Value: "400" },
        dataList: { Attributes: "dataList", Value: "1,2" },
        count: { Attributes: "count", Value: 10 },
        interval: { Attributes: "interval", Value: "1000" },
        refreshInterval: { Attributes: "refreshInterval", Value: "2000" }
    }), left, top, zIndex, isLock);
    this.TrendContainerID = id + "_trendContainer";
    this.TrendGridID = id + "_trendGrid";
    this.TrendPlotID = id + "_trendPlot";
    this.ButtonStartID = id + "_button_start";
    this.ButtonStopID = id + "_button_stop";
    this.RealtimeTrendSocket;
    this.TrendData = [[["2013-01-01", 0]]];
    this.TrendDataStack;
    this.TrendLabelList;
    this.DataList = [];
    this.Plot;
    this.Timer;
    this.Init(id, containerID);
}
Irlovan.RealtimeTrendAppointed.prototype = new Irlovan.Control();
Irlovan.RealtimeTrendAppointed.prototype.constructor = Irlovan.RealtimeTrendAppointed;
//init div element
Irlovan.RealtimeTrendAppointed.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.TrendContainerID + "' style='position:absolute;width:" + this.Config.width.Value + "px;'>" +
       "<div id='" + this.TrendPlotID + "' style='height:" + this.Config.plotHeight.Value + "px;width:" + this.Config.width.Value + "px; '></div>" +
       "<input value='Start' style='width:120px;height:30px;' type='button' id='" + this.ButtonStartID + "' />" +
       "<input value='Stop' style='width:120px;height:30px;' type='button' id='" + this.ButtonStopID + "' />" +
       "</div>", this.ID, this.Pos);
    this.Events();
    this.InitDataList();
    this.PlotInit(this.Config.tagName.Value, this.TrendData);
    this.Start();
}
Irlovan.RealtimeTrendAppointed.prototype.PlotInit = function (titleName, data) {
    this.Plot = $.jqplot(this.TrendPlotID, data, {
        title: titleName,
        axes: { xaxis: { renderer: $.jqplot.DateAxisRenderer } },
        cursor: {
            show: true,
            tooltipLocation: 'sw',
            zoom: true
        },
        highlighter: {
            show: true,
            sizeAdjust: 7.5
        },
        series: this.TrendLabelList,
        legend: {
            show: true,
            location: 'nw',
            placement: 'inside'
        }
    });
}
Irlovan.RealtimeTrendAppointed.prototype.Events = function () {
    //start trend
    document.getElementById(this.ButtonStartID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        this.TrendRefresh();
    }), false);
    //stop trend
    document.getElementById(this.ButtonStopID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        this.TimerDispose();
    }), false);
}
Irlovan.RealtimeTrendAppointed.prototype.UpdateTrend = function (dataList) {
    var data = dataList.split(";")
    var labelList = [];
    for (var i = 0; i < data.length; i++) {
        var index = this.TrendDataStack.indexOf(data[i].split(",")[0]);
        if (index == -1) { continue; }
        while (!this.TrendData[index]) { this.TrendData.push([]); }
        var value;
        if (data[i].split(",")[1] == "true") { value = 1; }
        else if (data[i].split(",")[1] == "false") { value = 0; }
        else { value = data[i].split(",")[1]; }
        if (this.TrendData[index].length >=parseInt(this.Config.count.Value)) {
            this.TrendData[index].shift();
        }
        this.TrendData[index].push([data[i].split(",")[2], parseFloat(value)]);
    }
    data = null;
    labelList = null;

    this.Plot.replot({
        data: this.TrendData,
        series: this.TrendLabelList,
        legend: {
            show: true,
            location: 'nw',
            placement: 'inside'
        }
    });
}
Irlovan.RealtimeTrendAppointed.prototype.InitDataList = function () {
    var dataList = this.Config.dataList.Value.split(",");
    for (var i = 0; i < dataList.length; i++) {
        this.AddData(dataList[i]);
    }
}
Irlovan.RealtimeTrendAppointed.prototype.AddData = function (name) {
    if (!name) { return; }
    var data = this.GetAddVariable(name);
    if (data) {
        this.DataList.push(data.Name);
    }
}
Irlovan.RealtimeTrendAppointed.prototype.Start = function () {
    if ((this.RealtimeTrendSocket) || (this.Timer)) { this.SocketDispose(); }
    this.StartWebSocket(Irlovan.IrlovanHelper.Bind(this, this.SendRequest));;
}
Irlovan.RealtimeTrendAppointed.prototype.GetAddVariable = function (name) {
    var dataList = Irlovan.Global.RealtimeDataList;
    for (var i = 0; i < dataList.length; i++) {
        var doc = Irlovan.IrlovanHelper.XDocumentFromString(dataList[i]);
        if (doc.documentElement.getAttribute("Name") == name) {
            return { Name: doc.documentElement.getAttribute("Name"), ID: doc.documentElement.getAttribute("ID"), DataType: doc.documentElement.getAttribute("DataType"), Description: doc.documentElement.getAttribute("Description") };
        }
    }
    return null;
}
Irlovan.RealtimeTrendAppointed.prototype.SendRequest = function () {
    if (this.DataList.length == 0) { return; }
    this.TrendDataInit(this.DataList);
    var RealTimeDataHandler = {};
    RealTimeDataHandler.DataSubcriptionInit = this.DataList;
    RealTimeDataHandler.ModeSubcription ="-"+ this.Config.interval.Value;
    this.RealtimeTrendSocket.Send(Irlovan.IrlovanHelper.JsonToString(RealTimeDataHandler));
}
Irlovan.RealtimeTrendAppointed.prototype.TrendDataInit = function (dataSubcriptionArray) {
    //this.TrendData = [];
    for (var i = 0; i < dataSubcriptionArray.length; i++) {
        this.TrendData.push([]);
        this.TrendDataStack.push(dataSubcriptionArray[i]);
        this.TrendLabelList.push({ label: dataSubcriptionArray[i] });
    }
}
Irlovan.RealtimeTrendAppointed.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "width":
            document.getElementById(this.TrendContainerID).style.width = data + "px";
            document.getElementById(this.TrendPlotID).style.width = data + "px";
            break;
        case "plotHeight":
            document.getElementById(this.TrendPlotID).style.height = data + "px";
            break;
        case "count":
            this.SocketDispose();
            this.StartWebSocket(Irlovan.IrlovanHelper.Bind(this, this.SendRequest));
            break;
        case "interval":
            this.SocketDispose();
            this.StartWebSocket(Irlovan.IrlovanHelper.Bind(this, this.SendRequest));
            break;
        case "refreshInterval":
            this.SocketDispose();
            this.StartWebSocket(Irlovan.IrlovanHelper.Bind(this, this.SendRequest));
            break;
        default:
            break;
    }
}
Irlovan.RealtimeTrendAppointed.prototype.StartWebSocket = function (onSend) {
    this.TrendDataStack = [];
    this.TrendLabelList = [];
    this.RealtimeTrendSocket = new Irlovan.Communication.Websocket(
        'ws://' + Irlovan.Global.Domain + '/RealtimeDataHandler', '', null, onSend,
     Irlovan.IrlovanHelper.Bind(this, function (evt) {
         this.UpdateTrend(evt.data);
     }), null);
    //this.TrendRefresh();
}
Irlovan.RealtimeTrendAppointed.prototype.TrendRefresh = function () {
    if (!this.Timer) {
        this.Timer = setInterval(Irlovan.IrlovanHelper.Bind(this, function () {
            this.Plot.replot({
                data: this.TrendData,
                series: this.TrendLabelList,
                legend: {
                    show: true,
                    location: 'nw',
                    placement: 'inside'
                }
            });
        }), parseInt(this.Config.refreshInterval.Value));
    }
}
Irlovan.RealtimeTrendAppointed.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    this.SocketDispose();
    this.TimerDispose();
    this.TrendData = [];
    this.TrendDataStack = [];
    this.TrendDataStack = null;
    this.TrendLabelList = [];
    this.TrendLabelList = null;
    this.TrendData = null;
}
Irlovan.RealtimeTrendAppointed.prototype.SocketDispose = function () {
    if (this.RealtimeTrendSocket) {
        this.RealtimeTrendSocket.Close();
        this.RealtimeTrendSocket = null;
    }
}
Irlovan.RealtimeTrendAppointed.prototype.TimerDispose = function () {
    if (this.Timer) {
        clearInterval(this.Timer);
        this.Timer = null;
    }
}














