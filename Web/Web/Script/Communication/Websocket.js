//Copyright(c) 2013,HIT All rights reserved.
//Des:Websocket Lib
//Author:Irlovan   
//Date:2013-03-28
//modification :

Irlovan.Communication.Websocket = function (uri, protocol, onclose, onopen, onmessage, onerror) {
    this.Socket;
    var support = "MozWebSocket" in window ? 'MozWebSocket' : ("WebSocket" in window ? 'WebSocket' : null);
    if (support != null) {
        //using native websocket
        if (protocol && protocol.length > 0) {
            this.Socket = new window[support](uri, protocol);
        }
        else {
            this.Socket = new window[support](uri);
        }  
        this.Socket.onmessage = onmessage;
        this.Socket.onopen = onopen;
        this.Socket.onclose = onclose;
        this.Socket.onerror = onerror;
        this.Send = function (message) { this.Socket.send(message); }
        this.Close = function () { this.Socket.close(); }
        return;
    }
    alert("Your browser cannot support WebSocket!");
}



















