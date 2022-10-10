//Copyright(c) 2015,HIT All rights reserved.
//Des:VURMenu
//Author:Irlovan   
//Date:2015-08-31
//modification :

Irlovan.Menu.VURMenu = function () {
    this.VURMenuContainerID = "PortviewVUR_Container";
    this.VURMenuBackgroundID = "PortviewVUR_Container_Background";
    this.TimeStampID = "PortviewVUR_Container_TimeStamp";
    this.DatePickerClass = "PortviewVUR_Container_DateTimePicker_Class";
    this.ButtonPlayID = "PortviewVUR_Container_Button_Play";
    this.ButtonRewindID = "PortviewVUR_Container_Button_Rewind";
    this.ButtonForwardID = "PortviewVUR_Container_Button_Forward";
    this.SpeedTagID = "PortviewVUR_Container_speed_tag";
    this.TimeStamp;
    this.SpeedScale = 1;
    this.MaxSpeedScale = 6;
    this.Interval = Irlovan.Global.VURInterval;
    this.RecorderHandler = new Irlovan.Handler.RecorderHandler();
    this.CreateVURContainer();
    this.EventInit(Irlovan.IrlovanHelper.Bind(this, function (selectedDateTime, e) {
        this.TimeStamp = selectedDateTime;
    }));
}

Irlovan.Menu.VURMenu.prototype.CreateVURContainer = function () {
    Irlovan.ControlHelper.CreateControlByStr(
    "<div id='" + this.VURMenuContainerID + "' style='z-index:10000;position: absolute;width:100px;top:600px;left:500px'>" +
    "<div id='" + this.VURMenuBackgroundID + "' style='background-color:white;position: absolute;width:400px;;border:4px solid black;height:40px; top: 20px;left:-10px;' />" +
    "<input type='text' style='position:absolute;top: 35px;left: 0px;' id='" + this.TimeStampID + "' class='" + this.DatePickerClass + "'/>" +
    "<div style='background-image:url(Images/VUR/rewind.png);background-size:100% 100%;position:absolute;left: 210px;top:30px;width:30px;height:30px;' id='" + this.ButtonRewindID + "' />" +
    "<div style='background-image:url(Images/VUR/play.png);background-size:100% 100%;position:absolute;left: 250px;top:30px;width:30px;height:30px;' id='" + this.ButtonPlayID + "' />" +
    "<div style='background-image:url(Images/VUR/forward.png);background-size:100% 100%;position:absolute;left: 290px;top:30px;width:30px;height:30px;' id='" + this.ButtonForwardID + "' />" +
    "<div style='position:absolute;left: 340px;top:35px;width:30px;height:30px;' id='" + this.SpeedTagID + "' >x1</div>" +
    "</div>", Irlovan.Global.BodyID, null);
    this.MakeDragable();
}

Irlovan.Menu.VURMenu.prototype.TimeStampCallback = function (timeStamp) {
    document.getElementById(this.TimeStampID).value = timeStamp;
    this.TimeStamp = timeStamp;
}

Irlovan.Menu.VURMenu.prototype.EventInit = function (onDatePickerSelect) {
    $('.' + this.DatePickerClass).datetimepicker({
        timeFormat: 'HH:mm:ss',
        stepHour: 1,
        stepMinute: 1,
        stepSecond: 1,
        onSelect: onDatePickerSelect
    });
    $('.' + this.DatePickerClass).datepicker("option", "dateFormat", "yy'-'mm'-'dd'");
    this.PlayEventInit();
    this.ForwardEventInit();
    this.RewindEventInit();
}

Irlovan.Menu.VURMenu.prototype.PlayEventInit = function () {
    document.getElementById(this.ButtonPlayID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        if (document.getElementById(this.ButtonPlayID).style.backgroundImage.indexOf("play.png") > -1) { this.Play(); } else { this.Pause(); }
    }), false);
}
Irlovan.Menu.VURMenu.prototype.Play = function () {
    document.getElementById(this.ButtonPlayID).style.backgroundImage = "url('Images/VUR/pause.png')";
    this.Interval = parseFloat(Irlovan.Global.VURInterval) * (1.0 / this.SpeedScale);
    this.RecorderHandler.VURRecorder.Play(Irlovan.Global.VURRecorderName, this.TimeStamp, this.Interval, Irlovan.IrlovanHelper.Bind(this, this.TimeStampCallback));
}
Irlovan.Menu.VURMenu.prototype.Pause = function () {
    document.getElementById(this.ButtonPlayID).style.backgroundImage = "url('Images/VUR/play.png')";
    this.RecorderHandler.VURRecorder.Pause(Irlovan.Global.VURRecorderName);
}
Irlovan.Menu.VURMenu.prototype.Keep = function () {
    if (document.getElementById(this.ButtonPlayID).style.backgroundImage.indexOf("play.png") > -1) {
        this.Pause();
    } else {
        this.Play();
    }
}
Irlovan.Menu.VURMenu.prototype.ForwardEventInit = function () {
    document.getElementById(this.ButtonForwardID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        if (this.SpeedScale >= this.MaxSpeedScale) { this.SpeedScale = this.MaxSpeedScale; }
        else { this.SpeedScale = (this.SpeedScale < 1) ? (1.0 / ((1.0 / this.SpeedScale) - 1)) : (this.SpeedScale + 1); }
        this.SpeedScaleChange();
    }), false);
}
Irlovan.Menu.VURMenu.prototype.RewindEventInit = function () {
    document.getElementById(this.ButtonRewindID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function (event) {
        if (this.SpeedScale <= (1.0 / this.MaxSpeedScale)) { this.SpeedScale = (1.0 / this.MaxSpeedScale); }
        else { this.SpeedScale = (this.SpeedScale <= 1) ? (1.0 / ((1.0 / this.SpeedScale) + 1)) : (this.SpeedScale - 1); }
        this.SpeedScaleChange();
    }), false);
}
Irlovan.Menu.VURMenu.prototype.SpeedScaleChange = function () {
    this.Pause();
    document.getElementById(this.SpeedTagID).innerHTML = "x" + this.SpeedScale.toFixed(2);
}
Irlovan.Menu.VURMenu.prototype.MakeDragable = function () {
    Irlovan.ControlHelper.MakeControlDragable(this.VURMenuContainerID);
}