///Copyright(c) 2015,HIT All rights reserved.
///Summary:Session interface
///Author:Irlovan
///Date:2015-11-10
///Description:
///Modification:      

using System;

namespace Irlovan.Canal
{
    public interface IServerSession : IDisposable
    {

        #region Property

        /// <summary>
        /// Session ID
        /// </summary>
        /// <param name="messsage"></param>
        string ID { get; }

        /// <summary>
        /// Session Path
        /// </summary>
        string Path { get; }

        #endregion Property

        #region Function

        /// <summary>
        /// Send message
        /// </summary>
        /// <param name="messsage"></param>
        void Send(string messsage);

        #endregion Function

    }
}
