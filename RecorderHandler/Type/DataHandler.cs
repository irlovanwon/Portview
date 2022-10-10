///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Realtime data handler
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using Irlovan.Canal;
using Irlovan.Lib.XML;
using Irlovan.Log;
using Irlovan.Message;
using Irlovan.Recorder;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

namespace Irlovan.Handlers
{
    internal class DataHandler : BaseHandler
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="session"></param>
        /// <param name="local"></param>
        internal DataHandler(IServerSession session, LocalInterface.LocalInterface local)
            : base(session, local) { }

        #endregion Structure

        #region Field

        internal const string Name = "DataRecorderHandler";

        #endregion Field

        #region Function

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="session"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public override bool Handle(IServerSession session, XElement element) {
            if (!base.Handle(session, element)) { return false; }
            string startTimeStr, endTimeStr, count, dataName; bool isDesc;
            if (!XML.InitStringAttr<string>(element, RecorderHandler.StartTimeAttr, out startTimeStr)) { return false; }
            if (!XML.InitStringAttr<string>(element, RecorderHandler.EndTimeAttr, out endTimeStr)) { return false; }
            if (!XML.InitStringAttr<string>(element, RecorderHandler.CountAttr, out count)) { return false; }
            if (!XML.InitStringAttr<bool>(element, RecorderHandler.IsDescAttr, out isDesc)) { return false; }
            XML.InitStringAttr<string>(element, RecorderHandler.DataNameAttr, out dataName);
            List<IIndustryDataMessage> message; DateTime startTime, endTime;
            if (!DateTime.TryParse(startTimeStr, LocalInterface.Config.RecorderQueryCulture, DateTimeStyles.None, out startTime)) { return false; }
            if (!DateTime.TryParse(endTimeStr, LocalInterface.Config.RecorderQueryCulture, DateTimeStyles.None, out endTime)) { return false; }
            try { message = ((IRealtimeDataRecorder)Recorder).Read(startTime, endTime, count, dataName, isDesc); }
            catch (Exception e) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.ReadRecorderFailed + Recorder.RecorderName + ":" + e.ToString()); return false; }
            if ((message == null) && (message.Count == 0)) { return false; }
            XElement result = new XElement(Name);
            foreach (var item in message) {
                result.Add(item.ToXML());
            }
            Session.Send(result.ToString());
            return true;
        }

        /// <summary>
        /// Load Recorder
        /// </summary>
        /// <param name="recorder"></param>
        /// <returns></returns>
        public override bool LoadRecorder(IRecorder recorder) {
            if (!base.LoadRecorder(recorder)) { return false; }
            if (!(recorder is IRealtimeDataRecorder)) { return false; }
            Recorder = recorder;
            return true;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
        }

        #endregion Function

    }
}
