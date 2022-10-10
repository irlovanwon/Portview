///Copyright(c) 2013,HIT All rights reserved.
///Summary:IRealtimeData
///Author:Irlovan
///Date:2013-04-11
///Description:
///Modification:2015-01-29

using Irlovan.DataQuality;
using Irlovan.Message;
using System;

namespace Irlovan.Database
{

    public interface IIndustryData<T> : IIndustryData
    {

        #region Property

        /// <summary>
        /// Value of a very type
        /// </summary>
        new T Value { get; }

        #endregion Property

        #region Function

        /// <summary>
        /// Set value of data to device
        /// </summary>
        /// <param name="value"></param>
        void WriteValue(T value);

        /// <summary>
        /// Set value of data
        /// </summary>
        /// <param name="value"></param>
        bool ReadValue(T value, QualityEnum quality = QualityEnum.Good);

        #endregion Function

    }

    public interface IIndustryData : IDatabase
    {

        #region Property

        /// <summary>
        /// Value of a very type
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Type of a data
        /// </summary>
        Type DataType { get; set; }

        /// <summary>
        /// TimeStamp when new data value is set
        /// </summary>
        DateTime TimeStamp { get; }

        /// <summary>
        /// shows the condition of a data
        /// </summary>
        QualityEnum Quality { get; set; }

        /// <summary>
        /// history data stack count
        /// </summary>
        int QueueCount { get; set; }

        /// <summary>
        /// Store message when data change
        /// </summary>
        DataMessageBox MessageBox { get; }

        #endregion Property

        #region Event

        /// <summary>
        /// Triggered when data value if Changed
        /// </summary>
        event DataChangeHandler DataChange;

        /// <summary>
        /// Triggered when value(s) of data(s) is set to device
        /// </summary>
        event WriteDataHandler WriteData;

        #endregion Event

        #region Function

        /// <summary>
        /// Set value of data to device
        /// </summary>
        /// <param name="value"></param>
        void WriteValue(object value);

        /// <summary>
        /// Set value of data
        /// </summary>
        /// <param name="value"></param>
        bool ReadValue(object value, QualityEnum quality = QualityEnum.Good);

        #endregion Function

    }

}
