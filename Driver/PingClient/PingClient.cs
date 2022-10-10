///Copyright(c) 2015,HIT All rights reserved.
///Summary:Ping client
///Author:Irlovan
///Date:2015-11-23
///Description:
///Modification:      

using Irlovan.Database;
using Irlovan.Lib.XML;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Driver
{
    public class PingClient : Driver
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="source"></param>
        /// <param name="config"></param>
        public PingClient(Catalog source, XElement config)
            : base(source, config) { }

        #endregion Structure

        #region Field

        private const string SoftModePara = "SoftMode";
        private const string TimeoutPara = "Timeout";
        private const int DefaultUpdateRate = 1000;
        private const int ErrorProtectionTime = 1000;
        private List<PingData> _pingList = new List<PingData>();

        #endregion Field

        #region Function

        /// <summary>
        /// Init for the very driver
        /// </summary>
        public override void Init() {
            base.Init();
            PingInit();
        }

        /// <summary>
        /// run opc client
        /// </summary>
        public override void Run() {
            base.Run();
            if (!InitState) { return; }
            ServerConnectedHandler(DateTime.Now);
            StartPing();
        }

        /// <summary>
        /// Init for Mode Dictionary
        /// </summary>
        private void PingInit() {
            IEnumerable<XElement> groupList = Config.Elements(GroupTagName);
            if (groupList == null) { InitState = false; return; }
            foreach (var item in groupList) {
                string groupName;
                if (!XML.InitStringAttr<string>(item, GroupNamePara, out groupName)) { InitState = false; }
                if (string.IsNullOrEmpty(groupName)) { continue; }
                int updateRate = DefaultUpdateRate;
                if (!XML.InitStringAttr<int>(item, UpdateRatePara, out updateRate)) { InitState = false; }
                IEnumerable<XElement> dataListConfig = item.Elements(DataTagPara);
                if (dataListConfig == null) { continue; }
                GroupInit(dataListConfig, groupName, updateRate);
            }
        }

        /// <summary>
        /// Init the group of ping data
        /// </summary>
        /// <param name="config"></param>
        /// <param name="groupName"></param>
        /// <param name="updateRate"></param>
        private void GroupInit(IEnumerable<XElement> dataListConfig, string groupName, int updateRate) {
            foreach (var item in dataListConfig) {
                bool isSoftMode = false;
                if (!XML.InitStringAttr<bool>(item, SoftModePara, out isSoftMode)) { continue; }
                string name;
                if (!XML.InitStringAttr<string>(item, RealtimeDataPara, out name)) { continue; }
                if (string.IsNullOrEmpty(name)) { continue; }
                int timeout;
                if (!XML.InitStringAttr<int>(item, TimeoutPara, out timeout)) { continue; }
                if (timeout <= 0) { continue; }
                DriverData data = (DriverData)DataList[groupName].GetData(name);
                if (data == null) { continue; }
                _pingList.Add(new PingData(data, isSoftMode, timeout, updateRate));
            }
        }

        /// <summary>
        /// Start Ping Command
        /// </summary>
        /// <param name="nameOrAddress"></param>
        /// <returns></returns>
        private void StartPing() {
            if (!InitState) { return; }
            foreach (var item in _pingList) {
                item.Ping();
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            foreach (var item in _pingList) {
                item.Dispose();
            }
            _pingList.Clear();
        }

        #endregion Function

    }
}
