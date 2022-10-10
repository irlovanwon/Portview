///Copyright(c) 2015,HIT All rights reserved.
///Summary:Timer extention
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using System;
using System.Timers;

namespace Irlovan.Lib.Timer
{
    public static class Timer
    {

        /// <summary>
        /// Set Interval
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="action"></param>
        /// <param name="timer"></param>
        public static void SetInterval(Action<object, ElapsedEventArgs> action, ref System.Timers.Timer timer, int interval = 1000) {
            if (timer == null) { timer = new System.Timers.Timer(interval); }
            timer.Elapsed += new ElapsedEventHandler(action);
            timer.Enabled = true;
        }

        /// <summary>
        /// Set timeout
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="action"></param>
        /// <param name="timer"></param>
        public static void SetTimeout(Action<object, ElapsedEventArgs> action, ref System.Timers.Timer timer, int interval = 0) {
            if (timer == null) { timer = new System.Timers.Timer(interval); }
            timer.Enabled = false;
            timer.Stop();
            timer.Elapsed += new ElapsedEventHandler(action);
            timer.AutoReset = false;
            if (interval > 0) { timer.Interval = interval; }
            timer.Enabled = true;
            timer.Start();
        }


        /// <summary>
        /// Set timeout
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="action"></param>
        /// <param name="timer"></param>
        public static void SetTimeout(Action<object, ElapsedEventArgs> action, int interval = 0) {
            System.Timers.Timer timer = new System.Timers.Timer(interval);
            timer.Enabled = false;
            timer.Stop();
            timer.Elapsed += new ElapsedEventHandler((object o, ElapsedEventArgs e) => { action(o, e); timer.Dispose(); });
            timer.AutoReset = false;
            if (interval > 0) { timer.Interval = interval; }
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
