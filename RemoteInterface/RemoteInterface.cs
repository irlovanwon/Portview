///Copyright(c) 2015,HIT All rights reserved.
///Summary:RemoteInterface
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:     

using Irlovan.Canal;
using Irlovan.Handlers;
using Irlovan.Lib.Symbol;
using Irlovan.Log;
using System;
using System.Collections.Generic;

namespace Irlovan.RemoteInterface
{
    public class RemoteInterface : IDisposable
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="local"></param>
        public RemoteInterface(LocalInterface.LocalInterface local) {
            if (local == null) { return; }
            _localInterface = local;
            HandlerTypeInit();
            StartRemoteListen();
        }

        #endregion Structure

        #region Field

        private LocalInterface.LocalInterface _localInterface;
        //defaut handler
        private Dictionary<IServerSession, IHandler> _handlers = new Dictionary<IServerSession, IHandler>();
        private Dictionary<string, Type> _handlerTypeDic = new Dictionary<string, Type>();
        private object _sessionLock = new object();

        #endregion Field

        #region Property

        /// <summary>
        /// Server of communication canal
        /// </summary>
        public IServer CanalServer { get; private set; }

        #endregion Property

        #region Delegate

        /// <summary>
        /// ClientNotify event handler
        /// </summary>
        /// <param name="count"></param>
        public delegate void ClientNotifyHandler(int count);

        #endregion Delegate

        #region Event

        /// <summary>
        /// Event to notify count of client session when changed
        /// </summary>
        public event ClientNotifyHandler ClientNotify;

        #endregion Event

        #region Function

        /// <summary>
        /// start remote service
        /// </summary>
        public void StartRemoteListen() {
            CanalServer = new WSServer(_localInterface.Config.Port);
            CanalServer.SessionConnect += SessionConnectHandler;
            CanalServer.MessageReceive += MessageReceivedHandler;
            CanalServer.DataReceive += DataReceivedHandler;
            CanalServer.SessionDisconnect += SessionDisconnectedHandler;
            string port = _localInterface.Config.Port.ToString();
            try {
                CanalServer.StartListen();
            }
            catch (Exception e) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.ListenPortFailed + port + Symbol.NewLine_Symbol + e.ToString());
                return;
            }
            Global.Info.LogRecorder.Log(LogLevelEnum.Warn, Irlovan.Lib.Properties.Resources.ServerStart);
        }

        /// <summary>
        /// recycle
        /// </summary>
        public void Dispose() {
            _handlerTypeDic.Clear();
            if (CanalServer != null) {
                CanalServer.MessageReceive -= MessageReceivedHandler;
                CanalServer.DataReceive -= DataReceivedHandler;
                CanalServer.SessionConnect -= SessionConnectHandler;
                CanalServer.SessionDisconnect -= SessionDisconnectedHandler;
                CanalServer.StopListen();
                CanalServer.Dispose();
                CanalServer = null;
            }
        }

        /// <summary>
        /// Handler for receiving message
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        private void MessageReceivedHandler(IServerSession session, string message) {
            if (_handlers.ContainsKey(session)) {
                _handlers[session].Handle(session, message);
            }
        }

        /// <summary>
        /// Handler for receiving data
        /// </summary>
        /// <param name="session"></param>
        /// <param name="data"></param>
        private void DataReceivedHandler(IServerSession session, byte[] data) { }

        /// <summary>
        /// Handler for session connected
        /// </summary>
        /// <param name="session"></param>
        private void SessionConnectHandler(IServerSession session) {
            lock (_sessionLock) {
                Type handlerType = GetHandlerType(session.Path.Substring(1, session.Path.Length - 1));
                if (handlerType == null) { return; }
                if (_handlers.ContainsKey(session)) { _handlers.Remove(session); }
                _handlers.Add(session, (IHandler)Activator.CreateInstance(handlerType, new Object[] { session, _localInterface }));
                ClientNotification(_handlers.Count);
            }
        }

        /// <summary>
        /// Handler for session disconnected
        /// </summary>
        /// <param name="session"></param>
        private void SessionDisconnectedHandler(IServerSession session) {
            lock (_sessionLock) {
                if (_handlers.ContainsKey(session)) {
                    _handlers[session].Dispose();
                    _handlers[session] = null;
                    _handlers.Remove(session);
                    ClientNotification(_handlers.Count);
                }
            }
        }

        /// <summary>
        /// get handler type
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Type GetHandlerType(string name) {
            if (_handlerTypeDic.ContainsKey(name)) {
                return _handlerTypeDic[name];
            }
            return null;
        }

        /// <summary>
        /// Init Handler Type
        /// </summary>
        private void HandlerTypeInit() {
            foreach (var item in _localInterface.Chip.Handler.AssemblyList) {
                string fullName = Chip.ChipLoader.NameSpaceValue_Handler + item.Key;
                _handlerTypeDic.Add(item.Key, item.Value.GetType(fullName));
            }
        }

        /// <summary>
        /// ClientNotification
        /// </summary>
        /// <param name="count"></param>
        private void ClientNotification(int count) {
            if (ClientNotify != null) { ClientNotify(count); }
        }

        #endregion Function

    }
}
