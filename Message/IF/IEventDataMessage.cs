///Copyright(c) 2015,HIT All rights reserved.
///Summary:EventDataMessage interface
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using System;

namespace Irlovan.Message
{
    public interface IEventDataMessage : IDataMessage
    {

        /// <summary>
        /// Trigger time for event
        /// </summary>
        DateTime StartTime { get; }

        /// <summary>
        /// reset time for event
        /// </summary>
        DateTime EndTime { get; }

        /// <summary>
        /// Indication for Event
        /// </summary>
        string Indication { get; }

        /// <summary>
        /// Event Level for event
        /// </summary>
        string EventLevel { get; }

    }
}
