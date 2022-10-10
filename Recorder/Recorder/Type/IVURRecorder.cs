///Copyright(c) 2015,HIT All rights reserved.
///Summary:VUR recorder interface
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using Irlovan.Lib.Array;
using System;

namespace Irlovan.Recorder
{
    public interface IVURRecorder
    {

        #region Function

        /// <summary>
        /// Read data from database
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        MatrixArray<string> ReadCache(DateTime timeStamp);

        /// <summary>
        /// Timestamp title of the vur recorder
        /// </summary>
        string DateTitle { get; }

        /// <summary>
        /// DateFormat of the vur recorder
        /// </summary>
        string DateFormat { get; }

        #endregion Function

    }
}
