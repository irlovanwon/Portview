//Copyright(c) 2013,HIT All rights reserved.
//Des:HistoryEventViewer
//Author:Irlovan   
//Date:2013-05-14
//modification :2013-07-29
//modification :2015-08-27
//modification :2015-09-15 date picker bug fixed

Irlovan.Include.Using("Script/Control/Lib/TimePicker/Lib/jquery-ui-timepicker-addon.css");
Irlovan.Include.Using("Script/Control/Lib/TimePicker/TimePicker.js");

Irlovan.Control.HistoryEventViewer = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "HistoryEventViewer", id, containerID, gridContainerID, controlClass, config ? config : ({
        tagName: { Attributes: "tagName", Value: "HistoryEventViewer" },
        height: { Attributes: "height", Value: "300px" },
        width: { Attributes: "width", Value: "1024px" },
        recorderList: { Attributes: "recorderList", Value: "" },
        eventLevelList: { Attributes: "eventLevelList", Value: "" },
        maxNumber: { Attributes: "maxNumber", Value: 100 }
    }), left, top, zIndex, isLock);
    this.HistoryEventChartID = this.ID + "_container_historyEventGrid";
    this.HistoryEventChartContainerID = this.ID + "_container_historyEventGrid_container";
    this.ContainerID = this.ID + "_container";
    this.ButtonCountID = this.ID + "button_count";
    this.ButtonQueryID = this.ID + "button_query";
    this.ButtonOrderID = this.ID + "button_order";
    this.ExportID = this.ID + "_export";
    this.HistoryEventStartTimeID = this.ID + "HistoryEvent_DateTime_Start";
    this.HistoryEventEndTimeID = this.ID + "HistoryEvent_DateTime_End";
    this.StartTimePickerID = this.ID + "HistoryEvent_DateTime_Start_picker";
    this.EndTimePickerID = this.ID + "HistoryEvent_DateTime_End_picker";
    this.NameSelectID = this.ID + "name_select";
    this.RecorderSelectID = this.ID + "_recorder_select";
    this.EventLevelSelectID = this.ID + "_eventLevel_select";
    this.HistoryEventGrid;
    this.HistoryEventStartTime = "";
    this.OrderDescChar = "↓";
    this.OrderAscChar = "↑";
    this.Map = {};
    this.HistoryEventEndTime = "";
    this.NameInputID = this.ID + "_nameInput"
    this.Init(id, containerID);
    this.SetRecorderList();
    this.SetEventLevelList();
    this.MultipleSelectInit();
    this.Handler = new Irlovan.Handler.RecorderHandler();
}
Irlovan.Control.HistoryEventViewer.prototype = new Irlovan.Control.Classic();
Irlovan.Control.HistoryEventViewer.prototype.constructor = Irlovan.Control.HistoryEventViewer;
//init div element
Irlovan.Control.HistoryEventViewer.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.ContainerID + "' style='position:relative;width:" + this.Config.width.Value + ";height:" + this.Config.height.Value + ";'>" +
       "<div style='position:relative;float: top;'>" +
       "<p style='position:absolute;top: 0px;left: 0px;'>StartTime: <input type='text' id='" + this.HistoryEventStartTimeID + "'/></p>" +
       "<p style='position:absolute;top: 40px;left: 0px;'>EndTime  : <input type='text' id='" + this.HistoryEventEndTimeID + "'/></p>" +
       "<p style='position:absolute;top: 0px;left: 250px;'>Count: <input type='text' id='" + this.ButtonCountID + "' /></p>" +
       "<p style='position:absolute;top: 0px;left: 590px;'><label for='culture'>EventLevel:</label>" +
       "<select id='" + this.EventLevelSelectID + "' multiple='multiple'>" +
       "</select>" +
       "</p>" +
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
       "<input value='Query' style='position:absolute;right: 10px;top:48px;width:120px;height:30px;' type='button' id='" + this.ButtonQueryID + "' />" +
       "</div>" +
       "<div id='" + this.HistoryEventChartContainerID + "' style='top:80px;position:absolute;width:" + this.Config.width.Value + ";height:" + this.Config.height.Value + ";'>" +
       "</div>" +
       "</div>", this.ID, this.Pos);
    this.EventInit();
    this.DatePickerInit(Irlovan.IrlovanHelper.Bind(this, function (selectedDateTime, e) {
        this.HistoryEventStartTime = selectedDateTime;
        $('#' + this.HistoryEventEndTimeID).datetimepicker('option', 'minDate', $('#' + this.HistoryEventStartTimeID).datetimepicker('getDate'));
    }), this.HistoryEventStartTimeID, this.StartTimePickerID);
    this.DatePickerInit(Irlovan.IrlovanHelper.Bind(this, function (selectedDateTime, e) {
        this.HistoryEventEndTime = selectedDateTime;
        $('#' + this.HistoryEventStartTimeID).datetimepicker('option', 'maxDate', $('#' + this.HistoryEventEndTimeID).datetimepicker('getDate'));
    }), this.HistoryEventEndTimeID, this.EndTimePickerID);
    if (!this.HistoryEventGrid) {
        this.HistoryEventGrid = new Irlovan.PropertyGrid(this.HistoryEventChartContainerID, this.HistoryEventChartID, this.HistoryEventChartID + "_PropertyGrid_Table", this.HistoryEventChartID + "_PropertyGrid_Page", Irlovan.IrlovanHelper.Bind(this, Irlovan.RealtimeData.HistoryEventDataConfig), "HistoryEvent", this.Pos, null);
        $('#' + this.HistoryEventGrid.ID).jqGrid('setGridWidth', parseFloat(this.Config.width.Value));
        $('#' + this.HistoryEventGrid.ID).jqGrid('setGridHeight', parseFloat(this.Config.height.Value));
        $('#' + this.HistoryEventGrid.ID).jqGrid('setCaption', this.Config.tagName.Value);
    }
}

/**Dispose**/
Irlovan.Control.HistoryEventViewer.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    this.Handler.Dispose();
    this.Handler = null;
}
Irlovan.Control.HistoryEventViewer.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "tagName":
            $('#' + this.HistoryEventGrid.ID).jqGrid('setCaption', this.Config.tagName.Value);
            break;
        case "height":
            document.getElementById(this.ContainerID).style.height = data;
            document.getElementById(this.HistoryEventChartContainerID).style.height = data;
            $('#' + this.HistoryEventGrid.ID).jqGrid('setGridHeight', parseFloat(data));
            break;
        case "width":
            document.getElementById(this.ContainerID).style.width = data;
            document.getElementById(this.HistoryEventChartContainerID).style.width = data;
            $('#' + this.HistoryEventGrid.ID).jqGrid('setGridWidth', parseFloat(data));
            break;
        case "recorderList":
            this.SetRecorderList();
            break;
        case "eventLevelList":
            this.SetEventLevelList();
            $("#" + this.EventLevelSelectID).multiselect("refresh");
            break;
        default:
            break;
    }
}
Irlovan.Control.HistoryEventViewer.prototype.EventInit = function () {
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
        this.CreateRecorderMap();
        new Irlovan.ExpressionEditor(Irlovan.Global.ExpressionEditorID, Irlovan.Global.BodyID, this.Config.left.Value, this.Config.top.Value, document.getElementById(this.NameInputID).value, Irlovan.IrlovanHelper.Bind(this, function (e) {
            document.getElementById(this.NameInputID).value = e;
        }), this.Map[$("#" + this.RecorderSelectID).val()]);
    }), false);
}

Irlovan.Control.HistoryEventViewer.prototype.DatePickerInit = function (onDatePickerSelect, containerID, pickerID) {
    $('#' + containerID).datetimepicker({
        timeID: pickerID,
        timeFormat: 'HH:mm:ss',
        stepHour: 1,
        stepMinute: 1,
        stepSecond: 1,
        onSelect: onDatePickerSelect
    });
    //$('.' + containerID).datepicker("option", "dateFormat", "yy'-'mm'-'dd'");
}

/**Handle events message**/
Irlovan.Control.HistoryEventViewer.prototype.Handle = function (message) {
    this.HistoryEventGrid.LoadData(message);
}
Irlovan.Control.HistoryEventViewer.prototype.Subcribe = function () {
    if ((this.HistoryEventStartTime) && (this.HistoryEventEndTime)) {
        var count = $("#" + this.ButtonCountID).spinner("value");
        count = ((count) ? count : this.Config.maxNumber.Value);
        if ($("#" + this.RecorderSelectID).val()) {
            var eventLevel = $("#" + this.EventLevelSelectID).val();
        }
        if (document.getElementById(this.NameInputID).style.visibility == "visible") {
            var dataName = document.getElementById(this.NameInputID).value;
        }
        var isDesc = (document.getElementById(this.ButtonOrderID).value == this.OrderDescChar);
        this.HistoryEventGrid.LoadData({});
        this.Handler.EventRecorder.Subcribe($("#" + this.RecorderSelectID).val(), this.HistoryEventStartTime, this.HistoryEventEndTime, eventLevel, dataName, count, isDesc, Irlovan.IrlovanHelper.Bind(this, this.Handle));
    } else {
        alert("Please set the starttime and endtime ");
    }
}
Irlovan.Control.HistoryEventViewer.prototype.Order = function () {
    document.getElementById(this.ButtonOrderID).value = (document.getElementById(this.ButtonOrderID).value == this.OrderAscChar) ? this.OrderDescChar : this.OrderAscChar;
}
Irlovan.Control.HistoryEventViewer.prototype.SetRecorderList = function () {
    Irlovan.ControlHelper.ClearControl(this.RecorderSelectID);
    var recorders = this.Config.recorderList.Value.split(",");
    for (var i = 0; i < recorders.length; i++) {
        Irlovan.ControlHelper.CreateControlByStr(
        "<option value='" + recorders[i] + "' selected='selected'>" + recorders[i] +
        "</option>", this.RecorderSelectID, null);
    }
}
Irlovan.Control.HistoryEventViewer.prototype.SetEventLevelList = function () {
    Irlovan.ControlHelper.ClearControl(this.EventLevelSelectID);
    if ((!this.Config.eventLevelList) || (!this.Config.eventLevelList.Value)) { return; }
    var eventLevels = this.Config.eventLevelList.Value.split(",");
    for (var i = 0; i < eventLevels.length; i++) {
        Irlovan.ControlHelper.CreateControlByStr(
        "<option value='" + eventLevels[i] + "' selected='selected'>" + eventLevels[i] +
        "</option>", this.EventLevelSelectID, null);
    }
}
Irlovan.Control.HistoryEventViewer.prototype.ExportInit = function (data) {
    var csvContent;
    if (!data) { return; }
    csvContent = "data:text/csv;charset=utf-8,";
    data.forEach(function (infoArray, index) {
        var dataString = infoArray.join(",");
        csvContent += dataString + "\n";
    });
    var encodedUri = encodeURI(csvContent);
    var link = document.getElementById(this.ExportID);
    link.innerHTML = "Export";
    link.setAttribute("href", encodedUri);
    link.setAttribute("download", "Data.csv");
}
Irlovan.Control.HistoryEventViewer.prototype.MultipleSelectInit = function () {
    $("#" + this.EventLevelSelectID).multiselect();
}
Irlovan.Control.HistoryEventViewer.prototype.CreateRecorderMap = function (data) {
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