///Copyright(c) 2015,HIT All rights reserved.
///Summary:
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Irlovan.Structure
{
    public class LayerGroup
    {

        #region Structure

        /// <summary>
        /// Layer Group Construction
        /// </summary>
        public LayerGroup(IFolderLayer owner) {
            _owner = owner;
            Init();
        }

        #endregion Structure

        #region Field

        private bool _isFileGroup = false;
        private string _dateFormat;
        private SortedDictionary<DateTime, ILayer> _childLayers;
        private IFolderLayer _owner;
        private int _maxChildCount;

        private const char FileFilterChar = '*';

        #endregion Field

        #region Function

        /// <summary>
        /// Append All Text
        /// </summary>
        /// <returns></returns>
        public bool AppendAllLines(DateTime timeStamp, IEnumerable<string> lines) {
            string timeStampStr = timeStamp.ToString(_dateFormat);
            ILayer childLayer = GetChildLayer(timeStamp, timeStampStr);
            if (!childLayer.AppendAllLines(timeStamp, lines)) { return false; }
            CheckSize();
            return true;
        }

        /// <summary>
        /// Append All Text
        /// </summary>
        /// <returns></returns>
        public bool AppendAllText(DateTime timeStamp, string text) {
            string timeStampStr = timeStamp.ToString(_dateFormat);
            ILayer childLayer = GetChildLayer(timeStamp, timeStampStr);
            if (!childLayer.AppendAllText(timeStamp, text)) { return false; }
            CheckSize();
            return true;
        }

        /// <summary>
        /// Read All Lines
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public IEnumerable<string> ReadAllLines(DateTime timeStamp) {
            ILayer childLayer = NavigateLayer(timeStamp);
            if (childLayer == null) { return null; }
            return childLayer.ReadAllLines(timeStamp);
        }

        /// <summary>
        /// Read All Text
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public string ReadAllText(DateTime timeStamp) {
            ILayer childLayer = NavigateLayer(timeStamp);
            if (childLayer == null) { return null; }
            return childLayer.ReadAllText(timeStamp);
        }

        /// <summary>
        /// Navigate Layer by timestamp
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        private ILayer NavigateLayer(DateTime timeStamp) {
            ILayer layer = FindLayer(timeStamp);
            if (layer != null) { return layer; }
            if (!_isFileGroup) { return null; }
            bool findStart = false;
            ILayer result = null;
            foreach (var item in _childLayers) {
                DateTime date = item.Key;
                //if (!DateTime.TryParseExact(, _dateFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out date)) { continue; }
                if (timeStamp.CompareTo(date) > 0) { result = item.Value; findStart = true; }
                if ((timeStamp.CompareTo(date) < 0) && findStart) { return result; }
            }
            return null;
        }

        /// <summary>
        /// FindLayer
        /// </summary>
        /// <returns></returns>
        private ILayer FindLayer(DateTime timeStamp) {
            string timeStampStr = timeStamp.ToString(_dateFormat);
            foreach (var item in _childLayers) {
                if (timeStampStr == item.Key.ToString(_dateFormat)) { return item.Value; }
            }
            return null;
        }

        /// <summary>
        /// Refresh Group
        /// </summary>
        /// <returns></returns>
        public void Refresh(bool isChildRefresh = true) {
            try {
                _childLayers.Clear();
                DirectoryInfo dir = new DirectoryInfo(_owner.Path);
                if (_isFileGroup) {
                    RefreshFile(dir);
                }
                else {
                    RefreshFolder(dir, isChildRefresh);
                }
                CheckSize();
            }
            catch (Exception) {
            }
        }

        /// <summary>
        /// Init
        /// </summary>
        private void Init() {
            _childLayers = new SortedDictionary<DateTime, ILayer>();
            _isFileGroup = _owner.Clockwork.IsFileGroup(_owner.Index - 1);
            _dateFormat = _owner.Clockwork[_owner.Index].DateFormat;
            _maxChildCount = _owner.Clockwork[_owner.Index].MaxChildCount;
        }

        /// <summary>
        /// Get ChildLayer
        /// </summary>
        private ILayer GetChildLayer(DateTime timeStamp, string fileName) {
            ILayer layer = FindLayer(timeStamp);
            if (layer != null) { return layer; }
            ILayer newLayer;
            int index = _owner.Index + 1;
            if (_isFileGroup) {
                newLayer = new FileLayer(fileName, _owner.Clockwork, index, _owner);
            }
            else {
                newLayer = new FolderLayer(fileName, _owner.Clockwork, index, _owner);
            }
            _childLayers.Add(timeStamp, newLayer);
            return newLayer;
        }

        /// <summary>
        /// Check if the group size over flow
        /// </summary>
        /// <returns></returns>
        private void CheckSize() {
            if (_childLayers.Count <= _maxChildCount) { return; }
            foreach (var item in _childLayers.ToList().GetRange(0, _childLayers.Count - _maxChildCount)) {
                if (!_childLayers[item.Key].Delete()) { continue; }
                _childLayers.Remove(item.Key);
            }
        }

        /// <summary>
        /// RefreshFolder by HD files
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="isChildRefresh"></param>
        private void RefreshFolder(DirectoryInfo dir, bool isChildRefresh) {
            foreach (var item in dir.GetDirectories()) {
                DateTime date;
                if (!DateTime.TryParseExact(item.Name, _dateFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out date)) { DeleteIrrelatedFolder(item.FullName); continue; }
                IFolderLayer layer = (IFolderLayer)GetChildLayer(date, item.Name);
                if (isChildRefresh) { layer.Refresh(); }
            }
        }

        /// <summary>
        ///  RefreshFile by HD files
        /// </summary>
        /// <param name="dir"></param>
        private void RefreshFile(DirectoryInfo dir) {
            foreach (var item in dir.GetFiles(FileFilterChar + _owner.Clockwork.FileNameExtention)) {
                DateTime date;
                if (!DateTime.TryParseExact(Path.GetFileNameWithoutExtension(item.Name), _dateFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out date)) { DeleteIrrelatedFile(item.FullName); continue; }
                IFileLayer layer = (IFileLayer)GetChildLayer(date, Path.GetFileNameWithoutExtension(item.Name));
            }
        }

        /// <summary>
        /// Delete Irrelated File
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        private bool DeleteIrrelatedFile(string fullPath) {
            try {
                File.Delete(fullPath);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// Delete Irrelated Folder
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        private bool DeleteIrrelatedFolder(string fullPath) {
            try {
                Directory.Delete(fullPath);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        #endregion Function

    }
}
