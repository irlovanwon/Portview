//Copyright(c) 2013,HIT All rights reserved.
//Des:
//Author:Irlovan   
//Date:
//modification :

Irlovan.IrlovanHelper = {
    //as:svg.addEventListener("DOMAttrModified", Irlovan.IrlovanHelper.Bind(this, this._attrHandle), false);
    //reference:http://stackoverflow.com/questions/183214/javascript-callback-scope
    //To bind "this"  Support:ECMAScript 5 
    //For browsers which do not support ES5 yet, MDN provides the following shim:
    //    if (!Function.prototype.bind) {  
    //  Function.prototype.bind = function (oThis) {  
    //      if (typeof this !== "function") {  
    //          // closest thing possible to the ECMAScript 5 internal IsCallable function  
    //          throw new TypeError("Function.prototype.bind - what is trying to be bound is not callable");  
    //      }  
    //      var aArgs = Array.prototype.slice.call(arguments, 1),   
    //          fToBind = this,   
    //          fNOP = function () {},  
    //          fBound = function () {  
    //              return fToBind.apply(this instanceof fNOP  
    //                                     ? this  
    //                                     : oThis || window,  
    //                                   aArgs.concat(Array.prototype.slice.call(arguments)));  
    //          };  
    //      fNOP.prototype = this.prototype;  
    //      fBound.prototype = new fNOP();  
    //      return fBound;  
    //  };  
    //} 
    Bind: function (scope, fn) {
        return function () {
            fn.apply(scope, arguments);
        };
    },
    GetControlByID: function (id) {
        for (var i = 0; i < Irlovan.Global.ControlList.length; i++) {
            if (id == Irlovan.Global.ControlList[i].ID) {
                return Irlovan.Global.ControlList[i];
            }
        }
    },
    CloneObj: function (obj) {
        if (!obj) { return null; }
        var o = obj.constructor === Array ? [] : {};
        for (var i in obj) {
            if (obj.hasOwnProperty(i)) {
                o[i] = typeof obj[i] === "object" ? this.CloneObj(obj[i]) : obj[i];
            }
        }
        return o;
    },
    CloneArr: function (arr) {
        var newArr = [];
        for (var i = 0; i < arr.length; i++) {
            if (typeof arr[i] === 'object') {
                newArr.push(this.CloneObj(arr[i]));
            } else {
                newArr.push(arr[i]);
            }
        }
        return newArr;
    },
    //DisconnectHandler : function () {
    //    Irlovan.Global.ServerConnected = false;
    //    var timeout = parseInt(Irlovan.Global.ServerTimeout);
    //    if (timeout > 0) {
    //        setTimeout(function () {
    //            $("#" + Irlovan.Global.ReloadBarID).css("visibility", "visible");
    //            location.reload();
    //        }, timeout)
    //    } else {
    //        alert("Disconnect");
    //    }
    //},
    /*=-=*********************************************************StringHelper****************************************************************=-=*/
    //return array of string with single, words, "fixed string of words"
    StringPureSplit: function (str) { return str.match(/\w+|"[^"]+"/g) },
    //In some case it's usefull
    StringBuilder: function () {
        this._strings = new Array;
        this.Append = function (str) { this._strings.push(str); }
        this.ToString = function () { return this._strings.join(""); }
    },

    /*=-=*********************************************************Dictionary&Hash****************************************************************=-=*/
    Dictionary: function () {
        this.Clear = function () { this.Dic = new Array(); }
        this.ContainsKey = function (key) {
            var exists = false;
            for (var i in this.Dic) {
                if (i == key && this.Dic[i] != null) {
                    exists = true;
                    break;
                }
            }
            return exists;
        };
        this.ContainsValue = function (value) {
            var contains = false;
            if (value != null) {
                for (var i in this.Dic) {
                    if (this.Dic[i] == value) {
                        contains = true;
                        break;
                    }
                }
            }
            return contains;
        }
        this.Value = function (key) { return this.Dic[key]; }
        this.IsEmpty = function () { return (this.Length == 0) ? true : false; }
        this.Add = function (key, value) {
            if (key == null || value == null) {
                throw 'NullPointerException {' + key + '},{' + value + '}';
            } else {
                if (!this.ContainsKey(key)) {
                    this.Dic[key] = value;
                    this.Length++;
                    this.Values.push(value);
                }
            }
        }
        this.Values = [];
        this.Remove = function (key) {
            var rtn = this.Dic[key];
            this.Values.Remove(this.Dic[key]);
            this.Dic[key] = null;
            this.Dic.splice(key, 1);
            this.Length--;
            return rtn;
        }
        this.Length = 0;
        this.ToString = function () {
            var result = '';
            for (var i in this.Dic) {
                if (this.Dic[i] != null)
                    result += '{' + i + '},{' + this.Dic[i] + '}/n';
            }
            return result;
        }
        this.Dic = new Array();
    },

    /*=-=*********************************************************XMLHelper****************************************************************=-=*/
    CreateXDocument: function () {
        return document.implementation.createDocument("", "", null);
    },
    CreateXElement: function (name) {
        return this.CreateXDocument().createElement(name);
    },
    //create xelement by a array contains tagname attributes
    CreateXElementByArray: function (xDocument, array) {
        var child = xDocument.createElement(array[0]);
        for (var i = 1; i < array.length; i += 2) {
            var attr = document.createAttribute(array[i]);
            attr.value = array[i + 1];
            child.setAttributeNode(attr);
        }
        return child;
    },
    XDocumentToString: function (doc) { return (new XMLSerializer()).serializeToString(doc) },
    CreateXDocument: function (root) { return $.parseXML("<" + root + "/>") },
    XDocumentFromString: function (str) {
        if (window.DOMParser) {
            var parser = new DOMParser();
            try {
                var result = parser.parseFromString(str, "text/xml");
            } catch (e) {
                return null;
            }
            return result;
        }
            // Internet Explorer
        else {
            var xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
            xmlDoc.async = false;
            xmlDoc.loadXML(str);
            return xmlDoc;
        }
    },
    IsXDocument: function (xDoc) {
        if (!xDoc) { return false; }
        if ((xDoc.documentElement.tagName == "parsererror") || (xDoc.documentElement.tagName == "html")) { return false; }
        return true;
    },
    JsonToString: function (objectLiteral) {
        return JSON.stringify(objectLiteral)
    },
    ErrorTextRepair: function (errorText) {
        return errorText.replace(/&(lt|gt|quot);/g, function (m, p) {
            return (p == "lt") ? "<" : (p == "gt") ? ">" : "'";
        });
    },
    PureTextRepair: function (errorText) {
        return errorText.replace(/[\ |\~|\`|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\-|\_|\+|\=|\||\\|\[|\]|\{|\}|\;|\:|\"|\'|\,|\<|\.|\>|\/|\?]/g, "");
    },
    ErrorXMLDetect: function (errorStr) {
        //字符                HTML字符        字符编码
        //和(and) &           &amp;            &#38;
        //单引号  ’          &apos;           &#39;
        //双引号  ”          &quot;           &#34;
        //大于号  >           &gt;             &#62;
        //小于号  <           &lt;             &#60;
        //也可以在字符两端加上原样输出标记，如要原样输出5&6,可以写成<![CDATA[5&6]]>。
        errorStr = errorStr.replace(/'/g, '&apos;');
        errorStr = errorStr.replace(/"/g, '&quot;');
        errorStr = errorStr.replace(/>/g, '&gt;');
        errorStr = errorStr.replace(/</g, '&lt;');
        errorStr = errorStr.replace(/\n/g, ' ');
        errorStr = errorStr.replace(/iif/g, 'Irlovan.Operator.IIF');
        errorStr = errorStr.replace(/  /g, ' ');
        return errorStr;
    },
    ErrorXMLRepair: function (errorStr) {
        errorStr = errorStr.replace(/&apos;/g, "'");
        errorStr = errorStr.replace(/&quot;/g, "\"");
        errorStr = errorStr.replace(/&gt;/g, '>');
        errorStr = errorStr.replace(/&lt;/g, '<');
        return errorStr;
    },
    /*=-=*********************************************************DOMHelper****************************************************************=-=*/
    ParseFloatByDOM: function (px) {
        return parseFloat(px.substring(0, px.indexOf("px")));
    }
}
/*=-=*********************************************************JSExtend****************************************************************=-=*/
String.prototype.getBytes = function () {
    var bytes = [];
    for (var i = 0; i < this.length; ++i) {
        bytes.push(this.charCodeAt(i));
    }
    return bytes;
};
Array.prototype.Contain = function (element) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] === element) {
            return true;
        }
    }
    return false;
}
Array.prototype.Remove = function (element) {
    for (var i = this.length - 1; i--;) {
        if (this[i] === element) array.splice(i, 1);
    }
}
//get the last element of the array
Array.prototype.GetLast = function () {
    return this[this.length - 1];
}



















