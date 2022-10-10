///Copyright(c) 2015,HIT All rights reserved.
///Summary:IndustryDataMessage interface
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      

using Irlovan.DataQuality;
using Irlovan.Lib.XML;
using System;
using System.Text;
using System.Xml.Linq;

namespace Irlovan.Message
{
    public class IndustryDataMessage : DataMessage, IIndustryDataMessage
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dataType"></param>
        /// <param name="timeStamp"></param>
        /// <param name="description"></param>
        /// <param name="quality"></param>
        public IndustryDataMessage(string name, string value, Type dataType, DateTime timeStamp, string description, QualityEnum quality)
            : base(name, description) {
            TimeStamp = timeStamp;
            Value = value;
            _quality = quality;
            _dataType = dataType;
        }

        /// <summary>
        /// Construct from xml
        /// </summary>
        /// <param name="config"></param>
        public IndustryDataMessage(XElement config)
            : base(config) {
            if (InitState == false) { return; }
            if (Config.Name != XMLTag) { InitState = false; return; }
            XML.InitStringAttr<DateTime>(Config, TimeStampPara, out _timeStamp);
            if (!XML.InitStringAttr<string>(Config, ValuePara, out _value)) { InitState = false; return; }
            if (!XML.InitStringAttr<QualityEnum>(Config, QualityPara, out _quality)) { InitState = false; return; }
            XML.InitStringAttr<Type>(Config, DataTypePara, out _dataType);
        }

        #endregion Structure

        #region Field

        private const string TimeStampPara = "TimeStamp";
        private const string ValuePara = "Value";
        private const string XMLTag = "InDataMessage";
        private const string QualityPara = "Quality";
        private const string DataTypePara = "DataType";
        private DateTime _timeStamp;
        private Type _dataType;
        private string _value;
        private QualityEnum _quality;

        #endregion Field

        #region Property

        /// <summary>
        /// Trigger when data change
        /// </summary>
        public DateTime TimeStamp {
            get { return _timeStamp; }
            private set {
                if (value != _timeStamp) {
                    _timeStamp = value;
                }
            }
        }

        /// <summary>
        /// value for data
        /// </summary>
        public string Value {
            get { return _value; }
            private set {
                if (value != _value) {
                    _value = value;
                }
            }
        }

        /// <summary>
        /// Quality for data
        /// </summary>
        public QualityEnum Quality {
            get { return _quality; }
            private set {
                if (value != _quality) {
                    _quality = value;
                }
            }
        }

        #endregion Property

        #region Function

        /// <summary>
        /// Write Basic Info to String
        /// </summary>
        /// <returns></returns>
        internal override XElement ToBasicXML() {
            XElement result = base.ToBasicXML();
            result.Name = XMLTag;
            if (!string.IsNullOrEmpty(Value)) {
                result.SetAttributeValue(ValuePara, Value);
            }
            result.SetAttributeValue(QualityPara, Quality.ToString());
            return result;
        }

        /// <summary>
        /// Write All Info to String
        /// </summary>
        /// <returns></returns>
        internal override XElement ToTypicXML() {
            XElement result = base.ToTypicXML();
            result.Name = XMLTag;
            result.SetAttributeValue(TimeStampPara, TimeStamp);
            result.SetAttributeValue(DataTypePara, _dataType);
            return result;
        }

        /// <summary>
        /// Write All Info to String
        /// </summary>
        /// <returns></returns>
        internal override XElement ToAllXML() {
            XElement result = base.ToAllXML();
            return result;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() { }

        /// <summary>
        /// Write Basic Info to String
        /// </summary>
        /// <returns></returns>
        internal override string ToBasicString() {
            StringBuilder result = new StringBuilder();
            result.Append(base.ToBasicString());
            result.Append(SplitChar);
            result.Append(Value.ToString());
            result.Append(SplitChar);
            result.Append(Quality.ToString());
            return result.ToString();
        }

        /// <summary>
        /// Equals Comparison
        /// </summary>
        public bool Equals(IIndustryDataMessage message) {
            if (message == null) { return false; }
            if (message.Name != Name) { return false; }
            if (!message.Value.Equals(Value)) { return false; }
            if (message.Quality != Quality) { return false; }
            return true;
        }

        /// <summary>
        /// Write All Info to String
        /// </summary>
        /// <returns></returns>
        internal override string ToTypicString() {
            StringBuilder result = new StringBuilder();
            result.Append(base.ToTypicString());
            result.Append(SplitChar);
            result.Append(TimeStamp.ToString());
            return result.ToString();
        }

        /// <summary>
        /// Write All Info to String
        /// </summary>
        /// <returns></returns>
        internal override string ToAllString() {
            StringBuilder result = new StringBuilder();
            result.Append(base.ToAllString());
            return result.ToString();
        }

        #endregion Function

    }
}
