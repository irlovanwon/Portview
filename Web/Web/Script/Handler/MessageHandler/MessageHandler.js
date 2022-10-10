//Copyright(c) 2013,HIT All rights reserved.
//Des:MessageHandler
//Author:Irlovan   
//Date:2013-05-27
//modification :2014-07-21
//modification :2015-03-19 Modify All Logic!!!

Irlovan.Include.Using("Script/Handler/MessageHandler/DataMessage.js");
Irlovan.Include.Using("Script/Handler/MessageHandler/DatabaseMessage.js");

/**Construction**/
Irlovan.Handler.MessageHandler = function (onInit) {
    this.ServerID = "MessageHandler";
    this.WebSocket;
    this.StartListen(onInit);
    this.DataMessage = new Irlovan.Handler.DataMessage(this.WebSocket);
    this.DatabaseMessage = new Irlovan.Handler.DatabaseMessage(this.WebSocket);
}

/**Start Listen To Server**/
Irlovan.Handler.MessageHandler.prototype.StartListen = function (onInit) {
    if (this.WebSocket) { this.Dispose(); }
    this.WebSocket = new Irlovan.Communication.Websocket(
            'ws://' + Irlovan.Global.Domain_Message + '/' + this.ServerID, '',
        Irlovan.IrlovanHelper.Bind(this, function (evt) {
            Irlovan.Global.LoadBar.On();
        }),
        function () {
            if (onInit) { onInit(); }
        },
         Irlovan.IrlovanHelper.Bind(this, function (evt) {
             this.Handle(evt.data);
         }),
         Irlovan.IrlovanHelper.Bind(this, function (evt) {
             Irlovan.Global.LoadBar.On();
         }));
}

/**Dispose**/
Irlovan.Handler.MessageHandler.prototype.Dispose = function () {
    if (this.WebSocket) {
        this.WebSocket.Close();
        this.WebSocket = null;
    }
    this.DataMessage.Dispose();
}

/**Handle data from server**/
Irlovan.Handler.MessageHandler.prototype.Handle = function (data) {
    var doc = Irlovan.Lib.XML.ParseFromString(data);
    if (!doc) { return; }
    this.DataMessage.Handle(doc);
    this.DatabaseMessage.Handle(doc);
}











