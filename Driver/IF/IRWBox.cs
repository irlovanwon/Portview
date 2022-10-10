///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:RWBox interface
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using System;
using System.Collections.Generic;

namespace Irlovan.Driver
{
    public interface IRWBox : IDisposable
    {

        #region Event

        /// <summary>
        /// Write one data value to device
        /// </summary>
        event WriteDataHandler WriteData;

        /// <summary>
        /// Write multiple data values to device
        /// </summary>
        event WriteDataListHandler WriteDataList;

        #endregion Event

        #region Function

        /// <summary>
        /// Get Driver Data List Ready to Write 
        /// </summary>
        Dictionary<string, object> GetReadyBox();

        /// <summary>
        /// Push driver data to RWBox 
        /// </summary>
        void Push(IDriverData data);

        #endregion Function

    }
}
