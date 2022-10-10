//Copyright(c) 2015,HIT All rights reserved.
//Des:MSCMD for windows command line
//Author:Irlovan   
//Date:2015-08-24
//modification :

/**Construction**/
Irlovan.Handler.MSCMD = function (session) {
    this.RootTag = "MSCMD";
    this.CommandAttr = "Command";
    this.WebSocket = session;
}

/**Order the command**/
Irlovan.Handler.MSCMD.prototype.Order = function (cmsCode) {
    var doc = Irlovan.Lib.XML.CreateXDocument(this.RootTag);
    doc.documentElement.setAttribute(this.CommandAttr, cmsCode);
    this.WebSocket.Send(Irlovan.Lib.XML.ToString(doc));
}
