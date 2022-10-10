///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:OPC Group box
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using Irlovan.DataQuality;
using Irlovan.Lib.Symbol;
using Irlovan.Log;
using OPCAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Xml.Linq;

namespace Irlovan.Driver
{
    internal class GroupBox : IDisposable
    {

        #region Structure

        /// <summary>
        /// GroupBox construction
        /// </summary>
        internal GroupBox(IGroup dataList, OPCServer server, XElement config) {
            _server = server;
            _dataList = dataList;
            _config = config;
            AddGroup();
        }

        #endregion Structure

        #region Field

        //Default updata rate for all groups
        private const short UpdateRate = 800;

        private OPCServer _server;
        private IGroup _dataList;
        private XElement _config;
        private const string UpdateRatePara = "UpdateRate";
        private List<IDriverData> _driverDataList;
        private OPCGroup _group;
        private Array _serverHandleArray;
        private const int opcValueGood = 192;
        private const int opcValueForced = 216;

        #endregion Field

        #region Property

        /// <summary>
        /// OPCGroup
        /// </summary>
        internal OPCGroup Group {
            get { return _group; }
        }

        #endregion Property

        #region Function

        /// <summary>
        /// Init OPC Group
        /// </summary>
        private void AddGroup() {
            string groupName = _config.Attribute(Driver.GroupNamePara).Value;
            try {
                System.Threading.Thread.Sleep(50);
                _group = _server.OPCGroups.Add(groupName);
                var count = _dataList.Count;
                Array itemIDArray = GetChildAttributeArray(_dataList);
                Array clientHandleArray = Lib.Array.Array.CreateSerialArray(0, count);
                _serverHandleArray = Array.CreateInstance(typeof(Int32), count);
                Array errorArray = Array.CreateInstance(typeof(Int32), count + 1);
                _group.UpdateRate = (_config.Attribute(UpdateRatePara).Value == null) ? UpdateRate : int.Parse(_config.Attribute(UpdateRatePara).Value);
                _group.IsActive = true;
                _group.IsSubscribed = true;
                //_group.OPCItems.DefaultIsActive = true;
                CreateServerHandleIdMap(groupName, _serverHandleArray);
                _driverDataList = new List<IDriverData>();
                foreach (var item in _dataList.DataBox.Values) {
                    _driverDataList.Add(item);
                }
                _group.DataChange += DataChangeHandler;
                _group.OPCItems.AddItems(count, ref itemIDArray, ref clientHandleArray, out _serverHandleArray, out errorArray);
            }
            catch (Exception e) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, groupName + Symbol.Colon_Char + e.ToString());
            }
        }

        /// <summary>
        /// get the list of a named attibute from the child of the xelement
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attrName"></param>
        /// <param name="opc"></param>
        /// <returns></returns>
        private Array GetChildAttributeArray(IGroup dataList) {
            List<string> result = new List<string>();
            result.Add("");
            foreach (var item in dataList.DataBox) {
                result.Add(item.Value.Address);
            }
            return result.ToArray();
        }

        /// <summary>
        /// CreateServerHandleIdMap
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="serverHandleArray"></param>
        private void CreateServerHandleIdMap(string groupName, Array serverHandleArray) {
            Dictionary<string, int> map = new Dictionary<string, int>();
            int index = 0;
            List<int> serverHandleList = new List<int>();
            foreach (var item in serverHandleArray) {
                serverHandleList.Add((int)item);
            }
            foreach (var item in _dataList.DataBox) {
                map.Add(item.Key, serverHandleList[index]);
                index++;
            }
        }

        /// <summary>
        /// Handler for datachange
        /// </summary>
        /// <param name="transactionID"></param>
        /// <param name="numItems"></param>
        /// <param name="clientHandles"></param>
        /// <param name="itemValues"></param>
        /// <param name="qualities"></param>
        /// <param name="timeStamps"></param>
        private void DataChangeHandler(int transactionID, int numItems, ref Array clientHandles, ref Array itemValues, ref Array qualities, ref Array timeStamps) {
            List<int> clients = Lib.Array.Array.ArrayToList<int>(clientHandles);
            List<int> qualityList = Lib.Array.Array.ArrayToList<int>(qualities);
            List<object> values = Lib.Array.Array.ArrayToList<object>(itemValues);
            for (int i = 0; i < values.Count; i++) {
                IDriverData data = _driverDataList[clients[i] - 1];
                object value = values[i];
                if (value == null) { continue; }
                int qualityCode = qualityList[i];
                QualityEnum quality = ((qualityCode == opcValueForced) || ((qualityCode == opcValueGood))) ? QualityEnum.Good : QualityEnum.Bad;
                data.ReadValue(values[i], quality);
            }
        }

        /// <summary>
        /// Read Complete handler
        /// </summary>
        private void ReadCompleteHandler(int transactionID, int numItems, ref Array clientHandles, ref Array itemValues, ref Array qualities, ref Array timeStamps, ref Array errors) {
            DataChangeHandler(transactionID, numItems, ref clientHandles, ref itemValues, ref qualities, ref timeStamps);
        }


        //data to be write to opc channal
        [HandleProcessCorruptedStateExceptions]
        public void Write(Dictionary<string, object> data) {
            Array values = Lib.Array.Array.Combin<object>(new object[] { string.Empty }, data.Values.Select(i => i).ToArray());
            Array errors = Array.CreateInstance(typeof(Int32), data.Count + 1);
            Array serverHandles = CreateServerHandleArray(data);
            int cancelID;
            try { _group.AsyncWrite(data.Count, ref serverHandles, ref values, out errors, 0, out cancelID); }
            catch (Exception) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.WriteDataFailed);
            }
        }

        /// <summary>
        /// CreateServerHandleArray
        /// </summary>
        /// <returns></returns>
        private Array CreateServerHandleArray(Dictionary<string, object> data) {
            List<int> serverHandleArray = Lib.Array.Array.ArrayToList<int>(_serverHandleArray);
            List<int> result = new List<int>();
            result.Add(0);
            foreach (var item in data.Keys) {
                int serverHandler = serverHandleArray[_driverDataList.FindIndex(a => a.Data.FullName == item)];
                result.Add(serverHandler);
            }
            return result.ToArray();
        }

        /// <summary>
        /// Dispose Element
        /// </summary>
        public void Dispose() {
            _group.DataChange -= DataChangeHandler;
        }

        #endregion Function

    }
}
