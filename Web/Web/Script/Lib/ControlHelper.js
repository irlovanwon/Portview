//Copyright(c) 2013,HIT All rights reserved.
//Des:ControlHelper
//Author:Irlovan   
//Date:2013-03-22
//modification :

/*
*using
*JQueryBase:<script src="Script/Control/Lib/jquery-1.9.1.js" type="text/javascript"></script>
*JQueryExtention:<script src="Script/Control/Lib/jquery-ui.custom.js" type="text/javascript"></script>
*UserDefine:
<script src="Script/Lib/IrlovanHelper.js" type="text/javascript"></script>
<script src="Script/Lib/ControlHelper.js" type="text/javascript"></script>
*/


Irlovan.ControlHelper = {
    GetElement: function (target) {
        if (typeof target == "string") {
            if (document.getElementById(target)) {
                target = document.getElementById(target);
            } else {
                var id = target;
                target = document.createElement('div');
                target.id = id;
            }
        }
        return target;
    },
    MakeControlDragable: function (id) {
        $(document).ready(function () {
            $(function () {
                $("#" + id).draggable({ disabled: false });
            });
        })
    },
    MakeControlUnDragable: function (id) {
        $(document).ready(function () {
            $(function () {
                $("#" + id).draggable({ disabled: true });
                $("#" + id).draggable("destroy");
            });
        })
    },
    ClearControl: function (control) {
        control = Irlovan.ControlHelper.GetElement(control);
        while (control.hasChildNodes()) { control.removeChild(control.firstChild); }
    },
    //delete a control from it's container
    DeleteControl: function (control) {
        control = Irlovan.ControlHelper.GetElement(control);
        if (control.parentElement) {
            control.parentElement.removeChild(control);
        }
    },
    //target is the container ,element is the element to be added
    AppendElement: function (target, child, pos) {
        target = Irlovan.ControlHelper.GetElement(target);
        child = Irlovan.ControlHelper.GetElement(child);
        if (child.parentElement != target) {
            target.appendChild(child);
        }
        if (pos) {
            child.style.left = pos.left;
            child.style.top = pos.top;
        }
    },
    CreateButton: function (id, containerId, context, onclick, pos, css) {
        var button = document.createElement("input");
        button.type = "button";
        button.id = id;
        button.value = context;
        if (onclick) {
            button.onclick = onclick;
        }
        Irlovan.ControlHelper.AppendElement(containerId, button, pos);
        return button;
    },
    CreateControlByStr: function (str, containerID, pos) {
        var xElement = Irlovan.IrlovanHelper.XDocumentFromString(str).childNodes[0];
        var control = this.CreateControlByXElement(xElement);
        if (containerID) { this.AppendElement(containerID, control, pos); }
        return control;
    },
    CreateControlByXElement: function (doc) {
        if (doc.nodeName == "#text") { return doc.textContent; }
        var control = document.createElement(doc.nodeName);
        for (var i = 0; i < doc.attributes.length; i++) {
            //eval("control." + doc.attributes[i].name + "='" + doc.attributes[i].value + "'");
            control.setAttribute(doc.attributes[i].name, doc.attributes[i].value);
        }
        for (var i = 0; i < doc.childNodes.length; i++) {
            if (doc.childNodes[i].nodeName == "#text") {
                control.innerHTML = doc.childNodes[i].textContent;
            } else {
                control.appendChild(this.CreateControlByXElement(doc.childNodes[i]));
            }
        }
        return control;
    },
    ControlVisible: function (idList, visible) {
        var ids = idList.split(",");
        for (var i = 0; i < ids.length; i++) {
            var control = document.getElementById(ids[i]);
            if (!control) { continue; }
            if ((control.childElementCount != 1) && control.lastChild) {
                control.lastChild.style.visibility = ((visible == true) || (visible == "true")) ? "visible" : "hidden";
            } else {
                control.style.visibility = ((visible == true) || (visible == "true")) ? "visible" : "hidden";
            }
        }
    },
    RevVisible: function (idList) {
        var ids = idList.split(",");
        for (var i = 0; i < ids.length; i++) {
            var control = document.getElementById(ids[i]);
            if (!control) { continue; }
            if ((control.childElementCount != 1) && control.lastChild) {
                control.lastChild.style.visibility = (control.lastChild.style.visibility == "visible") ? "hidden" : "visible";
            } else {
                control.style.visibility = (control.style.visibility == "visible") ? "hidden" : "visible";
            }
        }
    },
    MouseHover: function (id, info) {
        var infos = info.split(";");
        if (infos.length != 2) { return; }
        var visible = infos[0].split(",");
        if (visible.length != 2) { return; }
        var control = document.getElementById(id);
        if (!control) { return; }
        if (visible[0] == visible[1]) {
            $("#" + id).on(visible[0], function () {
                Irlovan.ControlHelper.RevVisible(infos[1]);
            })
        } else {
            $("#" + id).on(visible[0], function () {
                Irlovan.ControlHelper.ControlVisible(infos[1], true);
            })
            $("#" + id).on(visible[1], function () {
                Irlovan.ControlHelper.ControlVisible(infos[1], false);
            })
        }

    }
}
