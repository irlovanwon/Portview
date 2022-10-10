//Copyright(c) 2015,HIT All rights reserved.
//Des:Message format for HistoryData 
//Author:Irlovan   
//Date:2015-08-27


/**Construction**/
Irlovan.Message.IndustryDataMessage = function (name, timeStamp, desc, value, dataType) {
    this.Message = {};
    this.NameAttr = "Name";
    this.DescriptionAttr = "Description";
    this.TimeStampAttr = "TimeStamp";
    this.ValueAttr = "Value";
    this.DataTypeAttr = "DataType";
    this.XMLTag = "InDataMessage";
    if (!name) { return; }
    this.CreateMessage(name, timeStamp, desc, value, dataType);
}

/**Industry DataMessage ToArray**/
Irlovan.Message.IndustryDataMessage.prototype.ToArray = function () {
    var result = {};
    for (var item in this.Message) {
        result[item] = this.Message[item];
    }
    return this.Message;
}

/**Parse XML**/
Irlovan.Message.IndustryDataMessage.prototype.ParseXElement = function (element) {
    if (!element.nodeName) { return; }
    if (element.nodeName != this.XMLTag) { return; }
    this.CreateMessage(element.getAttribute(this.NameAttr), element.getAttribute(this.TimeStampAttr), element.getAttribute(this.DescriptionAttr), element.getAttribute(this.ValueAttr), element.getAttribute(this.DataTypeAttr));
}

/**Create Message Object Literal**/
Irlovan.Message.IndustryDataMessage.prototype.CreateMessage = function (name, timeStamp, desc, value, dataType) {
    this.Message[this.NameAttr] = name;
    this.Message[this.DescriptionAttr] = desc;
    this.Message[this.TimeStampAttr] = timeStamp;
    this.Message[this.ValueAttr] = value;
    this.Message[this.DataTypeAttr] = dataType;
}