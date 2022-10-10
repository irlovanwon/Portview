///Copyright(c) 2015,HIT All rights reserved.
///Summary:WebSocket Server
///Author:Irlovan
///Date:2015-11-23
///Description:reference:Super WebSocket Library With apache license 2.0
///Modification:2015-11-10

using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketEngine;
using SuperWebSocket;

namespace Irlovan.Canal
{
    public class WSServer : Server
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="config"></param>
        public WSServer(int port)
            : base(port) { }

        #endregion Structure

        #region Field

        private WebSocketServer _socketServer;
        private object _sessionSyncLock = new object();
        private IBootstrap _bootstrap;
        private const string SuperWebSocket_Setup_Name = "SuperWebSocket";
        private const string SuperWebSocket_Setup_IP = "Any";
        private const int SuperWebSocket_Setup_MaxRequest_Length = 1000000;

        #endregion Field

        #region Function

        /// <summary>
        /// Start to listen
        /// </summary>
        /// <param name="port"></param>
        public override void StartListen() {
            base.StartListen();
            var rootConfig = new RootConfig();
            _socketServer = new WebSocketServer();
            _socketServer.NewMessageReceived += MessageReceived_EventHandler;
            _socketServer.NewSessionConnected += SessionConnected_EventHandler;
            _socketServer.NewDataReceived += DataReceived_EventHandler;
            _socketServer.SessionClosed += SessionClosed_EventHandler;
            _socketServer.Setup(rootConfig,
                new ServerConfig {
                    Name = SuperWebSocket_Setup_Name,
                    MaxRequestLength = SuperWebSocket_Setup_MaxRequest_Length,
                    Ip = SuperWebSocket_Setup_IP,
                    Port = Port,
                    Mode = SocketMode.Tcp
                });
            _bootstrap = new DefaultBootstrap(new RootConfig(), new IWorkItem[] { _socketServer });
            _bootstrap.Start();
        }

        /// <summary>
        /// Stop listen
        /// </summary>
        public override void StopListen() {
            base.StopListen();
            StopSuperWebSocketListen();
            StopBootStrap();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            StopSuperWebSocketListen();
            StopBootStrap();
        }

        /// <summary>
        /// StopBootStrap
        /// </summary>
        private void StopBootStrap() {
            if (_bootstrap != null) { _bootstrap.Stop(); }
        }

        /// <summary>
        /// Stop SuperWebSocket Listen
        /// </summary>
        private void StopSuperWebSocketListen() {
            _socketServer.NewMessageReceived -= MessageReceived_EventHandler;
            _socketServer.NewSessionConnected -= SessionConnected_EventHandler;
            _socketServer.NewDataReceived -= DataReceived_EventHandler;
            _socketServer.SessionClosed -= SessionClosed_EventHandler;
        }

        /// <summary>
        /// Session connected event handler
        /// </summary>
        /// <param name="session"></param>
        private void SessionConnected_EventHandler(WebSocketSession session) {
            lock (_sessionSyncLock) {
                WSServerSession wsSession = new WSServerSession(session);
                SessionDic.Add(wsSession.ID, wsSession);
                SessionConnectTrigger(wsSession);
            }
        }

        /// <summary>
        /// Session closed event handler
        /// </summary>
        /// <param name="session"></param>
        /// <param name="reason"></param>
        private void SessionClosed_EventHandler(WebSocketSession session, CloseReason reason) {
            lock (_sessionSyncLock) {
                SessionDisconnectTrigger(SessionDic[session.SessionID]);
                SessionDic[session.SessionID].Dispose();
                SessionDic.Remove(session.SessionID);
            }
        }

        /// <summary>
        /// MessageReceived_EventHandler
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        private void MessageReceived_EventHandler(WebSocketSession session, string message) {
            MessageReceiveTrigger(SessionDic[session.SessionID], message);
        }

        /// <summary>
        /// DataReceived_EventHandler
        /// </summary>
        /// <param name="session"></param>
        /// <param name="data"></param>
        private void DataReceived_EventHandler(WebSocketSession session, byte[] data) {
            DataReceiveTrigger(SessionDic[session.SessionID], data);
        }

        #endregion Function

    }

}
