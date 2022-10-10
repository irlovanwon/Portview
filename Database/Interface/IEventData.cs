///Copyright(c) 2013,Irlovan All rights reserved.
///Summary:
///Author:Irlovan
///Date:2014-09-30
///Description:
///Modification:

using Irlovan.Message;
using System;

namespace Irlovan.Database
{
    public interface IEventData : IIndustryData<bool>
    {

        #region Property

        /// <summary>
        /// Value of a very type
        /// </summary>
        new bool Value { get; set; }

        /// <summary>
        /// Triggered when event from false to true 
        /// </summary>
        DateTime StartTime { get; }

        /// <summary>
        /// Triggered when event from true to false
        /// </summary>
        DateTime EndTime { get; }

        /// <summary>
        /// Indication shows an solution for the event
        /// </summary>
        string Indication { get; }

        /// <summary>
        /// event level
        /// </summary>
        string EventLevel { get; }

        /// <summary>
        /// Store event when event becomes history
        /// </summary>
        DataMessageBox EventMessageBox { get; }

        #endregion Property

        #region Event

        /// <summary>
        /// Event to trigger recording for history events
        /// </summary>
        event HistoryEventHandler HistoryEvent;

        /// <summary>
        /// Event to trigger realtime events
        /// </summary>
        event RealEventHandler RealtimeEvent;

        #endregion Event

    }

}
