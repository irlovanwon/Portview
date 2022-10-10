//Copyright(c) 2013,HIT All rights reserved.
//Des:StatisticViewer
//Author:Irlovan   
//Date:2013-05-14
//modification :2013-07-29

Irlovan.Include.Using("Script/Communication/Websocket.js");
Irlovan.Include.Using("Script/PropertyGridConfig.js");
Irlovan.Include.Using("Script/RealtimeData/RealtimeData.js");
Irlovan.Include.Using("Script/Control/Lib/TimePicker/Lib/jquery-ui-timepicker-addon.css");
Irlovan.Include.Using("Script/Control/Lib/TimePicker/TimePicker.js");

Irlovan.StatisticViewer = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "StatisticViewer", id, containerID, gridContainerID, controlClass, config ? config : ({
        tagName: { Attributes: "tagName", Value: "StatisticViewer" },
        height: { Attributes: "height", Value: "300px" },
        width: { Attributes: "width", Value: "1024px" },
        recorderList: { Attributes: "recorderList", Value: "" },
        maxNumber: { Attributes: "maxNumber", Value: 100 }
    }), left, top, zIndex, isLock);
    this.StatisticChartID = this.ID + "_container_statisticEventGrid";
    this.StatisticChartContainerID = this.ID + "_container_statisticEventGrid_container";
    this.ContainerID = this.ID + "_container";
    this.DatePickerClass = this.ID + "_statistic_DateTime_class";
    this.ButtonQueryID = this.ID + "button_query";
    this.ExportID = this.ID + "_export";
    this.StatisticTimestampID = this.ID + "_statistic_DateTime";
    this.RecorderSelectID = this.ID + "_recorder_select";
    this.StatisticGrid;
    this.StatisticTimestamp = "";
    this.Init(id, containerID);
    this.SetRecorderList();
    this.WebSocket = new Irlovan.Communication.Websocket(
      'ws://' + Irlovan.Global.Domain + '/SQLHandler', '', null, null,
       Irlovan.IrlovanHelper.Bind(this, function (evt) {
           this._handler(Irlovan.RealtimeData.ReadDataByXML(evt.data));
           this.ExportInit(Irlovan.RealtimeData.ReadCSVDataByXML(evt.data));
       }), null);
}
Irlovan.StatisticViewer.prototype = new Irlovan.Control();
Irlovan.StatisticViewer.prototype.constructor = Irlovan.StatisticViewer;
//init div element
Irlovan.StatisticViewer.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.ContainerID + "' style='position:relative;width:" + this.Config.width.Value + ";height:" + this.Config.height.Value + ";'>" +
       "<div style='position:relative;float: top;'>" +
       "<p style='position:absolute;top: 0px;left: 0px;'>Time: <input type='text' id='" + this.StatisticTimestampID + "' class='" + this.DatePickerClass + "'/></p>" +
       "<p style='position:absolute;top: 0px;left: 250px;'><label for='culture'>RecorderSelect:</label>" +
       "<select id='" + this.RecorderSelectID + "'>" +
       "</select>" +
       "</p>" +
       "<input value='Query' style='position:absolute;right: 10px;top:0px;width:120px;height:30px;' type='button' id='" + this.ButtonQueryID + "' />" +
       "<a value='Export' style='position:absolute;right: 140px;top:0px;width:120px;height:30px;' id='" + this.ExportID + "' />" +
       "</div>" +
       "<div id='" + this.StatisticChartContainerID + "' style='top:40px;position:absolute;width:" + this.Config.width.Value + ";height:" + this.Config.height.Value + ";'>" +
       "</div>" +
       "</div>", this.ID, this.Pos);
    this._eventInit(this.ID, Irlovan.IrlovanHelper.Bind(this, function (selectedDateTime, e) {
        this.StatisticTimestamp = selectedDateTime;
    }));
    if (!this.StatisticGrid) {
        this.StatisticGrid = new Irlovan.PropertyGrid(this.StatisticChartContainerID, this.StatisticChartID, this.StatisticChartID + "_PropertyGrid_Table", this.StatisticChartID + "_PropertyGrid_Page", Irlovan.IrlovanHelper.Bind(this, Irlovan.RealtimeData.StatisticConfig), "Statistic", this.Pos, null);
        $('#' + this.StatisticGrid.ID).jqGrid('setGridWidth', parseFloat(this.Config.width.Value));
        $('#' + this.StatisticGrid.ID).jqGrid('setGridHeight', parseFloat(this.Config.height.Value));
        $('#' + this.StatisticGrid.ID).jqGrid('setCaption', this.Config.tagName.Value);
    }

}
Irlovan.StatisticViewer.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    this.WebSocket.Close();
    this.WebSocket = null;
}
Irlovan.StatisticViewer.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "tagName":
            $('#' + this.StatisticGrid.ID).jqGrid('setCaption', this.Config.tagName.Value);
            break;
        case "height":
            document.getElementById(this.ContainerID).style.height = data;
            document.getElementById(this.StatisticChartContainerID).style.height = data;
            $('#' + this.StatisticGrid.ID).jqGrid('setGridHeight', parseFloat(data));
            break;
        case "width":
            document.getElementById(this.ContainerID).style.width = data;
            document.getElementById(this.StatisticChartContainerID).style.width = data;
            $('#' + this.StatisticGrid.ID).jqGrid('setGridWidth', parseFloat(data));
            break;
        case "recorderList":
            this.SetRecorderList();
            break;
        default:
            break;
    }
}
Irlovan.StatisticViewer.prototype._eventInit = function (id, onDatePickerSelect) {
    $('.' + this.DatePickerClass).datetimepicker({
        timeFormat: 'HH:mm:ss',
        stepHour: 1,
        stepMinute: 1,
        stepSecond: 1,
        onSelect: onDatePickerSelect
    });
    $('.' + this.DatePickerClass).datepicker("option", "dateFormat", "yy'-'mm'-'dd'");
    //$("#" + this.ButtonCountID).spinner();
    //$("#" + this.NameSelectID).change(Irlovan.IrlovanHelper.Bind(this, function () {
    //    document.getElementById(this.NameInputID).style.visibility = ($("#" + this.NameSelectID).val() == "all") ? "hidden" : "visible";
    //}));
    document.getElementById(this.ButtonQueryID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        this.WebSocket.Send(this._requestMessage());
    }), false);
}
Irlovan.StatisticViewer.prototype._handler = function (data) {
    this.StatisticGrid.LoadData(data.Data);
}
Irlovan.StatisticViewer.prototype._requestMessage = function () {
    if (this.StatisticTimestampID) {
        var result = { Statistic: { TimeStamp: this.StatisticTimestamp, RecorderName: $("#" + this.RecorderSelectID).val() } }
        return Irlovan.IrlovanHelper.JsonToString(result);
    } else {
        alert("Please set time ");
        return null;
    }
}
Irlovan.StatisticViewer.prototype.SetRecorderList = function () {
    Irlovan.ControlHelper.ClearControl(this.RecorderSelectID);
    var recorders = this.Config.recorderList.Value.split(",");
    for (var i = 0; i < recorders.length; i++) {
        Irlovan.ControlHelper.CreateControlByStr(
        "<option value='" + recorders[i] + "' selected='selected'>" + recorders[i] +
        "</option>", this.RecorderSelectID, null);
    }
}
Irlovan.StatisticViewer.prototype.ExportInit = function (data) {
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