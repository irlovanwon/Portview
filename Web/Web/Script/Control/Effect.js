//Copyright(c) 2015,HIT All rights reserved.
//Des:Effect 
//Statememt:This function is only for HIT HVLV System
//Author:Irlovan   
//Date:2015-01-12
//modification :2015-03-31

Irlovan.Control.Effect = function (message, id) {
    if (!message) { return; }
    this.CommandSplitChar = "*";
    this.ID = id;
    var cmds = message.split("*");
    for (var i = 0; i < cmds.length; i++) {
        var cmd = cmds[i].split("|");
        if (!cmd) { return; }
        if (cmd.length != 2) { return; }
        this.Mode(cmd[0], cmd[1]);
    }
}
Irlovan.Control.Effect.prototype.Mode = function (name, command) {
    switch (name) {
        case "Show":
            this.Show(command);
        default:
    }
}
//<option value="blind">Blind</option>
// <option value="bounce">Bounce</option>
// <option value="clip">Clip</option>
// <option value="drop">Drop</option>
// <option value="explode">Explode</option>
// <option value="fold">Fold</option>
// <option value="highlight">Highlight</option>
// <option value="puff">Puff</option>
// <option value="pulsate">Pulsate</option>
// <option value="shake">Shake</option>
// <option value="slide">Slide</option>
Irlovan.Control.Effect.prototype.Show = function (command) {
    var infos = command.split(";");
    //0:trigger event 1: effect type 2:control ids
    if (infos.length != 4) { return; }
    if (!infos[0]) { return; }
    if (!infos[1]) { return; }
    if (!infos[2]) { return; }
    if (!infos[3]) { return; }
    try{   var time = parseInt(infos[0]);}catch(e){return;}
    var events = infos[1].split(",");
    if (events.length != 2) { return; }
    var control = document.getElementById(this.ID);
    if (!control) { return; }
    var effects = infos[2].split(",");
    if (effects.length != 2) { return; }
    var options = {direction: 'right' };
    var ids = infos[3].split(",");
    for (var i = 0; i < ids.length; i++) {
        $('#' + ids[i]).hide();
    }
    if (events[0] == events[1]) {
        if (events[0] == "null") { return; }
        $("#" + this.ID).on(events[0], function () {
            for (var i = 0; i < ids.length; i++) {
                if ($('#' + ids[i]).is(':visible') == false) { $("#" + ids[i]).show(effects[0],options, time); }
                else { $("#" + ids[i]).hide(effects[0], time); }
            }
        })
    } else {
        if (events[0] != "null") {
            $("#" + this.ID).on(events[0], function () {
                for (var i = 0; i < ids.length; i++) {
                    if ($('#' + ids[i]).is(':visible') == false) { $("#" + ids[i]).show(effects[0], options, time, null); }
                }
            })
        }
        if (events[1] != "null") {
            $("#" + this.ID).on(events[1], function () {
                for (var i = 0; i < ids.length; i++) {
                    if ($('#' + ids[i]).is(':visible') == true) { $("#" + ids[i]).hide(effects[0], options, time, null); }
                }
            })
        }
    }
}
