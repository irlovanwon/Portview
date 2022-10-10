///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Handler for communication canal
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using Irlovan.Canal;
using System.Xml.Linq;

namespace Irlovan.Handlers
{
    public class Handler : IHandler
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="session"></param>
        /// <param name="local"></param>
        public Handler(IServerSession session, LocalInterface.LocalInterface local) {
            Session = session;
            LocalInterface = local;
        }

        #endregion Structure

        #region Property

        /// <summary>
        /// session of the handler
        /// </summary>
        public IServerSession Session { get; set; }

        /// <summary>
        /// Local Interface to handler
        /// </summary>
        public LocalInterface.LocalInterface LocalInterface { get; set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Handler String
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual bool Handle(IServerSession session, string message) { return HandleSession(session); }

        /// <summary>
        /// Handle Bytes
        /// </summary>
        /// <param name="session"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual bool Handle(IServerSession session, byte[] bytes) { return HandleSession(session); }

        /// <summary>
        /// Handler String
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual bool Handle(IServerSession session, XElement element) { return HandleSession(session); }

        /// <summary>
        /// Check is the session matches
        /// </summary>
        /// <returns></returns>
        private bool HandleSession(IServerSession session) {
            if ((session.ID != Session.ID) || (LocalInterface == null)) { return false; }
            return true;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose() { }

        #endregion Function

    }
}
