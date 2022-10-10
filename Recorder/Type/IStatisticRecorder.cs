///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Statistic recorder interface
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using Irlovan.Lib.Array;
using System;

namespace Irlovan.Recorder
{
    public interface IStatisticRecorder : IRecorder
    {

        #region Function

        /// <summary>
        /// Read data from database
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        MatrixArray<string> Read(DateTime timeStamp);

        #endregion Function

    }
}
