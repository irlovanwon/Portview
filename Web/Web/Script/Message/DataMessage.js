//Copyright(c) 2015,HIT All rights reserved.
//Des:Message format for realtime data info
//Author:Irlovan   
//Date:2015-03-19


/**Construction**/
Irlovan.Message.DataMessage = function (name, value, quality) {
    this.Name = name;
    this.Value = value;
    this.Quality = quality;
}

/**Equal to another data message**/
Irlovan.Message.DataMessage.prototype.Equals = function (dataMessage) {
    if ((!dataMessage.Name) || dataMessage.Name != this.Name) { return false; }
    if ((!dataMessage.Value) || dataMessage.Value != this.Value) { return false; }
    if ((!dataMessage.Quality) || dataMessage.Quality != this.Quality) { return false; }
    return true;
}

/**Equal to another data message**/
Irlovan.Message.DataMessage.prototype.EqualsValue = function (dataMessage) {
    if ((!dataMessage.Name) || dataMessage.Name != this.Name) { return false; }
    if ((!dataMessage.Value) || dataMessage.Value != this.Value) { return false; }
    return true;
}

/**Equal to another data message**/
Irlovan.Message.DataMessage.prototype.EqualsQuality = function (dataMessage) {
    if ((!dataMessage.Name) || dataMessage.Name != this.Name) { return false; }
    if ((!dataMessage.Quality) || dataMessage.Quality != this.Quality) { return false; }
    return true;
}