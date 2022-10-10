///Copyright(c) 2015,HIT All rights reserved.
///Summary：
///Author：Irlovan
///Date：2015-03-12
///Description：
///Modification：
      
      
using System;
using System.Timers;

namespace Irlovan
{
    public static class H
    {

        public static void SetInterval(int interval, Action<object, ElapsedEventArgs> action, out System.Timers.Timer timer) {
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(action);
            timer.Interval = interval;
            timer.Enabled = true;
        }

        public static void SetTimeout(int interval, Action<object, ElapsedEventArgs> action, out System.Timers.Timer timer) {
            timer = new System.Timers.Timer();
            timer.Enabled = false;
            timer.Stop();
            timer.Elapsed += new ElapsedEventHandler(action);
            timer.Interval = interval;
            timer.AutoReset = false;
            timer.Enabled = true;
            timer.Start();
        }

        /// <summary>
        /// Dispose a Timer
        /// </summary>
        public static void DisposeTimer(System.Timers.Timer timer) {
            if (timer != null) {
                timer.Stop();
                timer.Close();
                timer.Dispose();
                timer = null;
            }
        }

    }
}
