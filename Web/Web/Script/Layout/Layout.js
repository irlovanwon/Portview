//Copyright(c) 2013,HIT All rights reserved.
//Des:Layout.js
//Author:Irlovan   
//Date:2013-05-24
//modification :

Irlovan.Layout = function () {
    this._span;
    this._layoutPos;
    this._isLeftPressed = false;
    this._isTopPressed = false;
    this._isCtrlPressed = false;
    this._isCKeyPressed = false;
    this._isVKeyPressed = false;
    this._isMKeyPressed = false;
    this._isXKeyPressed = false;
    this._isWKeyPressed = false;
    this._isAKeyPressed = false;
    this._isDeletePressed = false;
    this._isCast = false;
    this._controlList = [];
    this._idNum = 0;
    this._keyDownEventListen();
    this.KeyMap = {
        CtrlKey: 17,
        UpArrow: 38,
        LeftArrow: 37,
        CKey: 67,
        MKey: 77,
        VKey: 86,
        XKey: 88,
        WKey: 87,
        AKey: 65,
        Delete: 46
    }
}

Irlovan.Layout.prototype.Layout = function (e) {
    //if ((this._isLeftPressed == true) || (this._isTopPressed == true)) { this._layout(e); }
    this._layout(e);
}
Irlovan.Layout.prototype._layout = function (e) {
    this.Alignment(e);
    this.Select(e);
}
Irlovan.Layout.prototype._keyDownEventListen = function () {
    document.addEventListener("keydown", Irlovan.IrlovanHelper.Bind(this, function (e) {
        switch (e.keyCode) {
            case this.KeyMap.WKey:
                this._isWKeyPressed = true;
                break;
            case this.KeyMap.AKey:
                this._isAKeyPressed = true;
                break;
            case this.KeyMap.CtrlKey:
                this._isCtrlPressed = true;
                break;
            case this.KeyMap.MKey:
                this._isMKeyPressed = true;
                this.MCopy();
                break;
            case this.KeyMap.CKey:
                this._isCKeyPressed = true;
                this.Copy();
                break;
            case this.KeyMap.VKey:
                this._isVKeyPressed = true;
                this.Paste();
                break;
            case this.KeyMap.Delete:
                this._isDeletePressed = true;
                this.Delete();
                break;
            case this.KeyMap.XKey:
                this._isXKeyPressed = true;
                this.Cast();
                break;
            default:
                break;
        }
    }), false);
    document.addEventListener("keyup", Irlovan.IrlovanHelper.Bind(this, function (e) {
        switch (e.keyCode) {
            case this.KeyMap.WKey:
                this._isWKeyPressed = false;
                this._span = null;
                this._layoutPos = null;
                break;
            case this.KeyMap.AKey:
                this._isAKeyPressed = false;
                this._span = null;
                this._layoutPos = null;
                break;
            case this.KeyMap.CtrlKey:
                this._isCtrlPressed = false;
                break;
            case this.KeyMap.CKey:
                this._isCKeyPressed = false;
                break;
            case this.KeyMap.VKey:
                this._isVKeyPressed = false;
                break;
            case this.KeyMap.XKey:
                this._isXKeyPressed = false;
                break;
            case this.KeyMap.Delete:
                this._isDeletePressed = false;
                break;
            default:
                break;
        }
    }), false);
}
Irlovan.Layout.prototype._getElement = function (element) {
    while (element.parentNode.id != Irlovan.Global.ControlContainerID) {
        element = element.parentNode;
    }
    return element;
}

//for top left alignment
Irlovan.Layout.prototype.Alignment = function (e) {
    //doing a repaire for the innerdiv click problem
    if ((!this._isAKeyPressed) && (!this._isWKeyPressed)) { return; }
    var element = this._getElement(e.target);
    if (!this._layoutPos) {
        this._layoutPos = {};
        this._layoutPos.left = element.style.left;
        this._layoutPos.top = element.style.top;
        return;
    } else if ((this._layoutPos) && (!this._span)) {
        var left = Irlovan.IrlovanHelper.ParseFloatByDOM(element.style.left);
        var top = Irlovan.IrlovanHelper.ParseFloatByDOM(element.style.top);
        var originLeft = Irlovan.IrlovanHelper.ParseFloatByDOM(this._layoutPos.left);
        var originTop = Irlovan.IrlovanHelper.ParseFloatByDOM(this._layoutPos.top);
        if (this._isAKeyPressed == true) {
            document.getElementById(element.id).style.left = originLeft + "px";
            this._span = top - originTop;
        }
        if (this._isWKeyPressed == true) {
            document.getElementById(element.id).style.top = originTop + "px";
            this._span = left - originLeft;
        }
        this._layoutPos.left = element.style.left;
        this._layoutPos.top = element.style.top;
    } else if ((this._layoutPos) && (this._span)) {
        if (this._isAKeyPressed == true) {
            document.getElementById(element.id).style.left = this._layoutPos.left;
            document.getElementById(element.id).style.top = (Irlovan.IrlovanHelper.ParseFloatByDOM(this._layoutPos.top) + this._span) + "px";
        }
        if (this._isWKeyPressed == true) {
            document.getElementById(element.id).style.top = this._layoutPos.top;
            document.getElementById(element.id).style.left = (Irlovan.IrlovanHelper.ParseFloatByDOM(this._layoutPos.left) + this._span) + "px";
        }
        this._layoutPos.left = element.style.left;
        this._layoutPos.top = element.style.top;
    }
}
//selected an element or multi select
Irlovan.Layout.prototype.Select = function (e) {
    if (this._isCtrlPressed) {
        //doing a repaire for the innerdiv click problem
        var element = this._getElement(e.target);
        var control = Irlovan.IrlovanHelper.GetControlByID(element.id);
        element.style.border = (control.Selected) ? "" : "1px solid black";
        element.style.overflowY = (control.Selected) ? "" : "scroll";
        element.style.overflowX = (control.Selected) ? "" : "scroll"
        control.Selected = (control.Selected) ? false : true;
    }
}
//CopyControl
Irlovan.Layout.prototype.Copy = function () {
    if ((this._isCtrlPressed) && (this._isCKeyPressed)) {
        this._controlList = null;
        this._controlList = [];
        for (var i = 0; i < Irlovan.Global.ControlList.length; i++) {
            if (Irlovan.Global.ControlList[i].Selected) {
                var para = [];
                para.push(Irlovan.Global.ControlList[i].Name);
                para.push(Irlovan.Global.ControlContainerID);
                para.push(Irlovan.Global.GridContainerID);
                para.push(Irlovan.Global.ControlList[i].Class);
                para.push(Irlovan.Global.ControlList[i].Config);
                para.push((parseFloat(Irlovan.Global.ControlList[i].Config.left.Value) + 20) + "px");
                para.push((parseFloat(Irlovan.Global.ControlList[i].Config.top.Value) + 20) + "px");
                para.push(Irlovan.Global.ControlList[i].zIndex);
                para.push(Irlovan.Global.ControlList[i].OnLayout);
                para.push(Irlovan.Global.ControlList[i].ID);
                var result = Irlovan.IrlovanHelper.CloneObj(para);
                this._controlList.push(result);

            }
        }
    }
}
//CopyControl
Irlovan.Layout.prototype.MCopy = function () {
    if ((this._isCtrlPressed) && (this._isMKeyPressed)) {
        this._controlList = null;
        this._controlList = [];
        for (var i = 0; i < Irlovan.Global.ControlList.length; i++) {
            if (Irlovan.Global.ControlList[i].Selected) {
                var para = [];
                para.push(Irlovan.Global.ControlList[i].Name);
                para.push(Irlovan.Global.ControlContainerID);
                para.push(Irlovan.Global.GridContainerID);
                para.push(Irlovan.Global.ControlList[i].Class);
                para.push(Irlovan.Global.ControlList[i].Config);
                para.push((parseFloat(Irlovan.Global.ControlList[i].Config.left.Value)) + "px");
                para.push((parseFloat(Irlovan.Global.ControlList[i].Config.top.Value)) + "px");
                para.push(Irlovan.Global.ControlList[i].zIndex);
                para.push(Irlovan.Global.ControlList[i].OnLayout);
                para.push(Irlovan.Global.ControlList[i].ID);
                var result = Irlovan.IrlovanHelper.CloneObj(para);
                this._controlList.push(result);
            }
        }
    }
}
//CastControl
Irlovan.Layout.prototype.Cast = function () {
    if ((this._isCtrlPressed) && (this._isXKeyPressed)) {
        this._controlList = null;
        this._controlList = [];
        for (var i = 0; i < Irlovan.Global.ControlList.length; i++) {
            if (Irlovan.Global.ControlList[i].Selected) {
                var para = [];
                para.push(Irlovan.Global.ControlList[i].Name);
                para.push(Irlovan.Global.ControlContainerID);
                para.push(Irlovan.Global.GridContainerID);
                para.push(Irlovan.Global.ControlList[i].Class);
                para.push(Irlovan.Global.ControlList[i].Config);
                para.push((parseFloat(Irlovan.Global.ControlList[i].Config.left.Value) + 20) + "px");
                para.push((parseFloat(Irlovan.Global.ControlList[i].Config.top.Value) + 20) + "px");
                para.push(Irlovan.Global.ControlList[i].zIndex);
                para.push(Irlovan.Global.ControlList[i].OnLayout);
                para.push(Irlovan.Global.ControlList[i].ID);
                var result = Irlovan.IrlovanHelper.CloneObj(para);
                this._controlList.push(result);
                Irlovan.Global.ControlList[i].Dispose();
                Irlovan.Global.ControlList[i] = null;
                Irlovan.Global.ControlList.splice(i, 1);
                i--;
                this._isCast = true;
            }
        }
    }
}
//CastControl
Irlovan.Layout.prototype.Delete = function () {
    if (this._isDeletePressed) {
        for (var i = 0; i < Irlovan.Global.ControlList.length; i++) {
            if (Irlovan.Global.ControlList[i].Selected) {
                Irlovan.Global.ControlList[i].Dispose();
                Irlovan.Global.ControlList[i] = null;
                Irlovan.Global.ControlList.splice(i, 1);
                i--;
            }
        }
    }
}
//CopyControl
Irlovan.Layout.prototype.Paste = function () {
    if ((this._isCtrlPressed) && (this._isVKeyPressed) && (this._controlList.length != 0)) {
        var controlList = Irlovan.IrlovanHelper.CloneObj(this._controlList);
        for (var i = 0; i < controlList.length; i++) {
            var id = this.GetID(controlList[i][9]);
            var result = eval("new Irlovan.Control." + controlList[i][0] +
            "(id,controlList[i][1],controlList[i][2],controlList[i][3],controlList[i][4],controlList[i][5],controlList[i][6],null,controlList[i][8])");
            Irlovan.Global.ControlList.push(result);
        }
        if (this._isCast) {
            this._controlList = null;
            this._controlList = [];
        }
        this._isCast = false;
    }
}
Irlovan.Layout.prototype.GetID = function (name) {
    var id = name + "_p";
    while (document.getElementById(id)) { id = id + "_p"; }
    return id;
}

