///Copyright(c) 2013,Irlovan All rights reserved.
///Summary:EventData(or Call it BoolData)
///Author:Irlovan
///Date:2013-04-03
///Description:The Data that Type is bool
///Modification:
///2015-01-28

using Irlovan.Lib.XML;
using Irlovan.Message;
using System;
using System.Xml.Linq;

namespace Irlovan.Database
{
    public class EventData : IndustryData<bool>, IEventData
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public EventData(string name, string description = null)
            : base(name, description) { }

        /// <summary>
        /// EventData from xml config
        /// </summary>
        public EventData(XElement config)
            : base(config) {
            ReadXML(config);
        }

        #endregion Structure

        #region Field

        internal new const string SpeciesName = "EventData";
        private const string IndicationPara = "Indication";
        private const string EventLevelPara = "EventLevel";
        private object _lock = new object();
        private string _indication;
        private string _eventLevel;

        #endregion Field

        #region Property

        /// <summary>
        /// Triggered when event from false to true 
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// Triggered when event from true to false
        /// </summary>
        public DateTime EndTime { get; private set; }

        /// <summary>
        /// Indication shows an solution for the event
        /// </summary>
        public string Indication {
            get { return _indication; }
            private set {
                if (_indication != value) {
                    _indication = value;
                }
            }
        }

        /// <summary>
        /// event level
        /// </summary>
        public string EventLevel {
            get { return _eventLevel; }
            private set {
                if (_eventLevel != value) {
                    _eventLevel = value;
                }
            }
        }

        /// <summary>
        /// Store event when event becomes history
        /// </summary>
        public DataMessageBox EventMessageBox { get; private set; }

        #endregion Property

        #region Event

        /// <summary>
        /// Event to trigger realtime events
        /// </summary>
        public event RealEventHandler RealtimeEvent;

        /// <summary>
        /// Event to trigger recording for history events
        /// </summary>
        public event HistoryEventHandler HistoryEvent;

        #endregion Event

        #region Function

        /// <summary>
        /// Set value of data
        /// </summary>
        /// <param name="value"></param>
        public new bool ReadValue(object value) {
            if (!base.ReadValue(value)) { return false; }
            lock (_lock) {
                if (Value) { Alarm(); }
                else { Reset(); }
                return true;
            }
        }

        /// <summary>
        /// Get Database from xml config file
        /// </summary>
        /// <returns></returns>
        public new void ReadXML(XElement element) {
            if (QueueCount <= 0) { QueueCount = 1; }
            EventMessageBox.MaxCount = QueueCount;
            if (!XML.InitStringAttr<string>(element, IndicationPara, out _indication)) { ErrorParaList.Add(IndicationPara); InitState = false; }
            if (!XML.InitStringAttr<string>(element, EventLevelPara, out _eventLevel)) { ErrorParaList.Add(EventLevelPara); InitState = false; }
        }

        /// <summary>
        /// Write Database to xml config file
        /// </summary>
        /// <returns></returns>
        public new XElement WriteXML() {
            XElement result = base.WriteXML();
            result.SetAttributeValue(IndicationPara, _indication);
            result.SetAttributeValue(EventLevelPara, _eventLevel);
            return result;
        }

        /// <summary>
        /// Init
        /// </summary>
        public override void Init() {
            base.Init();
            Species = SpeciesName;
            EventMessageBox = new DataMessageBox();
        }

        /// <summary>
        /// Alarm
        /// </summary>
        private void Alarm() {
            StartTime = DateTime.Now;
            EndTime = DateTime.MinValue;
            if (RealtimeEvent != null) {
                RealtimeEvent(StartTime);
            }
        }

        /// <summary>
        /// Reset
        /// </summary>
        private void Reset() {
            EndTime = DateTime.Now;
            if (HistoryEvent != null) { HistoryEvent(StartTime, EndTime); }
            if (QueueCount != 0) { EventMessageBox.Push(new EventDataMessage(FullName, StartTime, EndTime, EventLevel.ToString(), Description.ToString(), Indication.ToString())); }
        }

        #endregion Function
    }

    /// <summary>
    /// Realtime Event Handler
    /// </summary>
    /// <param name="startTime"></param>
    public delegate void RealEventHandler(DateTime startTime);

    /// <summary>
    /// History Event Handler
    /// </summary>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    public delegate void HistoryEventHandler(DateTime startTime, DateTime endTime);

}
