//Copyright(c) 2013,HIT All rights reserved.
//Des:PropertyGrid
//Author:Irlovan   
//Date:2013-03-22
//modification :

Irlovan.Include.Using("Script/Grid/Lib/JSGrid/jquery.jqGrid.js");

//PropertyGrid
//gridID PropertyGrid's ID
//gridClass PropertyGrid's Class 
//tableID, pagerID the id inside the control (tableID is the config id of the grid)
Irlovan.PropertyGrid = function (containerID, gridID, tableID, pagerID, config, title, pos, onSaveCell, onSelectCell, onSelectRow) {
    this.ID = tableID;
    this.GridID = gridID;
    this.ContainerID = containerID;
    Irlovan.PropertyGrid.prototype.Init(containerID, gridID, tableID, pagerID, pos);
    config(tableID, pagerID, title, onSaveCell, onSelectCell, onSelectRow);
    jQuery('#' + this.ID).jqGrid('gridResize');
}
Irlovan.PropertyGrid.prototype.LoadData = function (array) {
    $('#' + this.ID).jqGrid('clearGridData');
    for (var i = 0; i <= array.length; i++) {
        jQuery('#' + this.ID).jqGrid('addRowData', i, array[i]);
    }
}
Irlovan.PropertyGrid.prototype.Pos = function () {
    return { Left: document.getElementById(this.GridID).style.left, Top: document.getElementById(this.GridID).style.top }
}
Irlovan.PropertyGrid.prototype.Init = function (containerID, gridID, tableID, pagerID, pos) {
    Irlovan.ControlHelper.DeleteControl(gridID);
    Irlovan.ControlHelper.CreateControlByStr(
        //'position: absolute; right: 10px; top: 10px;
    "<div id='" + gridID + "' style='z-index :1;position: relative; width:1px;left: 0px; top: 0px' >" +
    "<table id='" + tableID + "' />" +
    "<div id='" + pagerID + "' />" +
    "</div>", containerID, pos);
    document.getElementById(containerID).style.zIndex = 10000;
}
Irlovan.PropertyGrid.prototype.MakeDragable = function () {
    Irlovan.ControlHelper.MakeControlDragable(this.ContainerID);
}