///Copyright(c) 2015,HIT All rights reserved.
///Summary:Canal client interface
///Author:Irlovan
///Date:2015-11-23
///Description:
///Modification:      

using System;

namespace Irlovan.Canal
{
    interface IClient : IDisposable
    {

        #region Property

        /// <summary>
        /// Target ip of webarc server
        /// </summary>
        string ServerIP { get; }

        /// <summary>
        /// Port of target ip
        /// </summary>
        int Port { get; }

        /// <summary>
        /// Socket state
        /// </summary>
        ClientState State { get; }

        /// <summary>
        /// Name of the relavant server handler
        /// </summary>
        string Flag { get; }

        #endregion Property

        #region Event

        /// <summary>
        /// MessageReceived event
        /// </summary>
        event MessageReceivedHandler MessageReceived;

        /// <summary>
        /// Socket Opened event
        /// </summary>
        event OpenedHandler Opened;

        /// <summary>
        /// Socket error event
        /// </summary>
        event ErrorHandler Error;

        /// <summary>
        /// Socket closed event
        /// </summary>
        event ClosedHandler Closed;

        #endregion Event

        #region Function

        /// <summary>
        /// Open connection with server
        /// </summary>
        void Open();

        /// <summary>
        /// Send message to server
        /// </summary>
        /// <param name="message"></param>
        void Send(string message);

        /// <summary>
        /// Trigger for MessageReceived event
        /// </summary>
        void MessageReceivedTrigger(string message);

        /// <summary>
        /// Trigger for Socket Opened event
        /// </summary>
        void SocketOpenedTrigger();

        /// <summary>
        /// Trigger for Socket error event
        /// </summary>
        void SocketErrorTrigger();

        /// <summary>
        /// Trigger for Socket closed event
        /// </summary>
        void SocketClosedTrigger();

        /// <summary>
        /// Update Client State
        /// </summary>
        ClientState UpdateClientState();

        #endregion Function

    }
}
