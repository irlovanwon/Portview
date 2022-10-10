///Copyright(c) 2015,HIT All rights reserved.
///Summary:Canal server
///Author:Irlovan
///Date:2015-11-10
///Description:Canal for communication
///Modification:      

using System.Collections.Generic;
namespace Irlovan.Canal
{
    public class Server : IServer
    {

        #region Structure

        /// <summary>
        /// Canal construction
        /// </summary>
        public Server(int port) {
            _port = port;
            SessionDic = new Dictionary<string, IServerSession>();
        }

        #endregion Structure

        #region Field

        private int _port;

        #endregion Field

        #region Property

        /// <summary>
        /// Canal port opened for clients
        /// </summary>
        public int Port {
            get { return _port; }
            private set {
                if (value != _port) {
                    _port = value;
                }
            }
        }

        /// <summary>
        /// SessionList
        /// </summary>
        public Dictionary<string, IServerSession> SessionDic { get; set; }

        #endregion Property

        #region Event

        /// <summary>
        /// MessageReceive Event
        /// </summary>
        public event MessageReceiveHandler MessageReceive;

        /// <summary>
        /// DataReceive Event
        /// </summary>
        public event DataReceiveHandler DataReceive;

        /// <summary>
        /// SessionConnect Event
        /// </summary>
        public event SessionConnectHandler SessionConnect;

        /// <summary>
        /// SessionDisconnect Event
        /// </summary>
        public event SessionDisconnectHandler SessionDisconnect;

        #endregion Event

        #region Function

        /// <summary>
        /// Start to listen
        /// </summary>
        /// <param name="port"></param>
        public virtual void StartListen() { }

        /// <summary>
        /// Stop listen
        /// </summary>
        public virtual void StopListen() { }

        /// <summary>
        /// Dispose canal server
        /// </summary>
        public virtual void Dispose() {
            foreach (var item in SessionDic) { item.Value.Dispose(); }
        }

        /// <summary>
        /// MessageReceive event trigger
        /// </summary>
        public void MessageReceiveTrigger(IServerSession session, string message) {
            if (MessageReceive != null) { MessageReceive(session, message); }
        }

        /// <summary>
        /// DataReceive event trigger
        /// </summary>
        public void DataReceiveTrigger(IServerSession session, byte[] data) {
            if (DataReceive != null) { DataReceive(session, data); }
        }

        /// <summary>
        /// SessionConnect event trigger
        /// </summary>
        public void SessionConnectTrigger(IServerSession session) {
            if (SessionConnect != null) { SessionConnect(session); }
        }

        /// <summary>
        /// SessionDisconnect event trigger
        /// </summary>
        public void SessionDisconnectTrigger(IServerSession session) {
            if (SessionDisconnect != null) { SessionDisconnect(session); }
        }

        #endregion Function

    }

    /// <summary>
    /// Reason for closing the session
    /// </summary>
    public enum SessionCloseReason
    {
        //Not finished**********************
    }

    /// <summary>
    /// MessageReceive event delegate 
    /// </summary>
    /// <param name="session"></param>
    /// <param name="message"></param>
    public delegate void MessageReceiveHandler(IServerSession session, string message);

    /// <summary>
    /// DataReceive event delegate 
    /// </summary>
    /// <param name="session"></param>
    /// <param name="data"></param>
    public delegate void DataReceiveHandler(IServerSession session, byte[] data);

    /// <summary>
    /// SessionConnect event delegate
    /// </summary>
    /// <param name="session"></param>
    public delegate void SessionConnectHandler(IServerSession session);

    /// <summary>
    /// SessionDisconnect event delegate 
    /// </summary>
    /// <param name="session"></param>
    public delegate void SessionDisconnectHandler(IServerSession session);

}
