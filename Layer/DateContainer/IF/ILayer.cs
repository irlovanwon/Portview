///Copyright(c) 2015,HIT All rights reserved.
///Summary:ILayer
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using System;
using System.Collections.Generic;

namespace Irlovan.Structure
{
    public interface ILayer : IDisposable
    {

        #region Property

        /// <summary>
        /// The path of the layer
        /// </summary>
        string Path { get; }

        /// <summary>
        /// The name of the layer
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Parent folder of the layer
        /// </summary>
        ILayer Parent { get; }

        /// <summary>
        /// Time stamp of the layer
        /// </summary>
        DateTime TimeStamp { get; }

        /// <summary>
        /// DateFormat of the layer
        /// </summary>
        string DateFormat { get; }

        /// <summary>
        /// InitState
        /// </summary>
        bool InitState { get; }

        /// <summary>
        /// Clockwork of the layer
        /// </summary>
        Clockwork Clockwork { get; }

        /// <summary>
        /// Layer Index
        /// </summary>
        int Index { get; }

        #endregion Property

        #region Function

        /// <summary>
        /// If the layer exists
        /// </summary>
        /// <returns></returns>
        bool Exist();

        /// <summary>
        /// Delete the layer
        /// </summary>
        /// <returns></returns>
        bool Delete();

        /// <summary>
        /// Append All Text
        /// </summary>
        /// <returns></returns>
        bool AppendAllLines(DateTime timeStamp, IEnumerable<string> lines);

        /// <summary>
        /// Append All Text
        /// </summary>
        /// <returns></returns>
        bool AppendAllText(DateTime timeStamp, string text);

        /// <summary>
        /// Read All Lines
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        IEnumerable<string> ReadAllLines(DateTime timeStamp);

        /// <summary>
        /// Read All Text
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        string ReadAllText(DateTime timeStamp);

        #endregion Function

    }
}
