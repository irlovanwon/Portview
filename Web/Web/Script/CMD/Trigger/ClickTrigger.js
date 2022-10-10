//Copyright(c) 2015,HIT All rights reserved.
//Des:Trigger
//Author:Irlovan   
//Date:2015-04-01
//modification :

Irlovan.CMD.ClickTrigger = function (triggerName, id, idList, action) {
    this.Name = "click";
    this.InitState= (triggerName == this.Name);
    if (!this.InitState) { return; }
    this.ID = id;
    this.IDList = idList;
    this.Action = action;
    this.TriggerType = triggerType;
}

/**Init**/
Irlovan.CMD.ClickTrigger.prototype.Init = function () {
    document.getElementById(this.ID).addEventListener("click", this.Action, false);
}

/**Dispose**/
Irlovan.CMD.ClickTrigger.prototype.Dispose = function () {
    document.getElementById(this.ID).removeEventListener("click", Irlovan.IrlovanHelper.Bind(this, this.Action), false);
}
