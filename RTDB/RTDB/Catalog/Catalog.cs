///Copyright(c) 2013,HIT All rights reserved.
///Summary:Catalog
///Author:Irlovan
///Date:2013-03-29
///Description:this is the structure(or bibliography) of the database
///Modification:2015-10-30

using Irlovan.Lib.Symbol;
using Irlovan.Lib.XML;
using Irlovan.Log;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Database
{
    public class Catalog : Database, ICatalog
    {

        #region Structure

        /// <summary>
        /// Catalog database construction
        /// </summary>
        public Catalog()
            : base() { }

        /// <summary>
        /// Catalog database construction
        /// </summary>
        /// <param name="name"></param>
        public Catalog(string name, string description = null)
            : base(name, description) { }

        /// <summary>
        /// Catalog construction from xml config
        /// </summary>
        public Catalog(XElement config) : base(config) { }

        #endregion Structure

        #region Field

        internal const string SpeciesName = "Catalog";

        #endregion Field

        #region Function

        /// <summary>
        /// Add Child Element to a calalog
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public void Add(IDatabase child) {
            if (child == null) { return; }
            if (ChildDic.ContainsKey(child.Name)) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.DatabaseKeyExist + Symbol.Colon_Char + child.Name); return; }
            ChildDic.Add(child.Name, child);
            child.Parent = this;
        }

        /// <summary>
        /// Remove a child element by name
        /// </summary>
        /// <param name="name"></param>
        public bool RemoveChild(string name) {
            if ((string.IsNullOrEmpty(name)) || (!ChildDic.ContainsKey(name))) { return false; }
            ChildDic.Remove(name);
            return true;
        }

        /// <summary>
        /// acquire data by it's name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IDatabase AcquireData(string fullName) {
            string[] route = FullName.Split(NameSplitChar);
            string[] targetRoute = fullName.Split(NameSplitChar);
            if (targetRoute.Length < route.Length) { return null; }
            for (int i = 0; i < route.Length; i++) {
                if (route[i] != targetRoute[i]) { return null; }
            }
            IDatabase result = this;
            for (int i = route.Length; i < targetRoute.Length; i++) {
                if (!result.ChildDic.ContainsKey(targetRoute[i])) { return null; }
                result = result.ChildDic[targetRoute[i]];
            }
            return result;
        }

        /// <summary>
        /// Acquire catalog by name from catalog
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ICatalog AcquireCatalog(string name) {
            IDatabase data = AcquireData(name);
            return ((data == null) || !(((Database)data) is ICatalog)) ? null : (ICatalog)data;
        }

        /// <summary>
        /// Acquire industrydata by name from catalog
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IIndustryData AcquireIndustryData(string name) {
            IDatabase data = AcquireData(name);
            return ((data == null) || !(((Database)data) is IIndustryData)) ? null : (IIndustryData)data;
        }

        /// <summary>
        /// Acquire industrydata by name from catalog
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IIndustryData<T> AcquireIndustryData<T>(string name) {
            IIndustryData data = AcquireIndustryData(name);
            return ((data == null) || !(((IIndustryData)data) is IIndustryData<T>)) ? null : (IIndustryData<T>)data;
        }

        /// <summary>
        /// Acquire eventdata by name from catalog
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEventData AcquireEventData(string name) {
            IDatabase data = AcquireData(name);
            return ((data == null) || !(((Database)data) is IEventData)) ? null : (IEventData)data;
        }

        /// <summary>
        /// Acquire all industry data
        /// </summary>
        /// <param name="includeChild">Is child Catalog included</param>
        /// <returns></returns>
        public List<IIndustryData> AcquireAll(bool includeChild) {
            List<IIndustryData> result = new List<IIndustryData>();
            foreach (Database item in ChildDic.Values) {
                if (item is IIndustryData) {
                    result.Add((IIndustryData)item);
                }
                if (includeChild && (item is ICatalog)) {
                    AddChildDataList(((ICatalog)item).AcquireAll(includeChild), ref result);
                }
            }
            return result;
        }

        /// <summary>
        /// Get Database from xml config file
        /// </summary>
        /// <returns></returns>
        public override void ReadXML(XElement element) {
            base.ReadXML(element);
            foreach (var item in element.Elements()) {
                IDatabase data = CreateChild(item.Name.ToString(), item);
                if ((data == null) || (!data.InitState)) { continue; }
                Add(data);
            }
        }

        /// <summary>
        /// Init
        /// </summary>
        public override void Init() {
            base.Init();
            Species = SpeciesName;
        }

        /// <summary>
        /// Create child
        /// </summary>
        /// <param name="name"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private IDatabase CreateChild(string name, XElement config) {
            switch (name) {
                case Catalog.SpeciesName:
                    return (Database)Activator.CreateInstance(typeof(Catalog), new Object[] { config });
                case EventData.SpeciesName:
                    return (Database)Activator.CreateInstance(typeof(EventData), new Object[] { config });
                case IndustryData<object>.SpeciesName:
                    string genericTypeStr;
                    if (!XML.InitStringAttr<string>(config, IndustryData<object>.DataTypePara, out genericTypeStr)) { return null; }
                    Type genericType = Type.GetType(genericTypeStr);
                    if (genericType == null) { return null; }
                    Type myGenericClass = typeof(IndustryData<>).MakeGenericType(new Type[] { genericType });
                    return (Database)Activator.CreateInstance(myGenericClass, new Object[] { config });
                default:
                    return null;
            }
        }

        /// <summary>
        /// AddChildDataList
        /// </summary>
        /// <param name="childDataList"></param>
        /// <param name="result"></param>
        private void AddChildDataList(List<IIndustryData> childDataList, ref List<IIndustryData> result) {
            foreach (var item in childDataList) {
                result.Add(item);
            }
        }

        #endregion Function

    }

}
