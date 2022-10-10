//Copyright(c) 2015,HIT All rights reserved.
//Des:Recorder Handler for realtime data sending & receiving
//Author:Irlovan   
//Date:2015-03-25

Irlovan.Include.Using("Script/Message/Message.js");

/**Construction**/
Irlovan.Handler.Recorder = function (session) {
    this.Name = "Recorder";
    this.Message = new Irlovan.Message.GUIRecorderMessage();
    this.Session = session;
}

/**Handle message from server**/
Irlovan.Handler.Recorder.prototype.Handle = function (xDocument) {
    var element = xDocument.documentElement;
    if (element.nodeName != this.Name) { return; }
    this.HandleSaveResultMessage(element);
}

/**Save message from server**/
Irlovan.Handler.Recorder.prototype.Save = function (pageList) {
    this.Session.Send(this.Message.CreateSaveMessage(pageList));
}

/**Handle message from server**/
Irlovan.Handler.Recorder.prototype.HandleSaveResultMessage = function (element) {
    this.Message.ShowResult(element);
}

/**Dispose**/
Irlovan.Handler.Recorder.prototype.Dispose = function () {
}



