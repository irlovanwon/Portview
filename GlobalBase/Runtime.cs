///Copyright(c) 2013,HIT All rights reserved.
///Summary:
///Author:Irlovan
///Date:2013-11-07
///Description:
///Modification:2015-11-12

using System;
using System.Diagnostics;

namespace Irlovan.Global
{
    public static class Runtime
    {

        /// <summary>
        /// close the program
        /// </summary>
        public static void ServerShutDown(string message = null) {
            if (message != null) {
                Action<string> notice = new Action<string>(NoticeWindow);
                Info.Dispatcher.Invoke(notice, message);
            }
            else {
                Process.GetCurrentProcess().Kill();
            }
        }

        /// <summary>
        /// OPEN NOTICE WINDOW
        /// </summary>
        private static void NoticeWindow(string message) {
            Irlovan.Control.NoticeWindow.Notice(message);
            Process.GetCurrentProcess().Kill();
        }

    }
}
