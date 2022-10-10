//Copyright(c) 2015,HIT All rights reserved.
//Des:Message format for statistic data info
//Author:Irlovan   
//Date:2015-09-16


/**Construction**/
Irlovan.Message.StatisticMessage = function () {
    this.Message = {};
    this.XMLTag = "MatrixRow";
}

/**MessageToArray**/
Irlovan.Message.StatisticMessage.prototype.ToArray = function () {
    var result = {};
    for (var item in this.Message) {
        result[item] = this.Message[item];
    }
    return this.Message;
}

/**Parse XML**/
Irlovan.Message.StatisticMessage.prototype.ParseXElement = function (element) {
    if (!element.nodeName) { return; }
    if (element.nodeName != this.XMLTag) { return; }
    for (var i = 0; i < element.attributes.length; i++) {
        this.Message[element.attributes[i].name] = element.attributes[i].value;
    }
}