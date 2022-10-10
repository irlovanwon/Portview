//Copyright(c) 2013,HIT All rights reserved.
//Des:XML Method Lib
//Author:Irlovan   
//Date:2015-03-19
//modification : 

Irlovan.Lib.XML = {};

/**CreateXDocument**/
Irlovan.Lib.XML.CreateXDocument = function (name) {
    return $.parseXML("<" + name + "/>");
    //return (new DOMParser()).parseFromString("<" + name + "/>", 'text/xml');
}

/**CreateXElement**/
Irlovan.Lib.XML.CreateXElement = function (name) {
    var doc = document.implementation.createDocument("", "", null);
    return doc.createElement(name);
}

/**Parse XML from string**/
Irlovan.Lib.XML.ParseFromString = function (message) {
    if (window.DOMParser) {
        var parser = new DOMParser();
        try {
            var result = parser.parseFromString(message, "text/xml");
        } catch (e) {
            return null;
        }
        return result;
    }
        // Internet Explorer
    else {
        var xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
        xmlDoc.async = false;
        xmlDoc.loadXML(message);
        return xmlDoc;
    }
}

/**ToString**/
Irlovan.Lib.XML.ToString = function (doc) {
    return (new XMLSerializer()).serializeToString(doc);
}