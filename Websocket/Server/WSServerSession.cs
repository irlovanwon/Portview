///Copyright(c) 2015,HIT All rights reserved.
///Summary:Websocket session
///Author:Irlovan
///Date:2015-11-10
///Description:
///Modification:

using SuperWebSocket;

namespace Irlovan.Canal
{
    public class WSServerSession : ServerSession
    {

        #region Structure

        /// <summary>
        /// Super Websocket session construction 
        /// </summary>
        /// <param name="wsSession"></param>
        public WSServerSession(WebSocketSession wsSession)
            : base(wsSession.SessionID, wsSession.Path) {
            WSSession = wsSession;
        }

        #endregion Structure

        #region Property

        internal WebSocketSession WSSession { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Send message
        /// </summary>
        /// <param name="messsage"></param>
        public override void Send(string messsage) {
            base.Send(messsage);
            WSSession.Send(messsage);
        }

        /// <summary>
        /// close and dispose for the session
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            WSSession.Close();
        }

        #endregion Function

    }
}
