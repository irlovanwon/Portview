///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:RWBox for data read and write
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using System;
using System.Collections.Generic;
using System.Timers;

namespace Irlovan.Driver
{
    public class RWBox : IRWBox
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="groupName"></param>
        public RWBox(string groupName) {
            _groupName = groupName;
            Init();
        }

        #endregion Structure

        #region Field

        private Dictionary<string, IDriverData> _rwDataList;
        private object _rwBoxLock = new object();
        private object _readyBoxLock = new object();
        private Dictionary<string, object> _readyBox;
        private const int ReadBoxInterval = 200;
        private Timer _timer;
        private string _groupName;

        #endregion Field

        #region Event

        public event WriteDataHandler WriteData;

        public event WriteDataListHandler WriteDataList;

        #endregion Event

        #region Function

        /// <summary>
        /// Push driver data to RWBox 
        /// </summary>
        public void Push(IDriverData data) {
            lock (_rwBoxLock) {
                if (_rwDataList.ContainsKey(data.Data.FullName)) { return; }
                _rwDataList.Add(data.Data.FullName, data);
                data.WriteDriverData += WriteDriverDataHandler;
            }
        }

        /// <summary>
        /// Get Driver Data List Ready to Write 
        /// </summary>
        public Dictionary<string, object> GetReadyBox() {
            lock (_readyBoxLock) {
                Dictionary<string, object> result;
                result = CloneReadyData(_readyBox);
                return result;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() {
            DisposeRWData();
            DisposeTimer();
        }

        /// <summary>
        /// DisposeTimer
        /// </summary>
        private void DisposeTimer() {
            if (_timer != null) { Lib.Timer.Timer.DisposeTimer(_timer); }
        }

        /// <summary>
        /// Dispose for RWData
        /// </summary>
        private void DisposeRWData() {
            lock (_rwBoxLock) {
                foreach (var item in _rwDataList) {
                    item.Value.WriteDriverData -= WriteDriverDataHandler;
                }
            }
        }

        /// <summary>
        /// Handler for WriteDriverData Event of DriverData
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="name"></param>
        /// <param name="addr"></param>
        /// <param name="value"></param>
        /// <param name="timeStamp"></param>
        private void WriteDriverDataHandler(string groupName, string name, string addr, object value, DateTime timeStamp) {
            if (WriteData != null) { WriteData(groupName, name, addr, value, timeStamp); }
            if (WriteDataList != null) { AddReadyBox(name, value); }
        }

        /// <summary>
        ///  Handler for Write DriverDataList 
        /// </summary>
        private void WriteDataListHandler() {
            Irlovan.Lib.Timer.Timer.SetInterval((object o, ElapsedEventArgs e) => {
                if (WriteDataList != null) { WriteDataList(_groupName, SendReadyData(), DateTime.Now); }
            }, ref _timer, ReadBoxInterval);
        }

        /// <summary>
        /// GetReadyData
        /// </summary>
        private Dictionary<string, object> SendReadyData() {
            lock (_readyBoxLock) {
                Dictionary<string, object> result;
                result = CloneReadyData(_readyBox);
                _readyBox.Clear();
                return result;
            }
        }

        /// <summary>
        /// CloneReadyData
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private Dictionary<string, object> CloneReadyData(Dictionary<string, object> source) {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (var item in source) {
                result.Add(item.Key, item.Value);
            }
            return result;
        }

        /// <summary>
        /// Add Ready Box
        /// </summary>
        private void AddReadyBox(string name, object value) {
            lock (_readyBoxLock) {
                if (!_readyBox.ContainsKey(name)) {
                    _readyBox.Add(name, value);
                }
                else {
                    _readyBox[name] = value;
                }
            }
        }

        /// <summary>
        /// Init for RWBox
        /// </summary>
        private void Init() {
            _readyBox = new Dictionary<string, object>();
            _rwDataList = new Dictionary<string, IDriverData>();
        }

        #endregion Function

    }
}
