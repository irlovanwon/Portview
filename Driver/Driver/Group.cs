///Copyright(c) 2015,HIT All rights reserved.
///Summary:Driver group base class
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      

using Irlovan.DataQuality;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Driver
{
    public class Group : IGroup
    {

        #region Structure

        /// <summary>
        /// Driver Gourp Construction
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="rwMode"></param>
        /// <param name="config"></param>
        public Group(string groupName, RWModeEnum rwMode, XElement config) {
            GroupName = groupName;
            RWMode = rwMode;
            Config = config;
            Init();
        }

        #endregion Structure

        #region Field

        private object _lock = new object();
        private Dictionary<string, IDriverData> _dataBox = new Dictionary<string, IDriverData>();
        private IRWBox _rwBox;

        #endregion Field

        #region Property

        /// <summary>
        /// Driver Data List<DriverAddr,DriverData>
        /// </summary>
        public Dictionary<string, IDriverData> DataBox {
            get { lock (_lock) { return _dataBox; } }
        }

        /// <summary>
        /// Read and Write Data List
        /// </summary>
        public IRWBox RWDataBox {
            get { return _rwBox; }
        }

        /// <summary>
        /// Config for the IGroup
        /// </summary>
        public XElement Config { get; private set; }

        /// <summary>
        /// Count of Chidren
        /// </summary>
        public int Count { get { return GetCount(); } }

        /// <summary>
        /// GroupName
        /// </summary>
        public string GroupName { get; private set; }

        /// <summary>
        /// Update rate of the group
        /// </summary>
        public int UpdateRate { get; set; }

        /// <summary>
        /// The Group is in Reading Mode/Writing Mode
        /// </summary>
        public RWModeEnum RWMode { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// push data to driver data box
        /// </summary>
        /// <param name="data"></param>
        public void Push(IDriverData data) {
            lock (_lock) {
                if (_dataBox.ContainsKey(data.Data.FullName)) { return; }
                _dataBox.Add(data.Data.FullName, data);
            }
        }

        /// <summary>
        ///remove data from driver data box by name
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name) {
            lock (_lock) {
                if (!_dataBox.ContainsKey(name)) { return; }
                _dataBox.Remove(name);
            }
        }

        /// <summary>
        /// Init Read/Write Box
        /// </summary>
        public void InitRWBox() {
            InitRWData();
        }

        /// <summary>
        /// If contain the named item
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contain(string name) {
            lock (_lock) {
                if (!_dataBox.ContainsKey(name)) { return false; }
                return true;
            }
        }

        /// <summary>
        /// get driver data
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IDriverData GetData(string name) {
            lock (_lock) {
                if (!_dataBox.ContainsKey(name)) { return null; }
                return _dataBox[name];
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() {
            if (_dataBox != null) { _dataBox.Clear(); }
            RWDataBox.Dispose();
        }

        /// <summary>
        /// Apply Quality to all data
        /// </summary>
        public void ApplyQuality(QualityEnum quality) {
            lock (_lock) {
                foreach (var item in _dataBox) {
                    item.Value.SetQuality(QualityEnum.Bad);
                }
            }
        }

        /// <summary>
        /// Init Box
        /// </summary>
        private void Init() {
            if (_dataBox != null) { _dataBox.Clear(); }
            _dataBox = new Dictionary<string, IDriverData>();
            _rwBox = new RWBox(GroupName);
        }

        /// <summary>
        /// Init Read and Write Data
        /// </summary>
        private void InitRWData() {
            lock (_lock) {
                foreach (var item in _dataBox) {
                    if (item.Value.Readonly) { continue; }
                    _rwBox.Push(item.Value);
                }
            }
        }

        /// <summary>
        /// Get the Count of the databox
        /// </summary>
        /// <returns></returns>
        private int GetCount() {
            lock (_lock) {
                return _dataBox.Count;
            }
        }

        #endregion Function
    }
}
