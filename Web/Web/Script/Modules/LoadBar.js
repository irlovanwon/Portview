//Copyright(c) 2015,HIT All rights reserved.
//Des:Load Bar Shows Connecting States With Server Or Other Progress
//Author:Irlovan   
//Date:2013-04-17
//modification :


Irlovan.Module.LoadBar = function () {
    this.ServerConnected = false;
    this.LoadBarMessage = "Initializing...............";
    this.DisconnectMessage = "Disconnect";
    this.Init(this.LoadBarMessage);
}
Irlovan.Module.LoadBar.prototype.Init = function (message) {
    Irlovan.ControlHelper.CreateControlByStr(
           "<div id='" + Irlovan.Global.ReloadBarID + "' style='position: absolute;top:" + screen.height / 2 + "px;left:0px;width:" + (parseInt(screen.width) - 2) + "px'>" +
           "<div class='progress-label' style='position: absolute;text-align:center;width:" + (parseInt(screen.width) - 2) + "px'></div>" +
           "</div>", Irlovan.Global.BodyID, null);
    $("#" + Irlovan.Global.ReloadBarID).progressbar({
        value: false
    });
    $("#" + Irlovan.Global.ReloadBarID).find(".ui-progressbar-value").css({
        "background": 'skyblue'
    });
    $(".progress-label").text(message);
}
Irlovan.Module.LoadBar.prototype.On = function () {
    this.ServerConnected = false;
    var timeout = parseInt(Irlovan.Global.ServerTimeout);
    if (timeout <= 0) { alert(this.DisconnectMessage); return; }
    setTimeout(function () {
        $("#" + Irlovan.Global.ReloadBarID).css("visibility", "visible");
        location.reload();
    }, timeout);
}

Irlovan.Module.LoadBar.prototype.Off = function () {
    this.ServerConnected = true;
    $("#" + Irlovan.Global.ReloadBarID).css("visibility", "hidden");
}

