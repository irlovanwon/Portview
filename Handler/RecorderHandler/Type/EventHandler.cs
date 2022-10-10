///Copyright(c) 2015,HIT All rights reserved.
///Summary:Event(alarm) handler
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
    internal class EventHandler : BaseHandler
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="session"></param>
        /// <param name="local"></param>
        internal EventHandler(IServerSession session, LocalInterface.LocalInterface local)
            : base(session, local) { }

        #endregion Structure

        #region Field

        internal const string Name = "EventRecorderHandler";

        #endregion Field

        #region Function

        /// <summary>
        /// Handler
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool Handle(IServerSession session, XElement element) {
            if (!base.Handle(session, element)) { return false; }
            string startTimeStr; string endTimeStr; string eventLevel; string count; string name; bool isDesc;
            if (!XML.InitStringAttr<string>(element, RecorderHandler.StartTimeAttr, out startTimeStr)) { return false; }
            if (!XML.InitStringAttr<string>(element, RecorderHandler.EndTimeAttr, out endTimeStr)) { return false; }
            if (!XML.InitStringAttr<string>(element, RecorderHandler.CountAttr, out count)) { return false; }
            if (!XML.InitStringAttr<bool>(element, RecorderHandler.IsDescAttr, out isDesc)) { return false; }
            XML.InitStringAttr<string>(element, RecorderHandler.EventLevelAttr, out eventLevel);
            XML.InitStringAttr<string>(element, RecorderHandler.DataNameAttr, out name);
            List<IEventDataMessage> message; DateTime startTime, endTime;
            if (!DateTime.TryParse(startTimeStr, LocalInterface.Config.RecorderQueryCulture, DateTimeStyles.None, out startTime)) { return false; }
            if (!DateTime.TryParse(endTimeStr, LocalInterface.Config.RecorderQueryCulture, DateTimeStyles.None, out endTime)) { return false; }
            try { message = ((IEventRecorder)Recorder).Read(startTime, endTime, count, name, (eventLevel != null) ? eventLevel.Split(RecorderHandler.EventLevelSplitChar) : null, isDesc); }
            catch (Exception e) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.ReadRecorderFailed + Recorder.RecorderName + ":" + e.ToString()); return false; }
            if (message == null) { return false; }
            XElement result = new XElement(Name);
            foreach (var item in message) { result.Add(item.ToXML()); }
            Session.Send(result.ToString());
            return true;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
        }

        /// <summary>
        /// Load Recorder
        /// </summary>
        /// <param name="recorder"></param>
        /// <returns></returns>
        public override bool LoadRecorder(IRecorder recorder) {
            if (!base.LoadRecorder(recorder)) { return false; }
            if (!(recorder is IEventRecorder)) { return false; }
            Recorder = recorder;
            return true;
        }

        #endregion Function

    }
}
