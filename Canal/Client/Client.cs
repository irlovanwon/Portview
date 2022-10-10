///Copyright(c) 2015,HIT All rights reserved.
///Summary:Canal client
///Author:Irlovan
///Date:2015-11-23
///Description:
///Modification:

namespace Irlovan.Canal
{
    public class Client
    {

        #region Structure

        /// <summary>
        /// Canal client construction
        /// </summary>
        /// <param name="serverIP"></param>
        /// <param name="port"></param>
        /// <param name="flag"></param>
        public Client(string serverIP, int port, string flag) {
            ServerIP = serverIP;
            Port = port;
            Flag = flag;
        }

        #endregion Structure

        #region Property

        /// <summary>
        /// Target ip of webarc server
        /// </summary>
        public string ServerIP { get; private set; }

        /// <summary>
        /// Port of target ip
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Name of the relavant server handler
        /// </summary>
        public string Flag { get; private set; }

        /// <summary>
        /// Socket state
        /// </summary>
        public ClientState State { get { return UpdateClientState(); } }

        #endregion Property

        #region Event

        /// <summary>
        /// MessageReceived event
        /// </summary>
        public event MessageReceivedHandler MessageReceived;

        /// <summary>
        /// Socket Opened event
        /// </summary>
        public event OpenedHandler Opened;

        /// <summary>
        /// Socket error event
        /// </summary>
        public event ErrorHandler Error;

        /// <summary>
        /// Socket closed event
        /// </summary>
        public event ClosedHandler Closed;

        #endregion Event

        #region Function

        /// <summary>
        /// Open connection with server
        /// </summary>
        public virtual void Open() { }

        /// <summary>
        /// Send message to server
        /// </summary>
        /// <param name="message"></param>
        public virtual void Send(string message) { }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose() { }

        /// <summary>
        /// Trigger for MessageReceived event
        /// </summary>
        public void MessageReceivedTrigger(string message) {
            if (MessageReceived != null) { MessageReceived(message); }
        }

        /// <summary>
        /// Trigger for Socket Opened event
        /// </summary>
        public void SocketOpenedTrigger() {
            if (Opened != null) { Opened(); }
        }

        /// <summary>
        /// Trigger for Socket error event
        /// </summary>
        public void SocketErrorTrigger() {
            if (Error != null) { Error(); }
        }

        /// <summary>
        /// Trigger for Socket closed event
        /// </summary>
        public void SocketClosedTrigger() {
            if (Closed != null) { Closed(); }
        }

        /// <summary>
        /// Update Client State
        /// </summary>
        public virtual ClientState UpdateClientState() {
            return ClientState.Closed;
        }

        #endregion Function

    }

    /// <summary>
    /// Handler for message received event
    /// </summary>
    /// <param name="message"></param>
    public delegate void MessageReceivedHandler(string message);

    /// <summary>
    /// Handler for socket open event
    /// </summary>
    public delegate void OpenedHandler();

    /// <summary>
    /// Handler for socket error event
    /// </summary>
    public delegate void ErrorHandler();

    /// <summary>
    /// Handler for socket closed event
    /// </summary>
    public delegate void ClosedHandler();

    /// <summary>
    /// Client State
    /// </summary>
    public enum ClientState
    {
        None = -1,
        Connecting = 0,
        Open = 1,
        Closing = 2,
        Closed = 3,
    }

}
