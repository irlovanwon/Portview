//Copyright(c) 2015,HIT All rights reserved.
//Des:DataMessage Handler for realtime data sending & receiving
//Author:Irlovan   
//Date:2015-03-19

Irlovan.Include.Using("Script/Message/Message.js");

/**Construction**/
Irlovan.Handler.DataMessage = function (session) {
    this.RegEx = "#";
    this.RootTag = "Data";
    this.ModeAttr = "Mode";
    this.SubcriptionTag = "SBC";
    this.StopTag = "STP";
    this.GroupTag = "Group";
    this.WriteDataTag = "WRT";
    this.ItemTag = "Item";
    this.InDataMessageTag = "InDataMessage";
    this.NameAttr = "Name";
    this.ValueAttr = "Value";
    this.QualityAttr = "Quality";
    this.GoodQuality = "Good";
    this.DefaultGroupName = "GUI";
    this.WebSocket = session;
    this.DataStack = {};//need reset when change page
}
/**Handle message from server**/
Irlovan.Handler.DataMessage.prototype.Handle = function (xDocument) {
    var element = xDocument.documentElement;
    if (element.nodeName != this.RootTag) { return; }
    this.HandleSubcription(element);
}

/**Subcribe datalist from server**/
Irlovan.Handler.DataMessage.prototype.Subcribe = function (controls) {
    this.InitDeadExpression(controls);
    this.WebSocket.Send(this.DataListSubcription(controls, Irlovan.Global.DataInterval));
}

/**Write data to server**/
Irlovan.Handler.DataMessage.prototype.WriteData = function (dataList) {
    var doc = Irlovan.Lib.XML.CreateXDocument(this.RootTag);
    var element = Irlovan.Lib.XML.CreateXElement(this.WriteDataTag);
    doc.documentElement.appendChild(element);
    this.AddWriteListXML(element, dataList);
    this.WebSocket.Send(Irlovan.Lib.XML.ToString(doc));
}

/**Stop datalist subcription from server**/
Irlovan.Handler.DataMessage.prototype.Stop = function () {
    var doc = Irlovan.Lib.XML.CreateXDocument(this.RootTag);
    var stopSubcription = Irlovan.Lib.XML.CreateXElement(this.StopTag);
    doc.documentElement.appendChild(stopSubcription);
    this.WebSocket.Send(Irlovan.Lib.XML.ToString(doc));
    this.DataStackDispose();
}

/**Handle subcription from server**/
Irlovan.Handler.DataMessage.prototype.HandleSubcription = function (element) {
    var subcriptions = element.getElementsByTagName(this.SubcriptionTag);
    if ((!subcriptions) || (subcriptions.length == 0)) { return; }
    var groups = element.getElementsByTagName(this.GroupTag);
    if ((!groups) || (groups.length == 0)) { return; }
    for (var i = 0; i < groups.length; i++) {
        this.HandleGroupMessage(groups[i]);
    }
}

/**Handle group from server by all elements**/
Irlovan.Handler.DataMessage.prototype.HandleGroupMessage = function (group) {
    var isInit = false;
    var dataElements = group.getElementsByTagName(this.InDataMessageTag);
    for (var i = 0; i < dataElements.length; i++) {
        if (this.HandleElement(dataElements[i]) == false) { isInit = true; }
    }
    if (!isInit) { return; }
    //reset init state!!!!!!!!!!!!!!!!!!
    isInit = false;
    this.HandleGroupMessage(group);
    for (var i = 0; i < dataElements.length; i++) {
        this.UpdateControlList(this.CreateDataMessage(dataElements[i]),true);
    }
}

/**Handle subcription from server by element**/
Irlovan.Handler.DataMessage.prototype.HandleElement = function (element) {
    var name = element.getAttribute(this.NameAttr);
    var dataMessage = this.CreateDataMessage(element);
    if (!this.DataStack[name]) { this.DataStack[name] = dataMessage; return false; }
    if ((this.DataStack[name]) && (this.DataStack[name].Equals(dataMessage))) { return; }
    var commCheck = false;
    if (!this.DataStack[name].EqualsQuality(dataMessage)) {commCheck = true;}
    this.DataStack[name] = dataMessage;
    this.UpdateControlList(dataMessage, commCheck);
}

/**CreateDataMessage**/
Irlovan.Handler.DataMessage.prototype.CreateDataMessage = function (element) {
    var name = element.getAttribute(this.NameAttr);
    var value = (element.getAttribute(this.ValueAttr)).toLowerCase();
    var quality = element.getAttribute(this.QualityAttr);
    return new Irlovan.Message.DataMessage(name, value, quality);

}

/**Update control list**/
Irlovan.Handler.DataMessage.prototype.UpdateControlList = function (dataMessage, commCheck) {
    for (var i = 0; i < Irlovan.Global.ControlList.length; i++) {
        this.UpdateControl(dataMessage, Irlovan.Global.ControlList[i], commCheck);
    }
}

/**Update control**/
Irlovan.Handler.DataMessage.prototype.UpdateControl = function (dataMessage, control, commCheck) {
    for (var i = 0; i < control.ExpressionList.length; i++) {
        this.UpdateExpression(dataMessage, control, control.ExpressionList[i], commCheck);
    }
}
/**Update expression**/
Irlovan.Handler.DataMessage.prototype.UpdateExpression = function (dataMessage, control, expression, commCheck) {
    var patch = expression.Expression.split(this.RegEx);
    if ((patch.length < 2) || (patch.length % 2 == 0)) { return null; }
    if (!this.NeedUpdate(patch, dataMessage.Name)) { return null; }
    var expressionResult = this.GetEvalExpression(patch, control, expression, commCheck);
}
/**Check if the expression need update**/
Irlovan.Handler.DataMessage.prototype.NeedUpdate = function (expressions, name) {
    for (var i = 0; i < expressions.length; i++) {
        if (expressions[i] == name) { return true; }
    }
    return false;
}
/**Get the eval value of the expression**/
Irlovan.Handler.DataMessage.prototype.GetEvalExpression = function (patch, control, expression, commCheck) {
    var expressionResult = "";
    var commOK = true;
    for (var i = 0; i < patch.length; i++) {
        if (i % 2 == 0) { expressionResult += patch[i]; }
        else {
            var dataMessage = this.GetValueByName(patch[i]);
            if (dataMessage == null) { control.FaultExpressionHighlight(); dataMessage = ''; }
            expressionResult += ("(" + dataMessage.Value + ")");
            if (!this.CommCheck(control, dataMessage.Quality, dataMessage, commCheck)) { commOK = false; }
        }
    }
    var value = this.EvalExpression(control, expressionResult);
    if ((commOK) && (value != null)) { this.SetValue(control, expression, value); }
}
/**Get value from stack by name**/
Irlovan.Handler.DataMessage.prototype.GetValueByName = function (name) {
    return this.DataStack[name];
}
/**Set value of the expression**/
Irlovan.Handler.DataMessage.prototype.SetValue = function (control, expression, value) {
    if (expression == null) { return; }
    control.SetValue(expression.Attributes, "Value", value);
}
/**Eval a expression**/
Irlovan.Handler.DataMessage.prototype.EvalExpression = function (control, expressionResult) {
    if (expressionResult == null) { return; }
    try {
        return eval(Irlovan.IrlovanHelper.ErrorXMLRepair(expressionResult));
    } catch (e) {
        control.FaultExpressionHighlight();
        return false;
    }
}
/**Check if the quality of the data is good**/
Irlovan.Handler.DataMessage.prototype.CommCheck = function (control, quality, dataMessage, commCheck) {
    if (!commCheck) {return true;}
    if (quality == this.GoodQuality) {
        control.On();
        return true;
    } else {
        control.Off();
        return false;
    }
}

/**AddWriteListXML**/
Irlovan.Handler.DataMessage.prototype.AddWriteListXML = function (element, dataList) {
    for (var i = 0; i < dataList.length; i++) {
        this.AddWriteDataXML(dataList[i], element);
    }
}
/**AddWriteDataXML**/
Irlovan.Handler.DataMessage.prototype.AddWriteDataXML = function (data, element) {
    var writeData = Irlovan.Lib.XML.CreateXElement(this.ItemTag);
    writeData.setAttribute(this.NameAttr, data[0]);
    writeData.setAttribute(this.ValueAttr, data[1]);
    element.appendChild(writeData);
}

/**Init dead expression**/
Irlovan.Handler.DataMessage.prototype.InitDeadExpression = function (controls) {
    for (var i = 0; i < controls.length; i++) {
        this.InitControlDeadExpression(controls[i]);
    }
}

/**Init control dead expression**/
Irlovan.Handler.DataMessage.prototype.InitControlDeadExpression = function (control) {
    for (var i = 0; i < control.ExpressionList.length; i++) {
        var expression = control.ExpressionList[i];
        if (expression.Expression.indexOf(this.RegEx) != -1) { continue; }
        try {
            var result = eval(Irlovan.IrlovanHelper.ErrorXMLRepair(expression.Expression));
            control.SetValue(expression.Attributes, "Value", result);
        }
        catch (e) {
            control.FaultExpressionHighlight();
        }
    }
}

/**DataListSubcription**/
Irlovan.Handler.DataMessage.prototype.DataListSubcription = function (controls, interval) {
    if ((!controls) || (controls.length == 0)) { return; }
    var doc = Irlovan.Lib.XML.CreateXDocument(this.RootTag);
    var subcription = Irlovan.Lib.XML.CreateXElement(this.SubcriptionTag);
    var group = Irlovan.Lib.XML.CreateXElement(this.GroupTag);
    doc.documentElement.appendChild(subcription);
    subcription.appendChild(group);
    group.setAttribute(this.ModeAttr, interval);
    group.setAttribute(this.NameAttr, this.DefaultGroupName);
    this.CreateSubcription(group, controls);
    return Irlovan.Lib.XML.ToString(doc);
}
/**CreateSubcription**/
Irlovan.Handler.DataMessage.prototype.CreateSubcription = function (element, controls) {
    for (var i = 0; i < controls.length; i++) {
        this.CreateControlSubcription(element, controls[i]);
    }
}
/**CreateControlSubcription**/
Irlovan.Handler.DataMessage.prototype.CreateControlSubcription = function (element, control) {
    var expressionList = control.ExpressionList;
    for (var i = 0; i < expressionList.length; i++) {
        var names = this.GetNameFromExpression(expressionList[i].Expression);
        if (!names) { continue; }
        this.AddSubcriptionXMLDocument(element, names);
    }
}
/**GetNameFromExpression**/
Irlovan.Handler.DataMessage.prototype.GetNameFromExpression = function (expression) {
    var patch = expression.split(this.RegEx);
    if ((patch.length < 2) || (patch.length % 2 == 0)) { return null; }
    var result = [];
    for (var i = 0; i < patch.length; i++) {
        if ((i % 2 == 1)) { result.push(patch[i]); }
    }
    return result;
}
/**AddNamesToDocument**/
Irlovan.Handler.DataMessage.prototype.AddSubcriptionXMLDocument = function (element, names) {
    var dataList = [];
    for (var i = 0; i < names.length; i++) {
        var name = names[i];
        if (dataList.indexOf(name) != -1) { continue; }
        dataList.push(name);
        var data = Irlovan.Lib.XML.CreateXElement(this.ItemTag);
        data.setAttribute(this.NameAttr, name);
        element.appendChild(data);
    }
}
/**Dispose**/
Irlovan.Handler.DataMessage.prototype.Dispose = function () {
    this.DataStackDispose();
}
Irlovan.Handler.DataMessage.prototype.DataStackDispose = function () {
    for (var item in this.DataStack) {
        this.DataStack[item] = null;
    }
    this.DataStack = null;
    this.DataStack = {};
}