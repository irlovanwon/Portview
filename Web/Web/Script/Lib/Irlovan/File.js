//Copyright(c) 2015,HIT All rights reserved.
//Des:File Method Lib
//Author:Irlovan   
//Date:2015-10-23
//modification : 

Irlovan.Lib.File = {};

Irlovan.Lib.File.DownloadFile = function (filename, text) {
    var currentHrefName = 'a';
    var pom = document.createElement(currentHrefName);
    pom.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(text));
    pom.setAttribute('download', filename);
    if (document.createEvent) {
        var event = document.createEvent('MouseEvents');
        event.initEvent('click', true, true);
        pom.dispatchEvent(event);
    }
    else {
        pom.click();
    }
}