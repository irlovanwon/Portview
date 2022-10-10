//Copyright(c) 2013,HIT All rights reserved.
//Des:RealtimeTrend
//Author:Irlovan   
//Date:2013-11-13
//modification :

Irlovan.Include.Using("Script/Communication/Websocket.js");
Irlovan.Include.Using("Script/Control/Lib/JqPlot/Head.js");

Irlovan.RealtimeTrend = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "RealtimeTrend", id, containerID, gridContainerID, controlClass, config ? config : ({
        tagName: { Attributes: "tagName", Value: "RealtimeTrend" },
        width: { Attributes: "width", Value: "1024" },
        gridHeight: { Attributes: "gridHeight", Value: "100" },
        plotHeight: { Attributes: "plotHeight", Value: "400" },
        interval: { Attributes: "interval", Value: "1000" },
        refreshInterval: { Attributes: "refreshInterval", Value: "2000" }
    }), left, top, zIndex, isLock);
    this.TrendGrid;
    this.TrendContainerID = id + "_trendContainer";
    this.NameInputID = id + "_nameInput";
    this.TrendGridID = id + "_trendGrid";
    this.ButtonAddID = id + "_button_add";
    this.TrendPlotID = id + "_trendPlot";
    this.ButtonRemoveID = id + "_button_remove";
    this.ButtonStartID = id + "_button_start";
    this.ButtonStopID = id + "_button_stop";
    this.TrendGridContainerID = id + "_trendGridContainer";
    this.ButtonCountID = id + "button_count";
    this.RealtimeTrendSocket;
    this.TrendData = [[["2013-01-01", 0]]];
    this.TrendDataStack = [];
    this.TrendLabelList = [];
    this.Plot;
    this.Timer;
    this.Init(id, containerID);
}
Irlovan.RealtimeTrend.prototype = new Irlovan.Control();
Irlovan.RealtimeTrend.prototype.constructor = Irlovan.RealtimeTrend;
//init div element
Irlovan.RealtimeTrend.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.TrendContainerID + "' style='position:absolute;width:" + this.Config.width.Value + "px;'>" +
       "<div id='" + this.TrendPlotID + "' style='height:" + this.Config.plotHeight.Value + "px;width:" + this.Config.width.Value + "px; '></div>" +
       "<p><label for='culture'>Select:</label>" +
       "<input style='width:200px;' type='text' id='" + this.NameInputID + "' />" +
       "<input value='Add' style='width:120px;height:30px;' type='button' id='" + this.ButtonAddID + "' />" +
       "<input value='Remove' style='width:120px;height:30px;' type='button' id='" + this.ButtonRemoveID + "' />" +
       "<input value='Start' style='width:120px;height:30px;' type='button' id='" + this.ButtonStartID + "' />" +
       "<input value='Stop' style='width:120px;height:30px;' type='button' id='" + this.ButtonStopID + "' />" +
       "<label> Count:</label>" +
       "<input type='text' id='" + this.ButtonCountID + "' />" +
       "</p>" +
       "<div id='" + this.TrendGridContainerID + "' style='width:" + this.Config.width.Value + "px;height:" + this.Config.gridHeight.Value + "px;'/>" +
       "</div>", this.ID, this.Pos);
    this.DataGridSelector();
    this.Events();
    this.PlotInit(this.Config.tagName.Value, this.TrendData);
    $("#" + this.ButtonCountID).spinner();
    document.getElementById(this.ButtonCountID).value = "10";
}
Irlovan.RealtimeTrend.prototype.PlotInit = function (titleName, data) {
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
Irlovan.RealtimeTrend.prototype.UpdateTrend = function (dataList) {
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
        if (this.TrendData[index].length >= $("#" + this.ButtonCountID).val()) {
            this.TrendData[index].shift();
        }
        this.TrendData[index].push([data[i].split(",")[2], parseFloat(value)]);
    }
}

Irlovan.RealtimeTrend.prototype.DataGridSelector = function () {
    if (!this.TrendGrid) {
        this.TrendGrid = new Irlovan.PropertyGrid(this.TrendGridContainerID, this.TrendGridID, this.TrendGridID + "_PropertyGrid_Table", this.TrendGridID + "_PropertyGrid_Page", Irlovan.IrlovanHelper.Bind(this, Irlovan.RealtimeData.TrendDataGrid), "TrendData", null, null, null, Irlovan.IrlovanHelper.Bind(this, this.VariableSelectHandler));
        $('#' + this.TrendGrid.ID).jqGrid('setGridWidth', parseFloat(this.Config.width.Value));
        $('#' + this.TrendGrid.ID).jqGrid('setGridHeight', parseFloat(this.Config.gridHeight.Value));
        $('#' + this.TrendGrid.ID).jqGrid('setCaption', this.Config.tagName.Value);
    }
}
Irlovan.RealtimeTrend.prototype.Events = function () {
    //auto complete
    document.getElementById(this.NameInputID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        new Irlovan.ExpressionEditor(Irlovan.Global.ExpressionEditorID, Irlovan.Global.BodyID, this.Config.left.Value, this.Config.top.Value, document.getElementById(this.NameInputID).value, Irlovan.IrlovanHelper.Bind(this, function (e) {
            document.getElementById(this.NameInputID).value = e;
        }));
    }), false);
    //delete row
    document.getElementById(this.ButtonRemoveID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        var dataSubcriptionArray = jQuery('#' + this.TrendGrid.ID).jqGrid('getGridParam', 'selarrrow');
        while (dataSubcriptionArray.length != 0) {
            jQuery('#' + this.TrendGrid.ID).jqGrid('delRowData', dataSubcriptionArray[0]);
        }
    }), false);
    //add row
    document.getElementById(this.ButtonAddID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        var name = document.getElementById(this.NameInputID).value;
        if (!name) { return; }
        var rows = jQuery('#' + this.TrendGrid.ID).jqGrid('getRowData');
        for (var i = 0; i < rows.length; i++) {
            if (rows[i]["Name"] == name) { return; }
        }
        var data = this.GetAddVariable(name);
        if (data) {
            jQuery('#' + this.TrendGrid.ID).jqGrid('addRowData', name, data);
            document.getElementById(this.NameInputID).value = "";
        }
    }), false);
    //start trend
    document.getElementById(this.ButtonStartID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        if ((this.RealtimeTrendSocket) || (this.Timer)) { this.SocketDispose(); }
        this.StartWebSocket(Irlovan.IrlovanHelper.Bind(this, this.SendRequest));
    }), false);
    //stop trend
    document.getElementById(this.ButtonStopID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        this.SocketDispose();
    }), false);
    //change count
    document.getElementById(this.ButtonCountID).addEventListener("change", Irlovan.IrlovanHelper.Bind(this, function (event) {
        this.SocketDispose();
        this.StartWebSocket(Irlovan.IrlovanHelper.Bind(this, this.SendRequest));
    }), false);
}
Irlovan.RealtimeTrend.prototype.GetAddVariable = function (name) {
    var dataList = Irlovan.Global.RealtimeDataList;
    for (var i = 0; i < dataList.length; i++) {
        var doc = Irlovan.IrlovanHelper.XDocumentFromString(dataList[i]);
        if (doc.documentElement.getAttribute("Name") == name) {
            return { Name: doc.documentElement.getAttribute("Name"), ID: doc.documentElement.getAttribute("ID"), DataType: doc.documentElement.getAttribute("DataType"), Description: doc.documentElement.getAttribute("Description") };
        }
    }
    return null;
}
Irlovan.RealtimeTrend.prototype.SendRequest = function () {
    var dataSubcriptionArray = jQuery('#' + this.TrendGrid.ID).jqGrid('getGridParam', 'selarrrow');
    if (dataSubcriptionArray.length == 0) { return; }
    this.TrendDataInit(dataSubcriptionArray);
    var RealTimeDataHandler = {};
    RealTimeDataHandler.DataSubcriptionInit = dataSubcriptionArray;
    RealTimeDataHandler.ModeSubcription = this.Config.interval.Value;
    this.RealtimeTrendSocket.Send(Irlovan.IrlovanHelper.JsonToString(RealTimeDataHandler));
}
Irlovan.RealtimeTrend.prototype.TrendDataInit = function (dataSubcriptionArray) {
    this.TrendData = [];
    for (var i = 0; i < dataSubcriptionArray.length; i++) {
        this.TrendData.push([]);
        this.TrendDataStack.push(dataSubcriptionArray[i]);
        this.TrendLabelList.push({ label: dataSubcriptionArray[i] });
    }
}
Irlovan.RealtimeTrend.prototype.VariableSelectHandler = function (rowid, status, e) {
    //for bind extend
}
Irlovan.RealtimeTrend.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "tagName":
            $('#' + this.TrendGrid.ID).jqGrid('setCaption', this.Config.tagName.Value);
            break;
        case "gridHeight":
            document.getElementById(this.TrendGridContainerID).style.height = data + "px";
            $('#' + this.TrendGrid.ID).jqGrid('setGridHeight', parseFloat(data));
            break;
        case "width":
            document.getElementById(this.TrendContainerID).style.width = data + "px";
            document.getElementById(this.TrendPlotID).style.width = data + "px";
            document.getElementById(this.TrendGridContainerID).style.width = data + "px";
            $('#' + this.TrendGrid.ID).jqGrid('setGridWidth', parseFloat(data));
            break;
        case "plotHeight":
            document.getElementById(this.TrendPlotID).style.height = data + "px";
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
Irlovan.RealtimeTrend.prototype.StartWebSocket = function (onSend) {
    this.RealtimeTrendSocket = new Irlovan.Communication.Websocket(
        'ws://' + Irlovan.Global.Domain + '/RealtimeDataHandler', '', null, onSend,
     Irlovan.IrlovanHelper.Bind(this, function (evt) {
         this.UpdateTrend(evt.data);
     }), null);
    this.TrendRefresh();
}
Irlovan.RealtimeTrend.prototype.TrendRefresh = function () {
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
Irlovan.RealtimeTrend.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    this.SocketDispose();
}
Irlovan.RealtimeTrend.prototype.SocketDispose = function () {
    if (this.RealtimeTrendSocket) {
        this.RealtimeTrendSocket.Close();
    }
    this.RealtimeTrendSocket = null;
    this.TrendDataStack = null;
    this.TrendDataStack = [];
    this.TrendLabelList = [];
    clearInterval(this.Timer);
    this.Timer = null;
}














