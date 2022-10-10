///Copyright(c) 2015,HIT All rights reserved.
///Summary:Notification handler for canal communication
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:      

using Irlovan.Canal;
using Irlovan.Lib.XML;
using Irlovan.Log;
using Irlovan.Notification;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Handlers
{
    public class NotificationHandler : Handler
    {

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="session"></param>
        /// <param name="local"></param>
        public NotificationHandler(IServerSession session, LocalInterface.LocalInterface local)
            : base(session, local) {
            InitFactory();

        }

        #region Field

        //SQL Factories
        private Dictionary<string, INotificationHandler> _factories = new Dictionary<string, INotificationHandler>();
        internal const string NotificationIDAttr = "ID";

        #endregion Field

        #region Function

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool Handle(IServerSession session, string message) {
            if (!base.Handle(session, message)) { return false; }
            if (LocalInterface.Notification == null) { return false; }
            XElement config = XML.Parse(message);
            if (config == null) { return false; }
            string factoryName = config.Name.ToString();
            if (!_factories.ContainsKey(factoryName)) { return false; }
            string notificationID;
            if (!XML.InitStringAttr<string>(config, NotificationIDAttr, out notificationID)) { return false; }
            INotification notification = GetNotification(notificationID);
            if ((notification == null) || (!_factories[factoryName].LoadNotification(notification))) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.HandlerNotFound + notificationID); return false; }
            return _factories[factoryName].Handle(Session, config);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            foreach (var item in _factories) {
                item.Value.Dispose();
            }
        }

        /// <summary>
        /// Init Message Factory
        /// </summary>
        private void InitFactory() {
            _factories.Add(EventNotificationHandler.Name, new EventNotificationHandler(Session, LocalInterface));
        }

        /// <summary>
        /// Get Notification by Notification ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private INotification GetNotification(string id) {
            return (!LocalInterface.Notification.NotificationList.ContainsKey(id)) ? null : LocalInterface.Notification.NotificationList[id];
        }

        #endregion Function

    }
}

