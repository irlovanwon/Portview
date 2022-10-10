///Copyright(c) 2015,Irlovan All rights reserved.
///Summary：
///Author：Irlovan
///Date：2015-07-27
///Description：
///Modification：

using Irlovan.Message;
using Irlovan.Recorder;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Irlovan.Handlers
{
    internal class SQL : Handler
    {

        #region Structure

        /// <summary>
        ///Construction
        /// </summary>
        internal SQL(WebSocketSession session, LocalInterface.LocalInterface local)
            : base(session, local) {
        }

        #endregion Structure

        #region Field

        internal const string SQLStartTimeAttr = "StartTime";
        internal const string SQLEndTimeAttr = "EndTime";
        internal const string SQLDataNameAttr = "DataName";
        internal const string SQLEventLevelAttr = "EventLevel";
        internal const string SQLCountAttr = "Count";
        internal const string SQLTimeStampAttr = "TimeStamp";
        internal const string RecorderNameAttr = "RecorderName";

        //public string TimeFormat = "MM/dd/yyyy HH:mm:ss";

        #endregion Field

        #region Function

        /// <summary>
        /// GetEvent Recorder by recorder name
        /// </summary>
        /// <returns></returns>
        internal T GetRecorder<T>(string recorderName) {
            T result;
            try { result = (T)LocalInterface.Recorder.RecorderList[recorderName]; }
            catch (Exception e) { GlobalBase.Global.LogRecorder.Log(Log.Config.LogTypeEnum.Error, GlobalBase.Properties.Resources.RecorderHandlerFailed + recorderName + ":" + e.ToString()); return default(T); }
            return result;
        }

        #endregion Function
    }
}
