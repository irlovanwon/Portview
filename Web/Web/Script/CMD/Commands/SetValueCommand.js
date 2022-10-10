//Copyright(c) 2013,HIT All rights reserved.
//Des:CMD
//Author:Irlovan   
//Date:2014-05-22
//modification :

Irlovan.Include.Using("Script/CMD/Trigger/ClickTrigger.js");


Irlovan.CMD.SetValueCommand = function (commandName, commandMessage, id) {
    this.Name = "ServerCMD";
    this.InitState = (commandName == this.Name);
    this.InfoSplitChar = "|";
    var info = commandMessage.split(this.InfoSplitChar);
    if (info.length != 3) { return; }
    this.ID = id;
    this.CMDHandler;
    this.Trigger;
    this.ServerName = info[0];
    this.TriggerName = info[1];
    this.Command = info[2];
    this.Init();
}
Irlovan.CMD.SetValueCommand.prototype.Init = function () {
    var server = Irlovan.Global.CMD[this.ServerName];
    if (!server) { return; }
    this.CMDHandler = server.Handler;
    if (!this.CMDHandler) { return; }
    this.Trigger = new Irlovan.CMD.ClickTrigger(this.TriggerName, this.ID, null, Irlovan.IrlovanHelper.Bind(this, this.Action));
    if (this.Trigger.InitState) {return;}
}

/**Start to run the conmand**/
Irlovan.CMD.SetValueCommand.prototype.Action = function () {
    this.CMDHandler.Run(this.Command);
}

/**Dispose**/
Irlovan.CMD.SetValueCommand.prototype.Dispose = function () {
    if (this.Trigger) {this.Trigger.Dispose();}
}