//Copyright(c) 2013,HIT All rights reserved.
//Des:
//Author:Irlovan   
//Date:2013-10-09
//modification :

Irlovan.DataAssigner = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "DataAssigner", id, containerID, gridContainerID, controlClass, config ? config : ({
        tagName: { Attributes: "tagName", Value: "DataName" },
        namePosFix: { Attributes: "namePosFix", Value: 10 },
        valuePosFix: { Attributes: "valuePosFix", Value:120 },
        textPosFix: { Attributes: "textPosFix", Value: 200 },
        buttonPosFix: { Attributes: "buttonPosFix", Value: 300 },
        value: { Attributes: "value", Value: "0" },
    }), left, top, zIndex, isLock);
    this.ValueTagID = this.ID + "_value";
    this.NameTagID = this.ID + "_name";
    this.TextTagID = this.ID + "_text";
    this.ButtonTagID = this.ID + "_button";
    this.Timer;
    this.Init(id, containerID);
    this.WriteData();
}
Irlovan.DataAssigner.prototype = new Irlovan.Control();
Irlovan.DataAssigner.prototype.constructor = Irlovan.DataAssigner;
//init div element
Irlovan.DataAssigner.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div style='position: relative;'>" +
       "<div id='"+this.NameTagID+"' style='position: absolute;top:0px;left:"+this.Config.namePosFix.Value+"px;'>"+this.Config.tagName.Value+"</div>" +
       "<div id='" + this.ValueTagID + "' style='position: absolute;top:0px;left:" + this.Config.valuePosFix.Value + "px;'>" + this.Config.value.Value + "</div>" +
       "<input id='" + this.TextTagID + "' style='position: absolute;top:0px;width:60px;left:" + this.Config.textPosFix.Value + "px;border:1px solid black;'></input>" +
       "<button id='" + this.ButtonTagID + "' type='button' style='position: absolute;top:-2px;left:" + this.Config.buttonPosFix.Value + "px;'>" + Irlovan.Language.Submit + "</button>" +
       "</div>", this.ID, this.Pos);
}
Irlovan.DataAssigner.prototype.SetValue = function (name, colName, data) {
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
        case "textPosFix":
            document.getElementById(this.TextTagID).style.left = data + "px";
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
Irlovan.DataAssigner.prototype.WriteData = function () {
    document.getElementById(this.ButtonTagID).addEventListener('click', Irlovan.IrlovanHelper.Bind(this, function () {
        document.getElementById(this.ButtonTagID).disabled = true;
        this.Timer = setTimeout(Irlovan.IrlovanHelper.Bind(this, function () {
            document.getElementById(this.ButtonTagID).disabled = false;
        }), 2000);
        var name = this.GetVariableFromExpression(this.Config.value.Expression);
        var value = document.getElementById(this.TextTagID).value;
        if (name && value) {
            var result = [];
            var data = [];
            data.push(name);
            data.push(value);
            result.push(data);
            Irlovan.Global.MessageHandler.WriteData(result);
            document.getElementById(this.TextTagID).value = "";
        }
    }), false);
}
Irlovan.DataAssigner.prototype.GetVariableFromExpression = function (expression) {
    var result = expression.substring(expression.indexOf("#") + 1, expression.indexOf("#", expression.indexOf("#") + 1));
    if (result == "") { return null; }
    return result;
}
Irlovan.DataAssigner.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    if (this.Timer) {
        clearTimeout(this.Timer);
        this.Timer = null;
    }
}