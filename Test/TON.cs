///Copyright(c) 2015,HIT All rights reserved.
///Summary：
///Author：Irlovan
///Date：2015-03-12
///Description：
///Modification：
      
      
using Irlovan.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Irlovan
{
    public class TON
    {

        #region Structure

        public TON(IIndustryData<bool> dataFrom, IIndustryData<bool> dataTo, int timeout) {
            _dataFrom = dataFrom;
            _dataTo = dataTo;
            _timeout = timeout;
        }

        public TON(IIndustryData<bool> dataTo, int timeout) {
            _dataTo = dataTo;
            _timeout = timeout;
        }

        #endregion Structure

        #region Field

        private IIndustryData<bool> _dataFrom;
        private IIndustryData<bool> _dataTo;
        private int _timeout;
        private bool _dataStack;
        private System.Timers.Timer _timer;

        #endregion Field

        #region Property
        #endregion Property

        #region Delegate
        #endregion Delegate

        #region Event
        #endregion Event

        #region Function

        /// <summary>
        /// Scan the TON
        /// </summary>
        public void Scan() {
            if (_dataFrom==null){return;}
            var result = _dataFrom.Value;
            if (_dataStack==result) {return;}
            if (result) {
                if (_timer!=null) {return;}
                H.SetTimeout(_timeout, (object o, ElapsedEventArgs e) => {
                    _dataTo.ReadValue(true);
                    H.DisposeTimer(_timer);
                }, out _timer);
            } else {
                H.DisposeTimer(_timer);
                _dataTo.ReadValue(false);
            }
            _dataStack = result;
        }

        /// <summary>
        /// Scan the TON
        /// </summary>
        public void Scan(bool value) {
            if (_dataFrom != null) { return; }
            if (_dataStack == value) { return; }
            if (value) {
                if (_timer != null) { return; }
                H.SetTimeout(_timeout, (object o, ElapsedEventArgs e) => {
                    _dataTo.ReadValue(true);
                    H.DisposeTimer(_timer);
                }, out _timer);
            } else {
                H.DisposeTimer(_timer);
                _dataTo.ReadValue(false);
            }
            _dataStack = value;
        }

        #endregion Function

        #region InterClass
        #endregion InterClass      
      
    }
}
