//Copyright(c) 2014,HIT All rights reserved.
//Des:zoom
//Author:Irlovan   
//Date:2014-07-15
//modification :

Irlovan.Zoom = function (width, height) {
    var container = document.getElementById(Irlovan.Global.ControlContainerID);
    if (Irlovan.Global.IsFullScreen) {
        if (/firefox/.test(navigator.userAgent.toLowerCase())) {
            //document.getElementById("body").style.overflow = "hidden";
            container.style.cssText = container.style.cssText + ";transform:scale(" + screen.width / width + "," + screen.height / height + ");transform-origin:" + Irlovan.Global.Zoom.Origin + ";"
        }
        if (/webkit/.test(navigator.userAgent.toLowerCase())) {
            container.style.cssText = container.style.cssText + ";-webkit-transform:scale(" + screen.width / width + "," + (screen.height / height) + ");-webkit-transform-origin:" + Irlovan.Global.Zoom.Origin + ";"
        } else {
            document.getElementById(Irlovan.Global.BodyID).style.overflow = "hidden";
            container.style.cssText = container.style.cssText + ";-ms-transform:scale(" + screen.width / width + "," + screen.height / height + ");transform-origin:" + Irlovan.Global.Zoom.Origin + ";"
        }
    } else {
        if (/firefox/.test(navigator.userAgent.toLowerCase())) {
            container.style.cssText = container.style.cssText + ";transform:scale(" + Irlovan.Global.Zoom.Scale + ");transform-origin:" + Irlovan.Global.Zoom.Origin + ";"
        }
        if (/webkit/.test(navigator.userAgent.toLowerCase())) {
            container.style.cssText = container.style.cssText + ";-webkit-transform:scale(" + Irlovan.Global.Zoom.Scale + ");-webkit-transform-origin:" + Irlovan.Global.Zoom.Origin + ";"
        } else {
            container.style.cssText = container.style.cssText + ";transform:scale(" + Irlovan.Global.Zoom.Scale + ");transform-origin:" + Irlovan.Global.Zoom.Origin + ";"
        }
    }
}
Irlovan.ZoomDiv = function (width, height, container) {
    if (Irlovan.Global.IsFullScreen) {
        if (/firefox/.test(navigator.userAgent.toLowerCase())) {
            //document.getElementById("body").style.overflow = "hidden";
            container.style.cssText = container.style.cssText + ";transform:scale(" + screen.width / width + "," + screen.height / height + ");transform-origin:" + Irlovan.Global.Zoom.Origin + ";"
        }
        if (/webkit/.test(navigator.userAgent.toLowerCase())) {
            container.style.cssText = container.style.cssText + ";-webkit-transform:scale(" + screen.width / width + "," + (screen.height / height) + ");-webkit-transform-origin:" + Irlovan.Global.Zoom.Origin + ";"
        } else {
            document.getElementById(Irlovan.Global.BodyID).style.overflow = "hidden";
            container.style.cssText = container.style.cssText + ";-ms-transform:scale(" + screen.width / width + "," + screen.height / height + ");transform-origin:" + Irlovan.Global.Zoom.Origin + ";"
        }
    } else {
        if (/firefox/.test(navigator.userAgent.toLowerCase())) {
            container.style.cssText = container.style.cssText + ";transform:scale(" + Irlovan.Global.Zoom.Scale + ");transform-origin:" + Irlovan.Global.Zoom.Origin + ";"
        }
        if (/webkit/.test(navigator.userAgent.toLowerCase())) {
            container.style.cssText = container.style.cssText + ";-webkit-transform:scale(" + Irlovan.Global.Zoom.Scale + ");-webkit-transform-origin:" + Irlovan.Global.Zoom.Origin + ";"
        } else {
            container.style.cssText = container.style.cssText + ";transform:scale(" + Irlovan.Global.Zoom.Scale + ");transform-origin:" + Irlovan.Global.Zoom.Origin + ";"
        }
    }
}
