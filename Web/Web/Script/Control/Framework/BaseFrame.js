//Copyright(c) 2013,HIT All rights reserved.
//Des:BaseFrame
//Author:Irlovan   
//Date:2013-05-13
//modification :

Irlovan.HtmlFramework.ControlFrame = function (id, containerID, pos, width, height) {
    this.Width = width;
    this.Height = height;
    this.ID = id;
    this.ControlContainerID = id + "_controlcontainer";
    this.ContainerID = containerID;
    this.Pos = pos;
    this.Init();
}

Irlovan.HtmlFramework.ControlFrame.prototype.Init = function () {
    Irlovan.ControlHelper.DeleteControl(this.ID);
    var module = Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.ID + "' style='position: relative;'>" +
       "<div style='position: relative;height:" + Irlovan.Global.ControlMenuHeight + "'/>" +
       "</div>", this.ContainerID, this.Pos);
}


