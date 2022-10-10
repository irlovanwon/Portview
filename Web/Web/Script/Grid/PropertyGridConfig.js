//Copyright(c) 2013,HIT All rights reserved.
//Des:PropertyGrid.js
//Author:Irlovan   
//Date:2013-05-13
//modification :

Irlovan.RealtimeData.HistoryEventDataConfig = function (tableID, pagerID, title) {
    Irlovan.RealtimeData.EventDataConfig(tableID, pagerID, title,
        ['EventName', 'StartTime', 'EndTime', 'Description', "EventLevel", "Indication"],
        [
           { name: 'Name', index: 'EventName', key: true, sortable: true, width: 80 },
           { name: 'StartTime', index: 'StartTime', sortable: true, width: 80, editoptions: {} },
           { name: 'EndTime', index: 'EndTime', sortable: true, width: 80, editoptions: {} },
           { name: 'Description', index: 'Description', sortable: true, width: 80, editoptions: {} },
           { name: 'EventLevel', index: 'EventLevel', sortable: true, width: 80, editoptions: {} },
           { name: 'Indication', index: 'Indication', sortable: true, width: 80, editoptions: {} }
        ]
        )
}
Irlovan.RealtimeData.StatisticConfig = function (tableID, pagerID, title) {
    Irlovan.RealtimeData.EventDataConfig(tableID, pagerID, title,
        ['TimeStamp', 'DeviceID', 'm1', 'm2', 'm3', "m4", "m5"],
        [
           { name: 'TimeStamp', index: 'TimeStamp', key: true, sortable: true, width: 80 },
           { name: 'DeviceID', index: 'DeviceID', sortable: true, width: 80, editoptions: {} },
           { name: 'm1', index: 'm1', sortable: true, width: 80, editoptions: {} },
           { name: 'm2', index: 'm2', sortable: true, width: 80, editoptions: {} },
           { name: 'm3', index: 'm3', sortable: true, width: 80, editoptions: {} },
           { name: 'm4', index: 'm4', sortable: true, width: 80, editoptions: {} },
           { name: 'm5', index: 'm5', sortable: true, width: 80, editoptions: {} }
        ]
        )
}
Irlovan.RealtimeData.CCTStatisticConfig = function (tableID, pagerID, title) {
    Irlovan.RealtimeData.EventDataConfig(tableID, pagerID, title,
        ['TimeStamp', 'DeviceID', 'HoistElapsedTime', 'GantryElapsedTime', 'TrolleyElapsedTime', "BoomElapsedTime", "HoistTrolleyElapsedTime", "LockCount", "HoistBrakeCount", "GantryBrakeCount", "TrolleyBrakeCount", "BoomBrakeCount", "MasterElapsedTime", "HMSCount1", "HMSCount2", "GMSCount1", "GMSCount2"],
        [
           { name: 'TimeStamp', index: 'TimeStamp', key: true, sortable: true, width: 80 },
           { name: 'DeviceID', index: 'DeviceID', sortable: true, width: 80, editoptions: {} },
           { name: 'HoistElapsedTime', index: 'Meter_Online', sortable: true, width: 80, editoptions: {} },
           { name: 'GantryElapsedTime', index: 'Close_Status', sortable: true, width: 80, editoptions: {} },
           { name: 'TrolleyElapsedTime', index: 'Open_Status', sortable: true, width: 80, editoptions: {} },
           { name: 'BoomElapsedTime', index: 'Trip_Status', sortable: true, width: 80, editoptions: {} },
           { name: 'HoistTrolleyElapsedTime', index: 'Line_Voltage_L1_L2', sortable: true, width: 80, editoptions: {} },
           { name: 'LockCount', index: 'Line_Voltage_L2_L3', sortable: true, width: 80, editoptions: {} },
           { name: 'HoistBrakeCount', index: 'Line_Voltage_L3_L1', sortable: true, width: 80, editoptions: {} },
           { name: 'GantryBrakeCount', index: 'Ph_1_Voltage', sortable: true, width: 80, editoptions: {} },
           { name: 'TrolleyBrakeCount', index: 'Ph_2_Voltage', sortable: true, width: 80, editoptions: {} },
           { name: 'BoomBrakeCount', index: 'Ph_3_Voltage', sortable: true, width: 80, editoptions: {} },
           { name: 'MasterElapsedTime', index: 'Ph_1_Current', sortable: true, width: 80, editoptions: {} },
           { name: 'HMSCount1', index: 'Ph_2_Current', sortable: true, width: 80, editoptions: {} },
           { name: 'HMSCount2', index: 'Ph_3_Current', sortable: true, width: 80, editoptions: {} },
           { name: 'GMSCount1', index: '3_Ph_Active_Power', sortable: true, width: 80, editoptions: {} },
           { name: 'GMSCount2', index: '3_Ph_Reactive_Power', sortable: true, width: 80, editoptions: {} }
        ]
        )
}
Irlovan.RealtimeData.RealtimeEventDataConfig = function (tableID, pagerID, title) {
    Irlovan.RealtimeData.EventDataConfig(tableID, pagerID, title,
        ['Name', 'Description', 'StartTime', 'EndTime', 'EventLevel', 'Indication'],
        [
           { name: 'Name', index: 'Name', key: true, sortable: true, width: 80 },
           { name: 'Description', index: 'Description', sortable: true, width: 80, editoptions: {} },
           { name: 'StartTime', index: 'StartTime', sortable: true, width: 80, editoptions: {} },
           { name: 'EndTime', index: 'EndTime', sortable: true, width: 80, editoptions: {} },
           { name: 'EventLevel', index: 'EventLevel', sortable: true, width: 80, editoptions: {} },
           { name: 'Indication', index: 'Indication', sortable: true, width: 80, editoptions: {} }
        ]
        )
}
Irlovan.RealtimeData.RealtimeDataConfig = function (tableID, pagerID, title) {
    Irlovan.RealtimeData.EventDataConfig(tableID, pagerID, title,
        ['Name', 'Description', 'TimeStamp', "Value", "DataType"],
        [
           { name: 'Name', index: 'Name', key: true, sortable: true, width: 80 },
           { name: 'Description', index: 'Description', sortable: true, width: 80, editoptions: {} },
           { name: 'TimeStamp', index: 'TimeStamp', sortable: true, width: 80, editoptions: {} },
           { name: 'Value', index: 'Value', sortable: true, width: 80, editoptions: {} },
           { name: 'DataType', index: 'DataType', sortable: true, width: 80, editoptions: {} }
        ]
        )
}
Irlovan.RealtimeData.TrendDataGrid = function (tableID, pagerID, title, onSaveCell, onSelectCell, onSelectRow) {
    Irlovan.RealtimeData.TrendDataConfig(tableID, pagerID, title,
        ['Name', 'ID', "DataType", 'Description'],
        [
           { name: 'Name', index: 'Name', key: true, sortable: true, width: 80 },
           { name: 'ID', index: 'ID', sortable: true, width: 80, editoptions: {} },
           { name: 'DataType', index: 'DataType', sortable: true, width: 80, editoptions: {} },
           { name: 'Description', index: 'Description', sortable: true, width: 80, editoptions: {} }
        ], onSelectRow)
}
Irlovan.RealtimeData.UserManageGrid = function (tableID, pagerID, title, onSaveCell, onSelectCell, onSelectRow) {
    Irlovan.RealtimeData.TrendDataConfig(tableID, pagerID, title,
        ['Name', 'Password', 'Level'],
        [
           { name: 'Name', index: 'Name', key: true, sortable: true, width: 80 },
           { name: 'Password', index: 'Password', sortable: true, width: 80, editoptions: {} },
           { name: 'Level', index: 'Level', sortable: true, width: 80, editoptions: {} }
        ], onSelectRow)
}
Irlovan.RealtimeData.TrendDataConfig = function (tableID, pagerID, title, colNamesArray, colModelArray, onRowSelect) {
    //config
    jQuery('#' + tableID).jqGrid({
        height: '768',
        width: '1024',
        //forceFit: true,
        url: "clientArray",
        //datatype: "local",
        colNames: colNamesArray,
        colModel: colModelArray,
        pager: '#' + pagerID,
        rowNum: 20,
        rowList: [10, 20, 30],
        sortname: 'Name',
        multiselect: true,
        viewrecords: true,
        sortorder: 'desc',
        caption: title ? title : 'PropertyGrid',
        editurl: "clientArray",
        datatype: 'local',
        onSelectRow: function (rowid, status, e) {
            onRowSelect(rowid, status, e);
        }
    });
    jQuery('#' + tableID).jqGrid('navGrid', '#' + pagerID, { add: false, edit: false, del: false });
}
Irlovan.RealtimeData.EventDataConfig = function (tableID, pagerID, title, colNamesArray, colModelArray) {
    //config
    jQuery('#' + tableID).jqGrid({
        height: '768',
        width: '1024',
        //forceFit: true,
        url: "clientArray",
        //datatype: "local",
        colNames: colNamesArray,
        colModel: colModelArray,
        pager: '#' + pagerID,
        rowNum: 20,
        rowList: [10, 20, 30],
        sortname: 'Name',
        viewrecords: true,
        sortorder: 'desc',
        caption: title ? title : 'PropertyGrid',
        editurl: "clientArray",
        datatype: 'local',
        afterInsertRow: function (rowid, rowdata, rowelem) {
            var backgroundColor = null;
            var fontColor = null;
            for (var item in Irlovan.Global.EventLevel) {
                if (rowdata.EventLevel == item) {
                    backgroundColor = Irlovan.Global.EventLevel[item].split(',')[0];
                    fontColor = Irlovan.Global.EventLevel[item].split(',')[1];
                    break;
                }
            }
            if (rowdata.EndTime) {
                backgroundColor = Irlovan.Global.HistoryEventColor.split(',')[0];
                fontColor = Irlovan.Global.HistoryEventColor.split(',')[1];
            }
            if ((backgroundColor)) {
                $("#" + rowid).find("td").css("background", backgroundColor);
                $("#" + rowid).find("td").css("color", fontColor);
            }
        }
    });
    jQuery('#' + tableID).jqGrid('navGrid', '#' + pagerID, { add: false, edit: false, del: false });
}
Irlovan.RealtimeData.PropertyGridConfig = function (tableID, pagerID, titleName, onSaveCell, onSelectCell) {
    //config
    jQuery('#' + tableID).jqGrid({
        height: 'auto',
        width: 'auto',
        forceFit: true,
        url: null,
        datatype: "local",
        colNames: ['Attributes', 'Value', 'Description', 'Expression', 'Readonly'],
        colModel: [
           { name: 'Attributes', index: 'Attributes', key: true, sortable: true, width: 80, editable: true },
           { name: 'Value', index: 'Value', sortable: true, width: 80, editable: true, editoptions: {} },
           { name: 'Description', index: 'Description', sortable: true, width: 80, editable: true, editoptions: {} },
           { name: 'Expression', index: 'Expression', sortable: true, width: 80, editable: true, editoptions: {} },
           { name: 'Readonly', index: 'Readonly', sortable: true, width: 80, editable: true, editoptions: {} }
        ],
        pager: '#' + pagerID,
        rowNum: 65535,
        sortname: 'Attributes',
        viewrecords: true,
        cellEdit: true,
        cellsubmit: 'clientArray',
        sortorder: "desc",
        caption: titleName,
        editurl: null,
        //editurl: "clientArray",
        datatype: "local",
        loadonce: true,
        loadComplete: function () {
            jQuery('#' + tableID).trigger("reloadGrid");
        },
        afterSaveCell: Irlovan.IrlovanHelper.Bind(this, function (rowid, name, val, iRow, iCol) {
            onSaveCell(rowid, name, val);
        }),
        onCellSelect: Irlovan.IrlovanHelper.Bind(this, function (rowid, iCol, cellcontent, e) {
            var cm = jQuery("#" + tableID).jqGrid("getGridParam", "colModel");
            var colName = cm[iCol];
            if (colName.name == "Expression") {
                onSelectCell(rowid, colName.name, "");
            }
        }),
    })
    jQuery('#' + tableID).jqGrid('navGrid', '#' + pagerID, { add: false, edit: false, del: false });
}
