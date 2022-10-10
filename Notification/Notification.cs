///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Notification base class
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using Irlovan.Database;
using Irlovan.Lib.XML;
using Irlovan.Register;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Notification
{
    public class Notification : INotification
    {

        #region Structure

        /// <summary>
        /// Notification
        /// </summary>
        /// <param name="source"></param>
        /// <param name="config"></param>
        public Notification(Catalog source, IRegister register, XElement config) {
            Source = source;
            Config = config;
            Register = register;
        }

        #endregion Structure

        #region Field

        public const string IDAttr = "ID";
        public const string DataTag = "Data";
        public const string DataNameAttr = "Name";
        public const string NoticeAttr = "Notice";
        public const char ErrorAttrSplitChar = ',';

        private string _id;

        #endregion Field

        #region Property

        /// <summary>
        /// A property shows if all the properties of Notification has been initiated
        /// </summary>
        public bool InitState { get; set; }

        /// <summary>
        /// Error Attribute List
        /// </summary>
        public List<string> ErrorAttr { get; set; }

        /// <summary>
        /// ID of Notification
        /// </summary>
        public string ID {
            get { return _id; }
            set {
                if (value != _id) {
                    _id = value;
                }
            }
        }

        /// <summary>
        /// Register
        /// </summary>
        public IRegister Register { get; private set; }

        /// <summary>
        /// All Datas 
        /// </summary>
        public Catalog Source { get; set; }

        /// <summary>
        /// Config file of Notification
        /// </summary>
        public XElement Config { get; set; }

        /// <summary>
        /// Data list of notification
        /// </summary>
        public Dictionary<string, IIndustryData> DataList { get; set; }

        /// <summary>
        /// When message is triggered to notice by a IIndustryData variable
        /// </summary>
        public IIndustryData<bool> Notice { get; set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Start to Run Recorder
        /// </summary>
        public virtual void Run() { }

        /// <summary>
        /// Init properties for Recorder
        /// </summary>
        public virtual void Init() {
            InitState = true;
            ErrorAttr = new List<string>();
            if (!XML.InitStringAttr<string>(Config, IDAttr, out _id)) { ErrorAttr.Add(IDAttr); InitState = false; }
            string noticeName;
            if (XML.InitStringAttr<string>(Config, NoticeAttr, out noticeName)) { Notice = Source.AcquireIndustryData<bool>(noticeName); }
            InitDataList();
        }

        /// <summary>
        /// Dispose for Recorder
        /// </summary>
        public virtual void Dispose() { }

        /// <summary>
        /// Init datalist of notification
        /// </summary>
        private void InitDataList() {
            DataList = new Dictionary<string, IIndustryData>();
            IEnumerable<XElement> dataConfigList = Config.Elements(DataTag);
            foreach (var item in dataConfigList) {
                string dataName;
                if (!XML.InitStringAttr<string>(item, DataNameAttr, out dataName)) { continue; }
                IIndustryData data = Source.AcquireIndustryData(dataName);
                if (data == null) { continue; }
                if (DataList.ContainsKey(dataName)) { continue; }
                DataList.Add(dataName, data);
            }
        }

        #endregion Function

    }
}
