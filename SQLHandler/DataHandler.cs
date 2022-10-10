///Copyright(c) 2015,Irlovan All rights reserved.
///Summary：DataHandler
///Author：Irlovan
///Date：2015-04-17
///Description：
///Modification：


using Irlovan.Message;
using Irlovan.Recorder;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Handlers
{
    internal class DataHandler : SQL
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="session"></param>
        /// <param name="local"></param>
        internal DataHandler(WebSocketSession session, LocalInterface.LocalInterface local)
            : base(session, local) { }

        #endregion Structure

        #region Field

        internal const string Name = "Data";
        private const string HistoryPara = "History";

        #endregion Field

        #region Function

        /// <summary>
        /// Handler String
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool Handle(WebSocketSession session, XElement element) {
            if (!base.Handle(session, element)) { return false; }
            HistoryQueryHandler(element);
            return true;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
        }

        /// <summary>
        /// handler for realtime data
        /// </summary>
        /// <param name="root"></param>
        private void HistoryQueryHandler(XElement element) {
            XElement config = element.Element(HistoryPara);
            if (config == null) { return; }
            string startTime; string endTime; string count; string recorderName; string name;
            if (!Helper.Helper.InitStringAttr<string>(config, RecorderNameAttr, out recorderName)) { return; }
            IRealtimeDataRecorder recorder = GetRecorder<IRealtimeDataRecorder>(recorderName);
            if (recorder == null) { return; }
            if (!Helper.Helper.InitStringAttr<string>(config, SQLStartTimeAttr, out startTime)) { return; }
            if (!Helper.Helper.InitStringAttr<string>(config, SQLEndTimeAttr, out endTime)) { return; }
            if (!Helper.Helper.InitStringAttr<string>(config, SQLCountAttr, out count)) { return; }
            Helper.Helper.InitStringAttr<string>(config, SQLDataNameAttr, out name);
            List<IIndustryDataMessage> message;
            try { message = recorder.Read(Convert.ToDateTime(startTime), Convert.ToDateTime(endTime), count, name); } catch (Exception e) { GlobalBase.Global.LogRecorder.Log(Log.Config.LogTypeEnum.Error, GlobalBase.Properties.Resources.ReadRecorderFailed + recorderName.ToString() + ":" + e.ToString()); return; }
            if ((message == null) && (message.Count == 0)) { return; }
            Session.Send(CreateSQLResultMessage(message));
        }

        /// <summary>
        /// Create SQL Result Message
        /// </summary>
        private string CreateSQLResultMessage(List<IIndustryDataMessage> message) {
            XElement result = new XElement(Name);
            XElement sql = new XElement(HistoryPara);
            foreach (var item in message) {
                sql.Add(item.ToXML());
            }
            return result.ToString();
        }

        #endregion Function

    }
}
