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
    public interface IRealtimeDataRecorder : IRecorder
    {

        #region Function

        /// <summary>
        /// Read Data from database
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="amount"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        List<IIndustryDataMessage> Read(DateTime startTime, DateTime endTime, object amount, object name = null, bool isDesc = true);

        /// <summary>
        /// Record data to database
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        bool Record(IEnumerable<IRecorderData> dataList, DateTime timeStamp);

        #endregion Function

    }
}
