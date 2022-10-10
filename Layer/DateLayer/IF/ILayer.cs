///Copyright(c) 2015,HIT All rights reserved.
///Summary:ILayer
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using System;
using System.Collections.Generic;
using System.IO;

namespace HIT.Layer
{
    public interface ILayer
    {

        #region Property

        /// <summary>
        /// The path of the layer
        /// </summary>
        DirectoryInfo Folder { get; }

        /// <summary>
        /// Folder Name
        /// </summary>
        string FolderName { get; }

        /// <summary>
        /// Parent folder of the layer
        /// </summary>
        ILayer Parent { get; }

        /// <summary>
        /// Info
        /// </summary>
        IInfo Info { get; }

        /// <summary>
        /// IsValid
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// TimeStamp
        /// </summary>
        DateTime TimeStamp { get; }

        /// <summary>
        /// DateFormat of Children
        /// </summary>
        SortedDictionary<DateTime, ILayer> Children { get; }

        #endregion Property

        #region Function

        /// <summary>
        /// Refresh
        /// </summary>
        void Refresh();

        /// <summary>
        /// delete layer
        /// </summary>
        /// <returns></returns>
        bool Delete();

        /// <summary>
        /// CheckSize
        /// </summary>
        void CheckSize();

        /// <summary>
        /// Navigate
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        DirectoryInfo Navigate(DateTime timeStamp);

        /// <summary>
        /// Append
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        void Append(DateTime timeStamp);

        #endregion Function

    }
}
