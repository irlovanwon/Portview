///Copyright(c) 2015,HIT All rights reserved.
///Summary:Canal server interface
///Author:Irlovan
///Date:2015-11-10
///Description:Canal for data communication
///Modification:

using System;
using System.Collections.Generic;

namespace Irlovan.Canal
{
    public interface IServer : IDisposable
    {

        #region Property

        /// <summary>
        /// Canal port opened for clients
        /// </summary>
        int Port { get; }

        /// <summary>
        /// SessionList
        /// </summary>
        Dictionary<string, IServerSession> SessionDic { get; set; }

        #endregion Property

        #region Event

        /// <summary>
        /// MessageReceive Event
        /// </summary>
        event MessageReceiveHandler MessageReceive;

        /// <summary>
        /// DataReceive Event
        /// </summary>
        event DataReceiveHandler DataReceive;

        /// <summary>
        /// SessionConnect Event
        /// </summary>
        event SessionConnectHandler SessionConnect;

        /// <summary>
        /// SessionDisconnect Event
        /// </summary>
        event SessionDisconnectHandler SessionDisconnect;

        #endregion Event

        #region Function

        /// <summary>
        /// Start to listen
        /// </summary>
        /// <param name="port"></param>
        void StartListen();

        /// <summary>
        /// Stop listen
        /// </summary>
        void StopListen();

        /// <summary>
        /// MessageReceive event trigger
        /// </summary>
        void MessageReceiveTrigger(IServerSession session, string message);

        /// <summary>
        /// DataReceive event trigger
        /// </summary>
        void DataReceiveTrigger(IServerSession session, byte[] data);

        /// <summary>
        /// SessionConnect event trigger
        /// </summary>
        void SessionConnectTrigger(IServerSession session);

        /// <summary>
        /// SessionDisconnect event trigger
        /// </summary>
        void SessionDisconnectTrigger(IServerSession session);

        #endregion Function

    }
}
