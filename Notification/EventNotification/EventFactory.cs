///Copyright(c) 2015,HIT All rights reserved.
///Summary:Event factory
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      

using Irlovan.Database;
using Irlovan.Lib.Symbol;
using Irlovan.Log;
using Irlovan.Message;
using Irlovan.Register;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Timers;
using System.Xml.Linq;

namespace Irlovan.Notification.EventNotification
{
    public class EventFactory : IDisposable
    {

        #region Structure

        /// <summary>
        /// An box to store realtime events 
        /// </summary>
        public EventFactory(Dictionary<string, IEventData> eventList, string name, IRegister register, string registerAddress = null, int interval = 200) {
            _interval = interval;
            _eventList = eventList;
            _registerAddress = registerAddress;
            _register = register;
            Name = name;
        }

        #endregion Structure

        #region Field

        private const string RealtimeEventRootName = "RealtimeEvent";
        private const string HistoryEventRootName = "HistoryEvent";
        private const string EventRootName = "Event";
        private const string XMLTagName = "EventBox";
        private int _maxEventCount = 20;
        private int _maxHistoryEventCount = 20;
        private int _interval;
        private string _registerAddress;
        private Timer _timer;
        private IRegister _register;
        //private OrderedDictionary _eventList = new OrderedDictionary();
        private Queue<IEventDataMessage> _historyEventMessage = new Queue<IEventDataMessage>();
        private OrderedDictionary _realtimeEventMessage = new OrderedDictionary();
        private Dictionary<string, IEventData> _eventList = new Dictionary<string, IEventData>();
        private List<bool> _eventStackList;

        private object _lock = new object();

        #endregion Field

        #region Property

        /// <summary>
        /// Name of the EventBox
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Maximum of Event Count 
        /// </summary>
        public int MaxEventCount {
            get { return _maxEventCount; }
            set {
                if (value != _maxEventCount) {
                    _maxEventCount = value;
                }
            }
        }

        /// <summary>
        /// Maximum of history Event Count 
        /// </summary>
        public int MaxHistoryEventCount {
            get { return _maxHistoryEventCount; }
            set {
                if (value != _maxHistoryEventCount) {
                    _maxHistoryEventCount = value;
                }
            }
        }

        #endregion Property

        #region Delegate

        /// <summary>
        /// handler for EventsTrigger Event
        /// </summary>
        public delegate void EventsTriggerHandler();

        #endregion Delegate

        #region Event

        /// <summary>
        /// Triggered when events state change
        /// </summary>
        public event EventsTriggerHandler EventsTrigger;

        #endregion Event

        #region Function

        /// <summary>
        /// Start Runing the box
        /// </summary>
        public void Run() {
            ModeSelect();
        }

        /// <summary>
        /// Write to message
        /// </summary>
        /// <returns></returns>
        public XElement ToRealtimeEventXML() {
            lock (_lock) {
                XElement result = new XElement(RealtimeEventRootName);
                foreach (IEventDataMessage item in _realtimeEventMessage.Values) {
                    result.AddFirst(item.ToXML());
                }
                return result;
            }
        }

        /// <summary>
        /// Write to message
        /// </summary>
        /// <returns></returns>
        public XElement ToHistoryEventXML() {
            lock (_lock) {
                XElement result = new XElement(HistoryEventRootName);
                foreach (var item in _historyEventMessage) {
                    result.AddFirst(item.ToXML());
                }
                return result;
            }
        }

        /// <summary>
        /// Write to message
        /// </summary>
        /// <returns></returns>
        public XElement ToEventXML() {
            XElement result = new XElement(EventRootName);
            foreach (var item in ToRealtimeEventXML().Elements()) {
                result.Add(item);
            }
            foreach (var item in ToHistoryEventXML().Elements()) {
                result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// select data recorder mode
        /// </summary>
        private void ModeSelect() {
            //Hybrid Mode
            if (_interval <= 0) { return; }
            _eventStackList = new List<bool>();
            PushStack(ref _eventStackList);
            HybridMode();
        }

        /// <summary>
        /// PushStack
        /// </summary>
        /// <param name="dataList"></param>
        private void PushStack(ref List<bool> dataList) {
            List<bool> result = new List<bool>();
            foreach (var item in _eventList) {
                result.Add(item.Value.Value);
            }
            dataList = result;
        }


        /// <summary>
        /// HybridMode
        /// </summary>
        /// <param name="interval"></param>
        private void HybridMode() {
            InitRealtimeEvent();
            InitRegister();
            if (EventsTrigger != null) { EventsTrigger(); }
            Lib.Timer.Timer.SetInterval((object o, ElapsedEventArgs e) => {
                bool isAlarm = false;
                int index = 0;
                foreach (var item in _eventList) {
                    if (item.Value.Value != _eventStackList[index]) {
                        HybridModeTrigger(item.Value, item.Value.Value, item.Value.StartTime, item.Value.EndTime);
                        _eventStackList[index] = item.Value.Value;
                        isAlarm = true;
                    }
                    index++;
                }
                if (isAlarm) {
                    if (EventsTrigger != null) { EventsTrigger(); }
                }
            }, ref _timer, _interval);
        }

        /// <summary>
        /// HybridModeTrigger
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="eventList"></param>
        private void HybridModeTrigger(IEventData data, bool value, DateTime startTime, DateTime endTime) {
            if (value) {
                AddRealtimeEvent(data, data.StartTime);
            }
            else {
                AddHistoryEvent(data);
                RemoveRealtimeEvent(data);
                RecordRegister();
            }
        }

        /// <summary>
        /// AddRealtimeEvent
        /// </summary>
        /// <param name="value"></param>
        /// <param name="time"></param>
        private void AddRealtimeEvent(IEventData data, DateTime time) {
            lock (_lock) {
                IEventDataMessage message = new EventDataMessage(data.FullName, time, DateTime.MinValue, data.EventLevel, data.Description, data.Indication);
                if (!message.InitState) { return; }
                AddRealtimeEvent(message);
            }
        }

        /// <summary>
        /// AddRealtimeEvent
        /// </summary>
        /// <param name="value"></param>
        /// <param name="time"></param>
        private void AddRealtimeEvent(IEventDataMessage data) {
            while (_realtimeEventMessage.Count >= _maxEventCount) { _realtimeEventMessage.RemoveAt(0); }
            if (_realtimeEventMessage.Contains(data.Name)) { _realtimeEventMessage.Remove(data.Name); }
            _realtimeEventMessage.Add(data.Name, data);
        }

        /// <summary>
        /// AddRealtimeEvent
        /// </summary>
        /// <param name="value"></param>
        /// <param name="time"></param>
        private void AddHistoryEvent(IEventData data) {
            lock (_lock) {
                AddHistoryEvent((IEventDataMessage)(data.EventMessageBox.ToDataMessages(1)[0]));
            }
        }

        /// <summary>
        /// AddRealtimeEvent
        /// </summary>
        /// <param name="value"></param>
        /// <param name="time"></param>
        private void AddHistoryEvent(IEventDataMessage message) {
            while (_historyEventMessage.Count >= _maxHistoryEventCount) { _historyEventMessage.Dequeue(); }
            _historyEventMessage.Enqueue(message);
        }


        /// <summary>
        /// remove RemoveRealtimeEvent
        /// </summary>
        /// <param name="name"></param>
        private void RemoveRealtimeEvent(IEventData data) {
            lock (_lock) {
                if (_realtimeEventMessage.Contains(data.FullName)) {
                    _realtimeEventMessage.Remove(data.FullName);
                }
            }
        }

        /// <summary>
        /// Record History Event To Register
        /// </summary>
        private void RecordRegister() {
            lock (_lock) {
                if (_register == null) { return; }
                XElement result = new XElement(XMLTagName);
                foreach (var item in _historyEventMessage) {
                    result.Add(item.ToXML());
                }
                _register.AsyncWrite<string>(_registerAddress, result.ToString());
            }
        }

        /// <summary>
        /// Init Realtime Evemt Message
        /// </summary>
        private void InitRealtimeEvent() {
            int index = 0;
            foreach (var item in _eventList) {
                if (item.Value.Value) { AddRealtimeEvent(item.Value, item.Value.StartTime); }
                _eventStackList[index] = item.Value.Value;
                index++;
            }
        }

        /// <summary>
        /// Init History Event Message From Register  
        /// </summary>
        private void InitRegister() {
            if (_register == null) { return; }
            string historyCache;
            _register.AsyncRead<string>(_registerAddress, out historyCache);
            XElement hitoryMessage;
            try {
                if (historyCache == null) { return; }
                hitoryMessage = XElement.Parse(historyCache);
            }
            catch (Exception e) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.RegisterInitFailed + Symbol.NewLine_Symbol + e.ToString());
                return;
            }
            InitHistoryEventMessage(hitoryMessage.Elements());
        }

        /// <summary>
        /// Init HitoryEvent Message from Register
        /// </summary>
        /// <param name="messages"></param>
        private void InitHistoryEventMessage(IEnumerable<XElement> messages) {
            lock (_lock) {
                foreach (var item in messages) {
                    IEventDataMessage message = new EventDataMessage(item);
                    if (!message.InitState) { continue; }
                    AddHistoryEvent(message);
                }
            }
        }


        /// <summary>
        /// If an event has happened
        /// </summary>
        /// <returns></returns>
        public bool Alarm() {
            lock (_lock) {
                return _realtimeEventMessage.Count > 0;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() {
            _eventList.Clear();
            _historyEventMessage.Clear();
            _realtimeEventMessage.Clear();
            if (_eventStackList != null) {
                _eventStackList.Clear();
            }
            if (_timer != null) {
                Lib.Timer.Timer.DisposeTimer(_timer);
            }
        }



        #endregion Function

    }
}
