//Copyright(c) 2015,HIT All rights reserved.
//Des:Message format for realtime data info
//Author:Irlovan   
//Date:2015-03-19


/**Construction**/
Irlovan.Message.EventMessage = function (name, startTime, endTime, desc, level, indication) {
    this.Message = {};
    this.NameAttr = "Name";
    this.DescriptionAttr = "Description";
    this.StartTimeAttr = "StartTime";
    this.EndTimeAttr = "EndTime";
    this.EventLevelAttr = "EventLevel";
    this.IndicationAttr = "Indication";
    this.XMLTag = "EDataMessage";
    if (!name) { return; }
    this.CreateMessage(name, startTime, endTime, desc, level, indication);
}

/**EventMessageToArray**/
Irlovan.Message.EventMessage.prototype.ToArray = function () {
    var result = {};
    for (var item in this.Message) {
        //var attr = {};
        result[item] = this.Message[item];
        //result.push(attr);
    }
    return this.Message;
}

/**Parse XML**/
Irlovan.Message.EventMessage.prototype.ParseXElement = function (element) {
    if (!element.nodeName) { return; }
    if (element.nodeName != this.XMLTag) { return; }
    this.CreateMessage(element.getAttribute(this.NameAttr), element.getAttribute(this.StartTimeAttr), element.getAttribute(this.EndTimeAttr), element.getAttribute(this.DescriptionAttr), element.getAttribute(this.EventLevelAttr), element.getAttribute(this.IndicationAttr));
}

/**Create Message Object Literal**/
Irlovan.Message.EventMessage.prototype.CreateMessage = function (name, startTime, endTime, desc, level, indication) {
    this.Message[this.NameAttr] = name;
    this.Message[this.DescriptionAttr] = desc;
    this.Message[this.StartTimeAttr] = startTime;
    this.Message[this.EndTimeAttr] = endTime;
    this.Message[this.IndicationAttr] = indication;
    this.Message[this.EventLevelAttr] = level;
}