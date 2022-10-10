///Copyright(c) 2015,HIT All rights reserved.
///Summary:IEther interface
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:      

namespace Irlovan.Register
{
    public interface IEther<T> : IUnit
    {

        #region Property

        /// <summary>
        /// Data info
        /// </summary>
        T Data { get; }

        #endregion Property

    }
}
