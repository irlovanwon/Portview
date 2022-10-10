///Copyright(c) 2015,HIT All rights reserved.
///Summary:Recorder interface
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using Irlovan.Database;
using Irlovan.DataQuality;
using System;
using System.Xml.Linq;

namespace Irlovan.Recorder
{
    public interface IRecorderData
    {

        #region Property

        /// <summary>
        /// ID of recorder data
        /// </summary>
        int ID { get; }

        /// <summary>
        /// value of recorder data
        /// </summary>
        object Value { get; }

        /// <summary>
        /// name of recorder data
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Description of recorder data
        /// </summary>
        string Description { get; }

        /// <summary>
        /// DataType of recorder data
        /// </summary>
        Type DataType { get; }

        /// <summary>
        /// IIndustryData
        /// </summary>
        IIndustryData Data { get; }

        /// <summary>
        /// Data Quality
        /// </summary>
        QualityEnum Quality { get; }

        /// <summary>
        /// Config file of recorder data
        /// </summary>
        XElement Config { get; }

        #endregion Property

    }
}
