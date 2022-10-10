///Copyright(c) 2015,HIT All rights reserved.
///Summary:Message interface
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      
      
using System;
using System.Xml.Linq;

namespace Irlovan.Message
{
    public interface IMessage : IDisposable
    {

        #region Function

        /// <summary>
        /// Write data message to xml
        /// </summary>
        /// <returns></returns>
        XElement ToXML(FormatEnum format = FormatEnum.All);

        /// <summary>
        /// Write data message to string
        /// </summary>
        /// <returns></returns>
        string ToString(FormatEnum format = FormatEnum.All);

        /// <summary>
        /// Init State
        /// </summary>
        bool InitState { get; }

        #endregion Function

    }

    /// <summary>
    /// StringFormatEnum For Data Message
    /// </summary>
    public enum FormatEnum { Basic, Typic, All }
}
