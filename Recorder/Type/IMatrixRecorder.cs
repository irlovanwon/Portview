///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Matrix recorder interface
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:      

using Irlovan.Lib.Array;
using System;

namespace Irlovan.Recorder
{
    public interface IMatrixRecorder : IRecorder
    {

        #region Function

        /// <summary>
        /// Read data line from database by timeStamp 
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        string[] Read(DateTime timeStamp);

        /// <summary>
        /// Read data
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="amount"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        MatrixArray<string> Read(DateTime startTime, DateTime endTime, object amount = null, string[] columns = null);

        #endregion Function

    }

}
