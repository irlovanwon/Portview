//Copyright(c) 2013,HIT All rights reserved.
//Des:RealtimeData
//Author:Irlovan   
//Date:2013-05-13
//modification :2014-10-08

Irlovan.RealtimeData.ReadDataByXML = function (xDoc) {
    if (!xDoc) {return;}//2014-10-08
    var element = Irlovan.IrlovanHelper.XDocumentFromString(xDoc);
    if (element) {
        var result = {};
        var root = element.documentElement;
        result.Name = root.getAttribute("Name");
        result.Data = [];
        for (var i = 0; i < root.childNodes.length; i++) {
            if (root.childNodes[i].nodeName == "#text") { continue; }
            var attrs = root.childNodes[i].attributes;
            var child = {};
            for (var j = 0; j < attrs.length; j++) {
                child[attrs[j].name] = attrs[j].value;
            }
            result.Data.push(child);
        }
        return result;
    }
}
Irlovan.RealtimeData.ReadCSVDataByXML = function (xDoc) {
    var element = Irlovan.IrlovanHelper.XDocumentFromString(xDoc);
    if (element) {
        var result = [];
        var root = element.documentElement;
        var isHead = true;
        for (var i = 0; i < root.childNodes.length; i++) {
            if (root.childNodes[i].nodeName == "#text") { continue; }
            var attrs = root.childNodes[i].attributes;
            var child = [];
            if (isHead == true) {
                var head = [];
                for (var j = 0; j < attrs.length; j++) {
                    head.push(attrs[j].name);
                }
                result.push(head);
                isHead = false;
            }
            for (var j = 0; j < attrs.length; j++) {
                child.push(attrs[j].value.replace(" ",""));
            }
            result.push(child);
        }
        return result;
    }
}