///Copyright(c) 2015,HIT All rights reserved.
///Summary:Message group for canal communication
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:      

using Irlovan.Canal;
using Irlovan.Database;
using Irlovan.Lib.XML;
using Irlovan.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Xml.Linq;

namespace Irlovan.Handlers
{
    internal class Group : Handler
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="session"></param>
        /// <param name="local"></param>
        /// <param name="config"></param>
        internal Group(IServerSession session, LocalInterface.LocalInterface local, XElement config)
            : base(session, local) {
            InitState = Init(config);
        }

        #endregion Structure

        #region Field

        private const string ModeAttr = "Mode";
        private int _interval;
        private List<IIndustryData> _dataList = new List<IIndustryData>();
        private System.Timers.Timer _timer;
        private string _groupName;
        //stack for data value
        private Dictionary<string, IndustryDataMessage> _dataStack = new Dictionary<string, IndustryDataMessage>();
        private object _lock = new object();

        #endregion Field

        #region Property

        /// <summary>
        /// interval of the group
        /// </summary>
        internal int Interval {
            get { return _interval; }
            set {
                if (value != _interval) {
                    _interval = value;
                }
            }
        }

        /// <summary>
        /// name of the group
        /// </summary>
        internal string GroupName {
            get { return _groupName; }
            set {
                if (value != _groupName) {
                    _groupName = value;
                }
            }
        }

        /// <summary>
        /// Initstate
        /// </summary>
        internal bool InitState { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// StartSBC
        /// </summary>
        internal void StartSBC() {
            if (!InitState) { return; }
            DisposeTimer();
            StackDataInit();
            HybridMode();
        }

        /// <summary>
        /// Send init data and push stack
        /// </summary>
        internal void StackDataInit() {
            DisposeStackData();
            if (_dataList.Count == 0) { return; }
            XElement message = CreateMessage();
            if (message == null) { return; }
            Session.Send(message.ToString());
        }

        /// <summary>
        /// Stop Subcription
        /// </summary>
        internal void StopSubcrition() {
            DisposeTimer();
            DisposeData();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            DisposeTimer();
            DisposeData();
            DisposeStackData();
        }

        /// <summary>
        /// Send data to client intervally
        /// </summary>
        /// <param name="dataSubcriptionList"></param>
        /// <param name="interval"></param>
        private void HybridMode() {
            DisposeTimer();
            if (_dataList.Count == 0) { return; }
            Irlovan.Lib.Timer.Timer.SetInterval((object o, ElapsedEventArgs e) => {
                XElement message = CreateMessage();
                if (message == null) { return; }
                Session.Send(message.ToString());
            }, ref _timer, _interval);
        }

        /// <summary>
        /// dispose stack data
        /// </summary>
        private void DisposeStackData() {
            foreach (var item in _dataStack) {
                item.Value.Dispose();
            }
            _dataStack.Clear();
        }

        /// <summary>
        /// CreateGroupMessage
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="dataList"></param>
        /// <returns></returns>
        internal XElement CreateMessage() {
            lock (_lock) {
                XElement result = new XElement(DataMessage.Name);
                XElement subcription = new XElement(DataMessage.SubcriptionPara);
                result.Add(subcription);
                XElement group = new XElement(DataMessage.GroupPara);
                group.SetAttributeValue(ModeAttr, _interval);
                group.SetAttributeValue(DataMessage.NamePara, GroupName);
                foreach (var item in _dataList) {
                    IndustryDataMessage message = new IndustryDataMessage(item.FullName, item.Value.ToString(), item.DataType, item.TimeStamp, item.Description, item.Quality);
                    if (message.Equals((_dataStack.ContainsKey(item.FullName)) ? _dataStack[item.FullName] : null)) { continue; }
                    _dataStack[item.FullName] = message;
                    group.Add(message.ToXML(FormatEnum.Basic));
                }
                if (!group.HasElements) { return null; }
                subcription.Add(group);
                return result;
            }
        }

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private bool Init(XElement config) {
            lock (_lock) {
                if (config.Name != DataMessage.GroupPara) { return false; }
                if (!XML.InitStringAttr<int>(config, ModeAttr, out _interval)) { return false; }
                if (!XML.InitStringAttr<string>(config, DataMessage.NamePara, out _groupName)) { return false; }
                int interval = Math.Abs(_interval);
                if (interval == 0) { return false; }
                IEnumerable<XElement> elements = config.Elements(DataMessage.ItemPara);
                if (elements.Count() == 0) { return false; }
                foreach (var item in elements) {
                    string name;
                    if (!XML.InitStringAttr<string>(item, DataMessage.NamePara, out name)) { continue; }
                    IIndustryData data = LocalInterface.Source.AcquireIndustryData(name);
                    if (data == null) { continue; }
                    _dataList.Add(data);
                }
                if (_dataList.Count == 0) { return false; }
                return true;
            }
        }

        /// <summary>
        /// Dispose Data Subcription Timer
        /// </summary>
        private void DisposeTimer() {
            Lib.Timer.Timer.DisposeTimer(_timer);
        }

        /// <summary>
        /// dispose subcripted data
        /// </summary>
        private void DisposeData() {
            lock (_lock) {
                _dataList.Clear();
            }
        }

        #endregion Function

    }
}
