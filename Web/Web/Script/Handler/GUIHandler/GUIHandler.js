//Copyright(c) 2015,HIT All rights reserved.
//Des:GUIHandler
//Author:Irlovan   
//Date:2015-03-24

Irlovan.Include.Using("Script/Handler/GUIHandler/Loader.js");
Irlovan.Include.Using("Script/Handler/GUIHandler/Recorder.js");

/**Construction**/
Irlovan.Handler.GUIHandler = function (onInit) {
    this.ServerID = "GUIHandler";
    this.WebSocket;
    this.StartListen(onInit);
    this.Loader = new Irlovan.Handler.Loader(this.WebSocket);
    this.Recorder = new Irlovan.Handler.Recorder(this.WebSocket);
}

/**Start Listen To Server**/
Irlovan.Handler.GUIHandler.prototype.StartListen = function (onInit) {
    if (this.WebSocket) { this.Dispose(); }
    this.WebSocket = new Irlovan.Communication.Websocket(
            'ws://' + Irlovan.Global.Domain_GUI + '/' + this.ServerID, '',
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
Irlovan.Handler.GUIHandler.prototype.Dispose = function () {
    if (this.WebSocket) {
        this.WebSocket.Close();
        this.WebSocket = null;
    }
    this.Loader.Dispose();
    this.Recorder.Dispose();
}

/**Handle data from server**/
Irlovan.Handler.GUIHandler.prototype.Handle = function (data) {
    var doc = Irlovan.Lib.XML.ParseFromString(data);
    if (!doc) { return; }
    this.Loader.Handle(doc);
    this.Recorder.Handle(doc);
}










