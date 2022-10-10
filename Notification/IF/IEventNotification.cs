///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Notification interface
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      

using Irlovan.Database;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Notification
{
    public interface IEventNotification
    {

        #region Property

        /// <summary>
        /// Max count for realtime event
        /// </summary>
        int MaxEventCount { get; set; }

        /// <summary>
        /// Max count for history event
        /// </summary>
        int MaxHistoryEventCount { get; set; }

        /// <summary>
        /// Event List 
        /// </summary>
        Dictionary<string, IEventData> EventList { get; }

        #endregion Property

        #region Event

        /// <summary>
        /// For Realtime Event Change
        /// </summary>
        event EventChangeHandler RealtimeEventChange;

        /// <summary>
        /// For History Event Change
        /// </summary>
        event EventChangeHandler HistoryEventChange;

        /// <summary>
        /// For Realtime & History Event Change
        /// </summary>
        event EventChangeHandler EventChange;

        #endregion Event

        #region Function

        /// <summary>
        /// Start to subcribe events
        /// </summary>
        void BeginSubcription();

        #endregion Function

    }

    /// <summary>
    /// Event Change Handler
    /// </summary>
    /// <param name="message"></param>
    public delegate void EventChangeHandler(XElement message);

}
