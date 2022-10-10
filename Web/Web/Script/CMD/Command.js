//Copyright(c) 2015,HIT All rights reserved.
//Des:Command 
//Statememt:Command for Control
//Author:Irlovan   
//Date:2015-04-01

Irlovan.Include.Using("Script/CMD/Commands/ServerCommand.js");

/**Construction**/
Irlovan.CMD.Command = function (message, id) {
    if (!message) { return; }
    this.TypeSplitChar = "|";
    this.CMD;
    this.ID = id;
    Init(message);
}

/**Init Command**/
Irlovan.CMD.Command.prototype.Init = function (message) {
    var info = message.split(this.TypeSplitChar);
    var commandName = info[0];
    var commandMessage = message.replace(name, "");
    RunCMD(commandName, commandMessage);
}

/**Run command**/
Irlovan.CMD.Command.prototype.RunCMD = function (commandName, commandMessage) {
    this.CMD = new Irlovan.CMD.ServerCommand(commandName, commandMessage, this.ID);
    if (this.CMD.InitState) {return;}
}
