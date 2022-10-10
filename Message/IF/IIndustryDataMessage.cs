///Copyright(c) 2015,HIT All rights reserved.
///Summary:IIndustryDataMessage
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using Irlovan.DataQuality;
using System;

namespace Irlovan.Message
{
    public interface IIndustryDataMessage : IDataMessage
    {

        /// <summary>
        /// Trigger when data change
        /// </summary>
        DateTime TimeStamp { get; }

        /// <summary>
        /// value for data
        /// </summary>
        string Value { get; }

        /// <summary>
        /// quality for data
        /// </summary>
        QualityEnum Quality { get; }

        /// <summary>
        /// Equals Comparison
        /// </summary>
        bool Equals(IIndustryDataMessage message);

    }

}
