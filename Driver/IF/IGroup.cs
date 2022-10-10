///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Driver group interface
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using Irlovan.DataQuality;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Driver
{
    public interface IGroup : IDisposable
    {

        #region Property

        /// <summary>
        /// Driver Data List
        /// </summary>
        Dictionary<string, IDriverData> DataBox { get; }

        /// <summary>
        /// Read and Write Data List
        /// </summary>
        IRWBox RWDataBox { get; }

        /// <summary>
        /// GroupName
        /// </summary>
        string GroupName { get; }

        /// <summary>
        /// Count of Chidren
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Update rate of the group
        /// </summary>
        int UpdateRate { get; set; }

        /// <summary>
        /// The Group is in Reading Mode/Writing Mode
        /// </summary>
        RWModeEnum RWMode { get; }

        /// <summary>
        /// Config for the IGroup
        /// </summary>
        XElement Config { get; }

        #endregion Property

        #region Function

        /// <summary>
        /// Init Read/Write Box
        /// </summary>
        void InitRWBox();

        /// <summary>
        /// push data to driver data box
        /// </summary>
        /// <param name="data"></param>
        void Push(IDriverData data);

        /// <summary>
        ///remove data from driver data box by name
        /// </summary>
        /// <param name="name"></param>
        void Remove(string name);

        /// <summary>
        /// If contain the named item
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool Contain(string name);

        /// <summary>
        /// get driver data
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IDriverData GetData(string name);

        /// <summary>
        /// Apply Quality to all data
        /// </summary>
        void ApplyQuality(QualityEnum quality);

        #endregion Function

    }

    /// <summary>
    /// Enum indicate if the group is in reading mode or in writing mode
    /// </summary>
    public enum RWModeEnum { Input, Output }
}
