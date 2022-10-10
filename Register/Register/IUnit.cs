///Copyright(c) 2015,HIT All rights reserved.
///Summary:Unit interface
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:      

using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Irlovan.Register
{
    public interface IUnit : IDisposable, ISerializable
    {

        #region Property

        /// <summary>
        /// id of a unit
        /// </summary>
        String ID { get; }

        /// <summary>
        /// Parent of a unit
        /// </summary>
        IUnit Parent { get; set; }

        /// <summary>
        /// Address of a unit
        /// </summary>
        IAddress Address { get; }

        /// <summary>
        /// Dimention Index of the Singularity
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Shows if the Unit Init Successfully
        /// </summary>
        bool InitState { get; }

        /// <summary>
        /// Root of the Unit
        /// </summary>
        IRegister Root { get; }

        /// <summary>
        /// Config file info
        /// </summary>
        XElement Config { get; }

        /// <summary>
        /// Serialization Info
        /// </summary>
        SerializationInfo SInfo { get; }

        /// <summary>
        /// Serialization StreamingContext
        /// </summary>
        StreamingContext SContext { get; }


        #endregion Property

        #region Function

        /// <summary>
        /// Init for the Unit
        /// </summary>
        /// <returns></returns>
        //bool Init();

        /// <summary>
        /// Wite info to Unit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool Write<T>(IAddress address, T data);

        /// <summary>
        /// Read info From Unit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        bool Read<T>(IAddress address, out T result);

        /// <summary>
        /// Deserialize
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <param name="address"></param>
        /// <param name="root"></param>
        /// <param name="parent"></param>
        bool Deserialize(IAddress address, IRegister root, IUnit parent = null);

        /// <summary>
        /// Init info from xml
        /// </summary>
        /// <param name="element"></param>
        void InitXML();

        /// <summary>
        /// Update ID
        /// </summary>
        bool UpdateID(string newID);

        /// <summary>
        /// Update Index
        /// </summary>
        void UpdateIndex();

        /// <summary>
        /// Update Address
        /// </summary>
        void UpdateAddress();

        /// <summary>
        /// Update Root
        /// </summary>
        void UpdateRoot(IRegister Register);

        #endregion Function

    }
}
