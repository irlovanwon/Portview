//Copyright(c) 2013,HIT All rights reserved.
//Des:
//Author:Irlovan   
//Date:2013-11-12
//modification :

Irlovan.Expression.ExpressionEditor = function (id, containerID, left, top, content, callback, dataSource) {
    this.ID = id;
    this.CallBack = callback;
    this.ContainerID = containerID;
    this.Pos = { left: left, top: top }
    this.AutoCompleteTagID = id + "_autocomplete";
    this.AutoCompleteFilterID = id + "_autocomplete_filter";
    this.TextTagID = id + "_text";
    this.ButtonConfirmID = id + "_button_confirm";
    this.Title = Irlovan.Global.ExpressionEditorTitle;
    this.Width = 600;
    this.Height = 200;
    this.Dispose();
    this.Init(id, containerID, this.Width, this.Height, this.Title, Irlovan.IrlovanHelper.Bind(this, this.Dispose));
    if (dataSource) {
        document.getElementById(this.AutoCompleteFilterID).value = dataSource;
    }
    this.AutoComplete(content, this.Filter, this.ID);
}
Irlovan.Expression.ExpressionEditor.prototype.AutoComplete = function (content, filter, id) {
    if (content) { document.getElementById(this.TextTagID).value = content; }
    this.UpdateSource();
    $("#" + this.AutoCompleteTagID).autocomplete({
        source: this.Source,
        select: function (event, ui) {
            var element = Irlovan.IrlovanHelper.XDocumentFromString(ui.item.value).documentElement;
            ui.item.value = element.getAttribute("Name");
        },
        open: function () {
            $('.ui-autocomplete').width(1300)
            $('.ui-autocomplete .ui-menu-item').css('fontSize', '14px');
        }
    });
    document.getElementById(this.ButtonConfirmID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, this.ConfirmHandler), false);
    document.getElementById(this.AutoCompleteTagID).addEventListener("click", Irlovan.IrlovanHelper.Bind(this, function () {
        this.UpdateSource();
        $("#" + this.AutoCompleteTagID).autocomplete("option", "source", this.Source);
    }), false);
}
Irlovan.Expression.ExpressionEditor.prototype.UpdateSource = function () {
    this.Source = null;
    this.Source = [];
    this.Source = Irlovan.IrlovanHelper.CloneArr(Irlovan.Global.RealtimeDataList);
    var value = document.getElementById(this.AutoCompleteFilterID).value;
    if (value) {
        for (var i = 0; i < value.split(",").length; i++) {
            this.Source = this.Filter(value.split(",")[i], this.Source);
        }
    }
}
Irlovan.Expression.ExpressionEditor.prototype.Filter = function (val, source) {
    var result = [];
    for (var i = 0; i < source.length; i++) {
        if (source[i].toLowerCase().indexOf(val.toLowerCase()) != -1) {
            result.push(source[i]);
        }
    }
    source = null;
    return result;
}
Irlovan.Expression.ExpressionEditor.prototype.Init = function (id, containerID, width, height, title, onClose) {
    Irlovan.ControlHelper.CreateControlByStr(
    "<div id='" + this.ID + "' style='background-color: white;'>" +
    "<p><label for='culture'>Select:</label>" +
    "<input type='text' style='width:" + (width - 160) + "px" + ";' id='" + this.AutoCompleteTagID + "'/>" +
    "<button type='button' id='" + this.ButtonConfirmID + "' width='80px' height='20px'>" + Irlovan.Language.Submit + "</button>" +
    "</p>" +
    "<p><label for='culture'>Filter   :</label>" +
    "<input type='text' style='width:" + (width - 160) + "px" + ";' id='" + this.AutoCompleteFilterID + "'/>" +
    "</p>" +
    "<textarea id='" + this.TextTagID + "' rows='5' cols='7' wrap='no/off' style='width:" + (width) + "px" + ";height:" + (height) + "px" + "'/>" +
    "</div>", this.ContainerID, null);
    $("#" + this.ID).dialog({
        width: "auto",
        height: "auto",
        title: title,
        close: function (event, ui) {
            onClose();
        }
    });
}
Irlovan.Expression.ExpressionEditor.prototype.ConfirmHandler = function () {
    if ((document.getElementById(this.TextTagID).value == "") || (document.getElementById(this.TextTagID).value == " ")) {
        this.CallBack(document.getElementById(this.AutoCompleteTagID).value);
    } else {
        this.CallBack(document.getElementById(this.TextTagID).value);
    }
    this.Dispose();
}
Irlovan.Expression.ExpressionEditor.prototype.Dispose = function () {
    if (document.getElementById(this.ButtonConfirmID)) {
        document.getElementById(this.ButtonConfirmID).removeEventListener("click", Irlovan.IrlovanHelper.Bind(this, this.ConfirmHandler), false);
        $("#" + this.AutoCompleteTagID).autocomplete("destroy");
    }
    $("#" + this.ID).dialog("destroy");
    if (document.getElementById(this.ID)) {
        Irlovan.ControlHelper.DeleteControl(this.ID);
    }
}