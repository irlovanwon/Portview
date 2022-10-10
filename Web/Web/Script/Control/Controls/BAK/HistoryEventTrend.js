//Copyright(c) 2013,HIT All rights reserved.
//Des:RealtimeTrend
//Author:Irlovan   
//Date:2013-11-13
//modification :

Irlovan.Include.Using("Script/Communication/Websocket.js");
Irlovan.Include.Using("Script/Control/Lib/JqPlot/Head.js");
Irlovan.Include.Using("Script/Control/Lib/TimePicker/Lib/jquery-ui-timepicker-addon.css");
Irlovan.Include.Using("Script/Control/Lib/TimePicker/TimePicker.js");

Irlovan.HistoryEventTrend = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "HistoryEventTrend", id, containerID, gridContainerID, controlClass, config ? config : ({
        tagName: { Attributes: "tagName", Value: "HistoryEventTrend" },
        width: { Attributes: "width", Value: "1024" },
        gridHeight: { Attributes: "gridHeight", Value: "100" },
        plotHeight: { Attributes: "plotHeight", Value: "400" },
        recorderList: { Attributes: "recorderList", Value: "" },
        maxNumber: { Attributes: "maxNumber", Value: 100 }
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
    this.Map = {};
    this.TrendGridContainerID = id + "_trendGridContainer";
    this.StyleSelectID = id + "_trend_style_select";
    this.HistoryEventTrendSocket;
    this.TrendData = [[["2013-01-01", 0]]];
    this.TrendDataStack = [];
    this.TrendLabelList = [];
    this.HistoryEventTrendStartTime = "";
    this.HistoryEventTrendEndTime = "";
    this.Style;
    this.HistoryEventTrendStartTimeID = this.ID + "HitoryTrend_DateTime_Start";
    this.HistoryEventTrendEndTimeID = this.ID + "HitoryTrend_DateTime_End";
    this.DatePickerClass = id + "HistoryEventTrend_DateTime";
    this.ButtonCountID = id + "button_count";
    this.RecorderSelectID = this.ID + "_recorder_select";
    this.Plot;
    this.Init(id, containerID);
}
Irlovan.HistoryEventTrend.prototype = new Irlovan.Control();
Irlovan.HistoryEventTrend.prototype.constructor = Irlovan.HistoryEventTrend;
//init div element
Irlovan.HistoryEventTrend.prototype.Init = function (id, containerID) {
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
       "<p>" +
       "<label for='culture'> RecorderSelect:</label>" +
       "<select id='" + this.RecorderSelectID + "'>" +
       "</select>" +
       "<label for='culture'> StyleSelect:</label>" +
       "<select id='" + this.StyleSelectID + "'>" +
       "<option value='DateAxisRenderer' selected='selected'>DateAxisRenderer</option>" +
       "<option value='PieRenderer'>PieRenderer</option>" +
       "</select>" +
       "</p>" +
       "<p><label> StartTime:</label><input type='text' id='" + this.HistoryEventTrendStartTimeID + "' class='" + this.DatePickerClass + "'/>" +
       "<label> EndTime:</label>" +
       "<input type='text' id='" + this.HistoryEventTrendEndTimeID + "' class='" + this.DatePickerClass + "' />" +
       "<label> Count:</label>" +
       "<input type='text' id='" + this.ButtonCountID + "' />" +
       "</p>" +
       "</p>" +
       "<div id='" + this.TrendGridContainerID + "' style='width:" + this.Config.width.Value + "px;height:" + this.Config.gridHeight.Value + "px;'/>" +
       "</div>", this.ID, this.Pos);
    this.DataGridSelector();
    this.Events();
    this.PlotInit(this.Config.tagName.Value, this.TrendData);
    this.DataPickerInit(this.ID, Irlovan.IrlovanHelper.Bind(this, function (selectedDateTime, e) {
        if (e.id == this.HistoryEventTrendStartTimeID) {
            this.HistoryEventTrendStartTime = selectedDateTime;
            $('#' + this.HistoryEventTrendEndTimeID).datetimepicker('option', 'minDate', $('#' + this.HistoryEventTrendStartTimeID).datetimepicker('getDate'));
        }
        else if (e.id == this.HistoryEventTrendEndTimeID) {
            $('#' + this.HistoryEventTrendStartTimeID).datetimepicker('option', 'maxDate', $('#' + this.HistoryEventTrendEndTimeID).datetimepicker('getDate'));
            this.HistoryEventTrendEndTime = selectedDateTime;
        }
    }));
    this.SetRecorderList();
    this.Style = $("#" + this.StyleSelectID).val();
    $("#" + this.ButtonCountID).spinner();
}
Irlovan.HistoryEventTrend.prototype.DataPickerInit = function (id, onDatePickerSelect) {
    $('.' + this.DatePickerClass).datetimepicker({
        timeFormat: 'HH:mm:ss',
        stepHour: 1,
        stepMinute: 1,
        stepSecond: 1,
        onSelect: onDatePickerSelect
    });
    $('.' + this.DatePickerClass).datepicker("option", "dateFormat", "yy'-'mm'-'dd'");
    $("#" + this.ButtonCountID).spinner();
}
Irlovan.HistoryEventTrend.prototype.SetStyle = function (style) {
    if (this.Plot) {
        this.Plot.destroy();
    }
    this.SocketDispose();
    switch (style) {
        case "DateAxisRenderer":
            this.TrendData = [[["2013-01-01", 0]]];
            this.PlotInit(this.Config.tagName.Value, this.TrendData);
            this.Style = style;
            break;
        case "PieRenderer":
            this.TrendData = [['Blank', 0]];
            this.PieInit(this.Config.tagName.Value, this.TrendData);
            this.Style = style;
            break;
        default:
    }
}
Irlovan.HistoryEventTrend.prototype.PieInit = function (titleName, data) {
    this.Plot = jQuery.jqplot(this.TrendPlotID, [data],
    {
        title: titleName,
        seriesDefaults: {
            // Make this a pie chart.
            renderer: jQuery.jqplot.PieRenderer,
            rendererOptions: {
                // Put data labels on the pie slices.
                // By default, labels show the percentage of the slice.
                showDataLabels: true
            }
        },
        legend: { show: true, location: 'e' }
    }
  );
}
Irlovan.HistoryEventTrend.prototype.PlotInit = function (titleName, data) {
    this.Plot = $.jqplot(this.TrendPlotID, data, {
        title: titleName,
        axes: {
            xaxis: {
                renderer: $.jqplot.DateAxisRenderer,
                tickOptions: { formatString: '%#H:%#M:%#S %Y-%#m-%#d' }
            }
        },
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
Irlovan.HistoryEventTrend.prototype.UpdateTrend = function (doc) {
    switch (this.Style) {
        case "DateAxisRenderer":
            this.UpdatePlot(doc);
            break;
        case "PieRenderer":
            this.UpdatePie(doc);
            break;
        default:
    }

}
Irlovan.HistoryEventTrend.prototype.UpdatePie = function (doc) {
    var xDoc = Irlovan.IrlovanHelper.XDocumentFromString(doc);
    var elements = xDoc.documentElement.getElementsByTagName("Item");
    if (this.TrendData.length < 1) {
        this.TrendData.push([]);
    }
    for (var i = 0; i < elements.length; i++) {
        var name = elements[i].getAttribute("EventName");
        var index = this.TrendDataStack.indexOf(name);
        if (index == -1) {
            this.TrendDataStack.push(name);
            this.TrendLabelList.push({ label: name });
            this.TrendData[0].push([name, 0]);
        } else {
            this.TrendData[0][index][1]++;
        }
    }
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
Irlovan.HistoryEventTrend.prototype.UpdatePlot = function (doc) {
    var xDoc = Irlovan.IrlovanHelper.XDocumentFromString(doc);
    var elements = xDoc.documentElement.getElementsByTagName("Item");
    for (var i = 0; i < elements.length; i++) {
        var index = this.TrendDataStack.indexOf(elements[i].getAttribute("EventName"));
        if (index == -1) { continue; }
        while (!this.TrendData[index]) { this.TrendData.push([]); }
        this.TrendData[index].push([elements[i].getAttribute("StartTime"), 0]);
        this.TrendData[index].push([elements[i].getAttribute("EndTime"), 1]);
    }
    this.Plot.replot({
        data: this.TrendData,
        series: this.TrendLabelList,
        legend: {
            show: true,
            location: 'nw',
            placement: 'inside'
        },
        cursor: {
            show: true,
            tooltipLocation: 'sw',
            zoom: true
        }
    });
}
Irlovan.HistoryEventTrend.prototype.DataGridSelector = function () {
    if (!this.TrendGrid) {
        this.TrendGrid = new Irlovan.PropertyGrid(this.TrendGridContainerID, this.TrendGridID, this.TrendGridID + "_PropertyGrid_Table", this.TrendGridID + "_PropertyGrid_Page", Irlovan.IrlovanHelper.Bind(this, Irlovan.RealtimeData.TrendDataGrid), "TrendData", null, null, null, Irlovan.IrlovanHelper.Bind(this, this.VariableSelectHandler));
        $('#' + this.TrendGrid.ID).jqGrid('setGridWidth', parseFloat(this.Config.width.Value));
        $('#' + this.TrendGrid.ID).jqGrid('setGridHeight', parseFloat(this.Config.gridHeight.Value));
        $('#' + this.TrendGrid.ID).jqGrid('setCaption', this.Config.tagName.Value);
    }
}
Irlovan.HistoryEventTrend.prototype.Events = function () {
    //auto complete
    document.getElementById(this.NameInputID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        this.CreateRecorderMap();
        new Irlovan.ExpressionEditor(Irlovan.Global.ExpressionEditorID, Irlovan.Global.BodyID, this.Config.left.Value, this.Config.top.Value, document.getElementById(this.NameInputID).value, Irlovan.IrlovanHelper.Bind(this, function (e) {
            document.getElementById(this.NameInputID).value = e;
        }), this.Map[$("#" + this.RecorderSelectID).val()]);
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
        if (this.HistoryEventTrendSocket) { this.SocketDispose(); }
        this.StartWebSocket(Irlovan.IrlovanHelper.Bind(this, this.SendRequest));
    }), false);
    //stop
    document.getElementById(this.ButtonStopID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        if (this.HistoryEventTrendSocket) {
            this.SocketDispose();
        }
    }), false);
    //datepickerevent
    $('.' + this.DatePickerClass).datepicker("option", "dateFormat", "yy'-'mm'-'dd'");
    document.getElementById(this.StyleSelectID).addEventListener("change", Irlovan.IrlovanHelper.Bind(this, function (event) {
        this.SetStyle($("#" + this.StyleSelectID).val());
    }), false);
    document.getElementById(this.StyleSelectID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
    }), false);
}
Irlovan.HistoryEventTrend.prototype.GetAddVariable = function (name) {
    var dataList = Irlovan.Global.RealtimeDataList;
    for (var i = 0; i < dataList.length; i++) {
        var doc = Irlovan.IrlovanHelper.XDocumentFromString(dataList[i]);
        if (doc.documentElement.getAttribute("Name") == name) {
            return { Name: doc.documentElement.getAttribute("Name"), ID: doc.documentElement.getAttribute("ID"), DataType: doc.documentElement.getAttribute("DataType"), Description: doc.documentElement.getAttribute("Description") };
        }
    }
    return null;
}
Irlovan.HistoryEventTrend.prototype.SendRequest = function () {
    if ((this.HistoryEventTrendStartTime) && (this.HistoryEventTrendEndTime)) {
        var dataSubcriptionArray = jQuery('#' + this.TrendGrid.ID).jqGrid('getGridParam', 'selarrrow');
        this.TrendDataInit(dataSubcriptionArray);
        if (dataSubcriptionArray.length == 0) {
            if (this.Style == "PieRenderer") {
                this.HistoryEventTrendSocket.Send(Irlovan.IrlovanHelper.JsonToString(this.RequestMessage()));
            }
            return;
        }
        for (var i = 0; i < dataSubcriptionArray.length; i++) {
            this.HistoryEventTrendSocket.Send(Irlovan.IrlovanHelper.JsonToString(this.RequestMessage(dataSubcriptionArray[i])));
        }
    } else {
        alert("Please set the starttime and endtime ");
    }
}
Irlovan.HistoryEventTrend.prototype.RequestMessage = function (name) {
    var result = { HistoryEvent: { StartTime: this.HistoryEventTrendStartTime, EndTime: this.HistoryEventTrendEndTime, RecorderName: $("#" + this.RecorderSelectID).val() } }
    if ($("#" + this.ButtonCountID).spinner("value")) {
        var count = $("#" + this.ButtonCountID).spinner("value");
        result.HistoryEvent.Amount = ((count) ? count : this.Config.maxNumber.Value);
    } else {
        result.HistoryEvent.Amount = this.Config.maxNumber.Value;
    }
    if (name) {
        result.HistoryEvent.EventName = name;
    }
    return result;
}
Irlovan.HistoryEventTrend.prototype.TrendDataInit = function (dataSubcriptionArray) {
    this.TrendData = [];
    for (var i = 0; i < dataSubcriptionArray.length; i++) {
        switch (this.Style) {
            case "DateAxisRenderer":
                this.TrendData.push([]);
                break;
            case "PieRenderer":
                if (this.TrendData.length < 1) {
                    this.TrendData.push([]);
                }
                this.TrendData[0].push([dataSubcriptionArray[i], 0]);
                break;
            default:
        }
        this.TrendDataStack.push(dataSubcriptionArray[i]);
        this.TrendLabelList.push({ label: dataSubcriptionArray[i] });
    }
}
Irlovan.HistoryEventTrend.prototype.VariableSelectHandler = function (rowid, status, e) {
    //for bind extend
}
Irlovan.HistoryEventTrend.prototype.SetValue = function (name, colName, data) {
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
        case "recorderList":
            this.SetRecorderList();
            break;
        default:
            break;
    }
}
Irlovan.HistoryEventTrend.prototype.StartWebSocket = function (onSend) {
    this.HistoryEventTrendSocket = new Irlovan.Communication.Websocket(
        'ws://' + Irlovan.Global.Domain + '/SQLHandler', '', null, onSend,
     Irlovan.IrlovanHelper.Bind(this, function (evt) {
         this.UpdateTrend(evt.data);
     }), null);
}
Irlovan.HistoryEventTrend.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    this.SocketDispose();
}
Irlovan.HistoryEventTrend.prototype.SocketDispose = function () {
    if (this.HistoryEventTrendSocket) {
        this.HistoryEventTrendSocket.Close();
    }
    this.HistoryEventTrendSocket = null;
    this.TrendDataStack = null;
    this.TrendDataStack = [];
    this.TrendLabelList = [];
}
Irlovan.HistoryEventTrend.prototype.SetRecorderList = function () {
    Irlovan.ControlHelper.ClearControl(this.RecorderSelectID);
    var recorders = this.Config.recorderList.Value.split(",");
    for (var i = 0; i < recorders.length; i++) {
        Irlovan.ControlHelper.CreateControlByStr(
        "<option value='" + recorders[i] + "' selected='selected'>" + recorders[i] +
        "</option>", this.RecorderSelectID, null);
    }
}
Irlovan.HistoryEventTrend.prototype.CreateRecorderMap = function (data) {
    var recorders = this.Config.recorderList.Value.split(",");
    var filters = this.Config.dataSource.Value.split(";");
    for (var i = 0; i < recorders.length; i++) {
        try {
            this.Map[recorders[i]] = filters[i];
        } catch (e) {
            continue;
        }
    }
}












