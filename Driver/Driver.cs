///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Driver base class
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      

using Irlovan.Database;
using Irlovan.DataQuality;
using Irlovan.Lib.Symbol;
using Irlovan.Lib.XML;
using Irlovan.Log;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Xml.Linq;

namespace Irlovan.Driver
{
    public abstract class Driver : IDriver
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="source"></param>
        /// <param name="config"></param>
        public Driver(Catalog source, XElement config) {
            Source = source;
            Config = config;
        }

        #endregion Structure

        #region Field

        public const string CommErrorPara = "CommError";
        public const string RealtimeDataPara = "RealtimeDataName";
        public const string GroupNamePara = "Name";
        public const string UpdateRatePara = "UpdateRate";
        public const string ReadonlyPara = "Readonly";
        public const string AddrPara = "Addr";
        public const string ExpressionPara = "Expression";
        public const string DriverNamePara = "Name";
        public const string GroupTagName = "Group";
        public const string DataTagPara = "Data";
        public const string RWModeAttr = "RWMode";
        private const string ReconnectTimeoutPara = "ReconnectTimeout";
        private const string ServerShutDownActiveReason = "Irlovan";
        private IIndustryData _comm;
        private System.Timers.Timer _reconnectTimer;
        private object _reconnectLock = new object();

        #endregion Field

        #region Property

        /// <summary>
        /// Driver Data List<groupName,IDriverDataBox>
        /// </summary>
        public Dictionary<string, IGroup> DataList { get; private set; }

        /// <summary>
        /// All datas
        /// </summary>
        public Catalog Source { get; set; }

        /// <summary>
        /// config for driver
        /// </summary>
        public XElement Config { get; set; }

        /// <summary>
        /// Uniq name for driver
        /// </summary>
        public string DriverName { get; set; }

        /// <summary>
        /// Comm data to indicate if the device is connected
        /// </summary>
        public string Comm { get; set; }

        /// <summary>
        /// bool to indicate if init success
        /// </summary>
        public bool InitState { get; set; }

        /// <summary>
        /// reconnect time out when device disconnected
        /// </summary>
        public int ReconnectTimeout { get; set; }

        /// <summary>
        /// Timer for reconnection
        /// </summary>
        public System.Timers.Timer ReconnectTimer { get; set; }

        /// <summary>
        /// Write Data Mode
        /// </summary>
        public WriteDataModeEnum WriteDataMode { get; set; }

        /// <summary>
        /// Communication Data to show if the driver is connected to the device
        /// </summary>
        public IIndustryData CommData {
            get { return _comm; }
            set {
                if (value != _comm) {
                    _comm = value;
                }
            }
        }

        #endregion Property

        #region Event

        /// <summary>
        /// server disconnected
        /// </summary>
        public event ServerShutDownHandler ServerShutDown;

        /// <summary>
        /// server connected
        /// </summary>
        public event ServerConnectedHandler ServerConnected;

        #endregion Event

        #region Function

        /// <summary>
        /// Properties int
        /// </summary>
        public virtual void Init() {
            InitState = true;
            if ((Source == null) && (Config == null)) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Warn, Lib.Properties.Resources.DriverNoSource + Symbol.Colon_Char + DriverName);
                InitState = false;
                return;
            }
            PropertiesInit();
            DataListInit();
            RWInit();
            CommDataInit();
        }

        /// <summary>
        /// Started to Run the Driver
        /// </summary>
        public virtual void Run() {
            if (!InitState) { return; }
            WriteDataModeSelect();
        }

        /// <summary>
        /// Stop driver
        /// </summary>
        public virtual void Stop() {
            try {
                Lib.Timer.Timer.DisposeTimer(_reconnectTimer);
            }
            catch (Exception) {
                _reconnectTimer = null;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose() {
            if (DataList != null) {
                DisposeWriteDataSubcription();
                DiposeGroup();
                DataList.Clear();
            }
            try {
                Lib.Timer.Timer.DisposeTimer(_reconnectTimer);
            }
            catch (Exception) {
                _reconnectTimer = null;
            }
        }

        /// <summary>
        /// Disconnect to the server
        /// </summary>
        public virtual void Disconnect() {
            Stop();
            if (ServerShutDown != null) {
                ServerShutDown(ServerShutDownActiveReason, DateTime.Now);
            }
        }

        /// <summary>
        /// ServerShutDownEventHandler
        /// </summary>
        /// <param name="reasom"></param>
        public virtual void ServerShutDownEventHandler(string reason, DateTime timeStamp) {
            Stop();
            Global.Info.LogRecorder.Log(LogLevelEnum.Warn, Lib.Properties.Resources.DriverShutDown + ":" + DriverName);
            SetCommErrorVaribale();
            if (ServerShutDown != null) {
                ServerShutDown(reason, timeStamp);
            }
            QualityBad();
            Reconnect();
        }

        /// <summary>
        /// device connected
        /// </summary>
        /// <param name="timeStamp"></param>
        public virtual void ServerConnectedHandler(DateTime timeStamp) {
            Global.Info.LogRecorder.Log(LogLevelEnum.Warn, Lib.Properties.Resources.DriverConnected + ":" + DriverName);
            if (_comm == null) { return; }
            _comm.ReadValue(true);
            if (ServerConnected != null) {
                ServerConnected(timeStamp);
            }
        }

        /// <summary>
        /// handler for WriteDriverData event
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="name"></param>
        /// <param name="addr"></param>
        /// <param name="value"></param>
        /// <param name="timeStamp"></param>
        public virtual void WriteDataHandler(string groupName, string name, string addr, object value, DateTime timeStamp) { }


        /// <summary>
        /// write data list handler
        /// </summary>
        /// <param name="timeStamp"></param>
        public virtual void WriteDataListHandler(string groupName, Dictionary<string, object> dataList, DateTime timeStamp) { }

        /// <summary>
        /// DiposeGroup
        /// </summary>
        private void DiposeGroup() {
            foreach (var item in DataList) {
                item.Value.Dispose();
            }
        }

        /// <summary>
        /// DisposeWriteDataSubcription
        /// </summary>
        private void DisposeWriteDataSubcription() {
            foreach (var item in DataList.Values) {
                item.RWDataBox.WriteDataList -= WriteDataListHandler;
                item.RWDataBox.WriteData -= WriteDataHandler;
            }
        }

        /// <summary>
        /// Init properties for driver
        /// </summary>
        private void PropertiesInit() {
            DriverNameInit();
            ReconnectTimeoutInit();
        }

        /// <summary>
        /// Init for driver name
        /// </summary>
        private void DriverNameInit() {
            XAttribute nameAttr = Config.Attribute(DriverNamePara);
            if (nameAttr == null) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, DriverName + ":" + Symbol.NewLine_Symbol + Lib.Properties.Resources.AttrNotFound + DriverNamePara);
                InitState = false;
                return;
            }
            DriverName = nameAttr.Value;
        }

        /// <summary>
        /// Init for ReconnectTimeout
        /// </summary>
        private void ReconnectTimeoutInit() {
            XAttribute timeout = Config.Attribute(ReconnectTimeoutPara);
            int result;
            if ((timeout == null) || (!int.TryParse(timeout.Value, out result))) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, DriverName + ":" + Symbol.NewLine_Symbol + Lib.Properties.Resources.AttrNotFound + ReconnectTimeoutPara);
                InitState = false;
                return;
            }
            ReconnectTimeout = result;
        }

        /// <summary>
        /// init for RW
        /// </summary>
        private void RWInit() {
            foreach (var item in DataList) {
                item.Value.InitRWBox();
            }
        }


        /// <summary>
        /// init for datalist
        /// </summary>
        private void DataListInit() {
            DataList = new Dictionary<string, IGroup>();
            foreach (var group in Config.Elements()) {
                if (!GroupInit(group)) { continue; }
            }
        }

        /// <summary>
        /// init in Group
        /// </summary>
        private bool GroupInit(XElement group) {
            if (group.Name != GroupTagName) { return false; }
            string groupName; RWModeEnum rwMode;
            if (!XML.InitStringAttr<string>(group, GroupNamePara, out groupName)) { return false; }
            if (!XML.InitStringAttr<RWModeEnum>(group, RWModeAttr, out rwMode)) { rwMode = RWModeEnum.Input; }
            if (DataList.ContainsKey(groupName)) { return false; }
            IGroup dataMap = new Group(groupName, rwMode, group);
            int updateRate;
            if (XML.InitStringAttr<int>(group, UpdateRatePara, out updateRate)) { dataMap.UpdateRate = updateRate; }
            foreach (var realtimeData in group.Elements()) {
                if (!DataInit(realtimeData, groupName, dataMap)) { continue; }
            }
            DataList.Add(groupName, dataMap);
            dataMap.ApplyQuality(QualityEnum.Bad);
            return true;
        }

        /// <summary>
        /// Each data in gourp init 
        /// </summary>
        /// <returns></returns>
        private bool DataInit(XElement realtimeData, string groupName, IGroup dataMap) {
            IIndustryData data; string name; bool readOnly; string address; DriverData driverData;
            if (!DataNameInit(realtimeData, out data, out name)) { return false; }
            if (!DataAddressInit(realtimeData, name, out address)) { return false; }
            ReadonlyInit(realtimeData, name, out readOnly);
            if (!DriverDataInit(realtimeData, readOnly, groupName, data, address, dataMap, out driverData)) { return false; }
            dataMap.Push(driverData);
            return true;
        }

        /// <summary>
        /// Init data name
        /// </summary>
        /// <returns></returns>
        private bool DataNameInit(XElement realtimeData, out IIndustryData data, out string name) {
            data = null; name = null;
            XAttribute nameAttr = realtimeData.Attribute(RealtimeDataPara);
            if (nameAttr == null) { return false; }
            name = nameAttr.Value;
            data = Source.AcquireIndustryData(name);
            if (data == null) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, DriverName + Lib.Properties.Resources.DriverDataNotFound + name);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Init data address
        /// </summary>
        /// <returns></returns>
        private bool DataAddressInit(XElement realtimeData, string name, out string address) {
            address = null;
            XAttribute addressAttr = realtimeData.Attribute(AddrPara);
            if (addressAttr == null) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Warn, DriverName + Lib.Properties.Resources.DriverAddrNotFound + name);
                return false;
            }
            address = addressAttr.Value;
            return true;
        }

        /// <summary>
        /// Init driver data
        /// </summary>
        /// <returns></returns>
        private bool DriverDataInit(XElement realtimeData, bool readOnly, string groupName, IIndustryData data, string address, IGroup dataMap, out DriverData driverData) {
            driverData = null;
            XAttribute expressionAttr = realtimeData.Attribute(ExpressionPara);
            if (dataMap.Contain(data.FullName)) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.DriverDataExist + data.FullName);
                return false;
            }
            driverData = new DriverData(data, groupName, readOnly, address, ((expressionAttr == null) || (expressionAttr.Value == string.Empty)) ? null : new Irlovan.Expression.Expression(expressionAttr.Value));
            return true;
        }

        /// <summary>
        /// Readonly property init
        /// </summary>
        private void ReadonlyInit(XElement realtimeData, string name, out bool readOnly) {
            if (!XML.InitStringAttr<bool>(realtimeData, ReadonlyPara, out readOnly)) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.ReadonlyPropertyError + ":" + name);
                InitState = false;
            }
        }

        /// <summary>
        /// Init comm data
        /// </summary>
        /// <returns></returns>
        private void CommDataInit() {
            XAttribute attr = Config.Attribute(CommErrorPara);
            if (attr == null) { return; }
            IIndustryData data = Source.AcquireIndustryData(attr.Value);
            if (data == null) { return; }
            if (data.DataType != typeof(System.Boolean)) { return; }
            Comm = attr.Value;
            _comm = Source.AcquireIndustryData(Comm);
            if ((_comm == null) || (_comm.DataType != typeof(System.Boolean))) { _comm = null; }
        }

        /// <summary>
        /// WriteDataModeSelect
        /// </summary>
        private void WriteDataModeSelect() {
            switch (WriteDataMode) {
                case WriteDataModeEnum.Single:
                    StartDataSubcription();
                    break;
                case WriteDataModeEnum.List:
                    StartDataListSubcription();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// StartDataSubcription
        /// </summary>
        private void StartDataSubcription() {
            foreach (var item in DataList.Values) {
                item.RWDataBox.WriteData += WriteDataHandler;
            }
        }

        /// <summary>
        /// StartDataListSubcription
        /// </summary>
        private void StartDataListSubcription() {
            foreach (var item in DataList.Values) {
                item.RWDataBox.WriteDataList += WriteDataListHandler;
            }
        }

        /// <summary>
        /// Makes all quality bad!!!!!!!!!!
        /// </summary>
        private void QualityBad() {
            if (DataList == null) { return; }
            foreach (var item in DataList) {
                item.Value.ApplyQuality(QualityEnum.Bad);
            }
        }

        /// <summary>
        /// When communication disconnected set the appointed variable to true
        /// </summary>
        private void SetCommErrorVaribale() {
            if (_comm == null) { return; }
            _comm.ReadValue(false);
        }

        /// <summary>
        /// Reconnect to the device
        /// </summary>
        public virtual void Reconnect() {
            Lib.Timer.Timer.DisposeTimer(_reconnectTimer);
            Lib.Timer.Timer.SetTimeout((object o, ElapsedEventArgs e) => {
                Stop();
                Run();
            }, ref _reconnectTimer, ReconnectTimeout);
        }

        #endregion Function
    }

    /// <summary>
    /// ServerShutDownHandler
    /// </summary>
    /// <param name="reason"></param>
    /// <param name="timeStamp"></param>
    public delegate void ServerShutDownHandler(string reason, DateTime timeStamp);

    /// <summary>
    /// ServerConnectedHandler
    /// </summary>
    /// <param name="timeStamp"></param>
    public delegate void ServerConnectedHandler(DateTime timeStamp);

    /// <summary>
    /// WriteDataHandler<T>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="groupName"></param>
    /// <param name="name"></param>
    /// <param name="addr"></param>
    /// <param name="value"></param>
    /// <param name="timeStamp"></param>
    public delegate void WriteDataHandler<T>(string groupName, string name, string addr, T value, DateTime timeStamp);

    /// <summary>
    /// WriteDataHandler
    /// </summary>
    /// <param name="groupName"></param>
    /// <param name="name"></param>
    /// <param name="addr"></param>
    /// <param name="value"></param>
    /// <param name="timeStamp"></param>
    public delegate void WriteDataHandler(string groupName, string name, string addr, object value, DateTime timeStamp);

    /// <summary>
    /// WriteDataListHandler
    /// </summary>
    /// <param name="groupName"></param>
    /// <param name="values"></param>
    /// <param name="timeStamp"></param>
    public delegate void WriteDataListHandler(string groupName, Dictionary<string, object> values, DateTime timeStamp);

    /// <summary>
    /// WriteDataModeEnum
    /// </summary>
    public enum WriteDataModeEnum { Single, List }

}
