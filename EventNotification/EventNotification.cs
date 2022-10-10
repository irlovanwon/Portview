///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:SQLServerHandler
///Author:Irlovan
///Date:2015-08-03
///Description:
///Modification:

using Irlovan.Database;
using Irlovan.Lib.XML;
using Irlovan.Log;
using Irlovan.Register;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Notification.EventNotification
{
    public class EventNotification : Notification, IEventNotification
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="source"></param>
        /// <param name="config"></param>
        public EventNotification(Catalog source, IRegister register, XElement config)
            : base(source, register, config) { }

        #endregion Structure

        #region Field

        private const string ColumnIDPara = "ID";
        private const string MaxEventCountAttr = "MaxEventCount";
        private const string MaxHistoryEventCountAttr = "MaxHistoryEventCount";
        private const string RegisterAddressAttr = "RegisterAddress";
        private EventFactory _eventFactory;
        private string _registerAddress;
        private int _maxEventCount;
        private int _maxHistoryEventCount;

        #endregion Field

        #region Property

        /// <summary>
        /// Max count for realtime event
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
        /// Max count for history event
        /// </summary>
        public int MaxHistoryEventCount {
            get { return _maxHistoryEventCount; }
            set {
                if (value != _maxHistoryEventCount) {
                    _maxHistoryEventCount = value;
                }
            }
        }

        /// <summary>
        /// Event List 
        /// </summary>
        public Dictionary<string, IEventData> EventList { get; private set; }

        #endregion Property

        #region Event

        /// <summary>
        /// For Realtime Event Change
        /// </summary>
        public event EventChangeHandler RealtimeEventChange;

        /// <summary>
        /// For History Event Change
        /// </summary>
        public event EventChangeHandler HistoryEventChange;

        /// <summary>
        /// For Realtime & History Event Change
        /// </summary>
        public event EventChangeHandler EventChange;

        #endregion Event

        #region Function

        /// <summary>
        /// Init
        /// </summary>
        public override void Init() {
            base.Init();
            EventListInit();
            if (!XML.InitStringAttr<int>(Config, MaxEventCountAttr, out _maxEventCount)) { ErrorAttr.Add(MaxEventCountAttr); InitState = false; }
            if (!XML.InitStringAttr<int>(Config, MaxHistoryEventCountAttr, out _maxHistoryEventCount)) { ErrorAttr.Add(MaxHistoryEventCountAttr); InitState = false; }
            XML.InitStringAttr<string>(Config, RegisterAddressAttr, out _registerAddress);
        }

        /// <summary>
        /// run recorder
        /// </summary>
        public override void Run() {
            base.Run();
            if (!InitState) { Irlovan.Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.AttributeNotInit + ID + ":" + Lib.Array.Array.ListToString(ErrorAttr, ErrorAttrSplitChar)); return; }
            RunEventFactory();
        }

        /// <summary>
        /// dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            if (_eventFactory == null) { return; }
            _eventFactory.EventsTrigger -= BeginSubcription;
            _eventFactory.Dispose();
        }

        /// <summary>
        /// Run Event Factory
        /// </summary>
        private void RunEventFactory() {
            _eventFactory = new EventFactory(EventList, ID, Register, _registerAddress);
            _eventFactory.MaxEventCount = MaxEventCount;
            _eventFactory.MaxHistoryEventCount = MaxHistoryEventCount;
            _eventFactory.EventsTrigger += BeginSubcription;
            _eventFactory.Run();
        }

        /// <summary>
        /// datalist init
        /// </summary>
        private void EventListInit() {
            EventList = new Dictionary<string, IEventData>();
            foreach (var item in DataList) {
                if (((object)item.Value is IEventData) && (!EventList.ContainsKey(item.Key))) {
                    EventList.Add(item.Key, (IEventData)item.Value);
                }
            }
        }

        /// <summary>
        /// Begin Subcription
        /// </summary>
        public void BeginSubcription() {
            if (_eventFactory == null) { return; }
            if (Notice != null) { Notice.ReadValue(_eventFactory.Alarm()); }
            if (RealtimeEventChange != null) { RealtimeEventChange(_eventFactory.ToRealtimeEventXML()); }
            if (HistoryEventChange != null) { HistoryEventChange(_eventFactory.ToHistoryEventXML()); }
            if (EventChange != null) { EventChange(_eventFactory.ToEventXML()); }
        }

        #endregion Function

    }
}



