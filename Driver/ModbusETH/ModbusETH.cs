///Copyright(c) 2016,HIT All rights reserved.
///Summary：Horner
///Author：Irlovan
///Date：2015-06-10
///Description：Using NModbus Lib MIT License
///https://code.google.com/p/nmodbus/
///Modification：

using Irlovan.Database;
using Irlovan.Lib.Symbol;
using Irlovan.Lib.XML;
using Irlovan.Log;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Xml.Linq;

namespace Irlovan.Driver
{
    public class ModbusETH : Driver
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        public ModbusETH(Catalog source, XElement config)
            : base(source, config) {
        }

        #endregion Structure

        #region Field

        public const string PortAttr = "Port";
        public const string IPAttr = "IP";
        public const string OffsetAttr = "Offset";
        public const string ModbusTypeAttr = "ModbusType";
        public const int DefaultPortNum = 502;

        private Dictionary<string, MGroup> _mGroups = new Dictionary<string, MGroup>();
        private ModbusIpMaster _master;
        private int _port;
        private string _ip;
        private TcpClient _tcpClient;
        private delegate void RunGroupHandler();

        #endregion Field

        #region Property

        /// <summary>
        /// Port Default to 502
        /// </summary>
        internal int Port {
            get { return _port; }
            private set {
                if (value != _port) {
                    _port = value;
                }
            }
        }

        /// <summary>
        /// IP Address of the device
        /// </summary>
        internal string IP {
            get { return _ip; }
            private set {
                if (value != _ip) {
                    _ip = value;
                }
            }
        }

        #endregion Property

        #region Function

        /// <summary>
        /// Init for the very driver
        /// </summary>
        public override void Init() {
            base.Init();
            _port = ModbusETH.DefaultPortNum;
            if (!XML.InitStringAttr<int>(Config, ModbusETH.PortAttr, out _port)) { InitState = false; }
            if (!XML.InitStringAttr<string>(Config, ModbusETH.IPAttr, out _ip)) { InitState = false; }
        }

        /// <summary>
        /// Init Modbus Groups
        /// </summary>
        private void InitModbusGroups() {
            foreach (var item in DataList) {
                _mGroups.Add(item.Key, new MGroup(item.Value, _master));
            }
        }

        /// <summary>
        /// Init Master
        /// </summary>
        private bool ConnectSlave() {
            try {
                _tcpClient = new TcpClient(_ip, _port);
                _master = ModbusIpMaster.CreateIp(_tcpClient);
            }
            catch (Exception e) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.ModbusDeviceInitFailed + Symbol.Space_Char + _ip + ":" + Symbol.NewLine_Symbol + e.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// Reconnect to the device
        /// </summary>
        public override void Reconnect() {
            base.Reconnect();
        }

        /// <summary>
        /// run opc client
        /// </summary>
        public override void Run() {
            base.Run();
            if (!InitState) { return; }
            if (!ConnectSlave()) { Disconnected(); return; }
            InitModbusGroups();
            ServerConnectedHandler(DateTime.Now);
            RunModbusGroup();
        }

        /// <summary>
        /// Run Modbus Group
        /// </summary>
        private void RunModbusGroup() {
            if (!_tcpClient.Connected) { Disconnected(); return; }
            foreach (var item in _mGroups) {
                if (item.Value.Group.RWMode == RWModeEnum.Input) { item.Value.Read(); }
                if (item.Value.Group.RWMode == RWModeEnum.Output) { item.Value.Write(); }
            }
            RunGroupHandler runGroup = new RunGroupHandler(RunModbusGroup);
            runGroup.BeginInvoke(null, null);
        }

        /// <summary>
        /// handler for WriteDriverData event
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="name"></param>
        /// <param name="addr"></param>
        /// <param name="value"></param>
        /// <param name="timeStamp"></param>
        public override void WriteDataHandler(string groupName, string name, string addr, object value, DateTime timeStamp) {
            base.WriteDataHandler(groupName, name, addr, value, timeStamp);
        }

        /// <summary>
        /// write data list handler
        /// </summary>
        /// <param name="timeStamp"></param>
        public override void WriteDataListHandler(string groupName, Dictionary<string, object> dataList, DateTime timeStamp) {
            base.WriteDataListHandler(groupName, dataList, timeStamp);
        }

        /// <summary>
        /// ServerShutDownEventHandler
        /// </summary>
        public override void ServerShutDownEventHandler(string reason, DateTime timeStamp) {
            base.ServerShutDownEventHandler(reason, timeStamp);
        }

        /// <summary>
        /// Disconnected
        /// </summary>
        private void Disconnected() {
            ServerShutDownEventHandler("", DateTime.Now);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            Clear();
        }

        public override void Stop() {
            base.Stop();
            Clear();
        }

        /// <summary>
        /// Clear();
        /// </summary>
        private void Clear() {
            foreach (var item in _mGroups) {
                item.Value.Dispose();
            }
            _mGroups.Clear();
            if (_master != null) {
                _master.Dispose();
            }
            if (_tcpClient != null) {
                _tcpClient.Close();
            }
        }

        #endregion Function

    }

}
