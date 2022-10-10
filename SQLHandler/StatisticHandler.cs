/////Copyright(c) 2015,Irlovan All rights reserved.
/////Summary：Statistic
/////Author：Irlovan
/////Date：2015-04-18
/////Description：
/////Modification：


//using Irlovan.Message;
//using Irlovan.Recorder;
//using SuperWebSocket;
//using System;
//using System.Collections.Generic;
//using System.Xml.Linq;

//namespace Irlovan.Handlers
//{
//    internal class StatisticHandler : SQL
//    {

//        #region Structure

//        /// <summary>
//        /// Construction
//        /// </summary>
//        /// <param name="session"></param>
//        /// <param name="local"></param>
//        internal StatisticHandler(WebSocketSession session, LocalInterface.LocalInterface local)
//            : base(session, local) { }

//        #endregion Structure

//        #region Field

//        internal const string Name = "Statistic";
//        private const string MatrixPara = "Matrix";

//        #endregion Field

//        #region Function

//        /// <summary>
//        /// Handler String
//        /// </summary>
//        /// <param name="session"></param>
//        /// <param name="message"></param>
//        /// <returns></returns>
//        public override bool Handle(WebSocketSession session, XElement element) {
//            if (!base.Handle(session, element)) { return false; }
//            DataHandler(element);
//            return true;
//        }

//        /// <summary>
//        /// Dispose
//        /// </summary>
//        public override void Dispose() {
//            base.Dispose();
//        }

//        /// <summary>
//        /// handler for realtime data
//        /// </summary>
//        /// <param name="root"></param>
//        private void DataHandler(XElement element) {
//            XElement config = element.Element(MatrixPara);
//            if (config == null) { return; }
//            string timeStamp; string recorderName;
//            if (!Helper.Helper.InitStringAttr<string>(config, RecorderNameAttr, out recorderName)) { return; }
//            IStatisticRecorder recorder = GetRecorder<IStatisticRecorder>(recorderName);
//            if (recorder == null) { return; }
//            if (!Helper.Helper.InitStringAttr<string>(config, SQLTimeStampAttr, out timeStamp)) { return; }
//            List<IMatrixMessage> message;
//            try { message = recorder.Read(Convert.ToDateTime(timeStamp)); } catch (Exception e) { GlobalBase.Global.LogRecorder.Log(Log.Config.LogTypeEnum.Error, GlobalBase.Properties.Resources.ReadRecorderFailed + recorderName.ToString() + ":" + e.ToString()); return; }
//            if ((message == null) && (message.Count == 0)) { return; }
//            Session.Send(CreateSQLResultMessage(message));
//        }

//        /// <summary>
//        /// Create SQL Result Message
//        /// </summary>
//        private string CreateSQLResultMessage(List<IMatrixMessage> message) {
//            XElement result = new XElement(Name);
//            XElement sql = new XElement(MatrixPara);
//            foreach (var item in message) {
//                sql.Add(item.ToXML());
//            }
//            return result.ToString();
//        }

//        #endregion Function

//    }
//}
