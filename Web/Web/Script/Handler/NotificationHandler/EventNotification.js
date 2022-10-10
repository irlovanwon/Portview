//Copyright(c) 2015,HIT All rights reserved.
//Des:Handler for Event notification
//Author:Irlovan   
//Date:2015-04-18

/**Construction**/
Irlovan.Handler.EventNotification = function (session) {
    this.RootTag = "EventNotificationHandler";
    this.NotificationIDAttr = "ID";
    this.RegisterTypeAttr = "Type";
    this.EventMessageTag = "EDataMessage";
    this.RegisterTypeBoth = "Both";
    this.RegisterTypeRealtime = "Realtime";
    this.RegisterTypeHistory = "History";
    this.WebSocket = session;
    this.RegisterCallback;
}
/**Handle message from server**/
Irlovan.Handler.EventNotification.prototype.Handle = function (xDocument) {
    var element = xDocument.documentElement;
    if (element.nodeName != this.RootTag) { return; }
    this.HandleRegister(element);
}

/**Handle register event**/
Irlovan.Handler.EventNotification.prototype.HandleRegister = function (element) {
    var eventMessage = element.getElementsByTagName(this.EventMessageTag);
    if (!eventMessage) { return; }
    var resultEvents = [];
    for (var i = 0; i < eventMessage.length; i++) {
        var message = new Irlovan.Message.EventMessage();
        message.ParseXElement(eventMessage[i]);
        resultEvents.push(message.ToArray());
    }
    if (this.RegisterCallback) { this.RegisterCallback(resultEvents); }
}

/**Subcribe datalist from server**/
Irlovan.Handler.EventNotification.prototype.Subcribe = function (notificationID, type, callback) {
    this.RegisterCallback = callback;
    if (this.WebSocket) { this.WebSocket.Send(this.CreateRegisterSubcribeMessage(notificationID, type)); }
}

/**Create Register Subcribe Message**/
Irlovan.Handler.EventNotification.prototype.CreateRegisterSubcribeMessage = function (notificationID, type) {
    var doc = Irlovan.Lib.XML.CreateXDocument(this.RootTag);
    doc.documentElement.setAttribute(this.NotificationIDAttr, notificationID);
    var resultType = this.SelectRegisterType(type);
    if (resultType == null) { return; }
    doc.documentElement.setAttribute(this.RegisterTypeAttr, resultType);
    return Irlovan.Lib.XML.ToString(doc);
}

/**Select Register Type**/
Irlovan.Handler.EventNotification.prototype.SelectRegisterType = function (type) {
    switch (type) {
        case this.RegisterTypeBoth:
            return this.RegisterTypeBoth;
        case this.RegisterTypeRealtime:
            return this.RegisterTypeRealtime;
        case this.RegisterTypeHistory:
            return this.RegisterTypeHistory;
        default:
            return null;
    }
}

/**Dispose**/
Irlovan.Handler.EventNotification.prototype.Dispose = function () {
}