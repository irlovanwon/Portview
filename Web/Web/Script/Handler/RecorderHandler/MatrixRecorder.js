//Copyright(c) 2015,HIT All rights reserved.
//Des:Handler for recorder matrix data query
//Author:Irlovan   
//Date:2015-09-29

/**Construction**/
Irlovan.Handler.MatrixRecorder = function (session) {
    this.RootTag = "MatrixRecorderHandler";
    this.StartTimeAttr = "StartTime";
    this.EndTimeAttr = "EndTime";
    this.RecorderNameAttr = "Name";
    this.MatrixTag = "MatrixArray";
    this.MatrixRowTag = "MatrixRow";
    this.ColumnsAttr = "Columns";
    this.WebSocket = session;
    this.Callback;
}

/**Handle message from server**/
Irlovan.Handler.MatrixRecorder.prototype.Handle = function (xDocument) {
    var element = xDocument.documentElement;
    if (element.nodeName != this.RootTag) { return; }
    this.HandleData(element);
}

/**Handle events**/
Irlovan.Handler.MatrixRecorder.prototype.HandleData = function (element) {
    var matrixData = element.getElementsByTagName(this.MatrixTag);
    if ((!matrixData) || (matrixData.length == 0)) { return; }
    var rowData = matrixData[0].getElementsByTagName(this.MatrixRowTag);
    var result = [];
    for (var i = 0; i < rowData.length; i++) {
        var message = new Irlovan.Message.MatrixMessage();
        message.ParseXElement(rowData[i]);
        result.push(message);
    }
    if (this.Callback) { this.Callback(result); }
}

/**Subcribe datalist from server**/
Irlovan.Handler.MatrixRecorder.prototype.Subcribe = function (recorderName, startTime, endTime, columnList, callback) {
    this.Callback = callback;
    if (this.WebSocket) { this.WebSocket.Send(this.CreateSubcribeMessage(recorderName, startTime, endTime, columnList)); }
}

/**Create Subcribe Message**/
Irlovan.Handler.MatrixRecorder.prototype.CreateSubcribeMessage = function (recorderName, startTime, endTime, columnList) {
    var doc = Irlovan.Lib.XML.CreateXDocument(this.RootTag);
    doc.documentElement.setAttribute(this.StartTimeAttr, startTime);
    doc.documentElement.setAttribute(this.EndTimeAttr, endTime);
    doc.documentElement.setAttribute(this.RecorderNameAttr, recorderName);
    doc.documentElement.setAttribute(this.ColumnsAttr, columnList);
    return Irlovan.Lib.XML.ToString(doc);
}

/**Dispose**/
Irlovan.Handler.MatrixRecorder.prototype.Dispose = function () {
}