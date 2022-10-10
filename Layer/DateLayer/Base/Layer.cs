///Copyright(c) 2015,HIT All rights reserved.
///Summary:Layer
///Author:Irlovan
///Date:2016-05-07
///Description:
///Modification:

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace HIT.Layer
{
    public class Layer : ILayer
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="clockwork"></param>
        /// <param name="index"></param>
        public Layer(DirectoryInfo path, IInfo info, ILayer parent = null) {
            IsValid = true;
            Init(path, info, parent);
        }

        #endregion Structure

        #region Field

        private DateTime _timestamp;

        #endregion Field

        #region Property

        /// <summary>
        /// The name of the layer
        /// </summary>
        public string FolderName { get; private set; }

        /// <summary>
        /// Path of the layer
        /// </summary>
        public DirectoryInfo Folder { get; set; }

        /// <summary>
        /// Parent folder of the layer
        /// </summary>
        public ILayer Parent { get; private set; }

        /// <summary>
        /// IsValid
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// TimeStamp
        /// </summary>
        public DateTime TimeStamp {
            get { return _timestamp; }
            private set {
                if (value != _timestamp) {
                    _timestamp = value;
                }
            }
        }

        /// <summary>
        /// Info
        /// </summary>
        public IInfo Info { get; private set; }

        /// <summary>
        /// DateFormat of Children
        /// </summary>
        public SortedDictionary<DateTime, ILayer> Children { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        private void Init(DirectoryInfo path, IInfo info, ILayer parent = null) {
            Info = info;
            Children = new SortedDictionary<DateTime, ILayer>();
            Parent = parent;
            Folder = path;
            FolderName = Folder.Name;
            if ((Parent != null) && (!DateTime.TryParseExact(FolderName, Info.DateFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out _timestamp))) {
                IsValid = false;
            }
        }

        /// <summary>
        /// If the layer exists
        /// </summary>
        /// <returns></returns>
        public bool Exist() { return Folder.Exists; }

        /// <summary>
        /// Refresh
        /// </summary>
        /// <returns></returns>
        public void Refresh() {
            DirectoryInfo[] directories = Folder.GetDirectories();
            IInfo info = Info.Next;
            if (info == null) { return; }
            foreach (var item in directories) {
                ILayer layer = new Layer(item, info, this);
                if (!layer.IsValid) { continue; }
                if (Children.ContainsKey(layer.TimeStamp)) { continue; }
                Children.Add(layer.TimeStamp, layer);
                layer.Refresh();
            }
        }

        /// <summary>
        /// CheckSize
        /// </summary>
        /// <returns></returns>
        public void CheckSize() {
            ILayer[] childLayers = Children.Values.ToArray();
            if ((Info.Next!=null)&&(childLayers.Length > Info.Next.MaxCount)) {
                for (int i = 0; i < childLayers.Length - Info.MaxCount; i++) {
                    ILayer removeLayer = childLayers[i];
                    RemoveLayer(removeLayer);
                }
            }
            foreach (var item in Children) {
                item.Value.CheckSize();
            }
        }

        /// <summary>
        /// RemoveLayer
        /// </summary>
        /// <returns></returns>
        private void RemoveLayer(ILayer removeLayer) {
            if (!removeLayer.Delete()) { return; }
            if (!Children.ContainsKey(removeLayer.TimeStamp)) { return; }
            Children.Remove(removeLayer.TimeStamp);
        }

        /// <summary>
        /// Delete the layer
        /// </summary>
        /// <returns></returns>
        public bool Delete() {
            try {
                Folder.Delete(true);
            }
            catch (Exception) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// navigate
        /// </summary>
        /// <returns></returns>
        public DirectoryInfo Navigate(DateTime timeStamp) {
            if ((Parent != null && (!string.Equals(TimeStamp.ToString(Info.DateFormat), timeStamp.ToString(Info.DateFormat))))) { return null; }
            foreach (var item in Children) {
                DirectoryInfo result = item.Value.Navigate(timeStamp);
                if (result == null) { continue; }
                return result;
            }
            return Folder;
        }

        /// <summary>
        /// Append
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public void Append(DateTime timeStamp) {
            if ((Parent != null && (!string.Equals(TimeStamp.ToString(Info.DateFormat), timeStamp.ToString(Info.DateFormat))))) { return; }
            if (Info.Next == null) { return; }
            string newDateStr = timeStamp.ToString(Info.Next.DateFormat);
            foreach (var item in Children) {
                if (item.Value.FolderName == newDateStr) { item.Value.Append(timeStamp); return; }
            }
            DirectoryInfo directory;
            try {
                directory = new DirectoryInfo(Folder.FullName + "/" + newDateStr);
                directory.Create();
            }
            catch (Exception) {
                return;
            }
            ILayer layer = new Layer(directory, Info.Next, this);
            Children.Add(layer.TimeStamp, layer);
            layer.Append(timeStamp);
        }

        #endregion Function

    }
}
