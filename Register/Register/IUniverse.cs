///Copyright(c) 2015,HIT All rights reserved.
///Summary:Universe interfaces
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Irlovan.Register
{
    public interface IUniverse : IDisposable
    {

        #region Property

        /// <summary>
        /// Map to locate units in universe
        /// </summary>
        Dictionary<string, IUnit> Map { get; }

        /// <summary>
        /// Max count of units in a universe
        /// </summary>
        int MaxCount { get; }

        /// <summary>
        /// Owner of the universe
        /// </summary>
        IUnit Owner { get; }

        #endregion Property

        #region Function

        /// <summary>
        /// Push Unit to universe
        /// </summary>
        bool Push(IUnit unit);

        /// <summary>
        /// remove Unit from universe by its id
        /// </summary>
        bool Remove(string id);

        /// <summary>
        /// Change ID from idFrom to idTo
        /// </summary>
        bool ChangeID(string idFrom, string idTo);

        /// <summary>
        /// if the universe contain a unit
        /// </summary>
        bool Contain(string id);

        /// <summary>
        /// get unit by id
        /// </summary>
        IUnit GetUnit(string id);

        /// <summary>
        /// update address
        /// </summary>
        void UpdateAddress(string parentID);

        /// <summary>
        /// update root
        /// </summary>
        void UpdateRoot(IRegister root);

        /// <summary>
        /// Update index
        /// </summary>
        void UpdateIndex();

        /// <summary>
        /// Deserialize
        /// </summary>
        bool Deserialize(SerializationInfo info, StreamingContext context, IAddress address, IRegister root);

        /// <summary>
        /// serialize
        /// </summary>
        void Serialize(SerializationInfo info, StreamingContext context);

        #endregion Function

    }
}
