//Copyright(c) 2012,VeSim All rights reserved.
//Script for .html
//Irlovan 
//2013-03-21
//the event define by me
Irlovan.Event = {   
    Send: function (target, eventType, data) {
        if (typeof target == "string") target = document.getElementById(target);
        //DOM event model
        if (document.createEvent) {
            var e = document.createEvent('Events');
            e.initEvent(eventType, true, true, "detail");
        }
        //IE event model
        else if (document.createEventObject) {
            var e = document.createEventObject();
        }
        else return;
        e.data = data;
        //DOM&IE
        if (target.dispatchEvent) { target.dispatchEvent(e); }
        else if (target.fireEvent) { target.fireEvent("on"+eventType,e);}
    },
    Receive: function (target,eventType, handler) {
        if (typeof target == "string") target = document.getElementById(target);
        //I Fuck U Mozilla DOM
        if (target.addEventListener) {
            target.addEventListener(eventType, handler, false);
        }
        //I Fuck U Microsoft IE event model
        else if (target.attachEvent) {
            target.attachEvent("on"+eventType,handler);
        }
    }
};

