//Copyright(c) 2013,HIT All rights reserved.
//Des:HistoryEventViewer
//Author:Irlovan   
//Date:2013-05-14
//modification :2013-07-29

Irlovan.Include.Using("Script/Communication/Websocket.js");
Irlovan.Include.Using("Script/PropertyGridConfig.js");
Irlovan.Include.Using("Script/RealtimeData/RealtimeData.js");
Irlovan.Include.Using("Script/Control/Lib/TimePicker/Lib/jquery-ui-timepicker-addon.css");
Irlovan.Include.Using("Script/Control/Lib/TimePicker/TimePicker.js");

Irlovan.HistoryEventViewer = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
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
    this.DatePickerClass = this.ID + "historyEvent_DateTime";
    this.ButtonCountID = this.ID + "button_count";
    this.ButtonQueryID = this.ID + "button_query";
    this.ExportID = this.ID + "_export";
    this.HistoryEventStartTimeID = this.ID + "HistoryEvent_DateTime_Start";
    this.HistoryEventEndTimeID = this.ID + "HistoryEvent_DateTime_End";
    this.NameSelectID = this.ID + "name_select";
    this.RecorderSelectID = this.ID + "_recorder_select";
    this.EventLevelSelectID = this.ID + "_eventLevel_select";
    this.HistoryEventGrid;
    this.Current;
    this.HistoryEventStartTime = "";
    this.Map = {};
    this.HistoryEventEndTime = "";
    this.NameInputID = this.ID + "_nameInput"
    this.Init(id, containerID);
    this.SetRecorderList();
    this.SetEventLevelList();
    this.MultipleSelectInit();
    this.WebSocket = new Irlovan.Communication.Websocket(
      'ws://' + Irlovan.Global.Domain + '/SQLHandler', '', null, null,
       Irlovan.IrlovanHelper.Bind(this, function (evt) {
           this._handler(Irlovan.RealtimeData.ReadDataByXML(evt.data));
           this.ExportInit(Irlovan.RealtimeData.ReadCSVDataByXML(evt.data));
       }), null);
}
Irlovan.HistoryEventViewer.prototype = new Irlovan.Control();
Irlovan.HistoryEventViewer.prototype.constructor = Irlovan.HistoryEventViewer;
//init div element
Irlovan.HistoryEventViewer.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.ContainerID + "' style='position:relative;width:" + this.Config.width.Value + ";height:" + this.Config.height.Value + ";'>" +
       "<div style='position:relative;float: top;'>" +
       "<p style='position:absolute;top: 0px;left: 0px;'>StartTime: <input type='text' id='" + this.HistoryEventStartTimeID + "' class='" + this.DatePickerClass + "'/></p>" +
       "<p style='position:absolute;top: 40px;left: 0px;'>EndTime  : <input type='text' id='" + this.HistoryEventEndTimeID + "' class='" + this.DatePickerClass + "' /></p>" +
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
       "<input value='Query' style='position:absolute;right: 10px;top:48px;width:120px;height:30px;' type='button' id='" + this.ButtonQueryID + "' />" +
       "<a value='Export' style='position:absolute;right: 140px;top:40px;width:120px;height:30px;' id='" + this.ExportID + "' />" +
       "</div>" +
       "<div id='" + this.HistoryEventChartContainerID + "' style='top:80px;position:absolute;width:" + this.Config.width.Value + ";height:" + this.Config.height.Value + ";'>" +
       "</div>" +
       "</div>", this.ID, this.Pos);
    this._eventInit(this.ID, Irlovan.IrlovanHelper.Bind(this, function (selectedDateTime, e) {
        if (this.Current == this.HistoryEventStartTimeID) {
            this.HistoryEventStartTime = selectedDateTime;
            $('#' + this.HistoryEventEndTimeID).datetimepicker('option', 'minDate', $('#' + this.HistoryEventStartTimeID).datetimepicker('getDate'));
        }
        else if (this.Current == this.HistoryEventEndTimeID) {
            $('#' + this.HistoryEventStartTimeID).datetimepicker('option', 'maxDate', $('#' + this.HistoryEventEndTimeID).datetimepicker('getDate'));
            this.HistoryEventEndTime = selectedDateTime;
        }
    }));
    if (!this.HistoryEventGrid) {
        this.HistoryEventGrid = new Irlovan.PropertyGrid(this.HistoryEventChartContainerID, this.HistoryEventChartID, this.HistoryEventChartID + "_PropertyGrid_Table", this.HistoryEventChartID + "_PropertyGrid_Page", Irlovan.IrlovanHelper.Bind(this, Irlovan.RealtimeData.HistoryEventDataConfig), "HistoryEvent", this.Pos, null);
        $('#' + this.HistoryEventGrid.ID).jqGrid('setGridWidth', parseFloat(this.Config.width.Value));
        $('#' + this.HistoryEventGrid.ID).jqGrid('setGridHeight', parseFloat(this.Config.height.Value));
        $('#' + this.HistoryEventGrid.ID).jqGrid('setCaption', this.Config.tagName.Value);
    }
}
Irlovan.HistoryEventViewer.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    this.WebSocket.Close();
    this.WebSocket = null;
}
Irlovan.HistoryEventViewer.prototype.SetValue = function (name, colName, data) {
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
Irlovan.HistoryEventViewer.prototype._eventInit = function (id, onDatePickerSelect) {
    $('.' + this.DatePickerClass).datetimepicker({
        timeID:'wee',
        timeFormat: 'HH:mm:ss',
        stepHour: 1,
        stepMinute: 1,
        stepSecond: 1,
        onSelect: onDatePickerSelect
    });
    //$('.' + this.DatePickerClass).datepicker("option", "dateFormat", "yy'-'mm'-'dd'");
    $("#" + this.ButtonCountID).spinner();
    $("#" + this.NameSelectID).change(Irlovan.IrlovanHelper.Bind(this, function () {
        document.getElementById(this.NameInputID).style.visibility = ($("#" + this.NameSelectID).val() == "all") ? "hidden" : "visible";
    }));
    document.getElementById(this.ButtonQueryID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        this.WebSocket.Send(this._requestMessage());
    }), false);
    document.getElementById(this.HistoryEventStartTimeID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        this.Current = this.HistoryEventStartTimeID;
    }), false);
    document.getElementById(this.HistoryEventEndTimeID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        this.Current = this.HistoryEventEndTimeID
    }), false); 
    document.getElementById(this.NameInputID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        this.CreateRecorderMap();
        new Irlovan.ExpressionEditor(Irlovan.Global.ExpressionEditorID, Irlovan.Global.BodyID, this.Config.left.Value, this.Config.top.Value, document.getElementById(this.NameInputID).value, Irlovan.IrlovanHelper.Bind(this, function (e) {
            document.getElementById(this.NameInputID).value = e;
        }),this.Map[$("#" + this.RecorderSelectID).val()]);
    }), false);
}
Irlovan.HistoryEventViewer.prototype._handler = function (data) {
    //if (data.Name != "HistoryEvent") { return false; }
    this.HistoryEventGrid.LoadData(data.Data);
}
Irlovan.HistoryEventViewer.prototype._requestMessage = function () {
    if ((this.HistoryEventStartTime) && (this.HistoryEventEndTime)) {
        var result = { HistoryEvent: { StartTime: this.HistoryEventStartTime, EndTime: this.HistoryEventEndTime, RecorderName: $("#" + this.RecorderSelectID).val() } }
        var count = $("#" + this.ButtonCountID).spinner("value");
        result.HistoryEvent.Amount = ((count) ? count : this.Config.maxNumber.Value);
        if ($("#" + this.RecorderSelectID).val()) {
            result.HistoryEvent.EventLevel = $("#" + this.EventLevelSelectID).val();
        }
        if (document.getElementById(this.NameInputID).style.visibility == "visible") {
            result.HistoryEvent.EventName = document.getElementById(this.NameInputID).value;
        }
        return Irlovan.IrlovanHelper.JsonToString(result);
    } else {
        alert("Please set the starttime and endtime ");
        return null;
    }
}
Irlovan.HistoryEventViewer.prototype.SetRecorderList = function () {
    Irlovan.ControlHelper.ClearControl(this.RecorderSelectID);
    var recorders = this.Config.recorderList.Value.split(",");
    for (var i = 0; i < recorders.length; i++) {
        Irlovan.ControlHelper.CreateControlByStr(
        "<option value='" + recorders[i] + "' selected='selected'>" + recorders[i] +
        "</option>", this.RecorderSelectID, null);
    }
}
Irlovan.HistoryEventViewer.prototype.SetEventLevelList = function () {
    Irlovan.ControlHelper.ClearControl(this.EventLevelSelectID);
    if ((!this.Config.eventLevelList) || (!this.Config.eventLevelList.Value)) { return; }
    var eventLevels = this.Config.eventLevelList.Value.split(",");
    for (var i = 0; i < eventLevels.length; i++) {
        Irlovan.ControlHelper.CreateControlByStr(
        "<option value='" + eventLevels[i] + "' selected='selected'>" + eventLevels[i] +
        "</option>", this.EventLevelSelectID, null);
    }
}
Irlovan.HistoryEventViewer.prototype.ExportInit = function (data) {
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
Irlovan.HistoryEventViewer.prototype.MultipleSelectInit = function () {
    $("#" + this.EventLevelSelectID).multiselect();
}
Irlovan.HistoryEventViewer.prototype.CreateRecorderMap = function (data) {
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