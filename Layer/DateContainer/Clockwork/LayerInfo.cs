///Copyright(c) 2015,HIT All rights reserved.
///Summary:LayerInfo
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using System;

namespace Irlovan.Structure
{
    public struct LayerInfo
    {

        #region

        /// <summary>
        /// Construction
        /// </summary>
        public LayerInfo(int maxChildCount, string dateFormat)
            : this() {
            MaxChildCount = maxChildCount;
            if (MaxChildCount <= 0) { MaxChildCount = Int32.MaxValue; }
            DateFormat = dateFormat;
        }

        #endregion

        #region Property

        /// <summary>
        /// Max Child Count
        /// </summary>
        public int MaxChildCount { get; private set; }

        /// <summary>
        /// Date Format
        /// </summary>
        public string DateFormat { get; private set; }

        #endregion Property

    }
}
