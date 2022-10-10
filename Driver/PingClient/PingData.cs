///Copyright(c) 2015,HIT All rights reserved.
///Summary:Ping data
///Author:Irlovan
///Date:2015-11-23
///Description:
///Modification:      

using System;
using System.Net.NetworkInformation;
using System.Timers;

namespace Irlovan.Driver
{
    internal class PingData : IDisposable
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isSoftMode"></param>
        /// <param name="timeout"></param>
        /// <param name="updateRate"></param>
        internal PingData(DriverData data, bool isSoftMode, int timeout, int updateRate) {
            IsSoftMode = isSoftMode;
            Data = data;
            Timeout = timeout;
            _updateRate = updateRate;
        }

        #endregion Structure

        #region Field

        internal Timer _timer;
        private const int ErrorProtectionTime = 1000;
        private const short DeadLockProtectionInterval = 50;
        private int _updateRate;

        #endregion Field

        #region Delegate

        private delegate void PingHandler();

        #endregion Delegate

        #region Property

        /// <summary>
        /// dirver data
        /// </summary>
        internal DriverData Data { get; private set; }

        /// <summary>
        /// in soft mode 
        /// </summary>
        internal bool IsSoftMode { get; private set; }

        /// <summary>
        /// Timeout for ping
        /// </summary>
        internal int Timeout { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// dispose for timer
        /// </summary>
        public void Dispose() {
            DisposeTimer();
        }

        /// <summary>
        /// Ping IP
        /// </summary>
        /// <param name="data"></param>
        internal void Ping() {
            using (Ping ping = new Ping()) {
                PingReply reply = SendSync(ping);
                bool result = false;
                SoftMode(reply, ping, ref result);
                SetValue(result);
            }
        }

        /// <summary>
        /// Ping target with result value
        /// </summary>
        private void PingResult(bool result) {
            DisposeTimer();
            Data.ReadValue(result);
        }

        /// <summary>
        /// Ping target failed with timeout
        /// </summary>
        private void PingTimeout() {
            if (_timer != null) { return; }
            InitTimer();
            Lib.Timer.Timer.SetTimeout((object o, ElapsedEventArgs e) => {
                Data.ReadValue(false);
                DisposeTimer();
            }, ref _timer, _updateRate);
        }

        /// <summary>
        /// Set Ping Result Value
        /// </summary>
        private void SetValue(bool result) {
            if ((result) || (_updateRate <= 0)) { PingResult(result); }
            else if (!result) { PingTimeout(); }
            //To avoid dead lock
            System.Threading.Thread.Sleep(DeadLockProtectionInterval);
            PingHandler ping = new PingHandler(Ping);
            ping.BeginInvoke(null, null);
        }

        /// <summary>
        /// Soft Mode for Ping command
        /// </summary>
        /// <param name="result"></param>
        /// <param name="data"></param>
        /// <param name="ping"></param>
        private void SoftMode(PingReply reply, Ping ping, ref bool result) {
            if (reply == null) { return; }
            result = (reply.Status == IPStatus.Success);
            if ((result == true) || (!IsSoftMode)) { return; }
            PingReply replySoftMode = SendSync(ping);
            result = (replySoftMode == null) ? false : (replySoftMode.Status == IPStatus.Success);
        }

        /// <summary>
        /// Send to IP Server
        /// </summary>
        /// <param name="ping"></param>
        /// <param name="data"></param>
        private PingReply SendSync(Ping ping) {
            try {
                return ping.Send(Data.Address, Timeout);
            }
            catch {
                System.Threading.Thread.Sleep(ErrorProtectionTime);
                return null;
            }
        }

        /// <summary>
        /// InitTimer
        /// </summary>
        private void InitTimer() {
            DisposeTimer();
            _timer = new Timer();
        }

        /// <summary>
        /// DisposeTimer
        /// </summary>
        private void DisposeTimer() {
            Lib.Timer.Timer.DisposeTimer(_timer);
            _timer = null;
        }

        #endregion Function

    }
}
