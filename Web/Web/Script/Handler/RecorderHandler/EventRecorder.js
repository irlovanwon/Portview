//Copyright(c) 2015,HIT All rights reserved.
//Des:Handler for recorder history data query
//Author:Irlovan   
//Date:2015-08-27

/**Construction**/
Irlovan.Handler.EventRecorder = function (session) {
    this.RootTag = "EventRecorderHandler";
    this.StartTimeAttr = "StartTime";
    this.EndTimeAttr = "EndTime";
    this.RecorderNameAttr = "Name";
    this.DataNameAttr = "DataName";
    this.EventLevelAttr = "EventLevel";
    this.CountAttr = "Count";
    this.IsDescAttr = "IsDesc";
    this.EventMessageTag = "EDataMessage";
    this.WebSocket = session;
    this.Callback;
}
/**Handle message from server**/
Irlovan.Handler.EventRecorder.prototype.Handle = function (xDocument) {
    var element = xDocument.documentElement;
    if (element.nodeName != this.RootTag) { return; }
    this.HandleEvents(element);
}

/**Handle events**/
Irlovan.Handler.EventRecorder.prototype.HandleEvents = function (element) {
    var eventMessage = element.getElementsByTagName(this.EventMessageTag);
    if ((!eventMessage)) { return; }
    var resultEvents = [];
    for (var i = 0; i < eventMessage.length; i++) {
        var message = new Irlovan.Message.EventMessage();
        message.ParseXElement(eventMessage[i]);
        resultEvents.push(message.ToArray());
    }
    if (this.Callback) { this.Callback(resultEvents); }
}

/**Subcribe datalist from server**/
Irlovan.Handler.EventRecorder.prototype.Subcribe = function (recorderName, startTime, endTime, eventLevel, dataName, count, isDesc, callback) {
    this.Callback = callback;
    if (this.WebSocket) { this.WebSocket.Send(this.CreateSubcribeMessage(recorderName, startTime, endTime, eventLevel, dataName, count, isDesc)); }
}

/**Create Subcribe Message**/
Irlovan.Handler.EventRecorder.prototype.CreateSubcribeMessage = function (recorderName, startTime, endTime, eventLevel, dataName, count, isDesc) {
    var doc = Irlovan.Lib.XML.CreateXDocument(this.RootTag);
    doc.documentElement.setAttribute(this.StartTimeAttr, startTime);
    doc.documentElement.setAttribute(this.EndTimeAttr, endTime);
    doc.documentElement.setAttribute(this.RecorderNameAttr, recorderName);
    doc.documentElement.setAttribute(this.IsDescAttr, isDesc);
    if (eventLevel) { doc.documentElement.setAttribute(this.EventLevelAttr, eventLevel); }
    if (dataName) { doc.documentElement.setAttribute(this.DataNameAttr, dataName); }
    doc.documentElement.setAttribute(this.CountAttr, count);
    return Irlovan.Lib.XML.ToString(doc);
}

/**Dispose**/
Irlovan.Handler.EventRecorder.prototype.Dispose = function () {
}