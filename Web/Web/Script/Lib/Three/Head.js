//Copyright(c) 2013,HIT All rights reserved.
//Des:Head.js
//Author:Irlovan   
//Date:2013-05-22
//modification :

/*
*using
*CSS:
*JQueryBase:
*JQueryExtention:
*UserDefine
<script src="Script/Lib/Three/Head.js" type="text/javascript"></script>
*/
Irlovan.ThreeJS = {}
Irlovan.ThreeJS.Include = function () {
    var pathtojsfiles = "Script/Lib/Three/Lib/"; // need to be ajusted
    // set include to false if you do not want some modules to be included
    var modules = [
        { include: true, incfile: 'src/Three.js' },
        { include: true, incfile: 'src/loaders/JSONLoader.js' }
    ];
    var filename;
    for (var i = 0; i < modules.length; i++) {
        if (modules[i].include === true) {
            filename = pathtojsfiles + modules[i].incfile;
            if (jQuery.browser.safari) {
                jQuery.ajax({ url: filename, dataType: 'script', async: false, cache: true });
            } else {
                if (jQuery.browser.msie) {
                    document.write('<script charset="utf-8" type="text/javascript" src="' + filename + '"></script>');
                } else {
                    IncludeJavaScript(filename);
                }
            }
        }
    }
    function IncludeJavaScript(jsFile) {
        var oHead = document.getElementsByTagName('head')[0];
        var oScript = document.createElement('script');
        oScript.setAttribute('type', 'text/javascript');
        oScript.setAttribute('language', 'javascript');
        oScript.setAttribute('src', jsFile);
        oHead.appendChild(oScript);
    }
}
Irlovan.ThreeJS.Include();

