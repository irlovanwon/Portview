//Copyright(c) 2013,HIT All rights reserved.
//Des:MenuFrame
//Author:Irlovan   
//Date:2013-07-15
//modification :

Irlovan.HtmlFramework.ControlFrame = function (id, containerID, pos, menuContainerID, closeMenuTag, activeMenuTag,width, height) {
    this.Width = width;
    this.Height = height;
    this.ID = id;
    this.MenuContainerID = menuContainerID;
    this.CloseMenuTag = closeMenuTag;
    this.ActiveMenuTag = activeMenuTag;
    this.ContainerID = containerID;
    this.Pos = pos;
    this.Init();
}

Irlovan.HtmlFramework.ControlFrame.prototype.Init = function () {
    Irlovan.ControlHelper.DeleteControl(this.ID);
    var module = Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.ID + "' style='position: relative;'>" +
       "<div id='" + this.MenuContainerID + "' style='position: relative;height:" + Irlovan.Global.ControlMenuHeight + "'>" +
       "<div id='" + this.CloseMenuTag + "' style='position: relative;width:22px;height:22px;background-image: url(Images/Tag/Close_lit.png);right:0px;top:0%;'></div>" +
       "<div id='" + this.ActiveMenuTag + "' style='position: relative;width:22px;height:22px;background-image: url(Images/Tag/Active_lit.png);right:-20px;top:-95%;'></div>" +
       "</div>" +
       "</div>", this.ContainerID, this.Pos);
    document.getElementById(this.MenuContainerID).style.visibility = "hidden";
    Irlovan.ControlHelper.MakeControlDragable(this.ID);
}

