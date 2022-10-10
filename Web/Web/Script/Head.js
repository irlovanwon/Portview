//Copyright(c) 2013,HIT All rights reserved.
//Des:head
//Author:Irlovan   
//Date:2013-04-17

//namespace
var Irlovan = {};
Irlovan.Communication = {};
Irlovan.RealtimeData = {};
Irlovan.HtmlFramework = {};
//the including engin
Irlovan.Head = function () {
    this.Pool = [];
    this.StartScript = "<script type='text/javascript' src='";
    this.EndScript = "'><\/script>";
    this.StartCSS = "<link rel='stylesheet' type='text/css' media='screen' href='";
    this.EndCSS = "'><\/link>";
}
//using a script or a css
Irlovan.Head.prototype.Using = function (data) {
    //if ((!this.ContainPool(data)) && (!this.ContainHtml(data))) {
    if (!this.ContainPool(data)) {
        var result = (data.indexOf(".js") == -1) ? (this.StartCSS + data + this.EndCSS) : (this.StartScript + data + this.EndScript);
        this.Pool.push(data);
        this.Write(result);
    }
}
Irlovan.Head.prototype.Write = function (data) {
    document.write(data);
}
//check if the html file contain the include
Irlovan.Head.prototype.ContainHtml = function (name) {
    var isJS = !(name.indexOf(".js") == -1);
    var es = document.getElementsByTagName(isJS ? 'script' : 'link');
    for (var i = 0; i < es.length; i++) {
        var type = isJS ? 'src' : 'href';
        if (es[i].getAttribute(type) == (name)) return true;
    }
    return false;
}
//check if the pool contain the include
Irlovan.Head.prototype.ContainPool = function (name) {
    for (var i = 0; i < this.Pool.length; i++) {
        if (this.Pool[i] == name) { return true; }
    }
    return false;
}
//the instance of a including
Irlovan.Include = new Irlovan.Head();
//the base lib to using
Irlovan.Include.Using("Styles/jquery-ui-1.10.2.css");
Irlovan.Include.Using("Script/Lib/jquery-1.9.1.js");
Irlovan.Include.Using("Script/Lib/jquery-ui.custom.js");
Irlovan.Include.Using("Script/Lib/jQueryRotateCompressed.js");
Irlovan.Include.Using("Script/Control/Lib/MultipleSelect/jquery.multiselect.css");
Irlovan.Include.Using("Script/Control/Lib/MultipleSelect/jquery.multiselect.min.js");
Irlovan.Include.Using("Script/Global.js");
Irlovan.Include.Using("Script/Language/Language_English.js");
Irlovan.Include.Using("Script/Lib/ControlHelper.js");
Irlovan.Include.Using("Script/Lib/IrlovanHelper.js");
Irlovan.Include.Using("Script/Lib/Event.js");
Irlovan.Include.Using("Script/Control/Control.js");
Irlovan.Include.Using("Script/Portview.js");
Irlovan.Include.Using("Script/VirtualUIReplay.js");
Irlovan.Include.Using("Script/Expression/Expression.js");
Irlovan.Include.Using("Script/Message/Message.js");
Irlovan.Include.Using("Script/Handler/Handler.js");
Irlovan.Include.Using("Script/Lib/Irlovan/Lib.js");
Irlovan.Include.Using("Script/RealtimeData/RealtimeData.js");
Irlovan.Include.Using("Script/GUI.js");
Irlovan.Include.Using("Script/Modules/Module.js");
Irlovan.Include.Using("Script/Grid/Grid.js");
Irlovan.Include.Using("Script/Control/Control.js");
Irlovan.Include.Using("Script/Menu/Menu.js");




