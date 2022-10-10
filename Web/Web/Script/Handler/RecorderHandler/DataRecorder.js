//Copyright(c) 2015,HIT All rights reserved.
//Des:Handler for recorder history data query
//Author:Irlovan   
//Date:2015-08-27

/**Construction**/
Irlovan.Handler.DataRecorder = function (session) {
    this.RootTag = "DataRecorderHandler";
    this.StartTimeAttr = "StartTime";
    this.EndTimeAttr = "EndTime";
    this.RecorderNameAttr = "Name";
    this.DataNameAttr = "DataName";
    this.CountAttr = "Count";
    this.IsDescAttr = "IsDesc";
    this.DataMessageTag = "InDataMessage";
    this.WebSocket = session;
    this.Callback;
}
/**Handle message from server**/
Irlovan.Handler.DataRecorder.prototype.Handle = function (xDocument) {
    var element = xDocument.documentElement;
    if (element.nodeName != this.RootTag) { return; }
    this.HandleDatas(element);
}

/**Handle datas**/
Irlovan.Handler.DataRecorder.prototype.HandleDatas = function (element) {
    var dataMessage = element.getElementsByTagName(this.DataMessageTag);
    if ((!dataMessage)) { return; }
    var resultDatas = [];
    for (var i = 0; i < dataMessage.length; i++) {
        var message = new Irlovan.Message.IndustryDataMessage();
        message.ParseXElement(dataMessage[i]);
        resultDatas.push(message.ToArray());
    }
    if (this.Callback) { this.Callback(resultDatas); }
}

/**Subcribe datalist from server**/
Irlovan.Handler.DataRecorder.prototype.Subcribe = function (recorderName, startTime, endTime, dataName, count, isDesc, callback) {
    this.Callback = callback;
    if (this.WebSocket) { this.WebSocket.Send(this.CreateSubcribeMessage(recorderName, startTime, endTime, dataName, count, isDesc)); }
}

/**Create Subcribe Message**/
Irlovan.Handler.DataRecorder.prototype.CreateSubcribeMessage = function (recorderName, startTime, endTime, dataName, count, isDesc) {
    var doc = Irlovan.Lib.XML.CreateXDocument(this.RootTag);
    doc.documentElement.setAttribute(this.StartTimeAttr, startTime);
    doc.documentElement.setAttribute(this.EndTimeAttr, endTime);
    doc.documentElement.setAttribute(this.RecorderNameAttr, recorderName);
    doc.documentElement.setAttribute(this.IsDescAttr, isDesc);
    if (dataName) { doc.documentElement.setAttribute(this.DataNameAttr, dataName); }
    doc.documentElement.setAttribute(this.CountAttr, count);
    return Irlovan.Lib.XML.ToString(doc);
}

/**Dispose**/
Irlovan.Handler.DataRecorder.prototype.Dispose = function () {
}