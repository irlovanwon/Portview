///Copyright(c) 2013,Irlovan All rights reserved.
///Summary:RealTimeData
///Author:Irlovan
///Date:2013-04-01
///Description:
///Modification:2015-10-30

using Irlovan.Lib.XML;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Database
{
    public class Database : IDatabase
    {

        #region Structure

        /// <summary>
        /// Database base construction
        /// </summary>
        public Database() {
            Init();
        }

        /// <summary>
        /// Database base construction
        /// </summary>
        public Database(string name, string description = null)
            : this() {
            _name = name;
            _desc = description;
        }

        /// <summary>
        /// Database base construction from xml config
        /// </summary>
        public Database(XElement config)
            : this() {
            ReadXML(config);
        }

        #endregion Structure

        #region Field

        private string _name;
        private string _desc;
        public const string NamePara = "Name";
        public const string DescPara = "Description";
        public const char NameSplitChar = '.';

        #endregion Field

        #region Property

        /// <summary>
        /// Catalog for the data
        /// </summary>
        public Catalog Parent { get; set; }

        /// <summary>
        /// Full name of data
        /// </summary>
        public string FullName { get { return GetFullName(); } }

        /// <summary>
        /// Name of data
        /// </summary>
        public string Name {
            get { return _name; }
            set {
                if (value != _name) {
                    _name = value;
                }
            }
        }

        /// <summary>
        /// Description of data
        /// </summary>
        public string Description {
            get { return _desc; }
            set {
                if (value != _desc) {
                    _desc = value;
                }
            }
        }

        /// <summary>
        /// InitState to show if the data inited in proper way from xml file
        /// </summary>
        public bool InitState { get; set; }

        /// <summary>
        ///List to collect error property para
        /// </summary>
        public List<string> ErrorParaList { get; set; }

        /// <summary>
        /// Data Species
        /// </summary>
        public string Species { get; set; }

        /// <summary>
        /// Child Catalogs
        /// </summary>
        public Dictionary<string, IDatabase> ChildDic { get; set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Get Database from xml config file
        /// </summary>
        /// <param name="element"></param>
        public virtual void ReadXML(XElement element) {
            XML.InitStringAttr<string>(element, DescPara, out _desc);
            if (!XML.InitStringAttr<string>(element, NamePara, out _name)) { ErrorParaList.Add(NamePara); InitState = false; }
        }

        /// <summary>
        /// Write Database to xml config file
        /// </summary>
        /// <returns></returns>
        public virtual XElement WriteXML() {
            XElement result = new XElement(Species.ToString());
            result.SetAttributeValue(NamePara, Name);
            if (!string.IsNullOrEmpty(Description)) { result.SetAttributeValue(DescPara, Description); }
            return result;
        }

        /// <summary>
        /// Remove this data
        /// </summary>
        public bool Remove() {
            if (Parent == null) { return false; }
            return Parent.RemoveChild(Name);
        }

        /// <summary>
        /// Init
        /// </summary>
        public virtual void Init() {
            InitState = true;
            ErrorParaList = new List<string>();
            ChildDic = new Dictionary<string, IDatabase>();
        }

        /// <summary>
        /// Get full name of the data
        /// </summary>
        /// <returns></returns>
        private string GetFullName() {
            return ((Parent == null) || string.IsNullOrEmpty(Parent.FullName)) ? Name : string.Concat(Parent.FullName, NameSplitChar, Name);
        }

        #endregion Function

    }

}
