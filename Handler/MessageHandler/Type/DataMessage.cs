///Copyright(c) 2015,HIT All rights reserved.
///Summary:Handler for realtime data message
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using Irlovan.Canal;
using Irlovan.Database;
using Irlovan.Lib.Convertor;
using Irlovan.Lib.XML;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Irlovan.Handlers
{
    internal class DataMessage : Handler
    {

        #region Structure

        /// <summary>
        /// Handler for realtime Event
        /// </summary>
        internal DataMessage(IServerSession session, LocalInterface.LocalInterface local)
            : base(session, local) {

        }

        #endregion Structure

        #region Field

        internal const string Name = "Data";
        internal const string SubcriptionPara = "SBC";
        internal const string StopPara = "STP";
        internal const string WriteDataPara = "WRT";
        internal const string ItemPara = "Item";
        internal const string NamePara = "Name";
        internal const string ValuePara = "Value";
        internal const string GroupPara = "Group";

        //Subcribed data list 
        private Dictionary<string, Group> _dataList = new Dictionary<string, Group>();

        private object _lock = new object();

        #endregion Field

        #region Function

        /// <summary>
        /// Handler String
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool Handle(IServerSession session, XElement element) {
            if (session.ID != Session.ID) { return false; }
            StartSubcription(element);
            StopSubcrition(element);
            WriteData(element);
            return true;
        }

        /// <summary>
        /// Start a subcription
        /// </summary>
        private void StartSubcription(XElement element) {
            XElement subcription = element.Element(SubcriptionPara);
            if (subcription == null) { return; }
            SubcriptionListInit(subcription);
            foreach (var item in _dataList) {
                item.Value.StartSBC();
            }
        }

        /// <summary>
        /// Stop Subcription
        /// </summary>
        private void StopSubcrition(XElement element) {
            XElement stopSubcription = element.Element(StopPara);
            if (stopSubcription == null) { return; }
            foreach (var item in stopSubcription.Elements(GroupPara)) {
                string groupName;
                if (!XML.InitStringAttr<string>(item, NamePara, out groupName)) { continue; }
                if (!_dataList.ContainsKey(groupName)) { continue; }
                _dataList[groupName].StopSubcrition();
            }
        }

        /// <summary>
        /// Subcription List Init
        /// </summary>
        private void SubcriptionListInit(XElement sbc) {
            lock (_lock) {
                DisposeDataList();
                foreach (var item in sbc.Elements(GroupPara)) {
                    Group group = GroupInit(item);
                    if (group == null) { continue; }
                    if (_dataList.ContainsKey(group.GroupName)) { continue; }
                    _dataList.Add(group.GroupName, group);
                }
            }
        }

        /// <summary>
        /// Init Group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private Group GroupInit(XElement group) {
            Group result = new Group(Session, LocalInterface, group);
            if (!result.InitState) { return null; }
            return result;
        }

        /// <summary>
        ///  Write Data to Driver
        /// </summary>
        /// <param name="element"></param>
        private void WriteData(XElement element) {
            XElement writeData = element.Element(WriteDataPara);
            if (writeData == null) { return; }
            IEnumerable<XElement> datas = writeData.Elements(ItemPara);
            if (datas.Count() == 0) { return; }
            foreach (var item in datas) {
                string dataName;
                string value;
                if (!XML.InitStringAttr<string>(item, NamePara, out dataName)) { continue; }
                if (!XML.InitStringAttr<string>(item, ValuePara, out value)) { continue; }
                IIndustryData data = LocalInterface.Source.AcquireIndustryData(dataName);
                if (data == null) { return; }
                object result;
                if (!Convertor.ConvertType(value, data.DataType, out result)) { continue; }
                data.WriteValue(result);
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            DisposeDataList();
        }

        /// <summary>
        /// DisposeDataListv
        /// </summary>
        private void DisposeDataList() {
            foreach (var item in _dataList) {
                item.Value.Dispose();
            }
            _dataList.Clear();
        }

        #endregion Function

    }
}