///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Event recorder interface
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:
      
using Irlovan.Message;
using System;
using System.Collections.Generic;

namespace Irlovan.Recorder
{
    public interface IEventRecorder : IRecorder
    {

        #region Function

        /// <summary>
        /// Read from recorder
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="amount"></param>
        /// <param name="name"></param>
        /// <param name="eventLevel"></param>
        /// <returns></returns>
        List<IEventDataMessage> Read(DateTime startTime, DateTime endTime, object amount, object name = null, string[] eventLevel = null, bool isDesc = true);

        /// <summary>
        /// record to recorder
        /// </summary>
        /// <param name="eventList"></param>
        /// <returns></returns>
        bool Record(List<IEventDataMessage> eventList);

        #endregion Function

    }

}
