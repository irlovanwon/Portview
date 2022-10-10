//Copyright(c) 2015,HIT All rights reserved.
//Des:DatabaseMessage Handler for Local database
//Author:Irlovan   
//Date:2015-03-26

Irlovan.Include.Using("Script/Message/Message.js");

/**Construction**/
Irlovan.Handler.DatabaseMessage = function (session) {
    this.RootTag = "Database";
    this.ItemTag = "Item";
    this.Session = session;
    this.Callback = null;
}

/**Handle message from server**/
Irlovan.Handler.DatabaseMessage.prototype.Handle = function (xDocument) {
    var element = xDocument.documentElement;
    if (element.nodeName != this.RootTag) { return; }
    this.HandleDatabase(element);
    if (this.Callback) { this.Callback(); }
}

/**Subcribe for Local database**/
Irlovan.Handler.DatabaseMessage.prototype.Subcribe = function (callback) {
    this.Callback = callback;
    var doc = Irlovan.Lib.XML.CreateXDocument(this.RootTag);
    this.Session.Send(Irlovan.Lib.XML.ToString(doc));
}

/**Handle Database Init**/
Irlovan.Handler.DatabaseMessage.prototype.HandleDatabase = function (element) {
    var dataItem = element.getElementsByTagName(this.ItemTag);
    if ((dataItem == null) || (dataItem.length == 0)) { return; }
    var result = [];
    for (var i = 0; i < dataItem.length; i++) {
        result.push(Irlovan.Lib.XML.ToString(dataItem[i]));
    }
    Irlovan.Global.RealtimeDataList = result;
    Irlovan.Global.LoadBar.Off();
}

/**Dispose**/
Irlovan.Handler.DatabaseMessage.prototype.Dispose = function () { }


