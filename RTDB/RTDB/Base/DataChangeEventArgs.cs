///Copyright(c) 2015,HIT All rights reserved.
///Summary:
///Author:Irlovan
///Date:2013
///Description:
///Modification:

using System;

namespace Irlovan.Database
{

    /// <summary>
    /// DataChangeEventArgs for DataChange event
    /// </summary>
    public class DataChangeEventArgs : EventArgs
    {

        /// <summary>
        /// Industry data value change event 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="timeStamp"></param>
        /// <param name="name"></param>
        public DataChangeEventArgs(object oldValue, object newValue, DateTime timeStamp, string name) {
            OldValue = oldValue;
            NewValue = newValue;
            TimeStamp = timeStamp;
            Name = name;
        }

        /// <summary>
        /// value before data changed
        /// </summary>
        public object OldValue { get; set; }

        /// <summary>
        /// value after data changed
        /// </summary>
        public object NewValue { get; set; }

        /// <summary>
        /// TimeStamp triggered when data changed
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Full name of the data
        /// </summary>
        public string Name { get; set; }

    }

}
