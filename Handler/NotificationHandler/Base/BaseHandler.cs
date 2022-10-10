///Copyright(c) 2015,HIT All rights reserved.
///Summary:Base class for notification handler
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using Irlovan.Canal;
using Irlovan.Notification;

namespace Irlovan.Handlers
{
    internal class BaseHandler : Handler, INotificationHandler
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="session"></param>
        /// <param name="local"></param>
        internal BaseHandler(IServerSession session, LocalInterface.LocalInterface local) : base(session, local) { }

        #endregion Structure

        #region Property

        /// <summary>
        /// Notification
        /// </summary>
        public INotification Notification { get; set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Load Notification
        /// </summary>
        /// <returns></returns>
        public virtual bool LoadNotification(INotification notification) { return (notification != null); }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
        }

        #endregion Function

    }
}
