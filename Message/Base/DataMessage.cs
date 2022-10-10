///Copyright(c) 2015,HIT All rights reserved.
///Summary:DataMessage
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using Irlovan.Lib.XML;
using System.Text;
using System.Xml.Linq;

namespace Irlovan.Message
{
    public class DataMessage : IDataMessage
    {

        #region Structure

        /// <summary>
        /// DataMessage
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public DataMessage(string name, string description)
            : this() {
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Construct from xml
        /// </summary>
        /// <param name="config"></param>
        public DataMessage(XElement config)
            : this() {
            Config = config;
            if (!XML.InitStringAttr<string>(Config, NamePara, out _name)) { InitState = false; return; }
            XML.InitStringAttr<string>(Config, DescriptionPara, out _desc);
        }

        /// <summary>
        /// Data Message
        /// </summary>
        internal DataMessage() {
            Init();
        }

        #endregion Structure

        #region Field

        internal const string NamePara = "Name";
        internal const string DescriptionPara = "Description";
        internal const char SplitChar = ';';
        private const string XMLTag = "Message";

        private string _name;
        private string _desc;

        #endregion Field

        #region Property

        /// <summary>
        /// Name of the event 
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
        /// Description for event
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
        /// Check if Message Init Successfull
        /// </summary>
        public bool InitState { get; internal set; }


        /// <summary>
        /// XML Config File
        /// </summary>
        /// <returns></returns>
        internal XElement Config { get; private set; }

        #endregion Property

        #region Function


        /// <summary>
        /// Write event message to xml
        /// </summary>
        /// <returns></returns>
        public XElement ToXML(FormatEnum format = FormatEnum.Basic) {
            if (format == FormatEnum.Basic) { return ToBasicXML(); }
            if (format == FormatEnum.All) { return ToAllXML(); }
            if ((format == FormatEnum.Typic)) { return ToTypicXML(); }
            return null;
        }

        /// <summary>
        /// Write Basic Info to String
        /// </summary>
        /// <returns></returns>
        internal virtual XElement ToBasicXML() {
            XElement result = new XElement(XMLTag);
            if (!string.IsNullOrEmpty(Name)) {
                result.SetAttributeValue(DataMessage.NamePara, Name);
            }
            return result;
        }

        /// <summary>
        /// Write All Info to String
        /// </summary>
        /// <returns></returns>
        internal virtual XElement ToTypicXML() {
            XElement result = ToBasicXML();
            if (!string.IsNullOrEmpty(Description)) {
                result.SetAttributeValue(DataMessage.DescriptionPara, Description);
            }
            return result;
        }

        /// <summary>
        /// Write All Info to String
        /// </summary>
        /// <returns></returns>
        internal virtual XElement ToAllXML() {
            XElement result = ToTypicXML();
            return result;
        }

        /// <summary>
        /// Write data message to string
        /// </summary>
        /// <returns></returns>
        public string ToString(FormatEnum format = FormatEnum.Basic) {
            if (format == FormatEnum.Basic) { return ToBasicString(); }
            if (format == FormatEnum.All) { return ToAllString(); }
            if ((format == FormatEnum.Typic)) { return ToTypicString(); }
            return null;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose() { }

        /// <summary>
        /// Write Basic Info to String
        /// </summary>
        /// <returns></returns>
        internal virtual string ToBasicString() {
            StringBuilder result = new StringBuilder();
            result.Append(Name);
            return result.ToString();
        }

        /// <summary>
        /// Write All Info to String
        /// </summary>
        /// <returns></returns>
        internal virtual string ToTypicString() {
            StringBuilder result = new StringBuilder();
            result.Append(ToBasicString());
            result.Append(SplitChar);
            return result.ToString();
        }

        /// <summary>
        /// Write All Info to String
        /// </summary>
        /// <returns></returns>
        internal virtual string ToAllString() {
            StringBuilder result = new StringBuilder();
            result.Append(ToTypicString());
            result.Append(SplitChar);
            result.Append(Description);
            return result.ToString();
        }

        /// <summary>
        /// Init for message
        /// </summary>
        private void Init() {
            InitState = true;
        }

        #endregion Function

    }
}
