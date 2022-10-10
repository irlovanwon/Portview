//Copyright(c) 2015,HIT All rights reserved.
//Des:Handler for recorder statistic data query
//Author:Irlovan   
//Date:2015-09-16

/**Construction**/
Irlovan.Handler.StatisticRecorder = function (session) {
    this.RootTag = "StatisticRecorderHandler";
    this.TimeStampAttr = "TimeStamp";
    this.RecorderNameAttr = "Name";
    this.MatrixTag = "MatrixArray";
    this.MatrixRowTag = "MatrixRow";
    this.WebSocket = session;
    this.Callback;
}
/**Handle message from server**/
Irlovan.Handler.StatisticRecorder.prototype.Handle = function (xDocument) {
    var element = xDocument.documentElement;
    if (element.nodeName != this.RootTag) { return; }
    this.HandleData(element);
}

/**Handle events**/
Irlovan.Handler.StatisticRecorder.prototype.HandleData = function (element) {
    var matrixData = element.getElementsByTagName(this.MatrixTag);
    if ((!matrixData) || (matrixData.length == 0)) { return; }
    var rowData = matrixData[0].getElementsByTagName(this.MatrixRowTag);
    var resultEvents = [];
    for (var i = 0; i < rowData.length; i++) {
        var message = new Irlovan.Message.StatisticMessage();
        message.ParseXElement(rowData[i]);
        resultEvents.push(message.ToArray());
    }
    if (this.Callback) { this.Callback(resultEvents); }
}

/**Subcribe datalist from server**/
Irlovan.Handler.StatisticRecorder.prototype.Subcribe = function (recorderName, timeStamp, callback) {
    this.Callback = callback;
    if (this.WebSocket) { this.WebSocket.Send(this.CreateSubcribeMessage(recorderName, timeStamp)); }
}

/**Create Subcribe Message**/
Irlovan.Handler.StatisticRecorder.prototype.CreateSubcribeMessage = function (recorderName, timeStamp) {
    var doc = Irlovan.Lib.XML.CreateXDocument(this.RootTag);
    doc.documentElement.setAttribute(this.TimeStampAttr, timeStamp);
    doc.documentElement.setAttribute(this.RecorderNameAttr, recorderName);
    return Irlovan.Lib.XML.ToString(doc);
}

/**Dispose**/
Irlovan.Handler.StatisticRecorder.prototype.Dispose = function () {
}