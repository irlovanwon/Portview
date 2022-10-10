//Copyright(c) 2013,HIT All rights reserved.
//Des:
//Author:Irlovan   
//Date:2013-10-09
//modification :

Irlovan.BoolAssigner = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "BoolAssigner", id, containerID, gridContainerID, controlClass, config ? config : ({
        tagName: { Attributes: "tagName", Value: "DataName" },
        namePosFix: { Attributes: "namePosFix", Value: 10 },
        valuePosFix: { Attributes: "valuePosFix", Value: 120 },
        timeout: { Attributes: "timeout", Value: 2000 },
        resetTimeout: { Attributes: "resetTimeout", Value: 0 },
        buttonPosFix: { Attributes: "buttonPosFix", Value: 200 },
        value: { Attributes: "value", Value: "0" },
    }), left, top, zIndex, isLock);
    this.ValueTagID = this.ID + "_value";
    this.NameTagID = this.ID + "_name";
    this.ButtonTagID = this.ID + "_button";
    this.Timer;
    this.Init(id, containerID);
    this.WriteData();
}
Irlovan.BoolAssigner.prototype = new Irlovan.Control();
Irlovan.BoolAssigner.prototype.constructor = Irlovan.BoolAssigner;
//init div element
Irlovan.BoolAssigner.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div style='position: relative;'>" +
       "<div id='"+this.NameTagID+"' style='position: absolute;top:0px;left:"+this.Config.namePosFix.Value+"px;'>"+this.Config.tagName.Value+"</div>" +
       "<div id='" + this.ValueTagID + "' style='position: absolute;top:0px;left:" + this.Config.valuePosFix.Value + "px;'>" + this.Config.value.Value + "</div>" +
       "<button id='" + this.ButtonTagID + "' type='button' style='position: absolute;top:-2px;left:" + this.Config.buttonPosFix.Value + "px;'>" + Irlovan.Language.Submit + "</button>" +
       "</div>", this.ID, this.Pos);
}
Irlovan.BoolAssigner.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "tagName":
            document.getElementById(this.NameTagID).innerHTML = data;
            break;
        case "namePosFix":
            document.getElementById(this.NameTagID).style.left = data+"px";
            break;
        case "valuePosFix":
            document.getElementById(this.ValueTagID).style.left = data + "px";
            break;
        case "buttonPosFix":
            document.getElementById(this.ButtonTagID).style.left = data + "px";
            break;
        case "value":
            document.getElementById(this.ValueTagID).innerHTML = data;
            break;
        default:
            break;
    }
}
Irlovan.BoolAssigner.prototype.WriteData = function () {
    document.getElementById(this.ButtonTagID).addEventListener('click', Irlovan.IrlovanHelper.Bind(this, function () {
        document.getElementById(this.ButtonTagID).disabled = true;
        this.Timer = setTimeout(Irlovan.IrlovanHelper.Bind(this, function () {
            document.getElementById(this.ButtonTagID).disabled = false;
        }), parseFloat(this.Config.timeout.Value));
        var name = this.GetVariableFromExpression(this.Config.value.Expression);
        var value = true;
        if (name && value) {
            var result = [];
            var data = [];
            data.push(name);
            data.push(value);
            result.push(data);
            Irlovan.Global.MessageHandler.WriteData(result);
            if ((this.Config.resetTimeout.Value == 0) || (this.Config.resetTimeout.Value == "0")) { return; }
            result[0][1] = false;
            this.WriteFalse(result,parseFloat(this.Config.resetTimeout.Value));
        }
    }), false);
}
Irlovan.BoolAssigner.prototype.WriteFalse = function (result,time) {
    setTimeout(Irlovan.IrlovanHelper.Bind(this, function () {
        Irlovan.Global.MessageHandler.WriteData(result);
    }), time);
}
Irlovan.BoolAssigner.prototype.GetVariableFromExpression = function (expression) {
    var result = expression.substring(expression.indexOf("#") + 1, expression.indexOf("#", expression.indexOf("#") + 1));
    if (result == "") { return null; }
    return result;
}
Irlovan.BoolAssigner.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    if (this.Timer) {
        clearTimeout(this.Timer);
        this.Timer = null;
    }
}