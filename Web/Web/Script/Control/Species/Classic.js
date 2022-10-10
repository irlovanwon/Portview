//Copyright(c) 2013,HIT All rights reserved.
//Des:superclass of all control
//Author:Irlovan   
//Date:2013-03-22
//modification :

//Irlovan.Include.Using("Script/Control/Lib/raphael.js");
Irlovan.Include.Using("Script/Grid/Grid.js");

Irlovan.Control.Classic = function (name, id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    if (!this.CheckID(id)) { return; }
    this.FieldInit(name, id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock);
    this.InitConfig();
    this.PropertyInit(config, left, top, zIndex, isLock, (config && config.tooltip) ? config.tooltip.Value : null, (config && config.rotate) ? config.rotate.Value : null, (config && config.centerPos) ? config.centerPos.Value : null, (config && config.visible) ? config.visible.Value : null, (config && config.tag) ? config.tag.Value : null, (config && config.effect) ? config.effect.Value : null);
}
//Defines All Fields Here
Irlovan.Control.Classic.prototype.FieldInit = function (name, id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    this.Name = name;
    this.ContainerID = containerID;
    this.GridContainerID = gridContainerID;
    this.ID = id;
    this.Class = controlClass;
    this.ControlClass = controlClass;
    this.Config = config;
    this.Selected = false;
    this.MenuContainerID = id + "_menucontainer";
    this.CloseMenuTag = this.MenuContainerID + "_close";
    //datasource filter for expression
    this.DataSource = null;
    this.ActiveMenuTag = this.MenuContainerID + "_active";
    this.Grid;
    this.TableID = this.ID + "controlgrid_table";
    this.ExpressionList = [];
}
//Init Property
Irlovan.Control.Classic.prototype.PropertyInit = function (config, left, top, zIndex, isLock, tooltop, rotate, centerPos, visible, tag, effect) {
    this.Config.left = { Attributes: "left", Value: (left ? left : 100), Description: "The Control Pos:Left", Expression: (((config.left) && (config.left.Expression)) ? config.left.Expression : "") };
    this.Config.top = { Attributes: "top", Value: (top ? top : 200), Description: "The Control Pos:Top", Expression: (((config.top) && (config.top.Expression)) ? config.top.Expression : "") };
    this.Config.tooltip = { Attributes: "tooltip", Value: (tooltop ? tooltop : ""), Description: "Show when mouse on control", Expression: (((config.tooltip) && (config.tooltip.Expression)) ? config.tooltip.Expression : "") };
    this.Config.rotate = { Attributes: "rotate", Value: (rotate ? rotate : 0), Description: "rotate the control", Expression: (((config.rotate) && (config.rotate.Expression)) ? config.rotate.Expression : "") };
    this.Config.centerPos = { Attributes: "centerPos", Value: (centerPos ? centerPos : "0%,0%"), Description: "rotate the control with a center", Expression: (((config.centerPos) && (config.centerPos.Expression)) ? config.centerPos.Expression : "") };
    this.Config.visible = { Attributes: "visible", Value: (visible ? visible : true), Description: "a property shows if the control is visible", Expression: (((config.visible) && (config.visible.Expression)) ? config.visible.Expression : "") };
    this.Config.tag = { Attributes: "tag", Value: (tag ? tag : ""), Description: "", Expression: (((config.tag) && (config.tag.Expression)) ? config.tag.Expression : "") };
    this.Config.effect = { Attributes: "effect", Value: (effect ? effect : ""), Description: "", Expression: (((config.effect) && (config.effect.Expression)) ? config.effect.Expression : "") };
    this.Config.dataSource = { Attributes: "dataSource", Value: (((config.dataSource) && (config.dataSource.Value)) ? config.dataSource.Value : ""), Description: "the expression source", Expression: (((config.dataSource) && (config.dataSource.Expression)) ? config.dataSource.Expression : "") };
    this.Config.zIndex = { Attributes: "zIndex", Value: (zIndex ? zIndex : "auto"), Description: "The Control ZIndex", Expression: (((config.zIndex) && (config.zIndex.Expression)) ? config.zIndex.Expression : "") };
    this.Config.id = { Attributes: "id", Value: this.ID, Description: "The Control Id", Expression: (((config.id) && (config.id.Expression)) ? config.id.Expression : "") };
    this.Config.isLock = { Attributes: "isLock", Value: (isLock ? isLock : false), Description: "Show if the control is Draggable", Expression: (((config.isLock) && (config.isLock.Expression)) ? config.isLock.Expression : "") };
}
//Set Value of The Property
Irlovan.Control.Classic.prototype.SetValue = function (name, colName, data) {
    switch (name) {
        case "left":
            document.getElementById(this.ID).style.left = data;
            break;
        case "top":
            document.getElementById(this.ID).style.top = data;
            break;
        case "tooltip":
            this.TooltipInit(data);
            break;
        case "rotate":
            this.Rotate(data, this.Config.centerPos.Value);
            break;
        case "centerPos":
            this.Rotate(this.Config.rotate.Value, data);
            break;
        case "visible":
            this.Visible(data);
            break;
        case "tag":
            this.Tag(data);
            break;
        case "effect":
            this.Effect(data);
            break;
        case "zIndex":
            document.getElementById(this.ID).style.zIndex = data;
            break;
        case "isLock":
            this.LockMenu(data);
            break;
        case "dataSource":
            this.FilterSourceInit(data);
            break;
        case "id":
            this.SetID(name, colName, data);
            this.IDInit(data);
            break;
        default:
            break;
    }
    this.GridDataChange(name, colName, data);
}
//Communication Error Indication
Irlovan.Control.Classic.prototype.Off = function (name) {
    //document.getElementById(this.ID).style.opacity = "0.8";
}
Irlovan.Control.Classic.prototype.On = function (name) {
    //document.getElementById(this.ID).style.opacity = "1";
}

//get the value of the attr
Irlovan.Control.Classic.prototype.GetValue = function (name) {
    return eval("this.Config." + name + ".Value");
}

//hight light fault expression
Irlovan.Control.Classic.prototype.FaultExpressionHighlight = function () {
    if (!Irlovan.Global.Layout) { return; }
    var control = document.getElementById(this.ID);
    control.style.backgroundColor = "red";
    control.style.border = "5px solid red";
}
//Dispose
Irlovan.Control.Classic.prototype.Dispose = function () {
    $("#" + this.ID).tooltip("destroy");
    Irlovan.ControlHelper.DeleteControl(this.ID);
}
Irlovan.Control.Classic.prototype.Init = function (id, containerID) {
    if (!id) { return; }
    this.GetExpressionList();
    var control = this.CreateControl(this.Config, { left: this.Config.left.Value, top: this.Config.top.Value }, this.Config.zIndex.Value, id, containerID, this.MenuContainerID, this.CloseMenuTag, this.ActiveMenuTag);
    this.ClickEventInit(control);
    this.LayoutInit();
    this.LockStateInit();
    this.TooltipInit(this.Config.tooltip.Value);
    //visible init
    this.Visible(this.Config.visible.Value);
    //rotate init
    this.Rotate(this.Config.rotate.Value, this.Config.centerPos.Value);
    //dataSourceFilter
    this.FilterSourceInit(this.Config.dataSource.Value);
}


//Init All Config Here
Irlovan.Control.Classic.prototype.InitConfig = function () {
    if (!this.Config) { this.Config = {}; }
}
//Check ID 
Irlovan.Control.Classic.prototype.CheckID = function (id) {
    if (!id) { return false; }
    if (!document.getElementById(id)) { return true; }
    alert("id :" + id + " has been used,check the file");
    return false;
}
//Create Control
Irlovan.Control.Classic.prototype.CreateControl = function (config, pos, zIndex, id, containerID, menuContainerID, closeMenuTag, activeMenuTag) {
    Irlovan.ControlHelper.DeleteControl(id);
    new Irlovan.HtmlFramework.ControlFrame(id, containerID, pos, menuContainerID, closeMenuTag, activeMenuTag);
    var control = document.getElementById(id);
    //to load the new pos
    control.style.position = "absolute";
    control.style.left = pos.left;
    control.style.top = pos.top;
    control.style.zIndex = zIndex;
    return document.getElementById(id);
}
//Layout Event Init
Irlovan.Control.Classic.prototype.LayoutInit = function () {
    if (!Irlovan.Global.Layout) { return; }
    document.getElementById(this.ID).addEventListener("dblclick", Irlovan.IrlovanHelper.Bind(this, function (event) {
        document.getElementById(this.MenuContainerID).style.visibility = "visible";
    }), false);
    document.getElementById(this.CloseMenuTag).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        for (var i = 0; i < Irlovan.Global.ControlList.length; i++) {
            if (Irlovan.Global.ControlList[i].ID != this.ID) { continue; }
            Irlovan.Global.ControlList[i].Dispose();
            Irlovan.Global.ControlList[i] = null;
            Irlovan.Global.ControlList.splice(i, 1);
        }
    }), false);
    document.getElementById(this.ActiveMenuTag).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        this.Activation();
    }), false);
    document.getElementById(this.ID).addEventListener("click", Irlovan.IrlovanHelper.Bind(Irlovan.Global.Layout, Irlovan.Global.Layout.Layout), false);
}
//ClickEventInit
Irlovan.Control.Classic.prototype.ClickEventInit = function (control) {
    control.addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (e) {
        var item = document.getElementById(this.ID);
        if (!item) { return; }
        this.SetValue("left", "Value", item.style.left);
        this.SetValue("top", "Value", item.style.top);
        //Create PropertyGrid
        this.CreateGrid();
    }), false);
}
//Create PropertyGrid
Irlovan.Control.Classic.prototype.CreateGrid = function () {
    if (document.getElementById(this.GridContainerID)) {
        var pos = this.Grid ? this.Grid.Pos() : null;
        Irlovan.ControlHelper.ClearControl(this.GridContainerID);
        this.Grid = new Irlovan.PropertyGrid(Irlovan.Global.PropertyGridContainerID, this.GridContainerID, this.TableID, 'controlgrid_pager', Irlovan.RealtimeData.PropertyGridConfig, "PropertyGrid", pos, Irlovan.IrlovanHelper.Bind(this, this.WriteGridHandler), Irlovan.IrlovanHelper.Bind(this, this.SelectGridHandler));
        this.Grid.MakeDragable();
        this.Grid.LoadData(Irlovan.Control.Classic.prototype.GetGridData(this.Config));
    }
}
Irlovan.Control.Classic.prototype.SelectGridHandler = function (name, colName, data) {
    new Irlovan.Expression.ExpressionEditor(Irlovan.Global.ExpressionEditorID, Irlovan.Global.BodyID, this.Config.left.Value, this.Config.top.Value, $('#' + this.TableID).jqGrid('getCell', name, colName), Irlovan.IrlovanHelper.Bind(this, function (e) {
        var indexer = $("#" + this.Grid.ID).jqGrid('getCol', 'Attributes', false);
        this.SetValue(indexer[name], colName, Irlovan.IrlovanHelper.ErrorXMLDetect(e));
        this.CreateGrid();
        Irlovan.Global.MessageHandler.DataMessage.Subcribe(Irlovan.Global.ControlList);
    }), this.Config.dataSource.Value);
}
Irlovan.Control.Classic.prototype.WriteGridHandler = function (name, colName, data) {
    switch (colName) {
        case "Attributes":
            return;
        default:
            break;
    }
    var result = Irlovan.IrlovanHelper.ErrorXMLDetect(data);
    var indexer = $("#" + this.Grid.ID).jqGrid('getCol', 'Attributes', false);
    this.SetValue(indexer[name], colName, result);
}
//get data grid need
Irlovan.Control.Classic.prototype.GetGridData = function (config) {
    var configArray = [];
    for (var key in config) {
        configArray.push(config[key]);
    }
    return configArray;
}
//Init LockState
Irlovan.Control.Classic.prototype.LockStateInit = function () {
    this.SetValue("isLock", "Value", this.Config.isLock.Value);
}
//Init Tooltip
Irlovan.Control.Classic.prototype.TooltipInit = function (data) {
    $("#" + this.ID).tooltip({ disabled: false });
    //tooltip
    if (!data) {
        $("#" + this.ID).tooltip("option", "disabled", true);
    } else {
        $("#" + this.ID).tooltip({
            items: 'div',
            content: data,
            open: function (event, ui) {
                ui.tooltip.css("max-width", "2000px");
                setTimeout(function () {
                    $(ui.tooltip).hide('explode');
                }, 5000);
            }
        });
    }
}
//init rotate states
Irlovan.Control.Classic.prototype.Rotate = function (angle, center) {
    var centerPos = center.split(",");
    $("#" + this.ID).rotate({
        angle: parseFloat(angle),
        center: [centerPos[0], centerPos[1]],
        animateTo: parseFloat(angle)
    });
}
//visible state set
Irlovan.Control.Classic.prototype.Visible = function (visible) {
    Irlovan.ControlHelper.ControlVisible(this.ID, visible);
}
//Init Filter Source
Irlovan.Control.Classic.prototype.FilterSourceInit = function (dataSource) {
    this.DataSource = dataSource;
}
//Tag For HIT HV LV Can be deleted out of hit
Irlovan.Control.Classic.prototype.Tag = function (idList) {
    var info = this.Config.tag.Value.split(";");
    var unbindResult;
    if (info.length == 2) {
        var eventInfo = info[0].split(",");
        if (eventInfo.length == 2) {
            unbindResult = (eventInfo[0] == eventInfo[1]) ? eventInfo[0] : (eventInfo[0] + ' ' + eventInfo[1]);
        }
    }
    if (unbindResult) {
        $('#' + this.ID).unbind(unbindResult);
    }
    if ((!idList) || (idList.split(";").length != 2)) { return; }
    Irlovan.ControlHelper.MouseHover(this.ID, idList);
}
//Effect
Irlovan.Control.Classic.prototype.Effect = function (data) {
    new Irlovan.Control.Effect(data, this.ID);
}
//Set Lock Menu for control
Irlovan.Control.Classic.prototype.LockMenu = function (data) {
    if (!Irlovan.Global.Layout) { Irlovan.ControlHelper.MakeControlUnDragable(this.ID); return; }
    if ((data == "true") || (data == true)) {
        this.DeActive();
    } else {
        this.Active();
    }
}
//Active for Lock Munu
Irlovan.Control.Classic.prototype.Active = function (id) {
    Irlovan.ControlHelper.MakeControlDragable(this.ID);
    document.getElementById(this.MenuContainerID).style.visibility = "visible";
    document.getElementById(this.ActiveMenuTag).style.backgroundImage = "url('Images/Tag/Active_lit.png')";
}
//DeActive for Lock Munu
Irlovan.Control.Classic.prototype.DeActive = function (id) {
    Irlovan.ControlHelper.MakeControlUnDragable(this.ID);
    document.getElementById(this.ActiveMenuTag).style.backgroundImage = "url('Images/Tag/DeActive_lit.png')";
    document.getElementById(this.MenuContainerID).style.visibility = "hidden";
}
//Activation for Lock Menu
Irlovan.Control.Classic.prototype.Activation = function () {
    var mode = document.getElementById(this.ActiveMenuTag).style.backgroundImage;
    var isLock = (mode.indexOf("DeActive_lit.png") !== -1) ? false : true;
    this.SetValue("isLock", "Value", isLock);
}
//Set ID for control
Irlovan.Control.Classic.prototype.SetID = function (name, colName, data) {
    if (document.getElementById(data)) {
        alert("the id is already exited");
        this.GridDataChange(name, colName, this.ID);
        return;
    }
    var element = document.getElementById(this.ID);
    var originID = element.id;
    element.id = data;
    this.ID = data;
    this.SetChildID(originID, element.id, element);
}
//Set Child ID for control
Irlovan.Control.Classic.prototype.SetChildID = function (originID, id, element) {
    for (var i = 0; i < element.childNodes.length; i++) {
        var child = element.childNodes[i];
        if (child.id && (child.id.indexOf(originID) != -1)) {
            child.id = child.id.replace(originID, id);
        }
        this.SetChildID(originID, id, child);
    }
}
Irlovan.Control.Classic.prototype.GridDataChange = function (name, colName, data) {
    //Event trigger for datachange
    if (document.getElementById(this.GridContainerID) && this.Grid) {
        this.GridDataChangeHandler(name, colName, data);
    }
    eval("this.Config." + name + "." + colName + "='" + data + "'");
    if (colName == "Expression") {
        Irlovan.Global.MessageHandler.DataMessage.Subcribe(Irlovan.Global.ControlList);
        this.GetExpressionList();
    }
}
Irlovan.Control.Classic.prototype.GetExpressionList = function () {
    this.ExpressionList = [];
    for (var item in this.Config) {
        if (eval("this.Config." + item + ".Expression" + "!= null") && (eval("this.Config." + item + ".Expression" + "!= ''"))) {
            this.ExpressionList.push(eval("this.Config." + item));
        }
    }
}
Irlovan.Control.Classic.prototype.GridDataChangeHandler = function (name, colName, data) {
    //***here because of a bug unsolved
    if ($("#" + this.Grid.ID).jqGrid('getInd', name, false) == false) {
        var indexer = $("#" + this.Grid.ID).jqGrid('getCol', 'Attributes', false);
        for (var i = 0; i < indexer.length; i++) {
            if (indexer[i] == name) {
                $("#" + this.Grid.ID).jqGrid('setCell', i, colName, data);
                break;
            }
        }
    } else {
        $("#" + this.Grid.ID).jqGrid('setCell', name, colName, data);
    }
}
//Init ID for ID Change
Irlovan.Control.Classic.prototype.IDInit = function (id) {
    this.ID = id;
    this.MenuContainerID = id + "_menucontainer";
    this.CloseMenuTag = this.MenuContainerID + "_close";
    this.ActiveMenuTag = this.MenuContainerID + "_active";
}
