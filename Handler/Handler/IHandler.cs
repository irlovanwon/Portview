///Copyright(c) 2015,HIT All rights reserved.
///Summary:Handler for communication canal
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:      

using Irlovan.Canal;
using System;
using System.Xml.Linq;

namespace Irlovan.Handlers
{
    public interface IHandler : IDisposable
    {

        #region Property

        /// <summary>
        /// Local Interface to handler
        /// </summary>
        LocalInterface.LocalInterface LocalInterface { get; set; }

        /// <summary>
        /// session of the handler
        /// </summary>
        IServerSession Session { get; set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Handle Bytes
        /// </summary>
        /// <param name="session"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Handle(IServerSession session, byte[] data);

        /// <summary>
        /// Handler String
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Handle(IServerSession session, string message);

        /// <summary>
        /// Handler String
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Handle(IServerSession session, XElement message);

        #endregion Function

    }
}
