//Copyright(c) 2015,HIT All rights reserved.
//Des:ResetSwitch
//Author:Irlovan   
//Date:2015-04-02
//modification :

Irlovan.Control.ResetSwitch = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "ResetSwitch", id, containerID, gridContainerID, controlClass, config ? config : ({
        value: { Attributes: "value", Value: false },
        size: { Attributes: "size", Value: 200 },
        timeout: { Attributes: "timeout", Value: 1000, Description: "less than 0 :update by remote" },
        resetTimeout: { Attributes: "resetTimeout", Value: 1000 },
        isSync: { Attributes: "isSync", Value: false, Description: "sync with server" },
    }), left, top, zIndex, isLock);
    this.Timeout;
    this.ResetTimeout;
    this.ButtonID = this.ID + "_reset1button";
    this.ButtonEnable = true;
    this.TimeoutEnable = true;
    this.Timer;
    this.Init(id, containerID);
    this.WriteData();
}

Irlovan.Control.ResetSwitch.prototype = new Irlovan.Control.Classic();
Irlovan.Control.ResetSwitch.prototype.constructor = Irlovan.Control.ResetSwitch;

/**init div element**/
Irlovan.Control.ResetSwitch.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var tag = Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.ButtonID + "' style='background-size:100% 100%;position: relative;width:" + this.Config.size.Value + "px;height:" + this.Config.size.Value + "px;background-image: url(Images/Button/Reset/1/ResetSwitch.png)'>" +
       "</div>", this.ID, this.Pos);
}
Irlovan.Control.ResetSwitch.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "value":
            this.UpdateValue(data);
            break;
        case "size":
            document.getElementById(this.ButtonID).style.width = data + "px";
            document.getElementById(this.ButtonID).style.height = data + "px";
            break;
        default:
            break;
    }
}

/**write data**/
Irlovan.Control.ResetSwitch.prototype.WriteData = function () {
    document.getElementById(this.ButtonID).addEventListener('click', Irlovan.IrlovanHelper.Bind(this, this.WriteDataHandler), false);
}

/**write data handler**/
Irlovan.Control.ResetSwitch.prototype.WriteDataHandler = function () {
    if (this.ButtonEnable == false) { return; }
    if (this.TimeoutEnable == false) { return; }
    var resetTimeout;
    try { resetTimeout = parseInt(this.Config.resetTimeout.Value); } catch (e) { return; }
    if (resetTimeout <= 0) { return; }
    this.ResetTimeout = resetTimeout;
    var name = this.GetVariableFromExpression(this.Config.value.Expression);
    var value = true;
    if (name == null) { return; }
    if (Irlovan.Lib.Help.Boolean(this.Config.isSync.Value) == false) { this.Press(); }
    this.TimeoutProtection();
    var result = [];
    var data = [];
    data.push(name);
    data.push(value);
    result.push(data);
    Irlovan.Global.MessageHandler.DataMessage.WriteData(result);
    result[0][1] = false;
    this.AutoReverse(result);
}

/**TimeoutProtection**/
Irlovan.Control.ResetSwitch.prototype.TimeoutProtection = function () {
    try { this.Timeout = parseInt(this.Config.timeout.Value); } catch (e) { return; }
    if (this.Timeout <= 0) { return; }
    this.TimeoutEnable == false;
    this.Timer = setTimeout(Irlovan.IrlovanHelper.Bind(this, function () {
        this.TimeoutEnable == true;
    }), this.Timeout);
}

/**AutoReset Button**/
Irlovan.Control.ResetSwitch.prototype.AutoReverse = function (result) {
    setTimeout(Irlovan.IrlovanHelper.Bind(this, function () {
        Irlovan.Global.MessageHandler.DataMessage.WriteData(result);
    }), this.ResetTimeout);
}

/**GetVariableFromExpression**/
Irlovan.Control.ResetSwitch.prototype.GetVariableFromExpression = function (expression) {
    if (!expression) { return null; }
    var result = expression.substring(expression.indexOf("#") + 1, expression.indexOf("#", expression.indexOf("#") + 1));
    if (result == "") { return null; }
    return result;
}

/**Press Reset Button**/
Irlovan.Control.ResetSwitch.prototype.Press = function () {
    this.ButtonEnable = false;
    document.getElementById(this.ButtonID).style.backgroundImage = "url('Images/Button/Reset/1/ResetSwitchPress.png')";
}

/**Release Reset Button**/
Irlovan.Control.ResetSwitch.prototype.Release = function () {
    this.ButtonEnable = true;
    document.getElementById(this.ButtonID).style.backgroundImage = "url('Images/Button/Reset/1/ResetSwitch.png')";
}

/**Update value from remote**/
Irlovan.Control.ResetSwitch.prototype.UpdateValue = function (value) {
    if (value == null) { return; }
    var result = Irlovan.Lib.Help.Boolean(value);
    if (result == true) {
        this.Press();
    } else {
        this.Release();
    }
}

/**Dispose Timer**/
Irlovan.Control.ResetSwitch.prototype.DisposeTimer = function (Timer) {
    if (Timer != null) {
        clearTimeout(Timer);
        Timer = null;
    }
}

/**Dispose**/
Irlovan.Control.ResetSwitch.prototype.Dispose = function () {
    document.getElementById(this.ButtonID).removeEventListener('click', Irlovan.IrlovanHelper.Bind(this, this.WriteDataHandler));
    this.DisposeTimer(this.Timeout);
    this.DisposeTimer(this.ResetTimeout);
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
}
