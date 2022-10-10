///Copyright(c) 2015,HIT All rights reserved.
///Summary:Websocket client class
///Author:Irlovan
///Date:2015-11-23
///Description:reference:Super WebSocket Library With apache license 2.0
////http://websocket4net.codeplex.com/downloads/get/454203
///Modification:

using Irlovan.Lib.Symbol;
using SuperSocket.ClientEngine;
using System;
using WebSocket4Net;

namespace Irlovan.Canal
{
    public class WSClient : Client
    {

        #region Structure

        /// <summary>
        /// Websocket client construction
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="flag"></param>
        public WSClient(string ip, int port, string flag)
            : base(ip, port, flag) {
            Init();
        }

        #endregion Structure

        #region Field

        private WebSocket _client;
        private const string StartAddressFlag = "ws://";

        #endregion Field

        #region Function

        /// <summary>
        /// Open connection with server
        /// </summary>
        public override void Open() {
            base.Open();
            _client.Open();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            _client.MessageReceived -= MessageRecieved_EventHandler;
            _client.Opened -= SocketOpened_EventHandler;
            _client.Error -= SocketError_EventHandler;
            _client.Closed -= SocketClosed_EventHandler;
            _client.Close();
        }

        /// <summary>
        /// Send message to server
        /// </summary>
        /// <param name="message"></param>
        public override void Send(string message) {
            base.Send(message);
            _client.Send(message);
        }

        /// <summary>
        /// UpdateClientState
        /// </summary>
        /// <returns></returns>
        public override ClientState UpdateClientState() {
            return TransferSUState(_client.State);
        }

        /// <summary>
        /// Transfer Super websocket client state to ClientState
        /// </summary>
        /// <returns></returns>
        private ClientState TransferSUState(WebSocketState state) {
            switch (state) {
                case WebSocketState.Closed:
                    return ClientState.Closed;
                case WebSocketState.Closing:
                    return ClientState.Closing;
                case WebSocketState.Connecting:
                    return ClientState.Connecting;
                case WebSocketState.None:
                    return ClientState.None;
                case WebSocketState.Open:
                    return ClientState.Open;
                default:
                    return ClientState.Closed;
            }
        }

        /// <summary>
        /// Init
        /// </summary>
        private void Init() {
            _client = new WebSocket(string.Concat(StartAddressFlag, ServerIP, Symbol.Colon_Char, Port, Symbol.Catalog_Char + Flag));
            _client.MessageReceived += MessageRecieved_EventHandler;
            _client.Opened += SocketOpened_EventHandler;
            _client.Error += SocketError_EventHandler;
            _client.Closed += SocketClosed_EventHandler;
        }

        /// <summary>
        /// Event handler superwebsocket client
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void MessageRecieved_EventHandler(object o, MessageReceivedEventArgs e) {
            MessageReceivedTrigger(e.Message);
        }

        /// <summary>
        ///  Event handler superwebsocket client
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void SocketOpened_EventHandler(object o, EventArgs e) {
            SocketOpenedTrigger();
        }

        /// <summary>
        ///  Event handler superwebsocket client
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void SocketError_EventHandler(object o, ErrorEventArgs e) {
            SocketErrorTrigger();
        }

        /// <summary>
        ///  Event handler superwebsocket client
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void SocketClosed_EventHandler(object o, EventArgs e) {
            SocketClosedTrigger();
        }

        #endregion Function

    }
}
