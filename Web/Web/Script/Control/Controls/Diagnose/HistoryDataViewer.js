//Copyright(c) 2013,HIT All rights reserved.
//Des:HistoryEventViewer
//Author:Irlovan   
//Date:2013-05-14
//modification :2013-07-29
//modification :2015-08-27
//modification :2015-09-15 date picker bug fixed

Irlovan.Include.Using("Script/Control/Lib/TimePicker/Lib/jquery-ui-timepicker-addon.css");
Irlovan.Include.Using("Script/Control/Lib/TimePicker/TimePicker.js");

Irlovan.Control.HistoryDataViewer = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "HistoryDataViewer", id, containerID, gridContainerID, controlClass, config ? config : ({
        tagName: { Attributes: "tagName", Value: "HistoryDataViewer" },
        height: { Attributes: "height", Value: "300px" },
        width: { Attributes: "width", Value: "1024px" },
        recorderList: { Attributes: "recorderList", Value: "" },
        maxNumber: { Attributes: "maxNumber", Value: 100 }
    }), left, top, zIndex, isLock);
    this.RealtimeDataChartID = this.ID + "_container_realtimeDataGrid";
    this.RealtimeDataChartContainerID = this.ID + "_container_realtimeDataGrid_container";
    this.DataViewerContainerID = this.ID + "_dataviewer_container";
    this.ButtonCountID = this.ID + "button_count";
    this.ButtonQueryID = this.ID + "button_query";
    this.ButtonOrderID = this.ID + "button_order";
    this.StartTimePickerID = this.ID + "RealtimeData_DateTime_Start_picker";
    this.EndTimePickerID = this.ID + "RealtimeData_DateTime_End_picker";
    this.StartTimeID = this.ID + "RealtimeData_DateTime_Start";
    this.EndTimeID = this.ID + "RealtimeData_DateTime_End";
    this.NameSelectID = this.ID + "name_select";
    this.RecorderSelectID = this.ID + "_recorder_select";
    this.ExportID = this.ID + "_export";
    this.OrderDescChar = "↓";
    this.OrderAscChar = "↑";
    this.RealtimeDataGrid;
    this.StartTime = "";
    this.EndTime = "";
    this.NameInputID = this.ID + "_nameInput"
    this.Init(id, containerID);
    this.SetRecorderList();
    this.Handler = new Irlovan.Handler.RecorderHandler();
}
Irlovan.Control.HistoryDataViewer.prototype = new Irlovan.Control.Classic();
Irlovan.Control.HistoryDataViewer.prototype.constructor = Irlovan.Control.HistoryDataViewer;
//init div element
Irlovan.Control.HistoryDataViewer.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.DataViewerContainerID + "' style='position:relative;width:" + this.Config.width.Value + ";height:" + this.Config.height.Value + ";'>" +
       "<div style='position:relative;float: top;'>" +
       "<p style='position:absolute;top: 0px;left: 0px;'>StartTime: <input type='text' id='" + this.StartTimeID + "'/></p>" +
       "<p style='position:absolute;top: 40px;left: 0px;'>EndTime: <input type='text' id='" + this.EndTimeID + "'/></p>" +
       "<p style='position:absolute;top: 0px;left: 250px;'>Count: <input type='text' id='" + this.ButtonCountID + "' /></p>" +
       "<p style='position:absolute;top: 40px;left: 250px;'><label for='culture'>RecorderSelect:</label>" +
       "<select id='" + this.RecorderSelectID + "'>" +
       "</select>" +
       "</p>" +
       "<p style='position:absolute;top: 40px;left: 450px;'><label for='culture'>Select:</label>" +
       "<select id='" + this.NameSelectID + "'>" +
       "<option value='all' selected='selected'>All</option>" +
       "<option value='name'>Name</option>" +
       "</select>" +
       "<input style='visibility:hidden;' type='text' id='" + this.NameInputID + "' />" +
       "</p>" +
       "<p style='position:absolute;top: 40px;right: 210px;'><label for='culture'>Order:</label>" +
       "<input value='" + this.OrderDescChar + "' type='button' id='" + this.ButtonOrderID + "' />" +
       "</p>" +
       "<input value='Query' style='position:absolute;right: 10px;top:40px;width:120px;height:30px;' type='button' id='" + this.ButtonQueryID + "' />" +
       "</div>" +
       "<div id='" + this.RealtimeDataChartContainerID + "' style='top:80px;position:absolute;width:" + this.Config.width.Value + ";height:" + this.Config.height.Value + ";'>" +
       "</div>" +
       "</div>", this.ID, this.Pos);
    this.EventInit();
    this.DatePickerInit(Irlovan.IrlovanHelper.Bind(this, function (selectedDateTime, e) {
        this.StartTime = selectedDateTime;
        $('#' + this.EndTimeID).datetimepicker('option', 'minDate', $('#' + this.StartTimeID).datetimepicker('getDate'));
    }), this.StartTimeID, this.StartTimePickerID);
    this.DatePickerInit(Irlovan.IrlovanHelper.Bind(this, function (selectedDateTime, e) {
        this.EndTime = selectedDateTime;
        $('#' + this.StartTimeID).datetimepicker('option', 'maxDate', $('#' + this.EndTimeID).datetimepicker('getDate'));
    }), this.EndTimeID, this.EndTimePickerID);
    if (!this.RealtimeDataGrid) {
        this.RealtimeDataGrid = new Irlovan.PropertyGrid(this.RealtimeDataChartContainerID, this.RealtimeDataChartID, this.RealtimeDataChartID + "_PropertyGrid_Table", this.RealtimeDataChartID + "_PropertyGrid_Page", Irlovan.IrlovanHelper.Bind(this, Irlovan.RealtimeData.RealtimeDataConfig), "HistoryEvent", this.Pos, null);
        $('#' + this.RealtimeDataGrid.ID).jqGrid('setGridWidth', parseFloat(this.Config.width.Value));
        $('#' + this.RealtimeDataGrid.ID).jqGrid('setGridHeight', parseFloat(this.Config.height.Value));
        $('#' + this.RealtimeDataGrid.ID).jqGrid('setCaption', this.Config.tagName.Value);
    }
}
Irlovan.Control.HistoryDataViewer.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    this.Handler.Dispose();
    this.Handler = null;
}
Irlovan.Control.HistoryDataViewer.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "tagName":
            $('#' + this.RealtimeDataGrid.ID).jqGrid('setCaption', this.Config.tagName.Value);
            break;
        case "height":
            document.getElementById(this.DataViewerContainerID).style.height = data;
            document.getElementById(this.RealtimeDataChartContainerID).style.height = data;
            $('#' + this.RealtimeDataGrid.ID).jqGrid('setGridHeight', parseFloat(data));
            break;
        case "width":
            document.getElementById(this.DataViewerContainerID).style.width = data;
            document.getElementById(this.RealtimeDataChartContainerID).style.width = data;
            $('#' + this.RealtimeDataGrid.ID).jqGrid('setGridWidth', parseFloat(data));
            break;
        case "recorderList":
            this.SetRecorderList();
            break;
        default:
            break;
    }
}
Irlovan.Control.HistoryDataViewer.prototype.EventInit = function () {
    $("#" + this.ButtonCountID).spinner();
    $("#" + this.NameSelectID).change(Irlovan.IrlovanHelper.Bind(this, function () {
        document.getElementById(this.NameInputID).style.visibility = ($("#" + this.NameSelectID).val() == "all") ? "hidden" : "visible";
    }));
    document.getElementById(this.ButtonQueryID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        this.Subcribe();
    }), false);
    document.getElementById(this.ButtonOrderID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        this.Order();
    }), false);
    document.getElementById(this.NameInputID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        new Irlovan.ExpressionEditor(Irlovan.Global.ExpressionEditorID, Irlovan.Global.BodyID, this.Config.left.Value, this.Config.top.Value, document.getElementById(this.NameInputID).value, Irlovan.IrlovanHelper.Bind(this, function (e) {
            document.getElementById(this.NameInputID).value = e;
        }));
    }), false);
}
Irlovan.Control.HistoryDataViewer.prototype.DatePickerInit = function (onDatePickerSelect, containerID, datepickerID) {
    $('#' + containerID).datetimepicker({
        timeID: datepickerID,
        timeFormat: 'HH:mm:ss',
        stepHour: 1,
        stepMinute: 1,
        stepSecond: 1,
        onSelect: onDatePickerSelect
    });
    //$('.' + this.DatePickerClass).datepicker("option", "dateFormat", "yy'-'mm'-'dd'");
}
Irlovan.Control.HistoryDataViewer.prototype.Handle = function (message) {
    this.RealtimeDataGrid.LoadData(message);
}
Irlovan.Control.HistoryDataViewer.prototype.Subcribe = function () {
    if ((this.StartTime) && (this.EndTime)) {
        var count = $("#" + this.ButtonCountID).spinner("value");
        count = (((count) && (count < this.Config.maxNumber.Value)) ? count : this.Config.maxNumber.Value);
        if (document.getElementById(this.NameInputID).style.visibility == "visible") {
            var dataName = document.getElementById(this.NameInputID).value;
        }
        var isDesc = (document.getElementById(this.ButtonOrderID).value == this.OrderDescChar);
        this.RealtimeDataGrid.LoadData({});
        this.Handler.DataRecorder.Subcribe($("#" + this.RecorderSelectID).val(), this.StartTime, this.EndTime, dataName, count, isDesc, Irlovan.IrlovanHelper.Bind(this, this.Handle));
    } else {
        alert("Please set the starttime and endtime ");
        return null;
    }
}
Irlovan.Control.HistoryDataViewer.prototype.Order = function () {
    document.getElementById(this.ButtonOrderID).value = (document.getElementById(this.ButtonOrderID).value == this.OrderAscChar) ? this.OrderDescChar : this.OrderAscChar;
}
Irlovan.Control.HistoryDataViewer.prototype.SetRecorderList = function () {
    Irlovan.ControlHelper.ClearControl(this.RecorderSelectID);
    var recorders = this.Config.recorderList.Value.split(",");
    for (var i = 0; i < recorders.length; i++) {
        Irlovan.ControlHelper.CreateControlByStr(
        "<option value='" + recorders[i] + "' selected='selected'>" + recorders[i] +
        "</option>", this.RecorderSelectID, null);
    }
}