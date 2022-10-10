//Copyright(c) 2013,HIT All rights reserved.
//Des:XML Method Lib
//Author:Irlovan   
//Date:2015-03-19
//modification : 

Irlovan.Lib.Help = {};

/**covert value to bool(avoid case issue)**/
Irlovan.Lib.Help.Boolean = function (value) {
    if (value == true) { return true; }
    if (value == false) { return false; }
    var result = value.toLowerCase();
    if (value == "true") { return true; }
    if (value == "false") { return false; }
}

