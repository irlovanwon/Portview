///Copyright(c) 2015,HIT All rights reserved.
///Summary:EventDataMessage
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      

using Irlovan.Lib.XML;
using System;
using System.Text;
using System.Xml.Linq;

namespace Irlovan.Message
{
    public class EventDataMessage : DataMessage, IEventDataMessage
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="name"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="eventLevel"></param>
        /// <param name="description"></param>
        /// <param name="indication"></param>
        public EventDataMessage(string name, DateTime startTime, DateTime endTime, string eventLevel, string description, string indication)
            : base(name, description) {
            StartTime = startTime;
            EndTime = endTime;
            EventLevel = eventLevel;
            Indication = indication;
        }

        /// <summary>
        /// Construct from xml
        /// </summary>
        /// <param name="config"></param>
        public EventDataMessage(XElement config)
            : base(config) {
            if (InitState == false) { return; }
            if (Config.Name != XMLTag) { InitState = false; return; }
            if (!XML.InitStringAttr<DateTime>(Config, StartTimePara, out _startTime)) { InitState = false; return; }
            XML.InitStringAttr<DateTime>(Config, EndTimePara, out _endTime);
            XML.InitStringAttr<string>(Config, EventLevelPara, out _eventLevel);
            XML.InitStringAttr<string>(Config, IndicationPara, out _indication);
        }

        #endregion Structure

        #region Field

        private const string StartTimePara = "StartTime";
        private const string EndTimePara = "EndTime";
        private const string EventLevelPara = "EventLevel";
        private const string IndicationPara = "Indication";
        private const string XMLTag = "EDataMessage";
        private DateTime _startTime = new DateTime();
        private DateTime _endTime = new DateTime();
        private string _indication;
        private string _eventLevel;

        #endregion Field

        #region Property

        /// <summary>
        /// Trigger time for event
        /// </summary>
        public DateTime StartTime {
            get { return _startTime; }
            private set {
                if (value != _startTime) {
                    _startTime = value;
                }
            }
        }

        /// <summary>
        /// reset time for event
        /// </summary>
        public DateTime EndTime {
            get { return _endTime; }
            private set {
                if (value != _endTime) {
                    _endTime = value;
                }
            }
        }

        /// <summary>
        /// Indication for Event
        /// </summary>
        public string Indication {
            get { return _indication; }
            private set {
                if (value != _indication) {
                    _indication = value;
                }
            }
        }

        /// <summary>
        /// Event Level for event
        /// </summary>
        public string EventLevel {
            get { return _eventLevel; }
            private set {
                if (value != _eventLevel) {
                    _eventLevel = value;
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
            if (StartTime != DateTime.MinValue) {
                result.SetAttributeValue(StartTimePara, StartTime.ToString());
            }
            if (EndTime != DateTime.MinValue) {
                result.SetAttributeValue(EndTimePara, EndTime.ToString());
            }
            return result;
        }

        /// <summary>
        /// Write All Info to String
        /// </summary>
        /// <returns></returns>
        internal override XElement ToTypicXML() {
            XElement result = base.ToTypicXML();
            if (!string.IsNullOrEmpty(EventLevel)) {
                result.SetAttributeValue(EventLevelPara, EventLevel);
            }
            return result;
        }

        /// <summary>
        /// Write All Info to String
        /// </summary>
        /// <returns></returns>
        internal override XElement ToAllXML() {
            XElement result = base.ToAllXML();
            if (!string.IsNullOrEmpty(Indication)) {
                result.SetAttributeValue(IndicationPara, Indication);
            }
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
            result.Append(StartTime.ToString());
            result.Append(SplitChar);
            result.Append(EndTime.ToString());
            return result.ToString();
        }

        /// <summary>
        /// Write Typic Info to String
        /// </summary>
        /// <returns></returns>
        internal override string ToTypicString() {
            StringBuilder result = new StringBuilder();
            result.Append(base.ToTypicString());
            result.Append(SplitChar);
            result.Append(EventLevel.ToString());
            return result.ToString();
        }

        /// <summary>
        /// Write All Info to String
        /// </summary>
        /// <returns></returns>
        internal override string ToAllString() {
            StringBuilder result = new StringBuilder();
            result.Append(base.ToAllString());
            result.Append(SplitChar);
            result.Append(Indication.ToString());
            return result.ToString();
        }

        #endregion Function

    }
}
