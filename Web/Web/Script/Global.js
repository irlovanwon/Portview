//Copyright(c) 2013,HIT All rights reserved.
//Des:Global.js
//Author:Irlovan   
//Date：2015-09-22
//modification :


//$.browser.mozilla = /firefox/.test(navigator.userAgent.toLowerCase());
//$.browser.webkit = /webkit/.test(navigator.userAgent.toLowerCase());
//$.browser.opera = /opera/.test(navigator.userAgent.toLowerCase());
//$.browser.msie = /msie/.test(navigator.userAgent.toLowerCase());

Irlovan.Global = {
    Port: "501",
    CurrentPage: "skyview",
    PageTo: "skyview",
    IsFullScreen: true,
    //if <=0 never reconnect when losting communication
    ServerTimeout: 2000,
    Overflow: "auto",//hidden,auto
    Zoom: {
        Origin: "0% 0%",
        Scale: "1,1"
    },
    Resolution: {
        Width: 1440,
        Height: 900
    },
    Edition: 1,
    EditionMode: {
        Admin: 1,
        Operator: 2
    },
    EventLevel: {
        "TRIP": "purple,white",
        "1": "red,white",
        "PERM": "blue,white",
        "FLT": "green,white",
        "MSG": "brown,white"
    },
    //Accelarate our loading speed!!!!
    GUICache: {
        "skyview": false,
    },
    VURRecorderName: "Test",
    VURInterval: 501,
    HistoryEventColor: "Black,white",
    Domain_GUI: "localhost",//建議請勿設為localhost 否則遠程無法訪問
    Domain_Message: "localhost",//建議請勿設為localhost 否則遠程無法訪問
    Domain_Notification: "localhost",//建議請勿設為localhost 否則遠程無法訪問
    Domain_CMD: "localhost",//建議請勿設為localhost 否則遠程無法訪問
    Domain_Recorder: "localhost",//建議請勿設為localhost 否則遠程無法訪問
    //if>0 hybridmode if==0 LightMode(Recommend) if<0 intervalMode
    DataInterval: 100,
    BodyID: "body",
    ControlContainerID: "MainControlContainer",
    ReloadBarID: "ReloadBar",
    GridContainerID: "controlgrid",
    ExpressionEditorID: "ExpressionEditor",
    PropertyGridContainerID: "PortviewPropertyGridContainer",
    ControlSelectorID: "controlselector",
    ExpressionEditorTitle: "Expression",
    OperatorModePosFix: "0px",
    ServerConnected: false,
    ControlList: [],
    GUIList: [],
    GUI: null,
    UIContainer: {},
    RealtimeDataList: [],
    MessageHandler: null,
    LoadBar: null,
    PropertyGrid: null,
    Layout: null,
    ControlMenuHeight: "24px",
    VURMenu:null
}

Irlovan.Global.Domain_GUI = Irlovan.Global.Domain_GUI + ":" + Irlovan.Global.Port;
Irlovan.Global.Domain_Message = Irlovan.Global.Domain_Message + ":" + Irlovan.Global.Port;
Irlovan.Global.Domain_Notification = Irlovan.Global.Domain_Notification + ":" + Irlovan.Global.Port;
Irlovan.Global.Domain_CMD = Irlovan.Global.Domain_CMD + ":" + Irlovan.Global.Port;
Irlovan.Global.Domain_Recorder = Irlovan.Global.Domain_Recorder + ":" + Irlovan.Global.Port;



