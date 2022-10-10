//Copyright(c) 2013,HIT All rights reserved.
//Des:Login.js
//Author:Irlovan   
//Date:2013-11-16


Irlovan.Include.Using("Script/Communication/Websocket.js");
Irlovan.Include.Using("Styles/jquery-ui-dialog-noclose.css");

Irlovan.Login = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, onLayout, isLock) {
    Irlovan.Control.Classic.call(this, "Login", id, containerID, gridContainerID, controlClass, config ? config : ({
        tagName: { Attributes: "tagName", Value: "Login" },
        height: { Attributes: "height", Value: "200" },
        width: { Attributes: "width", Value: "500" },
        textWidth: { Attributes: "textWidth", Value: "200" },
        posFix: { Attributes: "posFix", Value: "200" },
        uri: { Attributes: "uri", Value: "" },
        level: { Attributes: "level", Value: "" }
    }), left, top, zIndex, onLayout, isLock);
    this.LoginID = this.ID + "_login";
    this.UserNameTextID = this.ID + "_userName";
    this.PasswordTextID = id + "_password";
    this.ButtonConfirmID = id + "_buttonConfirm";
    this.LoginContainerID = id + "_login_container";
    this.LoginSocket;
    this.Init(id, containerID);

}
Irlovan.Login.prototype = new Irlovan.Control();
Irlovan.Login.prototype.constructor = Irlovan.Login;
//init div element
Irlovan.Login.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.LoginContainerID + "' style='width:"+this.Config.width.Value+";height:"+this.Config.height.Value+";'>" +
       "<div id='" + this.LoginID + "' >" +
       "<p>" +
       "<label> UserName:</label>" +
       "<textarea id='" + this.UserNameTextID + "' rows='5' cols='7' wrap='no/off' style='left:" + parseInt(this.Config.posFix.Value) + "px;position:absolute;width:" + parseInt(this.Config.textWidth.Value) + "px;height:25px;'/>" +
       "</p>" +
       "<p>" +
       "<label> Password:</label>" +
       "<input id='" + this.PasswordTextID + "' type='password'  style='left:" + parseInt(this.Config.posFix.Value) + "px;position:absolute;width:" + parseInt(this.Config.textWidth.Value) + "px;height:25px;'/>" +
       "</p>" +
       "<button type='button' id='" + this.ButtonConfirmID + "' width='80px' height='20px'>" + Irlovan.Language.Submit + "</button>" +
       "</div>" +
       "</div>", this.ID, this.Pos);
    this.StartWebSocket();
    this.DialogInit("Login", Irlovan.IrlovanHelper.Bind(this, this.DialogCloseHandler), this.LoginContainerID);
    this.EventInit();
}
Irlovan.Login.prototype.DialogInit = function (title, onClose, id) {
    $("#" + this.LoginID).dialog({
        dialogClass: "no-close",
        width: "auto",
        height: "auto",
        title: title,
        //close: function (event, ui) {
        //    onClose();
        //},
        appendTo: "#" + id
    });
    $("#" + this.LoginID).dialog("option", "height", parseInt(this.Config.height.Value));
    $("#" + this.LoginID).dialog("option", "width", parseInt(this.Config.width.Value));
    $("#" + this.LoginID).dialog("option", "draggable", false);
    $("#" + this.LoginID).dialog({ position: { my: "left top", at: "left bottom",of: document.getElementById(this.LoginContainerID) } });
}
Irlovan.Login.prototype.DialogCloseHandler = function () {
    document.getElementById(this.UserNameTextID).value = "";
    document.getElementById(this.PasswordTextID).value = "";
}
Irlovan.Login.prototype.EventInit = function () {
    document.getElementById(this.ButtonConfirmID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        var userName = document.getElementById(this.UserNameTextID).value;
        var password = document.getElementById(this.PasswordTextID).value;
        if ((!userName) || (!password) || (!this.Config.level.Value)) { alert("Please enter user name , password and level"); }
        this.LoginSocket.Send(Irlovan.IrlovanHelper.JsonToString({ Login: { UserName: userName, Password: password, Level: this.Config.level.Value } }));
    }), false);
}
Irlovan.Login.prototype.StartWebSocket = function () {
    if (this.LoginSocket) { return; }
    this.LoginSocket = new Irlovan.Communication.Websocket(
      'ws://' + Irlovan.Global.Domain + '/LoginHandler', '', null, null,
       Irlovan.IrlovanHelper.Bind(this, function (evt) {
           this.Handler(evt.data);
       }), function () {
           alert("Can't connect to the server");
       });
}
Irlovan.Login.prototype.Handler = function (data) {
    var xdoc = Irlovan.IrlovanHelper.XDocumentFromString(data);
    if (xdoc) {
        var root = xdoc.documentElement;
        if (root.getAttribute("Name") == "Login") {
            this.HandleLogin(root.getAttribute("IsLogin"));
        }
        if (root.getAttribute("Name") == "Warn") {
            this.HandleWarn(root.getAttribute("Message"));
        }
    }
}
Irlovan.Login.prototype.HandleLogin = function (isLogin) {
    if (isLogin == "true") {
        //Irlovan.Event.Send(Irlovan.Global.BodyID, "OnLoad", this.Config.uri.Value);
        //Irlovan.Global.GUI.LoadGUI(this.Config.uri.Value);
    }
}
Irlovan.Login.prototype.HandleWarn = function (message) {
    alert(message);
}


Irlovan.Login.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    if (this.LoginSocket) {
        this.LoginSocket.Close();
    }
    this.LoginSocket = null;
}
Irlovan.Login.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "tagName":
            $('#' + this.LoginID).dialog("option", "title", data);
            break;
        case "height":
            $("#" + this.LoginID).dialog("option", "height", parseInt(data));
            document.getElementById(this.LoginContainerID).style.height = data + "px";
            break;
        case "width":
            $("#" + this.LoginID).dialog("option", "width", parseInt(data));
            document.getElementById(this.LoginContainerID).style.width = data + "px";
            break;
        case "textWidth":
            document.getElementById(this.UserNameTextID).style.width = data + "px";
            document.getElementById(this.PasswordTextID).style.width = data + "px";
        case "posFix":
            document.getElementById(this.UserNameTextID).style.left = data + "px";
            document.getElementById(this.PasswordTextID).style.left = data + "px";
            break;
        default:
            break;
    }
}