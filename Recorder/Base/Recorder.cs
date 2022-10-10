///Copyright(c) 2014,Irlovan All rights reserved.
///Summary:
///Author:Irlovan
///Date:2014-12-11
///Description:
///Modification:2015-08-04****************Add recorder group
///Modification:2015-08-05****************Remove recorder group

using Irlovan.Database;
using Irlovan.Lib.XML;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Recorder
{
    public class Recorder : IRecorder
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="source"></param>
        /// <param name="config"></param>
        public Recorder(Catalog source, XElement config) {
            Source = source;
            Config = config;
        }

        #endregion Structure

        #region Field

        public const string RecorderNameTag = "Name";
        public const string DataTag = "Data";
        public const string DataNameAttr = "Name";
        public const string DataIDAttr = "ID";
        public const string IntervalTag = "Interval";
        public const string RecordExceptionTimeoutTag = "RecordExceptionTimeout";
        public const string CommAutoDetectingIntervalTag = "CommAutoDetectingInterval";
        public const char ErrorInfoSplitChar = ',';

        private string _recorderName;
        private int _interval;
        private int _commAutoDetectingInterval;
        private int _exceptionTimeout;

        #endregion Field

        #region Property

        /// <summary>
        /// A property shows if all the properties of recorder has been initiated
        /// </summary>
        public bool InitState { get; set; }

        /// <summary>
        /// Error Attribute List
        /// </summary>
        public List<string> ErrorAttr { get; set; }

        /// <summary>
        /// Name of the record,Uniq
        /// </summary>
        public string RecorderName {
            get { return _recorderName; }
            private set {
                if (value != _recorderName) {
                    _recorderName = value;
                }
            }
        }

        /// <summary>
        /// All Datas 
        /// </summary>
        public Catalog Source { get; set; }

        /// <summary>
        /// Config file for the very recorder
        /// </summary>
        public XElement Config { get; set; }

        /// <summary>
        /// Data list map coordinated by data name
        /// </summary>
        public Dictionary<string, IRecorderData> NameDictionary { get; private set; }

        /// <summary>
        /// Data list map coordinated by data id
        /// </summary>
        public Dictionary<int, IRecorderData> IDDictionary { get; private set; }

        /// <summary>
        /// Data List
        /// </summary>
        public List<IRecorderData> DataList { get; set; }

        /// <summary>
        /// mode for Recorder 
        /// more than 0 mean Hybrid mode
        /// less than 0 mean Interval mode
        /// </summary>
        public int Interval {
            get { return _interval; }
            set {
                if (_interval != value) {
                    _interval = value;
                }
            }
        }

        /// <summary>
        /// Exception reconnection timeout
        /// </summary>
        public int ExceptionTimeout {
            get { return _exceptionTimeout; }
            set {
                if (value != _exceptionTimeout) {
                    _exceptionTimeout = value;
                }
            }
        }

        /// <summary>
        /// Communication Init failed then auto detecting mode is on
        /// </summary>
        public int CommAutoDetectingInterval {
            get { return _commAutoDetectingInterval; }
            set {
                if (value != _commAutoDetectingInterval) {
                    _commAutoDetectingInterval = value;
                }
            }
        }

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
            NameDictionary = new Dictionary<string, IRecorderData>();
            IDDictionary = new Dictionary<int, IRecorderData>();
            DataList = new List<IRecorderData>();
            ErrorAttr = new List<string>();
            if (!XML.InitStringAttr<string>(Config, Recorder.RecorderNameTag, out _recorderName)) { ErrorAttr.Add(Recorder.RecorderNameTag); InitState = false; }
            if (!XML.InitStringAttr<int>(Config, Recorder.IntervalTag, out _interval)) { ErrorAttr.Add(Recorder.IntervalTag); InitState = false; }
            if (!XML.InitStringAttr<int>(Config, Recorder.CommAutoDetectingIntervalTag, out _commAutoDetectingInterval)) { ErrorAttr.Add(Recorder.CommAutoDetectingIntervalTag); InitState = false; }
            if (!XML.InitStringAttr<int>(Config, Recorder.RecordExceptionTimeoutTag, out _exceptionTimeout)) { ErrorAttr.Add(Recorder.RecordExceptionTimeoutTag); InitState = false; }
            InitDataList();
        }

        /// <summary>
        /// Dispose for Recorder
        /// </summary>
        public virtual void Dispose() { }

        /// <summary>
        /// Init Data List
        /// </summary>
        private void InitDataList() {
            IEnumerable<XElement> dataListConfig = Config.Elements(Recorder.DataTag);
            foreach (var item in dataListConfig) {
                int id;
                if (!XML.InitStringAttr<int>(item, Recorder.DataIDAttr, out id)) { continue; }
                if (IDDictionary.ContainsKey(id)) { continue; }
                string dataName;
                if (!XML.InitStringAttr<string>(item, Recorder.DataNameAttr, out dataName)) { continue; }
                IIndustryData data = Source.AcquireIndustryData(dataName);
                if (data == null) { continue; }
                if (NameDictionary.ContainsKey(data.FullName)) { continue; }
                IRecorderData recorderData = new RecorderData(data, id, item);
                NameDictionary.Add(data.FullName, recorderData);
                IDDictionary.Add(id, recorderData);
                DataList.Add(recorderData);
            }
        }

        #endregion Function

    }
}
