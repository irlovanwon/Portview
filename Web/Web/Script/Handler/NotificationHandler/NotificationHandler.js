//Copyright(c) 2013,HIT All rights reserved.
//Des:NotificationHandler
//Author:Irlovan   
//Date:2015-04-18
//modification :

Irlovan.Include.Using("Script/Handler/NotificationHandler/EventNotification.js");

/**Construction**/
Irlovan.Handler.NotificationHandler = function (onInit) {
    this.ServerID = "NotificationHandler";
    this.WebSocket;
    this.EventNotification;
    this.StartListen(onInit);
    this.InitState = false;
    this.EventNotification = new Irlovan.Handler.EventNotification(this.WebSocket);
}

/**Start Listen To Server**/
Irlovan.Handler.NotificationHandler.prototype.StartListen = function (onInit) {
    if (this.WebSocket) { this.Dispose(); }
    this.WebSocket = new Irlovan.Communication.Websocket(
            'ws://' + Irlovan.Global.Domain_Notification + '/' + this.ServerID, '', null,
        Irlovan.IrlovanHelper.Bind(this, function (evt) {
            if (onInit) { onInit(); }
            this.InitState = true;
        }),
         Irlovan.IrlovanHelper.Bind(this, function (evt) {
             this.Handle(evt.data);
         }), null);
}

/**Dispose**/
Irlovan.Handler.NotificationHandler.prototype.Dispose = function () {
    if (this.WebSocket) {
        this.WebSocket.Close();
        this.WebSocket = null;
    }
    if (this.EventNotification) {
        this.EventNotification.Dispose();
        this.EventNotification = null;
    }
}

/**Handle data from server**/
Irlovan.Handler.NotificationHandler.prototype.Handle = function (data) {
    var doc = Irlovan.Lib.XML.ParseFromString(data);
    if (!doc) { return; }
    this.EventNotification.Handle(doc);
}











