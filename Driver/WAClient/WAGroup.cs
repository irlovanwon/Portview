///Copyright(c) 2015,HIT All rights reserved.
///Summary:WAGroup
///Author:Irlovan
///Date:2015-05-16
///Description:
///Modification:

using System.Collections.Generic;

namespace Irlovan.Driver
{
    internal class WAGroup
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="group"></param>
        internal WAGroup(IGroup group) {
            _group = group;
            InitAddressMap();
        }

        #endregion Structure

        #region Field

        private IGroup _group;
        //Address Map <addressName,<dataName,>>
        private Dictionary<string, Dictionary<string, IDriverData>> _addressMap = new Dictionary<string, Dictionary<string, IDriverData>>();
        private object _lock = new object();

        #endregion Field

        #region Property

        /// <summary>
        /// WebArc Group
        /// </summary>
        internal IGroup Group {
            get { return _group; }
            set {
                if (value != _group) {
                    _group = value;
                }
            }
        }

        /// <summary>
        /// Address indexer
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        internal Dictionary<string, IDriverData> this[string address] {
            get { return GetDataList(address); }
        }

        #endregion Property

        #region Function

        /// <summary>
        /// Dispose
        /// </summary>
        internal void Dispose() {
            lock (_lock) {
                _addressMap.Clear();
            }
        }

        /// <summary>
        /// ContainAddress
        /// </summary>
        /// <returns></returns>
        internal bool ContainAddress(string address) {
            lock (_lock) {
                return _addressMap.ContainsKey(address);
            }
        }

        /// <summary>
        /// InitAddressMap
        /// </summary>
        private void InitAddressMap() {
            lock (_lock) {
                foreach (var item in _group.DataBox) {
                    FillAddressMap(item.Value);
                }
            }
        }

        /// <summary>
        /// InitAddressMap
        /// </summary>
        private void FillAddressMap(IDriverData driverData) {
            if (!_addressMap.ContainsKey(driverData.Address)) {
                Dictionary<string, IDriverData> dataMap = new Dictionary<string, IDriverData>();
                dataMap.Add(driverData.Data.FullName, driverData);
                _addressMap.Add(driverData.Address, dataMap);
            }
            else {
                Dictionary<string, IDriverData> dataMap = _addressMap[driverData.Address];
                if (dataMap.ContainsKey(driverData.Data.FullName)) { return; }
                dataMap.Add(driverData.Data.FullName, driverData);
            }
        }

        /// <summary>
        /// GetDataList by address indexer
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, IDriverData> GetDataList(string address) {
            lock (_lock) {
                if (!_addressMap.ContainsKey(address)) { return null; }
                return _addressMap[address];
            }
        }

        #endregion Function

    }
}
