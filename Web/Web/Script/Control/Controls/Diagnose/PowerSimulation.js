//Copyright(c) 2015,HIT All rights reserved.
//Des:Power Simulation
//Author:Irlovan   
//Date:2015-09-29
//modification :

Irlovan.Include.Using("Script/Lib/VIS/vis.css");
Irlovan.Include.Using("Script/Lib/VIS/vis.js");

Irlovan.Control.PowerSimulation = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "PowerSimulation", id, containerID, gridContainerID, controlClass, config ? config : ({
        recorderList: { Attributes: "recorderList", Value: "" },
        fillColor: { Attributes: "fillColor", Value: 'white' },
        strokeColor: { Attributes: "strokeColor", Value: 'black' },
        strokeType: { Attributes: "strokeType", Value: "solid", Description: "dotted, solid" },
        labels: { Attributes: "labels", Value: "1,2", Description: "label1,label2" },
        drawPoints: { Attributes: "drawPoints", Value: "circle", Description: "square, circle" },
        shaded: { Attributes: "shaded", Value: "bottom", Description: "top, bottom" },
        iniSelected: { Attributes: "iniSelected", Value: false },
        width: { Attributes: "width", Value: '250' },
        height: { Attributes: "height", Value: '250' },
        thickness: { Attributes: "thickness", Value: '2' }
    }), left, top, zIndex, isLock);
    this.EditionFix();
    this.RecID = this.ID + "_DataViewer";
    this.ContainerID = this.ID + "_trendContainer";
    this.StartTimeID = this.ID + "_PowerSimulation_DateTime_Start";
    this.EndTimeID = this.ID + "_PowerSimulation_DateTime_End";
    this.StartTimePickerID = this.ID + "HistoryEvent_DateTime_Start_picker";
    this.EndTimePickerID = this.ID + "HistoryEvent_DateTime_End_picker";
    this.DataSelectID = this.ID + "_dataselect";
    this.ButtonQueryID = this.ID + "_button_query";
    this.RecorderSelectID = this.ID + "_recorderSelect";
    this.AVERangeSelectID = this.ID + "_aveRangeSelect";
    this.LabelVisibleSelectID = this.ID + "_labelvisibleSelect";
    this.Graph2d;
    this.Container;
    this.Dataset;
    this.Groups;
    this.Options;
    this.Items = [];
    this.StartTime = "";
    this.EndTime = "";
    this.Init(id, containerID);
    this.Handler = new Irlovan.Handler.RecorderHandler();
}
Irlovan.Control.PowerSimulation.prototype = new Irlovan.Control.Classic();
Irlovan.Control.PowerSimulation.prototype.constructor = Irlovan.Control.PowerSimulation;

//init div element
Irlovan.Control.PowerSimulation.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.RecID + "' >" +
       "<p>" +
       "<div id='" + this.ContainerID + "' style='background-size:100% 100%;background-color: " + this.Config.fillColor.Value + ";border:" + this.Config.thickness.Value + "px " + this.Config.strokeType.Value + " " + this.Config.strokeColor.Value + ";position:relative;width:" + this.Config.width.Value + "px;height:" + this.Config.height.Value + "px;top:50px'/>" +
       "</p>" +
       "<p style='position:absolute;top: 0px;left: 0px;'>StartTime: <input type='text' id='" + this.StartTimeID + "'/></p>" +
       "<p style='position:absolute;top: 40px;left: 0px;'>EndTime  : <input type='text' id='" + this.EndTimeID + "'/></p>" +
       "<p style='position:absolute;top: 40px;left: 250px;'><label for='culture'>RecorderSelect:</label>" +
       "<select id='" + this.RecorderSelectID + "'>" +
       "</select>" +
       "<label for='" + this.DataSelectID + "'>DataSelect:</label>" +
       "<select id='" + this.DataSelectID + "' multiple='multiple'>" +
       "</select>" +
       "</p>" +

       "<p style='position:absolute;top: 0px;left: 450px;'><label for='culture'>Label Visible:</label>" +
       "<select id='" + this.LabelVisibleSelectID + "'>" +
       "<option value='True' selected='selected'>True</option>" +
       "<option value='False' selected='selected'>False</option>" +
       "</select>" +
       "</p>" +

       "<p style='position:absolute;top: 0px;left: 250px;'><label for='culture'>Average Range:</label>" +
       "<select id='" + this.AVERangeSelectID + "'>" +
       "<option value='Year' selected='selected'>Year</option>" +
       "<option value='Day' selected='selected'>Day</option>" +
       "<option value='Month' selected='selected'>Month</option>" +
       "</select>" +
       "</p>" +
       "<input value='Query' style='position:absolute;right:10px;top:48px;width:120px;height:30px;' type='button' id='" + this.ButtonQueryID + "' />" +
       "</div>", this.ID, this.Pos);
    this.DatePickerInit(Irlovan.IrlovanHelper.Bind(this, function (selectedDateTime, e) {
        this.StartTime = selectedDateTime;
        $('#' + this.EndTimeID).datetimepicker('option', 'minDate', $('#' + this.StartTimeID).datetimepicker('getDate'));
    }), this.StartTimeID, this.StartTimePickerID);
    this.DatePickerInit(Irlovan.IrlovanHelper.Bind(this, function (selectedDateTime, e) {
        this.EndTime = selectedDateTime;
        $('#' + this.StartTimeID).datetimepicker('option', 'maxDate', $('#' + this.EndTimeID).datetimepicker('getDate'));
    }), this.EndTimeID, this.EndTimePickerID);
    document.getElementById(this.ButtonQueryID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        this.LibDestroy();
        this.Subcribe();
    }), false);
    this.Start();
}

/**Init Date Picker plugin**/
Irlovan.Control.PowerSimulation.prototype.DatePickerInit = function (onDatePickerSelect, containerID, pickerID) {
    $('#' + containerID).datetimepicker({
        timeID: pickerID,
        timeFormat: 'HH:mm:ss',
        stepHour: 1,
        stepMinute: 1,
        stepSecond: 1,
        onSelect: onDatePickerSelect
    });
}

/**Init Group config info**/
Irlovan.Control.PowerSimulation.prototype.GroupInit = function () {
    this.Groups = new vis.DataSet();
    if (this.Config.labels.Value) {
        var labels = this.Config.labels.Value.split(",");
    }
    for (var i = 0; i < labels.length; i++) {
        this.Groups.add({
            id: i,
            content: labels[i],
            visible:false,
            options: {
                drawPoints: {
                    style: this.Config.drawPoints.Value, // square, circle
                },
                shaded: {
                    orientation: this.Config.shaded.Value// top, bottom
                }
            }
        });
        this.Groups.add({
            id: i + 1000,
            content: labels[i],
            visible:false,
            options: {
                drawPoints: {
                    style: this.Config.drawPoints.Value, // square, circle
                },
                shaded: {
                    orientation: this.Config.shaded.Value// top, bottom
                }
            }
        });
    }
}
Irlovan.Control.PowerSimulation.prototype.GridInit = function () {
    // create a graph2d with an (currently empty) dataset
    if (this.Items.length == 0) { return; }
    this.Container = document.getElementById(this.ContainerID);
    this.Dataset = new vis.DataSet(this.Items);
    this.Options = {
        legend: true,
        start: this.StartTime, // changed so its faster
        end: this.EndTime,
        width: '100%',
        //graphHeight: (parseFloat(this.Config.height.Value) - 50) + "px",
        minHeight: "50px",
        height: (parseFloat(this.Config.height.Value)) + "px",
        moveable: true
    }
    this.Graph2d = new vis.Graph2d(this.Container, this.Dataset, this.Groups, this.Options);
    this.Graph2d.fit();
}
Irlovan.Control.PowerSimulation.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "width":
            document.getElementById(this.ContainerID).style.width = data + "px";
            break;
        case "height":
            document.getElementById(this.ContainerID).style.height = data + "px";
            break;
        case "fillColor":
            document.getElementById(this.ContainerID).style.backgroundColor = this.Config.fillColor.Value;
            break;
        case "iniSelected":
            this.SelectAll();
            break;
        case "drawPoints":
        case "shaded":
            this.Start();
            break;
        case "recorderList":
            this.SetRecorderList();
            break;
        case "thickness":
        case "strokeType":
        case "strokeColor":
            document.getElementById(this.ContainerID).style.border = this.Config.thickness.Value + "px " + this.Config.strokeType.Value + " " + this.Config.strokeColor.Value;
            break;
        default:
            break;
    }
}

/**Start to simulate**/
Irlovan.Control.PowerSimulation.prototype.Start = function () {
    this.LibDestroy();
    this.GroupInit();
    this.DataSelectInit();
    this.SetRecorderList();
    this.MultipleSelectInit();
    this.SelectAll();
}

/**subcribe data from server**/
Irlovan.Control.PowerSimulation.prototype.Subcribe = function () {
    if ((!this.StartTime) || (!this.EndTime)) { alert("Please select time"); return; }
    var dataList = $("#" + this.DataSelectID).val();
    this.Handler.MatrixRecorder.Subcribe($("#" + this.RecorderSelectID).val(), this.StartTime, this.EndTime, dataList, Irlovan.IrlovanHelper.Bind(this, this.Handle));
}

/**handle data from server**/
Irlovan.Control.PowerSimulation.prototype.Handle = function (messages) {
    if (messages.length == 0) { return; }
    var labels = this.Config.labels.Value.split(",");
    for (var i = 0; i < labels.length; i++) {
        var groupID = i;
        var groupIDAVE = i + 1000;
        this.Groups.update({ id: groupID, visible: false });
        this.Groups.update({ id: groupIDAVE, visible: false });
    }
    var averageArray = [];
    var rangeArray = [];
    var dateCache = new Date(messages[0].TimeStamp);
    for (var i = 0; i < messages.length; i++) {
        var timeStampStr = messages[i].TimeStamp;
        var timeStamp = new Date(timeStampStr);
        if (this.InAVERange(dateCache, timeStamp)) {
            rangeArray.push(messages[i]);
        } else {
            averageArray.push(rangeArray);
            rangeArray = [];
        }
        dateCache = timeStamp;
        this.HandleMessage(messages[i]);
    }
    averageArray.push(rangeArray);
    this.HandleMonthMessage(averageArray);
    this.GridInit();
}
/**check if in average range**/
Irlovan.Control.PowerSimulation.prototype.InAVERange = function (dateCache, timeStamp) {
    var range = $("#" + this.AVERangeSelectID).val();
    var yearRange = (range == "Year");
    var monthRange = (range == "Month");
    var dayRange = (range == "Day");
    if (yearRange) { return (dateCache.getYear() == timeStamp.getYear()); }
    if (monthRange) { return (dateCache.getYear() == timeStamp.getYear()) && (dateCache.getMonth() == timeStamp.getMonth()); }
    if (dayRange) { return (dateCache.getYear() == timeStamp.getYear()) && (dateCache.getMonth() == timeStamp.getMonth()) && (dateCache.getDate() == timeStamp.getDate()); }
    return true;
}

Irlovan.Control.PowerSimulation.prototype.HandleMessage = function (matrixMessage, isAVE) {
    var labels = this.Config.labels.Value.split(",");
    for (var i = 0; i < labels.length; i++) {
        var value = matrixMessage.Message[labels[i]];
        if (value == null) { continue; }
        var groupID=(isAVE) ? (i + 1000) : i;
        this.Groups.update({ id: groupID, visible: true });
        var item = { x: matrixMessage.TimeStamp, y: value, group: groupID }
        var showLabelInfo = $("#" + this.LabelVisibleSelectID).val();
        var showLabel = (showLabelInfo == "True");
        if (showLabel) {
            item.label = { content: ("Time=" + matrixMessage.TimeStamp + ",Value=" + parseFloat(value).toFixed(2)), }
        }
        this.Items.push(item);
    }
}
Irlovan.Control.PowerSimulation.prototype.HandleMonthMessage = function (averageArray) {
    for (var i = 0; i < averageArray.length; i++) {
        this.CalculateAverage(averageArray[i]);
    }
}
Irlovan.Control.PowerSimulation.prototype.CalculateAverage = function (messageGroup) {
    var monthMessageStart = new Irlovan.Message.MatrixMessage();
    var monthMessageEnd = new Irlovan.Message.MatrixMessage();
    var count = messageGroup.length;
    var fistMatrixMessage = messageGroup[0];
    var lastMatrixMessage = messageGroup[count - 1];
    monthMessageStart.TimeStamp = fistMatrixMessage.TimeStamp
    monthMessageEnd.TimeStamp = lastMatrixMessage.TimeStamp
    var aveMessage = {};
    for (var item in fistMatrixMessage.Message) { aveMessage[item] = 0; }
    for (var i = 0; i < count; i++) {
        var message = messageGroup[i].Message;
        this.AddAVE(aveMessage, message, count);
    }
    monthMessageStart.Message = aveMessage;
    monthMessageEnd.Message = aveMessage;
    this.HandleMessage(monthMessageStart, true);
    this.HandleMessage(monthMessageEnd, true);
}
Irlovan.Control.PowerSimulation.prototype.AddAVE = function (aveMessage, currentMessage, count) {
    for (var item in aveMessage) {
        aveMessage[item] += (currentMessage[item] / count);
    }
}
Irlovan.Control.PowerSimulation.prototype.ToMonthData = function (date) {
    return ((date.getFullYear() + '-' + (date.getMonth() + 1)));
}

/*data select init*/
Irlovan.Control.PowerSimulation.prototype.DataSelectInit = function () {
    Irlovan.ControlHelper.ClearControl(this.DataSelectID);
    var labelInfo = this.Config.labels.Value;
    if (!labelInfo) { return; }
    var labels = labelInfo.split(",");
    for (var i = 0; i < labels.length; i++) {
        Irlovan.ControlHelper.CreateControlByStr(
        "<option value='" + labels[i] + "' selected='selected'>" + labels[i] +
        "</option>", this.DataSelectID, null);
    }
    $("#" + this.DataSelectID).on("multiselectclick", Irlovan.IrlovanHelper.Bind(this, function (event, ui) {
        /* event: the original event object ui.value: value of the checkbox ui.text: text of the checkbox ui.checked: whether or not the input was checked or unchecked (boolean) */
        var index = 0;
        var labels = this.Config.labels.Value.split(",");
        for (var i = 0; i < labels.length; i++) {
            if (labels[i] == ui.text) { index = i; }
        }
        var groupsData = this.Groups.get();
        // if visible, hide
        if (ui.checked == true) {
            this.Groups.update({ id: groupsData[index].id, visible: true });
        }
        else {
            this.Groups.update({ id: groupsData[index].id, visible: false });
        }
    }));
    $("#" + this.DataSelectID).on("multiselectcheckall", Irlovan.IrlovanHelper.Bind(this, function (event, ui) {
        var groupsData = this.Groups.get();
        for (var i = 0; i < groupsData.length; i++) {
            this.Groups.update({ id: groupsData[i].id, visible: true });
        }
    }));
    $("#" + this.DataSelectID).on("multiselectuncheckall", Irlovan.IrlovanHelper.Bind(this, function (event, ui) {
        var groupsData = this.Groups.get();
        for (var i = 0; i < groupsData.length; i++) {
            this.Groups.update({ id: groupsData[i].id, visible: false });
        }
    }));
}
Irlovan.Control.PowerSimulation.prototype.MultipleSelectInit = function () {
    $("#" + this.DataSelectID).multiselect();
}
Irlovan.Control.PowerSimulation.prototype.SelectAll = function () {
    if ((this.Config.iniSelected.Value == true) || (this.Config.iniSelected.Value == "true")) {
        $("#" + this.DataSelectID).multiselect("checkAll");
    } else {
        $("#" + this.DataSelectID).multiselect("uncheckAll");
    }
}
Irlovan.Control.PowerSimulation.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    this.LibDestroy();
}
Irlovan.Control.PowerSimulation.prototype.LibDestroy = function () {
    this.Items = [];
    if (this.Graph2d) {
        this.Graph2d.destroy();
        this.Graph2d = null;
    }
    $("#" + this.DataSelectID).off();
}
Irlovan.Control.PowerSimulation.prototype.SetRecorderList = function () {
    Irlovan.ControlHelper.ClearControl(this.RecorderSelectID);
    var recorderInfo = this.Config.recorderList.Value;
    if (!recorderInfo) { return; }
    var recorders = recorderInfo.split(",");
    for (var i = 0; i < recorders.length; i++) {
        Irlovan.ControlHelper.CreateControlByStr(
        "<option value='" + recorders[i] + "' selected='selected'>" + recorders[i] +
        "</option>", this.RecorderSelectID, null);
    }
}
Irlovan.Control.PowerSimulation.prototype.EditionFix = function () {
    if (!this.Config.recorderList) {
        this.Config.recorderList = { Attributes: "recorderList", Value: "" };
    }
}
