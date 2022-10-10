///Copyright(c) 2015,HIT All rights reserved.
///Summary:IFolderLayer
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

namespace Irlovan.Structure
{
    public interface IFolderLayer : ILayer
    {

        #region Property

        /// <summary>
        /// Group of the folder layer
        /// </summary>
        LayerGroup Group { get; }

        /// <summary>
        /// Max Count of Children
        /// </summary>
        int MaxChildCount { get; }

        #endregion Property

        #region Function

        /// <summary>
        /// Create Folder
        /// </summary>
        /// <returns></returns>
        bool CreateFolder();

        /// <summary>
        /// Refresh by HD files
        /// </summary>
        /// <returns></returns>
        void Refresh();

        #endregion Function

    }
}
