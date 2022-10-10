///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Driver data interface
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      

using Irlovan.Database;
using Irlovan.DataQuality;
using System;

namespace Irlovan.Driver
{
    public interface IDriverData : IDisposable
    {

        #region Property

        /// <summary>
        /// Industry Data Source
        /// </summary>
        IIndustryData Data { get; }

        /// <summary>
        /// Expression of the Driver Data
        /// </summary>
        Irlovan.Expression.Expression Expression { get; }

        /// <summary>
        /// Address to the client
        /// </summary>
        string Address { get; }

        /// <summary>
        /// Group Name In the driver
        /// </summary>
        string GroupName { get; }

        /// <summary>
        /// data set permittion to the client 
        /// </summary>
        bool Readonly { get; }

        /// <summary>s
        /// Quality of the driver data
        /// </summary>
        QualityEnum Quality { get; }

        #endregion Property

        #region Event

        event WriteDriverDataHandler WriteDriverData;

        #endregion Event

        #region Function

        /// <summary>
        /// read value for data
        /// </summary>
        /// <param name="data"></param>
        void ReadValue(object data, QualityEnum quality);

        /// <summary>
        /// write value to client
        /// </summary>
        /// <param name="data"></param>
        void WriteValue(object data);

        /// <summary>
        /// set the quality of the driver data
        /// </summary>
        /// <param name="data"></param>
        void SetQuality(QualityEnum quality);

        #endregion Function

    }

}
