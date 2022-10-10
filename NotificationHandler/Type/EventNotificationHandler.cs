///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Event notification handler
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using Irlovan.Canal;
using Irlovan.Lib.XML;
using Irlovan.Notification;
using System.Xml.Linq;

namespace Irlovan.Handlers
{
    internal class EventNotificationHandler : BaseHandler
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="session"></param>
        /// <param name="local"></param>
        internal EventNotificationHandler(IServerSession session, LocalInterface.LocalInterface local)
            : base(session, local) { }

        #endregion Structure

        #region Field

        internal const string Name = "EventNotificationHandler";
        internal const string AlarmTypeAttr = "Type";
        private enum AlarmTypeEnum { Realtime, History, Both }

        #endregion Field

        #region Function

        /// <summary>
        /// Handler String
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool Handle(IServerSession session, XElement element) {
            if (!base.Handle(session, element)) { return false; }
            DisposeNotification();
            AlarmTypeEnum type;
            if (!XML.InitStringAttr<AlarmTypeEnum>(element, AlarmTypeAttr, out type)) { return false; }
            Alarm(type, (IEventNotification)Notification);
            return true;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            DisposeNotification();
        }

        /// <summary>
        /// Dispose Notification
        /// </summary>
        private void DisposeNotification() {
            if ((Notification == null) || (!(Notification is IEventNotification))) { return; }
            IEventNotification notification = (IEventNotification)Notification;
            notification.RealtimeEventChange -= NotificationEventHandler;
            notification.EventChange -= NotificationEventHandler;
            notification.HistoryEventChange -= NotificationEventHandler;
        }

        /// <summary>
        /// Load Notification
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        public override bool LoadNotification(INotification notification) {
            if (!base.LoadNotification(notification)) { return false; }
            if (!(notification is IEventNotification)) { return false; }
            DisposeNotification();
            Notification = notification;
            return true;
        }

        /// <summary>
        /// Start Register
        /// </summary>
        private void Alarm(AlarmTypeEnum type, IEventNotification notification) {
            switch (type) {
                case AlarmTypeEnum.Both:
                    notification.EventChange += NotificationEventHandler;
                    break;
                case AlarmTypeEnum.History:
                    notification.HistoryEventChange += NotificationEventHandler;
                    break;
                case AlarmTypeEnum.Realtime:
                    notification.RealtimeEventChange += NotificationEventHandler;
                    break;
                default:
                    return;
            }
            notification.BeginSubcription();
        }

        /// <summary>
        /// Notification Event Handler
        /// </summary>
        /// <param name="message"></param>
        private void NotificationEventHandler(XElement message) {
            XElement result = CreateMessage(message);
            //to avoid dead lock
            System.Threading.Thread.Sleep(10);
            Session.Send(result.ToString());
        }

        /// <summary>
        /// Create Message
        /// </summary>
        private XElement CreateMessage(XElement message) {
            XElement result = new XElement(Name);
            result.Add(message);
            return result;
        }

        #endregion Function

    }
}
