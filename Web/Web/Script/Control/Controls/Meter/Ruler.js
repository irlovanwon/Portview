//Copyright(c) 2015,HIT All rights reserved.
//Des:Ruler
//Author:Irlovan   
//Date:2015-05-26
//modification :

Irlovan.Include.Using("Script/Control/Lib/jQRangeSlider5.7.1/css/iThing.css");
Irlovan.Include.Using("Script/Control/Lib/jQRangeSlider5.7.1/jQRangeSlider-withRuler-min.js");

Irlovan.Control.Ruler = function (id, containerID, gridContainerID, controlClass, config, left, top, zIndex, isLock) {
    Irlovan.Control.Classic.call(this, "Ruler", id, containerID, gridContainerID, controlClass, config ? config : ({
        minValue: { Attributes: "minValue", Value: 0 },
        maxValue: { Attributes: "maxValue", Value: 100 },
        width: { Attributes: "width", Value: '500' },
        height: { Attributes: "height", Value: '60' },
        arrows: { Attributes: "arrows", Value: 'true' },
        label: { Attributes: "label", Value: 'true' },
        minBound: { Attributes: "minBound", Value: 0 },
        maxBound: { Attributes: "maxBound", Value: 100 },
        minRange: { Attributes: "minRange", Value: 0 },
        maxRange: { Attributes: "maxRange", Value: 100 }
    }), left, top, zIndex, isLock);
    this.RulerID = id + "_ruler";
    this.RulerContainerID = id + "_ruler_container";
    this.Init(id, containerID);
    this.RulerInit();
}
Irlovan.Control.Ruler.prototype = new Irlovan.Control.Classic();
Irlovan.Control.Ruler.prototype.constructor = Irlovan.Control.Ruler;
//init div element
Irlovan.Control.Ruler.prototype.Init = function (id, containerID) {
    Irlovan.Control.Classic.prototype.Init.apply(this, [id, containerID]);
    var Ruler = Irlovan.ControlHelper.CreateControlByStr(
       "<div id='" + this.RulerContainerID + "' style='position:relative;width:" + this.Config.width.Value + "px;height:" + this.Config.height.Value + "px;'>" +
       "<div id='" + this.RulerID + "' style='position:absolute;width:" + this.Config.width.Value + "px;'/>" +
       "</div>", this.ID, this.Pos);
}
Irlovan.Control.Ruler.prototype.SetValue = function (name, colName, data) {
    Irlovan.Control.Classic.prototype.SetValue.apply(this, [name, colName, data]);
    switch (name) {
        case "minValue":
        case "maxValue":
            this.SetValueRange(this.Config.minValue.Value, this.Config.maxValue.Value);
            break;
        case "arrows":
            this.SetArrows(Irlovan.Lib.Help.Boolean(this.Config.arrows.Value));
            break;
        case "label":
            this.SetLabel(Irlovan.Lib.Help.Boolean(this.Config.label.Value));
            break;
        case "minBound":
        case "maxBound":
            this.SetBoundary(this.Config.minBound.Value, this.Config.maxBound.Value);
            break;
        case "minRange":
        case "maxRange":
            this.SetRange(this.Config.minRange.Value, this.Config.maxRange.Value);
            break;
        case "width":
        case "height":
            this.DisposeRuler();
            this.RulerInit();
            break;
        default:
            break;
    }
}
Irlovan.Control.Ruler.prototype.RulerInit = function () {
    document.getElementById(this.RulerID).style.width = this.Config.width.Value + "px";
    document.getElementById(this.RulerContainerID).style.height = this.Config.height.Value + "px";
    $("#" + this.RulerID).rangeSlider({
        scales: [
        // Primary scale
        {
            first: function (val) { return val; },
            next: function (val) { return val + 10; },
            stop: function (val) { return false; },
            label: function (val) { return val; },
        },
        // Secondary scale
        {
            first: function (val) { return val; },
            next: function (val) {
                if (val % 10 === 9) {
                    return val + 2;
                }
                return val + 1;
            },
            stop: function (val) { return false; },
            label: function (val) { return "_"; }
        }
        ]
    });
    this.SetArrows(Irlovan.Lib.Help.Boolean(this.Config.arrows.Value));
    this.SetLabel(Irlovan.Lib.Help.Boolean(this.Config.label.Value));
    this.SetBoundary(this.Config.minBound.Value, this.Config.maxBound.Value);
    this.SetRange(this.Config.minRange.Value, this.Config.maxRange.Value);
    this.SetValueRange(this.Config.minValue.Value, this.Config.maxValue.Value);
    $("#" + this.RulerID).rangeSlider({
        enabled: false
    });
}

/**enable or disable arrow**/
Irlovan.Control.Ruler.prototype.SetArrows = function (result) {
    if (result == null) { result = false; }
    $("#" + this.RulerID).rangeSlider({ arrows: result });
}

/**set boundary**/
Irlovan.Control.Ruler.prototype.SetBoundary = function (minBound, maxBound) {
    minBound = parseFloat(minBound);
    maxBound = parseFloat(maxBound);
    if (minBound >= maxBound) { minBound = 0; maxBound = 100; }
    $("#" + this.RulerID).rangeSlider({ bounds: { min: minBound, max: maxBound } });
}

/**set range**/
Irlovan.Control.Ruler.prototype.SetRange = function (minRange, maxRange) {
    minRange = parseFloat(minRange);
    maxRange = parseFloat(maxRange);
    if (minRange >= maxRange) { minRange = 0; maxRange = 100; }
    $("#" + this.RulerID).rangeSlider({ range: { min: minRange, max: maxRange } });
}

/**set value**/
Irlovan.Control.Ruler.prototype.SetValueRange = function (minValue, maxValue) {
    minValue = parseFloat(minValue);
    maxValue = parseFloat(maxValue);
    $("#" + this.RulerID).rangeSlider("values", minValue, maxValue);
}

/**set value**/
Irlovan.Control.Ruler.prototype.SetLabel = function (enable) {
    if (enable == null) { enable = "hide"; }
    else { enable = enable ? "show" : "hide"; }
    $("#" + this.RulerID).rangeSlider({ valueLabels: enable });
}

/**Dispose**/
Irlovan.Control.Ruler.prototype.Dispose = function () {
    Irlovan.Control.Classic.prototype.Dispose.apply(this, []);
    this.DisposeRuler();
}

/**DisposeRuler**/
Irlovan.Control.Ruler.prototype.DisposeRuler = function () {
    $("#" + this.RulerID).rangeSlider("destroy");
}

Irlovan.Control.Ruler.prototype.Off = function () {
    this.DisposeRuler();
    document.getElementById(this.RulerContainerID).style.backgroundColor = "black";
}
Irlovan.Control.Ruler.prototype.On = function () {
    document.getElementById(this.RulerContainerID).style.backgroundColor = "transparent";
    try { this.DisposeRuler(); } catch (e) { }
    this.RulerInit();
}