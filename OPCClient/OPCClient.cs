///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:OPC Client driver
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using Irlovan.Database;
using Irlovan.Lib.Symbol;
using Irlovan.Log;
using OPCAutomation;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Xml.Linq;

namespace Irlovan.Driver
{

    public class OPCClient : Driver
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="source"></param>
        /// <param name="config"></param>
        public OPCClient(Catalog source, XElement config)
            : base(source, config) { }

        #endregion Structure

        #region Field

        public OPCServer _server;
        //<GoupName,GroupBox>
        private Dictionary<string, GroupBox> _groupBox;

        private const string ServerNamePara = "ServerName";
        private const string ServerIPPara = "ServerIP";

        #endregion Field

        #region Property

        /// <summary>
        /// ProgID of OPC Server
        /// </summary>
        public string ServerName { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Init for the very driver
        /// </summary>
        public override void Init() {
            base.Init();
            ProgIDInit();
        }

        /// <summary>
        /// Init ProgID
        /// </summary>
        private void ProgIDInit() {
            XAttribute progIDAttr = Config.Attribute(ServerNamePara);
            if (progIDAttr == null) {
                InitState = false;
                return;
            }
            ServerName = progIDAttr.Value;
        }

        /// <summary>
        /// run opc client
        /// </summary>
        public override void Run() {
            base.Run();
            if (!InitState) { return; }
            RWInit();
            _groupBox = new Dictionary<string, GroupBox>();
            try {
                _server = new OPCServer();
                _server.ServerShutDown += OPCShutDownHandler;
            } catch (Exception e) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, DriverName + Symbol.Colon_Char + Lib.Properties.Resources.DriverConnectFail + Symbol.NewLine_Symbol + e.ToString());
                ServerShutDownEventHandler("", DateTime.Now);
                return;
            }
            if (!Connect()) { ServerShutDownEventHandler("", DateTime.Now); return; };
            ServerConnectedHandler(DateTime.Now);
            foreach (var item in Config.Elements()) {
                XAttribute groupNameAttr = item.Attribute(GroupNamePara);
                if (groupNameAttr == null) { continue; }
                string groupName = groupNameAttr.Value;
                if (_groupBox.ContainsKey(groupName)) { continue; }
                _groupBox.Add(groupName, new GroupBox(DataList[groupName], _server, item));
            }
        }

        /// <summary>
        /// Init RW Interface
        /// </summary>
        private void RWInit() {
            foreach (var item in DataList) {
                item.Value.RWDataBox.WriteData += WriteDataHandler;
            }
        }

        /// <summary>
        /// Dispose RW Interface
        /// </summary>
        private void RWDispose() {
            foreach (var item in DataList) {
                item.Value.RWDataBox.WriteData -= WriteDataHandler;
            }
        }

        /// <summary>
        /// connect to the server
        /// </summary>
        /// <returns></returns>
        public bool Connect() {
            try {
                _server.Connect(ServerName, ((Config.Attribute(ServerIPPara) == null) ? "" : Config.Attribute(ServerIPPara).Value));
                if (_server.ServerState != (int)OPCServerState.OPCRunning) {
                    Global.Info.LogRecorder.Log(LogLevelEnum.Error, DriverName + ":" + Lib.Properties.Resources.DriverConnectFail);
                    return false;
                }
            } catch (Exception e) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, DriverName + ":" + Lib.Properties.Resources.DriverConnectFail + Symbol.NewLine_Symbol + e.ToString());
                return false;
            }
            return true;
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
            Write(groupName, new Dictionary<string, object>() { { name, value } });
        }

        /// <summary>
        /// write data list handler
        /// </summary>
        /// <param name="timeStamp"></param>
        public override void WriteDataListHandler(string groupName, Dictionary<string, object> dataList, DateTime timeStamp) {
            base.WriteDataListHandler(groupName, dataList, timeStamp);
            Write(groupName, dataList);
        }

        //data to be write to opc channal
        [HandleProcessCorruptedStateExceptions]
        private void Write(string groupName, Dictionary<string, object> data) {
            if (!_groupBox.ContainsKey(groupName)) { return; }
            _groupBox[groupName].Write(data);
        }

        /// <summary>
        /// release
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            Clear();
        }

        /// <summary>
        /// Stop and reconnect
        /// </summary>
        public override void Stop() {
            base.Stop();
            Clear();
        }

        /// <summary>
        /// Clear
        /// </summary>
        private void Clear() {
            if (_server != null) {
                _server.Disconnect();
                _server.ServerShutDown -= OPCShutDownHandler;
                _server = null;
            }
            RWDispose();
            if (_groupBox != null) {
                foreach (var item in _groupBox) {
                    item.Value.Dispose();
                }
                _groupBox.Clear();
                _groupBox = null;
            }
        }

        /// <summary>
        /// ServerShutDownEventHandler
        /// </summary>
        public override void ServerShutDownEventHandler(string reason, DateTime timeStamp) {
            base.ServerShutDownEventHandler(reason, timeStamp);
        }

        /// <summary>
        /// OPCServerShutDownEventHandler
        /// </summary>
        /// <param name="reason"></param>
        private void OPCShutDownHandler(string reason) {
            ServerShutDownEventHandler(reason, DateTime.Now);
        }


        #endregion Function

    }
}
