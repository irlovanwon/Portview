///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Recorder data
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
    public class RecorderData : IRecorderData
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        public RecorderData(IIndustryData data, int id,XElement config) {
            ID = id;
            Data = data;
            Config = config;
        }

        #endregion Structure

        #region Property

        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// value of recorder data
        /// </summary>
        public object Value { get { return Data.Value; } }

        /// <summary>
        /// name of recorder data
        /// </summary>
        public string Name { get { return Data.FullName; } }

        /// <summary>
        /// Description of recorder data
        /// </summary>
        public string Description { get { return Data.Description; } }

        /// <summary>
        /// DataType of recorder data
        /// </summary>
        public Type DataType { get { return Data.DataType; } }

        /// <summary>
        /// Data
        /// </summary>
        public IIndustryData Data { get; private set; }

        /// <summary>
        /// Data Quality
        /// </summary>
        public QualityEnum Quality { get { return Data.Quality; } }

        /// <summary>
        /// Config file of recorder data
        /// </summary>
        public XElement Config { get; private set; }

        #endregion Property

    }
}
