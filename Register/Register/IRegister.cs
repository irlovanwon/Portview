///Copyright(c) 2015,HIT All rights reserved.
///Summary:Register interface
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using System;

namespace Irlovan.Register
{
    public interface IRegister : IDisposable
    {

        #region Property

        /// <summary>
        /// name of a register
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Hardware path
        /// </summary>
        String HDPath { get; }

        /// <summary>
        /// Random Access Memory
        /// </summary>
        ISingularity RAM { get; set; }

        /// <summary>
        /// Init State shows if Register inits properly
        /// </summary>
        bool InitState { get; }

        /// <summary>
        /// Interval For Mode Selection :-1 meams datachange mode >0 means interval mode =0 meams not record to HD
        /// </summary>
        int Interval { get; }

        /// <summary>
        /// Max Count of the Group dimension
        /// </summary>
        int Depth { get; }

        #endregion Property

        #region Function

        /// <summary>
        /// Read data from Register
        /// </summary>
        /// <returns></returns>
        bool AsyncRead<T>(string address, out T value);

        /// <summary>
        /// Write data to Register
        /// </summary>
        /// <returns></returns>
        bool AsyncWrite<T>(string address, T data);

        /// <summary>
        /// Remove data from Register
        /// </summary>
        /// <returns></returns>
        bool AsyncRemove(IAddress address);

        /// <summary>
        /// Record register to hardware
        /// </summary>
        /// <returns></returns>
        bool RecordtoHD();

        /// <summary>
        /// Init register from hardware
        /// </summary>
        /// <returns></returns>
        bool InitFromHD();

        /// <summary>
        /// Init register
        /// </summary>
        /// <returns></returns>
        bool Init();

        /// <summary>
        /// Select mode for record hd
        /// </summary>
        void Mode();

        #endregion Function

    }
}
