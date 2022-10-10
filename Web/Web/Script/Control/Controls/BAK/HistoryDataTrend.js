//Copyright(c) 2013,HIT All rights reserved.
//Des:RealtimeTrend
//Author:Irlovan   
//Date:2013-11-13
//modification :

Irlovan.Include.Using("Script/Communication/Websocket.js");
Irlovan.Include.Using("Script/Control/Lib/JqPlot/Head.js");
Irlovan.Include.Using("Script/Control/Lib/TimePicker/Lib/jquery-ui-timepicker-addon.css");
Irlovan.Include.Using("Script/Control/Lib/TimePicker/TimePicker.js");

Irlovan.HistoryDataTrend = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "HistoryDataTrend", id, containerID, gridContainerID, controlClass, config ? config : ({
        tagName: { Attributes: "tagName", Value: "HistoryDataTrend" },
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
    this.TrendGridContainerID = id + "_trendGridContainer";
    this.HistoryDataTrendSocket;
    this.TrendData = [[["2013-01-01", 0]]];
    this.TrendDataStack = [];
    this.TrendLabelList = [];
    this.HistoryDataTrendStartTime = "";
    this.HistoryDataTrendEndTime = "";
    this.HistoryDataTrendStartTimeID = this.ID + "_HistoryTrend_DateTime_Start";
    this.HistoryDataTrendEndTimeID = this.ID + "_HistoryTrend_DateTime_End";
    this.DatePickerClass = id + "HistoryDataTrend_DateTime";
    this.ButtonCountID = id + "button_count";
    this.RecorderSelectID = this.ID + "_recorder_select";
    this.Plot;
    this.Init(id, containerID);
}
Irlovan.HistoryDataTrend.prototype = new Irlovan.Control();
Irlovan.HistoryDataTrend.prototype.constructor = Irlovan.HistoryDataTrend;
//init div element
Irlovan.HistoryDataTrend.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.TrendContainerID + "' style='position:absolute;width:" + this.Config.width.Value + "px;'>" +
       "<div id='" + this.TrendPlotID + "' style='height:" + this.Config.plotHeight.Value + "px;width:" + this.Config.width.Value + "px; '></div>" +
       "<p><label for='culture'>Select:</label>" +
       "<input style='width:200px;' type='text' id='" + this.NameInputID + "' />" +
       "<input value='Add' style='width:120px;height:30px;' type='button' id='" + this.ButtonAddID + "' />" +
       "<input value='Remove' style='width:120px;height:30px;' type='button' id='" + this.ButtonRemoveID + "' />" +
       "<input value='Start' style='width:120px;height:30px;' type='button' id='" + this.ButtonStartID + "' />" +
       "<label for='culture'> RecorderSelect:</label>" +
       "<select id='" + this.RecorderSelectID + "'>" +
       "</select>" +
       "<p><label> StartTime:</label><input type='text' id='" + this.HistoryDataTrendStartTimeID + "' class='" + this.DatePickerClass + "'/>" +
       "<label> EndTime:</label>" +
       "<input type='text' id='" + this.HistoryDataTrendEndTimeID + "' class='" + this.DatePickerClass + "' />" +
       "<label> Count:</label>" +
       "<input type='text' id='" + this.ButtonCountID + "' />" +
       "</p>" +
       "</p>" +
       "<div id='" + this.TrendGridContainerID + "' style='width:" + this.Config.width.Value + "px;height:" + this.Config.gridHeight.Value + "px;'/>" +
       "</div>", this.ID, this.Pos);
    this.DataGridSelector();
    this.Events();
    this.PlotInit(this.Config.tagName.Value, this.TrendData);
    this.DataPickerInit(this.ID,this.ButtonAddID, Irlovan.IrlovanHelper.Bind(this, function (selectedDateTime, e) {
        if (e.id == this.HistoryDataTrendStartTimeID) {
            this.HistoryDataTrendStartTime = selectedDateTime;
            $('#' + this.HistoryDataTrendEndTimeID).datetimepicker('option', 'minDate', $('#' + this.HistoryDataTrendStartTimeID).datetimepicker('getDate'));
        }
        else if (e.id == this.HistoryDataTrendEndTimeID) {
            $('#' + this.HistoryDataTrendStartTimeID).datetimepicker('option', 'maxDate', $('#' + this.HistoryDataTrendEndTimeID).datetimepicker('getDate'));
            this.HistoryDataTrendEndTime = selectedDateTime;
        }
    }));
    this.SetRecorderList();
    $("#" + this.ButtonCountID).spinner();
}
Irlovan.HistoryDataTrend.prototype.DataPickerInit = function (id, focusID,onDatePickerSelect) {
    $('.' + this.DatePickerClass).datetimepicker({
        timeFormat: 'HH:mm:ss',
        stepHour: 1,
        stepMinute: 1,
        stepSecond: 1,
        onSelect: onDatePickerSelect
    });
    $('.' + this.DatePickerClass).datepicker("option", "dateFormat", "yy'-'mm'-'dd'");
}
Irlovan.HistoryDataTrend.prototype.PlotInit = function (titleName, data) {
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
Irlovan.HistoryDataTrend.prototype.UpdateTrend = function (doc) {
    var xDoc = Irlovan.IrlovanHelper.XDocumentFromString(doc);
    var elements = xDoc.documentElement.getElementsByTagName("Item");
    for (var i = 0; i < elements.length; i++) {
        var index = this.TrendDataStack.indexOf(elements[i].getAttribute("Name"));
        if (index == -1) { continue; }
        while (!this.TrendData[index]) { this.TrendData.push([]); }
        var value = elements[i].getAttribute("Value");
        if (value == "true") {
            this.TrendData[index].push([elements[i].getAttribute("TimeStamp"), 1]);
        }
        if (value == "false") {
            this.TrendData[index].push([elements[i].getAttribute("TimeStamp"), 0]);
        } else {
            this.TrendData[index].push([elements[i].getAttribute("TimeStamp"), parseFloat(value)]);
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
Irlovan.HistoryDataTrend.prototype.DataGridSelector = function () {
    if (!this.TrendGrid) {
        this.TrendGrid = new Irlovan.PropertyGrid(this.TrendGridContainerID, this.TrendGridID, this.TrendGridID + "_PropertyGrid_Table", this.TrendGridID + "_PropertyGrid_Page", Irlovan.IrlovanHelper.Bind(this, Irlovan.RealtimeData.TrendDataGrid), "TrendData", null, null, null, Irlovan.IrlovanHelper.Bind(this, this.VariableSelectHandler));
        $('#' + this.TrendGrid.ID).jqGrid('setGridWidth', parseFloat(this.Config.width.Value));
        $('#' + this.TrendGrid.ID).jqGrid('setGridHeight', parseFloat(this.Config.gridHeight.Value));
        $('#' + this.TrendGrid.ID).jqGrid('setCaption', this.Config.tagName.Value);
    }
}
Irlovan.HistoryDataTrend.prototype.Events = function () {
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
        if (this.HistoryDataTrendSocket) { this.SocketDispose(); }
        this.StartWebSocket(Irlovan.IrlovanHelper.Bind(this, this.SendRequest));
    }), false);
    //datepickerevent
    $('.' + this.DatePickerClass).datepicker("option", "dateFormat", "yy'-'mm'-'dd'");
}
Irlovan.HistoryDataTrend.prototype.GetAddVariable = function (name) {
    var dataList = Irlovan.Global.RealtimeDataList;
    for (var i = 0; i < dataList.length; i++) {
        var doc = Irlovan.IrlovanHelper.XDocumentFromString(dataList[i]);
        if (doc.documentElement.getAttribute("Name") == name) {
            return { Name: doc.documentElement.getAttribute("Name"), ID: doc.documentElement.getAttribute("ID"), DataType: doc.documentElement.getAttribute("DataType"), Description: doc.documentElement.getAttribute("Description") };
        }
    }
    return null;
}
Irlovan.HistoryDataTrend.prototype.SendRequest = function () {
    if ((this.HistoryDataTrendStartTime) && (this.HistoryDataTrendEndTime)) {
        var dataSubcriptionArray = jQuery('#' + this.TrendGrid.ID).jqGrid('getGridParam', 'selarrrow');
        if (dataSubcriptionArray.length == 0) { return; }
        this.TrendDataInit(dataSubcriptionArray);
        for (var i = 0; i < dataSubcriptionArray.length; i++) {
            this.HistoryDataTrendSocket.Send(Irlovan.IrlovanHelper.JsonToString(this.RequestMessage(dataSubcriptionArray[i])));
        }
    } else {
        alert("Please set the starttime and endtime ");
    }
}
Irlovan.HistoryDataTrend.prototype.RequestMessage = function (name) {
    var result = { RealtimeData: { StartTime: this.HistoryDataTrendStartTime, EndTime: this.HistoryDataTrendEndTime, RecorderName: $("#" + this.RecorderSelectID).val() } }
    if ($("#" + this.ButtonCountID).spinner("value")) {
        var count = $("#" + this.ButtonCountID).spinner("value");
        result.RealtimeData.Amount = ((count < parseInt(this.Config.maxNumber.Value)) ? count : this.Config.maxNumber.Value);
    } else {
        result.RealtimeData.Amount = this.Config.maxNumber.Value;
    }
    result.RealtimeData.Name = name;
    return result;
}
Irlovan.HistoryDataTrend.prototype.TrendDataInit = function (dataSubcriptionArray) {
    this.TrendData = [];
    for (var i = 0; i < dataSubcriptionArray.length; i++) {
        this.TrendData.push([]);
        this.TrendDataStack.push(dataSubcriptionArray[i]);
        this.TrendLabelList.push({ label: dataSubcriptionArray[i] });
    }
}
Irlovan.HistoryDataTrend.prototype.VariableSelectHandler = function (rowid, status, e) {
    //for bind extend
}
Irlovan.HistoryDataTrend.prototype.SetValue = function (name, colName, data) {
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
Irlovan.HistoryDataTrend.prototype.StartWebSocket = function (onSend) {
    this.HistoryDataTrendSocket = new Irlovan.Communication.Websocket(
        'ws://' + Irlovan.Global.Domain + '/SQLHandler', '', null, onSend,
     Irlovan.IrlovanHelper.Bind(this, function (evt) {
         this.UpdateTrend(evt.data);
     }), null);
}
Irlovan.HistoryDataTrend.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    this.SocketDispose();
}
Irlovan.HistoryDataTrend.prototype.SocketDispose = function () {
    if (this.HistoryDataTrendSocket) {
        this.HistoryDataTrendSocket.Close();
    }
    this.HistoryDataTrendSocket = null;
    this.TrendDataStack = null;
    this.TrendDataStack = [];
    this.TrendLabelList = [];
}
Irlovan.HistoryDataTrend.prototype.SetRecorderList = function () {
    Irlovan.ControlHelper.ClearControl(this.RecorderSelectID);
    var recorders = this.Config.recorderList.Value.split(",");
    for (var i = 0; i < recorders.length; i++) {
        Irlovan.ControlHelper.CreateControlByStr(
        "<option value='" + recorders[i] + "' selected='selected'>" + recorders[i] +
        "</option>", this.RecorderSelectID, null);
    }

}












