//Copyright(c) 2013,HIT All rights reserved.
//Des:StatisticViewer
//Author:Irlovan   
//Date:2013-05-14
//modification :2013-07-29
//modification :2015-09-16

Irlovan.Include.Using("Script/Control/Lib/TimePicker/Lib/jquery-ui-timepicker-addon.css");
Irlovan.Include.Using("Script/Control/Lib/TimePicker/TimePicker.js");

Irlovan.Control.StatisticViewer = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "StatisticViewer", id, containerID, gridContainerID, controlClass, config ? config : ({
        tagName: { Attributes: "tagName", Value: "StatisticViewer" },
        height: { Attributes: "height", Value: "300px" },
        width: { Attributes: "width", Value: "1024px" },
        recorderList: { Attributes: "recorderList", Value: "" }
    }), left, top, zIndex, isLock);
    this.StatisticChartID = this.ID + "_container_statisticEventGrid";
    this.StatisticChartContainerID = this.ID + "_container_statisticEventGrid_container";
    this.ContainerID = this.ID + "_container";
    this.ButtonQueryID = this.ID + "button_query";
    this.TimeStamp = "";
    this.StatisticTimestampID = this.ID + "_statistic_DateTime";
    this.StatisticTimestampPickerID = this.ID + "_statistic_DateTime_picker";
    this.RecorderSelectID = this.ID + "_recorder_select";
    this.StatisticGrid;
    this.Init(id, containerID);
    this.SetRecorderList();
    this.Handler = new Irlovan.Handler.RecorderHandler();
}
Irlovan.Control.StatisticViewer.prototype = new Irlovan.Control.Classic();
Irlovan.Control.StatisticViewer.prototype.constructor = Irlovan.Control.StatisticViewer;
/**init div element**/
Irlovan.Control.StatisticViewer.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.ContainerID + "' style='position:relative;width:" + this.Config.width.Value + ";height:" + this.Config.height.Value + ";'>" +
       "<div style='position:relative;float: top;'>" +
       "<p style='position:absolute;top: 0px;left: 0px;'>Time: <input type='text' id='" + this.StatisticTimestampID + "'/></p>" +
       "<p style='position:absolute;top: 0px;left: 250px;'><label for='culture'>RecorderSelect:</label>" +
       "<select id='" + this.RecorderSelectID + "'>" +
       "</select>" +
       "</p>" +
       "<input value='Query' style='position:absolute;right: 10px;top:0px;width:120px;height:30px;' type='button' id='" + this.ButtonQueryID + "' />" +
       "</div>" +
       "<div id='" + this.StatisticChartContainerID + "' style='top:40px;position:absolute;width:" + this.Config.width.Value + ";height:" + this.Config.height.Value + ";'>" +
       "</div>" +
       "</div>", this.ID, this.Pos);
    this.EventInit();
    this.DatePickerInit(Irlovan.IrlovanHelper.Bind(this, function (selectedDateTime, e) {
        this.TimeStamp = selectedDateTime;
    }), this.StatisticTimestampID, this.StatisticTimestampPickerID);
    if (!this.StatisticGrid) {
        this.StatisticGrid = new Irlovan.PropertyGrid(this.StatisticChartContainerID, this.StatisticChartID, this.StatisticChartID + "_PropertyGrid_Table", this.StatisticChartID + "_PropertyGrid_Page", Irlovan.IrlovanHelper.Bind(this, Irlovan.RealtimeData.StatisticConfig), "Statistic", this.Pos, null);
        $('#' + this.StatisticGrid.ID).jqGrid('setGridWidth', parseFloat(this.Config.width.Value));
        $('#' + this.StatisticGrid.ID).jqGrid('setGridHeight', parseFloat(this.Config.height.Value));
        $('#' + this.StatisticGrid.ID).jqGrid('setCaption', this.Config.tagName.Value);
    }
}
/**Dispose**/
Irlovan.Control.StatisticViewer.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    this.Handler.Dispose();
    this.Handler = null;
}

Irlovan.Control.StatisticViewer.prototype.SetValue = function (name, colName, data) {
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
Irlovan.Control.StatisticViewer.prototype.EventInit = function (id, onDatePickerSelect) {
    document.getElementById(this.ButtonQueryID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        this.Subcribe();
    }), false);
}
Irlovan.Control.StatisticViewer.prototype.DatePickerInit = function (onDatePickerSelect, containerID, pickerID) {
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
Irlovan.Control.StatisticViewer.prototype.Handle = function (message) {
    this.StatisticGrid.LoadData(message);
}

Irlovan.Control.StatisticViewer.prototype.Subcribe = function () {
    if (!this.TimeStamp) { alert("Please set time"); }
    this.StatisticGrid.LoadData({});
    this.Handler.StatisticRecorder.Subcribe($("#" + this.RecorderSelectID).val(), this.TimeStamp, Irlovan.IrlovanHelper.Bind(this, this.Handle));
}

Irlovan.Control.StatisticViewer.prototype.SetRecorderList = function () {
    Irlovan.ControlHelper.ClearControl(this.RecorderSelectID);
    var recorders = this.Config.recorderList.Value.split(",");
    for (var i = 0; i < recorders.length; i++) {
        Irlovan.ControlHelper.CreateControlByStr(
        "<option value='" + recorders[i] + "' selected='selected'>" + recorders[i] +
        "</option>", this.RecorderSelectID, null);
    }
}