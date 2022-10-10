///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:interface
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using Irlovan.Notification;

namespace Irlovan.Handlers
{
    internal interface INotificationHandler : IHandler
    {

        #region Property

        /// <summary>
        /// Notification
        /// </summary>
        INotification Notification { get; set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Load Notification
        /// </summary>
        /// <returns></returns>
        bool LoadNotification(INotification notification);

        #endregion Function

    }
}
