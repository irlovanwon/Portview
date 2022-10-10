///Copyright(c) 2016,HIT All rights reserved.
///Summary:
///Author:Irlovan
///Date:2016-06-12
///Description:

using Irlovan.Database;
using Irlovan.DataQuality;
using Irlovan.Lib.Array;
using Irlovan.Lib.XML;
using Irlovan.Log;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Timers;
using System.Xml.Linq;

namespace Irlovan.Recorder.TextRecorder
{
    public class MatrixRecorder : TextRecorder, IMatrixRecorder, IVURRecorder
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="source">Database Source</param>
        /// <param name="config">XML Config file of the recorder</param>
        public MatrixRecorder(Catalog source, XElement config)
            : base(source, config) {
        }

        #endregion Structure

        #region Field

        private Timer _timer;
        private DateTime _dateCache;
        private DateTime _lastUpdateTime;
        private string _extention = ".csv";
        private string _timeStampFormat = "MM-dd-yyyy_H:mm:ss::fff";
        private string _fileNameFormat = "MM-dd-yyyy_H-mm-ss-fff";
        private string _dateTitle;
        private int _totalRecordedCount = 0;
        // Message ready to be stored
        private Queue<string> _messageBox = new Queue<string>();
        private int _maxRows;
        private int _numberOfRowsPerRecord;
        private object _lock = new object();

        private const string TimeStampFormatAttr = "TimeStampFormat";
        private const string FileNameFormatAttr = "FileNameFormat";
        private const string DateCacheFormat = "MM-dd-yyyy";
        private const string TitleAttr = "Title";
        private const char CSVSplit = ',';
        private const string MaxRowsAttr = "MaxRows";
        private string NumberOfRowsPerRecordAttr = "NumberOfRowsPerRecord";
        public const string DateTitleAttr = "DateTitle";

        #endregion Field/

        #region Property

        /// <summary>
        /// Datalist<Title,Data>
        /// </summary>
        public Dictionary<string, IRecorderData> MatrixList { get; set; }

        /// <summary>
        /// Timestamp title of the vur recorder
        /// </summary>
        public string DateTitle {
            get { return _dateTitle; }
            private set {
                if (value != _dateTitle) {
                    _dateTitle = value;
                }
            }
        }

        /// <summary>
        /// DateFormat of the vur recorder
        /// </summary>
        public string DateFormat {
            get { return _timeStampFormat; }
            private set {
                if (value != _timeStampFormat) {
                    _timeStampFormat = value;
                }
            }
        }

        #endregion Property

        #region Function

        /// <summary>
        /// dispose for recorder
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            Lib.Timer.Timer.DisposeTimer(_timer);
        }

        /// <summary>
        /// init all properties for recorder
        /// </summary>
        public override void Init() {
            base.Init();
            if (Interval <= 0) { InitState = false; return; }
            if (!XML.InitStringAttr<string>(Config, TimeStampFormatAttr, out _timeStampFormat)) { InitState = false; return; }
            if (!XML.InitStringAttr<string>(Config, FileNameFormatAttr, out _fileNameFormat)) { InitState = false; return; }
            if (!XML.InitStringAttr<string>(Config, DateTitleAttr, out _dateTitle)) { InitState = false; return; }
            if (!XML.InitStringAttr<int>(Config, MaxRowsAttr, out _maxRows)) { InitState = false; return; }
            if (_maxRows <= 0) { _maxRows = Int32.MaxValue; }
            if ((!XML.InitStringAttr<int>(Config, NumberOfRowsPerRecordAttr, out _numberOfRowsPerRecord)) || (_numberOfRowsPerRecord <= 0)) { InitState = false; return; }
            MatrixListInit();
        }

        /// <summary>
        /// Start to Run the Recorder
        /// </summary>
        public override void Run() {
            base.Run();
            if (InitState != true) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.AttributeNotInit + RecorderName + ":" + Lib.Array.Array.ListToString(ErrorAttr, ErrorInfoSplitChar)); return; }
            Global.Info.LogRecorder.Log(LogLevelEnum.Warn, Lib.Properties.Resources.LogRecorderConnected + ":" + RecorderName);
            StartRecorder();
        }

        /// <summary>
        /// Read data from database
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public MatrixArray<string> ReadCache(DateTime timeStamp) {
            //IEnumerable<string> messages = DateContainer.ReadAllLines(timeStamp);
            //if (messages == null) { return null; }
            //List<string> messageList = messages.ToList<string>();
            //MatrixArray<string> result = new MatrixArray<string>(TitleConstrut().Split(CSVSplit));
            //int matchedIndex = FindStartIndex(timeStamp, messageList);
            //if (matchedIndex == -1) { return null; }
            //for (int i = matchedIndex; i < messageList.Count; i++) {
            //    result.AddRow(messageList[i].Split(CSVSplit));
            //}
            //return result;
            return null;
        }

        /// <summary>
        /// Find Match Index by timeStamp
        /// </summary>
        /// <returns></returns>
        private int FindStartIndex(DateTime timeStamp, List<string> messageList) {
            for (int i = 1; i < messageList.Count - 1; i++) {
                DateTime currentTimeStamp, nextTimeStamp;
                if (!DateTime.TryParseExact(messageList[i].Split(CSVSplit)[0], _timeStampFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out currentTimeStamp)) { continue; }
                if (currentTimeStamp.CompareTo(timeStamp) > 0) { continue; }
                if (!DateTime.TryParseExact(messageList[i + 1].Split(CSVSplit)[0], _timeStampFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out nextTimeStamp)) { continue; }
                if (nextTimeStamp.CompareTo(timeStamp) > 0) { return i; }
            }
            return -1;
        }

        /// <summary>
        /// Read data line from database by timeStamp
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public string[] Read(DateTime timeStamp) { return null; }

        /// <summary>
        /// Read history data from matrix recorder
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public MatrixArray<string> Read(DateTime startTime, DateTime endTime, object amount = null, string[] columns = null) { return null; }

        /// <summary>
        /// Matrix List Init
        /// </summary>
        private void MatrixListInit() {
            MatrixList = new Dictionary<string, IRecorderData>();
            foreach (var item in DataList) {
                string title;
                if (!XML.InitStringAttr<string>(item.Config, TitleAttr, out title)) { continue; }
                if (string.IsNullOrEmpty(title)) { continue; }
                MatrixList.Add(title, item);
            }
        }

        /// <summary>
        /// record data
        /// </summary>
        private void Record() {
            try {
                DateTime timeStamp = DateTime.Now;
                bool anotherDate = (!timeStamp.ToString(DateCacheFormat).Equals(_dateCache.ToString(DateCacheFormat)) || (timeStamp.CompareTo(_lastUpdateTime) < 0) || (_totalRecordedCount >= _maxRows));
                if ((!anotherDate) && (!CheckSize(timeStamp))) { return; }
                string[] message = CreateMessageLines();
                DateLayer.Append(_dateCache);
                DirectoryInfo directory = DateLayer.Navigate(_dateCache);
                string fileName = _dateCache.ToString(_fileNameFormat) + _extention;
                FileInfo file = new FileInfo(directory.FullName + "/" + fileName);
                //if (!file.Exists) { file.Create(); }
                file.Refresh();
                File.AppendAllLines(file.FullName, message);
                ClearMessageBox();
                if (anotherDate) { UpdateDateCache(timeStamp); }
                if (_totalRecordedCount == 0) { _messageBox.Enqueue(TitleConstrut()); }
                EnqueueMessageBox(timeStamp);
            }
            catch (Exception e) {

            }
        }

        /// <summary>
        /// CreateMessageLines
        /// </summary>
        /// <returns></returns>
        private string[] CreateMessageLines() {
            string[] result = new string[_messageBox.Count];
            _messageBox.CopyTo(result, 0);
            return result;
        }

        /// <summary>
        /// CheckSize
        /// </summary>
        private bool CheckSize(DateTime timeStamp) {
            if ((_messageBox.Count < _maxRows) && (_messageBox.Count < _numberOfRowsPerRecord)) { EnqueueMessageBox(timeStamp); return false; }
            while (_messageBox.Count > _numberOfRowsPerRecord) { _messageBox.Dequeue(); }
            return true;
        }

        /// <summary>
        /// Enqueue MessageBox
        /// </summary>
        /// <param name="message"></param>
        private void EnqueueMessageBox(DateTime timeStamp) {
            string message = MessageConstruct(timeStamp);
            _lastUpdateTime = timeStamp;
            _messageBox.Enqueue(message);
            _totalRecordedCount++;
        }

        /// <summary>
        /// UpdateDateCache
        /// </summary>
        /// <param name="timeStampStr"></param>
        private void UpdateDateCache(DateTime timeStamp) {
            _dateCache = timeStamp;
            _totalRecordedCount = 0;
        }

        /// <summary>
        /// Start Matrix Recorder
        /// </summary>
        private void StartRecorder() {
            DateTime initTimeStamp = DateTime.Now;
            _dateCache = initTimeStamp;
            _lastUpdateTime = initTimeStamp;
            ClearMessageBox();
            Lib.Timer.Timer.SetInterval((object o, ElapsedEventArgs e) => {
                lock (_lock) {
                    Record();
                }
            }, ref _timer, Interval);
        }

        /// <summary>
        /// clear message box
        /// </summary>
        private void ClearMessageBox() {
            _messageBox.Clear();
        }

        /// <summary>
        /// Construct a message
        /// </summary>
        private string MessageConstruct(DateTime timeStamp) {
            StringBuilder result = new StringBuilder();
            result.Append(timeStamp.ToString(_timeStampFormat));
            foreach (var item in MatrixList) {
                result.Append(CSVSplit);
                if (item.Value.Value == null) { continue; }
                object dataValue = (item.Value.Quality == QualityEnum.Good) ? item.Value.Value : string.Empty;
                result.Append(dataValue);
            }
            return result.ToString();
        }

        /// <summary>
        /// Construt a Title for message
        /// </summary>
        /// <returns></returns>
        private string TitleConstrut() {
            StringBuilder result = new StringBuilder();
            result.Append(_dateTitle);
            foreach (var item in MatrixList) {
                result.Append(CSVSplit);
                result.Append(item.Key);
            }
            return result.ToString();
        }

        #endregion Function

    }
}
