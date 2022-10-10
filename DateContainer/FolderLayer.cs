///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:
///Author:Irlovan
///Date:2015-08-10
///Description:
///Modification:

using System;
using System.Collections.Generic;
using System.IO;

namespace Irlovan.Structure
{
    public class FolderLayer : Layer, IFolderLayer
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="clockwork"></param>
        /// <param name="index"></param>
        public FolderLayer(string name, Clockwork clockwork, int index, ILayer parent = null)
            : base(name, clockwork, index, parent) { }

        #endregion Structure

        #region Field

        private object _lock = new object();

        #endregion Field

        #region Property

        /// <summary>
        /// Group of the folder layer
        /// </summary>
        public LayerGroup Group { get; private set; }

        /// <summary>
        /// Max Count of Children
        /// </summary>
        public int MaxChildCount { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Init
        /// </summary>
        public override void Init() {
            base.Init();
            MaxChildCount = Clockwork[Index].MaxChildCount;
            Group = new LayerGroup(this);
        }

        /// <summary>
        /// If the layer exists
        /// </summary>
        /// <returns></returns>
        public override bool Exist() {
            return Directory.Exists(Path);
        }

        /// <summary>
        /// Create Folder
        /// </summary>
        /// <returns></returns>
        public bool CreateFolder() {
            try {
                Directory.CreateDirectory(Path);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// Delete the layer
        /// </summary>
        /// <returns></returns>
        public override bool Delete() {
            try {
                Directory.Delete(Path, true);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// Append All Lines
        /// </summary>
        /// <returns></returns>
        public override bool AppendAllLines(DateTime timeStamp, IEnumerable<string> lines) {
            lock (_lock) {
                if ((!Exist()) && (!CreateFolder())) { return false; }
                return Group.AppendAllLines(timeStamp, lines);
            }
        }

        /// <summary>
        /// Append All Text
        /// </summary>
        /// <returns></returns>
        public override bool AppendAllText(DateTime timeStamp, string text) {
            lock (_lock) {
                if ((!Exist()) && (!CreateFolder())) { return false; }
                return Group.AppendAllText(timeStamp, text);
            }
        }

        /// <summary>
        /// Read All Lines
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public override IEnumerable<string> ReadAllLines(DateTime timeStamp) {
            lock (_lock) {
                if (!Exist()) { return null; }
                return Group.ReadAllLines(timeStamp);
            }
        }

        /// <summary>
        /// Read All Text
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public override string ReadAllText(DateTime timeStamp) {
            lock (_lock) {
                if (!Exist()) { return null; }
                return Group.ReadAllText(timeStamp);
            }
        }

        /// <summary>
        /// Refresh by HD files
        /// </summary>
        /// <returns></returns>
        public void Refresh() {
            Group.Refresh();
        }

        #endregion Function

    }
}
