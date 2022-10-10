//Copyright(c) 2015,HIT All rights reserved.
//Des:CMDExecutor 
//Statememt:CMDExecutor
//Author:Irlovan   
//Date:2015-04-01

/**Construction**/
Irlovan.CMD.CMDExecutor = function (message, id) {
    if (!message) { return; }
    this.CommandSplitChar = "*";
    this.ID = id;
    this.Commands = [];
    var cmds = message.split(this.CommandSplitChar);
    this.InitCommands(cmds, id);
}

/**Init Commands**/
Irlovan.CMD.CMDExecutor.prototype.InitCommands = function (cmds, id) {
    for (var i = 0; i < cmds.length; i++) {
        this.Commands.push(new Irlovan.CMD.Command(message,id));
    }
}

/**Dispose**/
Irlovan.CMD.CMDExecutor.prototype.Dispose = function (cmds, id) {
    for (var i = 0; i < this.Commands.length; i++) {
        this.Commands[i].Dispose();
    }
}
