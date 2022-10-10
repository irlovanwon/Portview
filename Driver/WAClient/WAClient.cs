///Copyright(c) 2013,HIT All rights reserved.
///Summary:WebsocketClientfor Net4.0
///Author:Irlovan
///Date:2013-10-15
///Description:

using Irlovan.Canal;
using Irlovan.Database;
using Irlovan.Lib.XML;
using Irlovan.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Irlovan.Driver
{
    public class WAClient : Driver
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="source"></param>
        /// <param name="config"></param>
        public WAClient(Catalog source, XElement config)
            : base(source, config) {
        }

        #endregion Structure

        #region Field

        private const string PortPara = "Port";
        private const string IPPara = "IP";
        private const string HandlerName = "MessageHandler";
        private const string RootTag = "Data";
        private const string ModeAttr = "Mode";
        private const string ItemTag = "Item";
        private const string NameAttr = "Name";
        private const string ValueAttr = "Value";
        private const string SubcriptionTag = "SBC";
        private const string GroupTag = "Group";
        private const string InDataMessageTag = "InDataMessage";
        private const string WriteDataTag = "WRT";
        private WSClient _client;
        private string _ip;
        private int _port;

        private Dictionary<string, WAGroup> _groupList = new Dictionary<string, WAGroup>();

        #endregion Field

        #region Property

        /// <summary>
        /// Target ip of webarc server
        /// </summary>
        public string IP {
            get { return _ip; }
            private set {
                if (value != _ip) {
                    _ip = value;
                }
            }
        }

        /// <summary>
        /// Port of target ip
        /// </summary>
        public int Port {
            get { return _port; }
            private set {
                if (_port != value) {
                    _port = value;
                }
            }
        }

        #endregion Property

        #region Function

        /// <summary>
        /// Init
        /// </summary>
        public override void Init() {
            base.Init();
            if (!XML.InitStringAttr<string>(Config, IPPara, out _ip)) { InitState = false; }
            if (!XML.InitStringAttr<int>(Config, PortPara, out _port)) { InitState = false; }
        }

        /// <summary>
        /// Run driver
        /// </summary>
        public override void Run() {
            base.Run();
            if (!InitState) { return; }
            InitWAGroup();
            RWInit();
            RunWebSocket();
        }

        /// <summary>
        /// Run Websocket
        /// </summary>
        private void RunWebSocket() {
            _client = new WSClient(IP, Port, HandlerName);
            _client.MessageReceived += MessageRecieved_EventHandler;
            _client.Opened += ConnectionOpened_EventHandler;
            _client.Error += SocketError_EventHandler;
            _client.Closed += ConnectionClosed_EventHandler;
            _client.Open();
        }

        /// <summary>
        /// Handler for message received event
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void MessageRecieved_EventHandler(string message) {
            TextReader messageStr = new StringReader(message);
            XElement messageElement = XElement.Load(messageStr);
            if (messageElement.Name != RootTag) { return; }
            XElement sbc = messageElement.Element(SubcriptionTag);
            if ((sbc == null)) { return; }
            IEnumerable<XElement> groupMessageList = sbc.Elements(GroupTag);
            if (groupMessageList.Count() == 0) { return; }
            foreach (var item in groupMessageList) {
                ParseGroupMessage(item);
            }
        }

        /// <summary>
        /// Handler for ConnectionOpened event
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void ConnectionOpened_EventHandler() {
            XElement message = CreateSBCMessage();
            if (message == null) { return; }
            _client.Send(message.ToString());
        }

        /// <summary>
        /// Handler for SocketError event
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void SocketError_EventHandler() {
            ServerShutDownEventHandler("", DateTime.Now);
        }

        /// <summary>
        /// Handler for ConnectionClosed event
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void ConnectionClosed_EventHandler() {
            ServerShutDownEventHandler("", DateTime.Now);
        }

        /// <summary>
        /// ParseGroupMessage
        /// </summary>
        private void ParseGroupMessage(XElement groupMessage) {
            string groupName;
            if (!XML.InitStringAttr<string>(groupMessage, NameAttr, out groupName)) { return; }
            foreach (var item in groupMessage.Elements(InDataMessageTag)) {
                IndustryDataMessage message = new IndustryDataMessage(item);
                if (!_groupList.ContainsKey(groupName)) { continue; }
                WAGroup group = _groupList[groupName];
                SetValues(group, message);
            }
        }

        /// <summary>
        /// SetValues
        /// </summary>
        private void SetValues(WAGroup group, IndustryDataMessage message) {
            if (!group.ContainAddress(message.Name)) { return; }
            Dictionary<string, IDriverData> dataList = group[message.Name];
            foreach (var item in dataList) {
                item.Value.ReadValue(message.Value, message.Quality);
            }
        }

        /// <summary>
        ///create data subcription message
        /// </summary>
        private XElement CreateSBCMessage() {
            XElement result = new XElement(RootTag);
            XElement sbc = new XElement(SubcriptionTag);
            foreach (var item in DataList) {
                XElement groupMessage = CreateGroupSBCMessage(item.Value);
                if (groupMessage == null) { continue; }
                sbc.Add(groupMessage);
            }
            if (sbc.Elements().Count() == 0) { return null; }
            result.Add(sbc);
            return result;
        }

        /// <summary>
        /// Create Group SBC Message
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private XElement CreateGroupSBCMessage(IGroup group) {
            if (group.Count == 0) { return null; }
            if (group.UpdateRate == 0) { return null; }
            XElement result = new XElement(GroupTag);
            result.SetAttributeValue(ModeAttr, group.UpdateRate);
            result.SetAttributeValue(NameAttr, group.GroupName);
            foreach (var data in group.DataBox) {
                XElement item = new XElement(ItemTag);
                item.SetAttributeValue(NameAttr, data.Value.Address);
                result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// create data settered message
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="dataBox"></param>
        /// <returns></returns>
        private XElement CreateWRTMessage(Dictionary<string, object> dataBox) {
            XElement result = new XElement(RootTag);
            XElement wrt = new XElement(WriteDataTag);
            foreach (var data in dataBox) {
                XElement item = new XElement(ItemTag);
                item.SetAttributeValue(NameAttr, data.Key);
                item.SetAttributeValue(ValueAttr, data.Value);
                wrt.Add(item);
            }
            result.Add(wrt);
            return result;
        }

        /// <summary>
        /// ServerShutDownEventHandler
        /// </summary>
        public override void ServerShutDownEventHandler(string reason, DateTime timeStamp) {
            base.ServerShutDownEventHandler(reason, timeStamp);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            RWDispose();
            DisposeWebSocket();
        }

        /// <summary>
        /// DisposeWebSocket
        /// </summary>
        private void DisposeWebSocket() {
            _client.MessageReceived -= MessageRecieved_EventHandler;
            _client.Opened -= ConnectionOpened_EventHandler;
            _client.Error -= SocketError_EventHandler;
            _client.Closed -= ConnectionClosed_EventHandler;
            if (_client.State != ClientState.Open) { return; }
            _client.Dispose();
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
            Write(groupName, new Dictionary<string, object>() { { addr, value } });
        }

        /// <summary>
        /// write data list handler
        /// </summary>
        /// <param name="timeStamp"></param>
        public override void WriteDataListHandler(string groupName, Dictionary<string, object> dataList, DateTime timeStamp) {
            base.WriteDataListHandler(groupName, dataList, timeStamp);
            Write(groupName, dataList);
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
        /// Write Data to Server
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="data"></param>
        private void Write(string groupName, Dictionary<string, object> data) {
            XElement wrtMessage = CreateWRTMessage(data);
            _client.Send(wrtMessage.ToString());
        }

        /// <summary>
        /// InitWAGroup
        /// </summary>
        private void InitWAGroup() {
            foreach (var item in DataList) {
                if (_groupList.ContainsKey(item.Key)) { continue; }
                _groupList.Add(item.Key, new WAGroup(item.Value));
            }
        }

        #endregion Function

    }
}
