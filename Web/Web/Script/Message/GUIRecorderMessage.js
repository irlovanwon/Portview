//Copyright(c) 2015,HIT All rights reserved.
//Des:Message format for GUI recorder saving
//Author:Irlovan   
//Date:2015-03-25


/**Construction**/
Irlovan.Message.GUIRecorderMessage = function () {
    this.Name = "Recorder";
    this.SaveTag = "Save";
    this.PageTag = "Page";
    this.StateAttr = "State";
    this.PathAttr = "Path";
    this.SplitChar = ":";
    this.SpaceChar = "   ";
    this.SaveFailStr = "Fail";
    this.SaveSuccessStr = "Success";
    this.ResultMessage=null;
}


/**Equal to another data message**/
Irlovan.Message.GUIRecorderMessage.prototype.ShowResult = function (element) {
    var saveResults = element.getElementsByTagName(this.SaveTag);
    if ((!saveResults) || (saveResults.length == 0)) { return; }
    var pages = saveResults[0].getElementsByTagName(this.PageTag);
    if ((!pages) || (pages.length == 0)) { return; }
    this.ResultMessage = "";
    for (var i = 0; i < pages.length; i++) {
        var page = pages[i];
        this.ResultMessage += ((this.PathAttr + this.SplitChar + page.getAttribute(this.PathAttr)) + this.SpaceChar + (this.StateAttr + this.SplitChar + page.getAttribute(this.StateAttr)));
    }
    if (this.ResultMessage==null) {return;}
    alert(this.ResultMessage);
}

/**Create save message**/
Irlovan.Message.GUIRecorderMessage.prototype.CreateSaveMessage = function (guiList) {
    var recorderDoc = Irlovan.Lib.XML.CreateXDocument(this.Name);
    var save = Irlovan.Lib.XML.CreateXElement(this.SaveTag);
    recorderDoc.documentElement.appendChild(save);
    for (var i = 0; i < guiList.length; i++) {
        save.appendChild(guiList[i]);
    }
    return Irlovan.Lib.XML.ToString(recorderDoc);
}