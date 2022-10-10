//Copyright(c) 2015,HIT All rights reserved.
//Des:CMD Handler
//Author:Irlovan   
//Date:2015-04-01
//modification :2015-04-01

/**Construction**/
Irlovan.Handler.CMDHandler = function (domain) {
    this.WebSocket;
    this.ServerID = "CMDHandler";
    this.Timeout = 10000;
    this.Domain = domain;
    this.StartWebsocket();
}

/**Start WebScocket**/
Irlovan.Handler.CMDHandler.prototype.StartWebsocket = function () {
    this.WebSocket = new Irlovan.Communication.Websocket(
          'ws://' + this.Domain + '/' + this.ServerID, '', null, null,
        Irlovan.IrlovanHelper.Bind(this, function (evt) {
            alert(evt.data);
        }),
        Irlovan.IrlovanHelper.Bind(this, function (evt) {
            this.Dispose();
            this.StartWebsocket();
        }));
}

/**RUN COMMAND**/
Irlovan.Handler.CMDHandler.prototype.Run = function (data) {
    this.WebSocket.Send(data);
}

/**Dispose**/
Irlovan.Handler.CMDHandler.prototype.Dispose = function () {
    if (this.WebSocket) {
        this.WebSocket.Close();
        this.WebSocket = null;
    }
}
