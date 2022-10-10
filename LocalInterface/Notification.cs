///Copyright(c) 2015,HIT All rights reserved.
///Summary:Notification interface
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using Irlovan.Database;
using Irlovan.Lib.Symbol;
using Irlovan.Lib.XML;
using Irlovan.Log;
using Irlovan.Notification;
using Irlovan.Register;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace Irlovan.LocalInterface
{
    public class Notification
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="source"></param>
        /// <param name="notificationAssemblyList"></param>
        public Notification(Catalog source, IRegister register, Dictionary<string, Assembly> notificationAssemblyList) {
            _register = register;
            _source = source;
            _notificationAssemblyList = notificationAssemblyList;
            NotificationList = new Dictionary<string, INotification>();
            RunNotification();
        }

        #endregion Structure

        #region Field

        private IRegister _register;
        private Catalog _source;
        private Dictionary<string, Assembly> _notificationAssemblyList;

        private const string NotificationRootName = "Notification";
        private const string NotificationFilePath = "\\Core\\Notification";

        /// <summary>
        /// Notification List
        /// </summary>
        public Dictionary<string, INotification> NotificationList { get; private set; }

        #endregion Field

        #region Delegate

        private delegate void RunNotificationHanlder();

        #endregion Delegate

        #region Function

        /// <summary>
        /// Run Notification
        /// </summary>
        internal void Run() {
            foreach (var item in NotificationList) {
                try {
                    RunNotificationHanlder runNotification = new RunNotificationHanlder(item.Value.Run);
                    runNotification.BeginInvoke(null, null);
                }
                catch (Exception e) {
                    Global.Info.LogRecorder.Log(LogLevelEnum.Error, e.ToString());
                    continue;
                }
            }
        }

        /// <summary>
        /// start to run Notification module
        /// </summary>
        private void RunNotification() {
            XDocument notificationDoc;
            string path = Global.Info.ProjectPath + NotificationFilePath;
            if (!System.IO.File.Exists(path)) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.NotificationNoResource); return; }
            try { notificationDoc = XDocument.Load(path); }
            catch (Exception e) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.NotificationNoResource + Symbol.NewLine_Symbol + e.ToString()); return; }
            if (notificationDoc.Document.Root.Name != NotificationRootName) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.NotificationNoResource); return; }
            foreach (var dll in notificationDoc.Root.Elements()) { InitDll(dll); }
        }

        /// <summary>
        /// Init Notification dll
        /// </summary>
        private void InitDll(XElement dll) {
            string dllName = dll.Name.ToString();
            string notificationDllFullName = Chip.ChipLoader.NameSpaceValue_Notification + dllName;
            if ((!_notificationAssemblyList.ContainsKey(dllName)) || (_notificationAssemblyList.Count == 0)) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.NotificationNoResource + notificationDllFullName); return; }
            CreateNotification(dll, dllName);
        }

        /// <summary>
        /// create notification instance
        /// </summary>
        /// <param name="config"></param>
        private void CreateNotification(XElement config, string dllName) {
            string notificationID;
            if (!XML.InitStringAttr<string>(config, Irlovan.Notification.Notification.IDAttr, out notificationID)) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.NoID); return; }
            string notificationDllFullName = Chip.ChipLoader.NameSpaceValue_Notification + dllName;
            string notificationFullName = notificationDllFullName + "." + config.Name.ToString();
            Type notificationType = _notificationAssemblyList[dllName].GetType(notificationFullName);
            if (notificationType == null) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.NotificationNoResource); return; }
            INotification notification = (INotification)Activator.CreateInstance(notificationType, new Object[] { _source, _register, config });
            notification.Init();
            if (!notification.InitState) { return; }
            if (!NotificationList.ContainsKey(notificationID)) { NotificationList.Add(notificationID, notification); }
            else { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.NotificationIDExist); }
        }

        #endregion Function

    }
}
