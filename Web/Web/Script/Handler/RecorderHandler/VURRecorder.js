//Copyright(c) 2015,HIT All rights reserved.
//Des:Handler for Virtual UI Replay
//Author:Irlovan   
//Date:2015-08-27

/**Construction**/
Irlovan.Handler.VURRecorder = function (session) {
    this.RootTag = "VURHandler";
    this.VURTag = "VUR";
    this.CommandAttr = "Command";
    this.TimeStampAttr = "TimeStamp";
    this.RecorderNameAttr = "Name";
    this.IntervalAttr = "Interval";
    this.PlayCommandStr = "Play";
    this.PauseCommandStr = "Pause";
    this.MatrixRowItemTag = "MatrixRow";
    this.RegEx = "#";
    this.WebSocket = session;
    this.Callback;
    this.DataCache = {};
}
/**Handle message from server**/
Irlovan.Handler.VURRecorder.prototype.Handle = function (xDocument) {
    var element = xDocument.documentElement;
    if (element.nodeName != this.RootTag) { return; }
    var vur = element.getElementsByTagName(this.VURTag);
    if ((!vur) || (vur.length == 0)) { this.UpdateVUR(); return; }
    this.HandleDatas(vur[0]);
}

/**Handle datas**/
Irlovan.Handler.VURRecorder.prototype.HandleDatas = function (element) {
    var dataMessages = element.getElementsByTagName(this.MatrixRowItemTag);
    var timeStamp = element.getAttribute(this.TimeStampAttr);
    if ((!dataMessages)) { return; }
    this.Callback(timeStamp);
    var dataMessage = dataMessages[0];
    this.DataCache = {};
    this.InitDataCache(dataMessage);
    this.UpdateVUR();
}

Irlovan.Handler.VURRecorder.prototype.InitDataCache = function (dataMessage) {
    if (!dataMessage) { return; }
    for (var i = 0; i < dataMessage.attributes.length; i++) {
        this.DataCache[dataMessage.attributes[i].name] = dataMessage.attributes[i].value;
    }
}

/**Start to play VUR**/
Irlovan.Handler.VURRecorder.prototype.Play = function (recorderName, timeStamp, interval, callback) {
    this.Callback = callback;
    if (this.WebSocket) { this.WebSocket.Send(this.CreatePlayMessage(recorderName, timeStamp, interval)); }
}

/**Start to play VUR**/
Irlovan.Handler.VURRecorder.prototype.Pause = function (recorderName) {
    if (this.WebSocket) { this.WebSocket.Send(this.CreatePauseMessage(recorderName)); }
}

/**Create Play Message**/
Irlovan.Handler.VURRecorder.prototype.CreatePlayMessage = function (recorderName, timeStamp, interval) {
    var doc = Irlovan.Lib.XML.CreateXDocument(this.RootTag);
    doc.documentElement.setAttribute(this.CommandAttr, this.PlayCommandStr);
    doc.documentElement.setAttribute(this.TimeStampAttr, timeStamp);
    doc.documentElement.setAttribute(this.RecorderNameAttr, recorderName);
    doc.documentElement.setAttribute(this.IntervalAttr, interval);
    return Irlovan.Lib.XML.ToString(doc);
}

/**Create Pause Message**/
Irlovan.Handler.VURRecorder.prototype.CreatePauseMessage = function (recorderName) {
    var doc = Irlovan.Lib.XML.CreateXDocument(this.RootTag);
    doc.documentElement.setAttribute(this.CommandAttr, this.PauseCommandStr);
    doc.documentElement.setAttribute(this.RecorderNameAttr, recorderName);
    return Irlovan.Lib.XML.ToString(doc);
}

/**Dispose**/
Irlovan.Handler.VURRecorder.prototype.Dispose = function () {
}

Irlovan.Handler.VURRecorder.prototype.UpdateVUR = function () {
    for (var i = 0; i < Irlovan.Global.ControlList.length; i++) {
        this.UpdateControl(Irlovan.Global.ControlList[i]);
    }
}

Irlovan.Handler.VURRecorder.prototype.UpdateControl = function (control) {
    for (var i = 0; i < control.ExpressionList.length; i++) {
        this.UpdateExpression(control.ExpressionList[i], control);
    }
}

/**Update expression**/
Irlovan.Handler.VURRecorder.prototype.UpdateExpression = function (expression, control) {
    var patch = expression.Expression.split(this.RegEx);
    if ((patch.length < 2) || (patch.length % 2 == 0)) { return null; }
    this.UpdateData(patch, control, expression);
}

/**Get the eval value of the expression**/
Irlovan.Handler.VURRecorder.prototype.UpdateData = function (patch, control, expression) {
    var expressionResult = "";
    for (var i = 0; i < patch.length; i++) {
        if (i % 2 == 0) { expressionResult += patch[i]; }
        else {
            var dataValue = this.DataCache[patch[i]]
            if (!dataValue) { control.Off(); return; }
            control.On()
            expressionResult += ("(" + dataValue.toLowerCase() + ")");
        }
    }
    var value = this.EvalExpression(control, expressionResult);
    if (value != null) { this.SetValue(control, expression, value); }
}
/**Eval a expression**/
Irlovan.Handler.VURRecorder.prototype.EvalExpression = function (control, expressionResult) {
    if (expressionResult == null) { return null; }
    try {
        return eval(Irlovan.IrlovanHelper.ErrorXMLRepair(expressionResult));
    } catch (e) {
        control.Off();
        control.FaultExpressionHighlight();
        return null;
    }
}

/**Set value of the expression**/
Irlovan.Handler.VURRecorder.prototype.SetValue = function (control, expression, value) {
    if (expression == null) { return; }
    control.SetValue(expression.Attributes, "Value", value);
}
