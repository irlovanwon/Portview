//Copyright(c) 2015,HIT All rights reserved.
//Des:Message format for statistic data info
//Author:Irlovan   
//Date:2015-09-29

/**Construction**/
Irlovan.Message.MatrixMessage = function () {
    this.Message = {};
    this.TimeStamp;
    this.XMLTag = "MatrixRow";
    this.DateColumnName = "TimeStamp";
}

/**Parse XML**/
Irlovan.Message.MatrixMessage.prototype.ParseXElement = function (element) {
    if (!element.nodeName) { return; }
    if (element.nodeName != this.XMLTag) { return; }
    for (var i = 0; i < element.attributes.length; i++) {
        var name = element.attributes[i].name;
        var value = element.attributes[i].value;
        if (name == this.DateColumnName) {
            this.TimeStamp = value;
        } else {
            this.Message[name] = value;
        }
    }
}