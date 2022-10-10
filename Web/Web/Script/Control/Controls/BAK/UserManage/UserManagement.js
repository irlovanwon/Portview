//Copyright(c) 2013,HIT All rights reserved.
//Des:UserManagement.js
//Author:Irlovan   
//Date:2013-11-16


Irlovan.Include.Using("Script/Communication/Websocket.js");

Irlovan.UserManagement = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, onLayout, isLock) {
    Irlovan.Control.Classic.call(this, "UserManagement", id, containerID, gridContainerID, controlClass, config ? config : ({
        tagName: { Attributes: "tagName", Value: "UserManagement" },
        height: { Attributes: "height", Value: "768px" },
        width: { Attributes: "width", Value: "1024px" },
        dataSource: { Attributes: "dataSource", Value: "IRLOVAN-PC\\IRLOVAN" },
        database: { Attributes: "database", Value: "Portview" },
    }), left, top, zIndex, onLayout, isLock);
    this.UsermanageGridID = this.ID + "_usermanage_grid";
    this.UserManageID = this.ID + "_container";
    this.ButtonAddID = id + "_userAdd";
    this.ButtonRemoveID = id + "_userRemove";
    this.UserManageSocket;
    this.SQLPassword = "";
    this.SQLDatasource = this.Config.dataSource.Value;
    this.SQLDatabase = this.Config.database.Value;
    this.SQLUserID = "";
    this.UsermanageGrid;
    this.Init(id, containerID);
    this.StartWebSocket();
}
Irlovan.UserManagement.prototype = new Irlovan.Control();
Irlovan.UserManagement.prototype.constructor = Irlovan.UserManagement;
//init div element
Irlovan.UserManagement.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.UserManageID + "' style='position:relative;width:" + this.Config.width.Value + ";height:" + this.Config.height.Value + ";'>" +
       "<p>" +
       "<input value='Add' style='width:120px;height:30px;' type='button' id='" + this.ButtonAddID + "' />" +
       "<input value='Remove' style='width:120px;height:30px;' type='button' id='" + this.ButtonRemoveID + "' />" +
       "</p>" +
       "<div id='" + this.UsermanageGridID + "' style='width:" + this.Config.width.Value + "px;height:" + this.Config.height.Value + "px;'/>" +
       "</div>", this.ID, this.Pos);
    this.PropertyGridInit();
    this.EventInit();
}
Irlovan.UserManagement.prototype.PropertyGridInit = function () {
    if (!this.UsermanageGrid) {
        this.UsermanageGrid = new Irlovan.PropertyGrid(this.UserManageID, this.UsermanageGridID, this.UsermanageGridID + "_PropertyGrid_Table", this.UsermanageGridID + "_PropertyGrid_Page", Irlovan.IrlovanHelper.Bind(this, Irlovan.RealtimeData.UserManageGrid), "UserManage", this.Pos, null, null, Irlovan.IrlovanHelper.Bind(this, this.VariableSelectHandler));
        $('#' + this.UsermanageGrid.ID).jqGrid('setGridWidth', parseFloat(this.Config.width.Value));
        $('#' + this.UsermanageGrid.ID).jqGrid('setGridHeight', parseFloat(this.Config.height.Value));
        $('#' + this.UsermanageGrid.ID).jqGrid('setCaption', this.Config.tagName.Value);
    }
}
Irlovan.UserManagement.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    if (this.UserManageSocket) {
        this.UserManageSocket.Close();
    }
    this.UserManageSocket = null;
}
Irlovan.UserManagement.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "tagName":
            $('#' + this.UsermanageGrid.ID).jqGrid('setCaption', this.Config.tagName.Value);
            break;
        case "height":
            document.getElementById(this.UserManageID).style.height = data;
            $('#' + this.UsermanageGrid.ID).jqGrid('setGridHeight', parseFloat(data));
            break;
        case "width":
            document.getElementById(this.UserManageID).style.width = data;
            $('#' + this.UsermanageGrid.ID).jqGrid('setGridWidth', parseFloat(data));
            break;
        case "dataSource":
            this.SQLDatasource = this.Config.dataSource.Value;
            break;
        case "database":
            this.SQLDatabase = this.Config.database.Value;
            break;
        default:
            break;
    }
}
Irlovan.UserManagement.prototype.EventInit = function () {
    //delete row
    document.getElementById(this.ButtonRemoveID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        var selected = jQuery('#' + this.UsermanageGrid.ID).jqGrid('getGridParam', 'selarrrow');
        if (selected.length == 0) { return; }
        var deleteArray = [];
        for (var i = 0; i < selected.length; i++) {
            deleteArray.push(selected[i]);
        }
        this.UserManageSocket.Send(Irlovan.IrlovanHelper.JsonToString(this.RequestMessage({ Delete: deleteArray })));
    }), false);
    //add row
    document.getElementById(this.ButtonAddID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        var userName = prompt("Select User Name:");
        var password = prompt("Set Password:");
        var level = prompt("Set Level:");
        if ((!userName) || (!password) || (!level)) { alert("UserName , password , level can't be null"); return; }
        var rows = jQuery('#' + this.UsermanageGrid.ID).jqGrid('getRowData');
        for (var i = 0; i < rows.length; i++) {
            if (rows[i]["Name"] == name) {
                alert("name already existed");
                return;
            }
        }
        var addArray = { UserName: userName, Password: password ,Level:level};
        this.UserManageSocket.Send(Irlovan.IrlovanHelper.JsonToString(this.RequestMessage({ Add: addArray })));
    }), false);
}
Irlovan.UserManagement.prototype.RequestMessage = function (data) {
    var result = {};
    result.UpdateUserInfo = { SQLDataSource: this.SQLDatasource, SQLDatabase: this.SQLDatabase, SQLUserID: this.SQLUserID, SQLPassword: this.SQLPassword };
    var rows = jQuery('#' + this.UsermanageGrid.ID).jqGrid('getRowData');
    result.UpdateUserInfo.Data = data;
    return result;
}
Irlovan.UserManagement.prototype.Login = function () {
    this.SQLUserID = prompt("UserID:");
    this.SQLPassword = prompt("Password:");
    if ((this.SQLDatasource) && (this.SQLDatabase) && (this.SQLPassword)) {
        this.UserManageSocket.Send(Irlovan.IrlovanHelper.JsonToString({ Login: { SQLDataSource: this.SQLDatasource, SQLDatabase: this.SQLDatabase, SQLUserID: this.SQLUserID, SQLPassword: this.SQLPassword } }));
    }
}
Irlovan.UserManagement.prototype.StartWebSocket = function () {
    if (this.UserManageSocket) { return; }
    this.UserManageSocket = new Irlovan.Communication.Websocket(
      'ws://' + Irlovan.Global.Domain + '/UserManagerHandler', '', null, Irlovan.IrlovanHelper.Bind(this, function (evt) {
          this.Login();
      }),
       Irlovan.IrlovanHelper.Bind(this, function (evt) {
           this.Handler(evt.data);
       }), function () {
           alert("Can't connect to the server");
       });
}
Irlovan.UserManagement.prototype.Handler = function (data) {
    var xdoc = Irlovan.IrlovanHelper.XDocumentFromString(data);
    if (xdoc) {
        var result = {};
        var root = xdoc.documentElement;
        if (root.getAttribute("Name") == "Login") {
            this.HandleLogin(root.getAttribute("IsLogin"));
        }
        if (root.getAttribute("Name") == "Warn") {
            this.HandleWarn(root.getAttribute("Message"));
        }
        if (root.getAttribute("Name") == "Refresh") {
            this.HandleRefresh(root);
        }
    }
}
Irlovan.UserManagement.prototype.HandleLogin = function (isLogin) {
}
Irlovan.UserManagement.prototype.VariableSelectHandler = function (rowid, status, e) {

}
Irlovan.UserManagement.prototype.HandleWarn = function (message) {
    alert(message);
}
Irlovan.UserManagement.prototype.HandleRefresh = function (element) {
    jQuery('#' + this.UsermanageGrid.ID).jqGrid('clearGridData');
    var elements = element.getElementsByTagName("User");
    for (var i = 0; i < elements.length; i++) {
        jQuery('#' + this.UsermanageGrid.ID).jqGrid('addRowData', name, { Name: elements[i].getAttribute("Name"), Password: elements[i].getAttribute("Password"), Level: elements[i].getAttribute("Level") });
    }
    $('#' + this.UsermanageGrid.ID).trigger('reloadGrid');
}