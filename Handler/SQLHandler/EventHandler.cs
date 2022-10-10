/////Copyright(c) 2015,Irlovan All rights reserved.
/////Summary：EventHandler for eventdata
/////Author：Irlovan
/////Date：2015-04-17
/////Description：
/////Modification：


//using Irlovan.Message;
//using Irlovan.Recorder;
//using SuperWebSocket;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Xml.Linq;

//namespace Irlovan.Handlers
//{
//    internal class EventHandler : SQL
//    {

//        #region Structure

//        internal EventHandler(WebSocketSession session, LocalInterface.LocalInterface local)
//            : base(session, local) { }

//        #endregion Structure

//        #region Field

//        internal const string Name = "Event";
//        private const string RegisterRecorderPara = "Register";
//        private const string SQLRecorderPara = "SQL";
//        private const string RegisterTypeAttr = "Type";
//        private const string RegisterHistoryPara = "History";
//        private const string RegisterRealtimePara = "Realtime";
//        private const string RegisterBothPara = "Both";
//        private const char EventLevelSplitChar = ',';

//        private IEventRecorder _eventRecorder;
//        //date culture from websocketclient
//        private CultureInfo _clientDateCulture = new CultureInfo("en-US");

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
//            RegisterHandler(element);
//            SQLEventHandler(element);
//            return true;
//        }

//        /// <summary>
//        /// Dispose
//        /// </summary>
//        public override void Dispose() {
//            base.Dispose();
//            if (_eventRecorder == null) { return; }
//            _eventRecorder.RealtimeEventChange -= RegisterEventHandler;
//            _eventRecorder.EventChange -= RegisterEventHandler;
//            _eventRecorder.HistoryEventChange -= RegisterEventHandler;
//        }

//        /// <summary>
//        /// handler for realtime & history event
//        /// </summary>
//        /// <param name="root"></param>
//        private void RegisterHandler(XElement element) {
//            XElement config = element.Element(RegisterRecorderPara);
//            if (config == null) { return; }
//            string recorderName;
//            string type;
//            if (!Helper.Helper.InitStringAttr<string>(config, RecorderNameAttr, out recorderName)) { return; }
//            if (!Helper.Helper.InitStringAttr<string>(config, RegisterTypeAttr, out type)) { return; }
//            if (!LocalInterface.Recorder.RecorderList.ContainsKey(recorderName)) { return; }
//            _eventRecorder = GetRecorder<IEventRecorder>(recorderName);
//            if (_eventRecorder == null) { return; }
//            StartRegister(type);
//        }

//        /// <summary>
//        /// Start Register
//        /// </summary>
//        private void StartRegister(string registerType) {
//            switch (registerType) {
//                case RegisterBothPara:
//                _eventRecorder.EventChange += RegisterEventHandler;
//                break;
//                case RegisterHistoryPara:
//                _eventRecorder.HistoryEventChange += RegisterEventHandler;
//                break;
//                case RegisterRealtimePara:
//                _eventRecorder.RealtimeEventChange += RegisterEventHandler;
//                break;
//                default:
//                return;
//            }
//            _eventRecorder.BeginInvokeEvents();
//        }

//        /// <summary>
//        /// Handler for realtime & history events of register
//        /// </summary>
//        /// <param name="message"></param>
//        private void RegisterEventHandler(XElement message) {
//            XElement result = new XElement(Name);
//            XElement register = new XElement(RegisterRecorderPara);
//            register.Add(message);
//            result.Add(register);
//            //to avoid dead lock
//            System.Threading.Thread.Sleep(10);
//            Session.Send(result.ToString());
//        }

//        /// <summary>
//        /// handler for realtime event
//        /// </summary>
//        /// <param name="root"></param>
//        private void SQLEventHandler(XElement element) {
//            XElement config = element.Element(SQLRecorderPara);
//            string startTime; string endTime; string eventLevel; string count; string recorderName; string name;
//            if (!Helper.Helper.InitStringAttr<string>(config, RecorderNameAttr, out recorderName)) { return; }
//            IEventRecorder eventRecorder = GetRecorder<IEventRecorder>(recorderName);
//            if (eventRecorder == null) { return; }
//            if (!Helper.Helper.InitStringAttr<string>(config, SQLStartTimeAttr, out startTime)) { return; }
//            if (!Helper.Helper.InitStringAttr<string>(config, SQLEndTimeAttr, out endTime)) { return; }
//            if (!Helper.Helper.InitStringAttr<string>(config, SQLCountAttr, out count)) { return; }
//            Helper.Helper.InitStringAttr<string>(config, SQLEventLevelAttr, out eventLevel);
//            Helper.Helper.InitStringAttr<string>(config, SQLDataNameAttr, out name);
//            List<IEventDataMessage> message;
//            try { message = eventRecorder.Read(Convert.ToDateTime(startTime, _clientDateCulture), Convert.ToDateTime(endTime.ToString(), _clientDateCulture), count, name, (eventLevel != null) ? eventLevel.Split(EventLevelSplitChar) : null); } catch (Exception e) { GlobalBase.Global.LogRecorder.Log(Log.Config.LogTypeEnum.Error, GlobalBase.Properties.Resources.ReadRecorderFailed + recorderName.ToString() + ":" + e.ToString()); return; }
//            if ((message == null) && (message.Count == 0)) { return; }
//            Session.Send(CreateSQLResultMessage(message));
//        }

//        /// <summary>
//        /// Create SQL Result Message
//        /// </summary>
//        private string CreateSQLResultMessage(List<IEventDataMessage> message) {
//            XElement result = new XElement(Name);
//            XElement sql = new XElement(SQLRecorderPara);
//            foreach (var item in message) {
//                sql.Add(item.ToXML());
//            }
//            return result.ToString();
//        }

//        #endregion Function

//    }
//}
