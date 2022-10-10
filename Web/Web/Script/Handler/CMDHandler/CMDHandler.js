//Copyright(c) 2015,HIT All rights reserved.
//Des:CMDHandler
//Author:Irlovan   
//Date: 2015-08-24
//modification :

Irlovan.Include.Using("Script/Handler/CMDHandler/MSCMD.js");

/**Construction**/
Irlovan.Handler.CMDHandler = function (onInit) {
    this.ServerID = "CMDHandler";
    this.WebSocket;
    this.StartListen(onInit);
    this.MSCMD = new Irlovan.Handler.MSCMD(this.WebSocket);
}

/**Start Listen To Server**/
Irlovan.Handler.CMDHandler.prototype.StartListen = function (onInit) {
    if (this.WebSocket) { this.Dispose(); }
    this.WebSocket = new Irlovan.Communication.Websocket(
            'ws://' + Irlovan.Global.Domain_CMD + '/' + this.ServerID, '', null,
        function () {
            if (onInit) { onInit(); }
        },
        Irlovan.IrlovanHelper.Bind(this, function (evt) {
            this.Handle(evt.data);
        }), null);
}

/**Dispose**/
Irlovan.Handler.CMDHandler.prototype.Dispose = function () {
    if (this.WebSocket) {
        this.WebSocket.Close();
        this.WebSocket = null;
    }
}

/**Handle data from server**/
Irlovan.Handler.CMDHandler.prototype.Handle = function (data) {
    alert(data);
}











