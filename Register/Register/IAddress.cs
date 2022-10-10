///Copyright(c) 2015,HIT All rights reserved.
///Summary:Register address interface
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:      

using System;

namespace Irlovan.Register
{
    public interface IAddress : IDisposable
    {

        #region Property

        /// <summary>
        /// Full Name of the address
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// First Name of the address
        /// </summary>
        string FirstName { get; }

        /// <summary>
        /// Last Name of the address
        /// </summary>
        string LastName { get; }

        /// <summary>
        /// Tags of the address
        /// </summary>
        string[] Tags { get; }

        /// <summary>
        /// Parse from string
        /// </summary>
        /// <param name="address"></param>
        bool Parse(string address);

        /// <summary>
        /// Parse from parent
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentAddress"></param>
        bool Parse(string id, IAddress parentAddress = null);

        /// <summary>
        /// Parse tags
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentAddress"></param>
        bool Parse(string[] tags);

        /// <summary>
        /// Get Child Address
        /// </summary>
        /// <returns></returns>
        IAddress GetChild();

        /// <summary>
        /// GetAddressFromRange
        /// </summary>
        /// <returns></returns>
        IAddress GetRange(int index, int length);


        #endregion Property

    }
}
