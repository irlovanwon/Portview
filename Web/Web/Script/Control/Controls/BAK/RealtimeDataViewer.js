//Copyright(c) 2013,HIT All rights reserved.
//Des:HistoryEventViewer
//Author:Irlovan   
//Date:2013-05-14
//modification :2013-07-29

Irlovan.Include.Using("Script/Communication/Websocket.js");
Irlovan.Include.Using("Script/PropertyGridConfig.js");
Irlovan.Include.Using("Script/RealtimeData/RealtimeData.js");

Irlovan.RealtimeDataViewer = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "RealtimeDataViewer", id, containerID, gridContainerID, controlClass, config ? config : ({
        tagName: { Attributes: "tagName", Value: "RealtimeDataViewer" },
        height: { Attributes: "height", Value: "300px" },
        width: { Attributes: "width", Value: "1024px" },
        recorderList: { Attributes: "recorderList", Value: "" },
        maxNumber: { Attributes: "maxNumber", Value: 100 }
    }), left, top, zIndex, isLock);
    this.RealtimeDataChartID = this.ID + "_container_realtimeDataGrid";
    this.RealtimeDataChartContainerID = this.ID + "_container_realtimeDataGrid_container";
    this.DataViewerContainerID = this.ID + "_dataviewer_container";
    this.DatePickerClass = this.ID + "realtimeData_DateTime";
    this.ButtonCountID = this.ID + "button_count";
    this.ButtonQueryID = this.ID + "button_query";
    this.StartTimeID = this.ID + "RealtimeData_DateTime_Start";
    this.EndTimeID = this.ID + "RealtimeData_DateTime_End";
    this.NameSelectID = this.ID + "name_select";
    this.RecorderSelectID = this.ID + "_recorder_select";
    this.ExportID = this.ID + "_export";
    this.RealtimeDataGrid;
    this.StartTime="";
    this.EndTime = "";
    this.NameInputID = this.ID + "_nameInput"
    this.Init(id, containerID);
    this.SetRecorderList();
    this.WebSocket = new Irlovan.Communication.Websocket(
      'ws://' + Irlovan.Global.Domain + '/SQLHandler', '', null, null,
       Irlovan.IrlovanHelper.Bind(this, function (evt) {
           this._handler(Irlovan.RealtimeData.ReadDataByXML(evt.data));
           this.ExportInit(Irlovan.RealtimeData.ReadCSVDataByXML(evt.data));
       }), null);
}
Irlovan.RealtimeDataViewer.prototype = new Irlovan.Control();
Irlovan.RealtimeDataViewer.prototype.constructor = Irlovan.RealtimeDataViewer;
//init div element
Irlovan.RealtimeDataViewer.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.DataViewerContainerID + "' style='position:relative;width:" + this.Config.width.Value + ";height:" + this.Config.height.Value + ";'>" +
       "<div style='position:relative;float: top;'>" +
       "<p style='position:absolute;top: 0px;left: 0px;'>StartTime: <input type='text' id='" + this.StartTimeID + "' class='" + this.DatePickerClass + "'/></p>" +
       "<p style='position:absolute;top: 40px;left: 0px;'>EndTime  : <input type='text' id='" + this.EndTimeID + "' class='" + this.DatePickerClass + "' /></p>" +
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
       "<input value='Query' style='position:absolute;right: 10px;top:40px;width:120px;height:30px;' type='button' id='" + this.ButtonQueryID + "' />" +
       "<a value='Export' style='position:absolute;right: 140px;top:40px;width:120px;height:30px;' id='" + this.ExportID + "' />" +
       "</div>" +
       "<div id='" + this.RealtimeDataChartContainerID + "' style='top:80px;position:absolute;width:" + this.Config.width.Value + ";height:" + this.Config.height.Value + ";'>" +
       "</div>" +
       "</div>", this.ID, this.Pos);
    this._eventInit(this.ID, Irlovan.IrlovanHelper.Bind(this,function (selectedDateTime, e) {
        if (e.id == this.StartTimeID) {
            this.StartTime = selectedDateTime;
            $('#' + this.EndTimeID).datetimepicker('option', 'minDate', $('#' + this.StartTimeID).datetimepicker('getDate'));
        }
        else if (e.id == this.EndTimeID) {
            $('#' + this.StartTimeID).datetimepicker('option', 'maxDate', $('#' + this.EndTimeID).datetimepicker('getDate'));
            this.EndTime = selectedDateTime;
        }
    }));
    if (!this.RealtimeDataGrid) {
        this.RealtimeDataGrid = new Irlovan.PropertyGrid(this.RealtimeDataChartContainerID, this.RealtimeDataChartID, this.RealtimeDataChartID + "_PropertyGrid_Table", this.RealtimeDataChartID + "_PropertyGrid_Page", Irlovan.IrlovanHelper.Bind(this, Irlovan.RealtimeData.RealtimeDataConfig), "HistoryEvent", this.Pos,null);
        $('#' + this.RealtimeDataGrid.ID).jqGrid('setGridWidth', parseFloat(this.Config.width.Value));
        $('#' + this.RealtimeDataGrid.ID).jqGrid('setGridHeight', parseFloat(this.Config.height.Value));
        $('#' + this.RealtimeDataGrid.ID).jqGrid('setCaption', this.Config.tagName.Value);
    }
}
Irlovan.RealtimeDataViewer.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    this.WebSocket.Close();
    this.WebSocket = null;
}
Irlovan.RealtimeDataViewer.prototype.SetValue = function (name, colName, data) {
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
Irlovan.RealtimeDataViewer.prototype._eventInit = function (id,onDatePickerSelect) {
    $('.' + this.DatePickerClass).datetimepicker({
        timeFormat: 'HH:mm:ss',
        stepHour: 1,
        stepMinute: 1,
        stepSecond: 1,
        onSelect: onDatePickerSelect
    });
    $('.' + this.DatePickerClass).datepicker("option", "dateFormat", "yy'-'mm'-'dd'");
    $("#" + this.ButtonCountID).spinner();
    $("#" + this.NameSelectID).change(Irlovan.IrlovanHelper.Bind(this,function () {
        document.getElementById(this.NameInputID).style.visibility = ($("#" + this.NameSelectID).val() == "all") ? "hidden" : "visible";
    }));
    document.getElementById(this.ButtonQueryID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        this.WebSocket.Send(this._requestMessage());
    }), false);
    document.getElementById(this.NameInputID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        new Irlovan.ExpressionEditor(Irlovan.Global.ExpressionEditorID, Irlovan.Global.BodyID, this.Config.left.Value, this.Config.top.Value, document.getElementById(this.NameInputID).value, Irlovan.IrlovanHelper.Bind(this, function (e) {
            document.getElementById(this.NameInputID).value = e;
        }));
    }), false);
}
Irlovan.RealtimeDataViewer.prototype._handler = function (data) {
    //if (data.Name != "HistoryEvent") { return false; }
    this.RealtimeDataGrid.LoadData(data.Data);
}
Irlovan.RealtimeDataViewer.prototype._requestMessage = function () {
    if ((this.StartTime) && (this.EndTime)) {
        var result = { RealtimeData: { StartTime: this.StartTime, EndTime: this.EndTime, RecorderName: $("#" + this.RecorderSelectID).val() } }
        var count = $("#" + this.ButtonCountID).spinner("value");
        result.RealtimeData.Amount =(((count)&&(count<this.Config.maxNumber.Value))? count:this.Config.maxNumber.Value);
        if (document.getElementById(this.NameInputID).style.visibility == "visible") {
            result.RealtimeData.Name = document.getElementById(this.NameInputID).value;
        }
        return Irlovan.IrlovanHelper.JsonToString(result);
    } else {
        alert("Please set the starttime and endtime ");
        return null;
    }
}
Irlovan.RealtimeDataViewer.prototype.SetRecorderList = function () {
    Irlovan.ControlHelper.ClearControl(this.RecorderSelectID);
    var recorders = this.Config.recorderList.Value.split(",");
    for (var i = 0; i < recorders.length; i++) {
        Irlovan.ControlHelper.CreateControlByStr(
        "<option value='" + recorders[i] + "' selected='selected'>" + recorders[i] +
        "</option>", this.RecorderSelectID, null);
    }
}
Irlovan.RealtimeDataViewer.prototype.ExportInit = function (data) {
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