///Copyright(c) 2015,HIT All rights reserved.
///Summary：
///Author：Irlovan
///Date：2015-03-09
///Description：
///Modification：


using Irlovan.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irlovan
{
    public class R_TRIG
    {

        #region Structure

        /// <summary>
        /// R_TRIG
        /// </summary>
        /// <param name="dataFrom"></param>
        /// <param name="dataTo"></param>
        public R_TRIG(IIndustryData<bool> dataFrom, IIndustryData<bool> dataTo) {
            _dataFrom = dataFrom;
            _dataTo = dataTo;
        }

        #endregion Structure

        #region Field

        private bool _ghost;

        //move data from
        private IIndustryData<bool> _dataFrom;

        //move data to
        private IIndustryData<bool> _dataTo;

        #endregion Field

        #region Property
        #endregion Property

        #region Delegate
        #endregion Delegate

        #region Event
        #endregion Event

        #region Function

        /// <summary>
        /// Scan the R_TRIG
        /// </summary>
        public void Scan() {
            _dataTo.ReadValue((_ghost && (!_dataFrom.Value)) ? true : false);
            _ghost = _dataFrom.Value;
        }

        #endregion Function

        #region InterClass
        #endregion InterClass

    }
}
