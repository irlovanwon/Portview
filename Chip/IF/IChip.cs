///Copyright(c) 2015,HIT All rights reserved.
///Summary:Dll chip interface
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      

using System.Collections.Generic;
using System.Reflection;

namespace Irlovan.Chip
{
    public interface IChip
    {

        #region Property

        /// <summary>
        /// DllPath for chips
        /// </summary>
        string DllPath { get; }

        /// <summary>
        /// NameSpace for chips
        /// </summary>
        string NameSpace { get; }

        /// <summary>
        /// AssemblyList of chips
        /// </summary>
        Dictionary<string, Assembly> AssemblyList { get; }

        #endregion Property

        #region Function

        /// <summary>
        /// load handlers
        /// </summary>
        void LoadDll();

        /// <summary>
        /// init properties for chips
        /// </summary>
        void Init();

        #endregion Function

    }
}
