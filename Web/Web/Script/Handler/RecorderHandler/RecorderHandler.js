//Copyright(c) 2015,HIT All rights reserved.
//Des:RecorderHandler
//Author:Irlovan   
//Date:2015-08-27
//modification :

Irlovan.Include.Using("Script/Handler/RecorderHandler/EventRecorder.js");
Irlovan.Include.Using("Script/Handler/RecorderHandler/DataRecorder.js");
Irlovan.Include.Using("Script/Handler/RecorderHandler/VURRecorder.js");
Irlovan.Include.Using("Script/Handler/RecorderHandler/StatisticRecorder.js");
Irlovan.Include.Using("Script/Handler/RecorderHandler/MatrixRecorder.js");

/**Construction**/
Irlovan.Handler.RecorderHandler = function (onInit) {
    this.ServerID = "RecorderHandler";
    this.WebSocket;
    this.EventRecorder;
    this.StartListen(onInit);
    this.InitState = false;
    this.EventRecorder = new Irlovan.Handler.EventRecorder(this.WebSocket);
    this.DataRecorder = new Irlovan.Handler.DataRecorder(this.WebSocket);
    this.VURRecorder = new Irlovan.Handler.VURRecorder(this.WebSocket);
    this.StatisticRecorder = new Irlovan.Handler.StatisticRecorder(this.WebSocket);
    this.MatrixRecorder = new Irlovan.Handler.MatrixRecorder(this.WebSocket);
}

/**Start Listen To Server**/
Irlovan.Handler.RecorderHandler.prototype.StartListen = function (onInit) {
    if (this.WebSocket) { this.Dispose(); }
    this.WebSocket = new Irlovan.Communication.Websocket(
            'ws://' + Irlovan.Global.Domain_Recorder + '/' + this.ServerID, '', null,
        Irlovan.IrlovanHelper.Bind(this, function (evt) {
            if (onInit) { onInit(); }
            this.InitState = true;
        }),
         Irlovan.IrlovanHelper.Bind(this, function (evt) {
             this.Handle(evt.data);
         }), null);
}

/**Dispose**/
Irlovan.Handler.RecorderHandler.prototype.Dispose = function () {
    if (this.WebSocket) {
        this.WebSocket.Close();
        this.WebSocket = null;
    }
    if (this.EventRecorder) {
        this.EventRecorder.Dispose();
        this.EventRecorder = null;
    }
    if (this.DataRecorder) {
        this.DataRecorder.Dispose();
        this.DataRecorder = null;
    }
    if (this.VURRecorder) {
        this.VURRecorder.Dispose();
        this.VURRecorder = null;
    }
    if (this.StatisticRecorder) {
        this.StatisticRecorder.Dispose();
        this.StatisticRecorder = null;
    }
    if (this.MatrixRecorder) {
        this.MatrixRecorder.Dispose();
        this.MatrixRecorder = null;
    }
}

/**Handle data from server**/
Irlovan.Handler.RecorderHandler.prototype.Handle = function (data) {
    var doc = Irlovan.Lib.XML.ParseFromString(data);
    if (!doc) { return; }
    this.EventRecorder.Handle(doc);
    this.DataRecorder.Handle(doc);
    this.VURRecorder.Handle(doc);
    this.StatisticRecorder.Handle(doc);
    this.MatrixRecorder.Handle(doc);
}











