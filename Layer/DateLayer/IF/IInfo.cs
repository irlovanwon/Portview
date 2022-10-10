///Copyright(c) 2015,HIT All rights reserved.
///Summary:ILayer
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HIT.Layer
{
    public interface IInfo
    {

        #region Property

        /// <summary>
        /// DateFormat
        /// </summary>
        string DateFormat { get; }

        /// <summary>
        /// MaxChildCount
        /// </summary>
        int MaxCount { get; }

        /// <summary>
        /// Next Info
        /// </summary>
        IInfo Next { get; }

        /// <summary>
        /// IsValid
        /// </summary>
        bool IsValid { get; }

        #endregion Property

        #region Function

        /// <summary>
        /// Read from xml
        /// </summary>
        /// <param name="config"></param>
        bool ReadXML(XElement config);

        #endregion Function

    }
}
