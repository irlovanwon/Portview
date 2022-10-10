///Copyright(c) 2013,Irlovan All rights reserved.
///Summary:databse
///Author:Irlovan
///Date:2013-03-29
///Description:this is the database for most sort of industry solution
///Modification:2015-01-29

using Irlovan.DataQuality;
using Irlovan.Lib.Convertor;
using Irlovan.Lib.Symbol;
using Irlovan.Lib.XML;
using Irlovan.Log;
using Irlovan.Message;
using System;
using System.Xml.Linq;

namespace Irlovan.Database
{

    public class IndustryData<T> : Database, IIndustryData<T>
    {
        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="name"></param>
        public IndustryData(string name, string description = null)
            : base(name, description) { }

        /// <summary>
        /// IndustryData from xml config
        /// </summary>
        public IndustryData(XElement config)
            : base(config) { }

        #endregion Structure

        #region Field

        internal const string SpeciesName = "IndustryData";
        public const string DataTypePara = "DataType";
        public const string InitValuePara = "InitValue";
        public const string QueueCountPara = "QueueCount";
        private object _lock = new object();
        private object _value;

        #endregion Field

        #region Property

        /// <summary>
        /// Value of a very type
        /// </summary>
        public T Value {
            get { return (T)_value; }
            set { ReadValue(value); }
        }

        /// <summary>
        /// For Interface
        /// </summary>
        object IIndustryData.Value { get { return this.Value; } }

        /// <summary>
        /// Type of a data
        /// </summary>
        public Type DataType { get; set; }

        /// <summary>
        /// TimeStamp when new data value is set
        /// </summary>
        public DateTime TimeStamp { get; private set; }

        /// <summary>
        /// shows the condition of a data
        /// </summary>
        public QualityEnum Quality { get; set; }

        /// <summary>
        /// history data stack count
        /// </summary>
        public int QueueCount { get; set; }

        /// <summary>
        ///  Box to store message of datachange
        /// </summary>
        public DataMessageBox MessageBox { get; private set; }

        #endregion Property

        #region Event

        /// <summary>
        /// Triggered when data value if Changed
        /// </summary>
        public event DataChangeHandler DataChange;

        /// <summary>
        /// Triggered when value(s) of data(s) is set to device
        /// </summary>
        public event WriteDataHandler WriteData;

        #endregion Event

        #region Function

        /// <summary>
        /// Set value of data to device
        /// </summary>
        /// <param name="value"></param>
        public void WriteValue(object value) {
            T result;
            if (!ValueTypeFix(value, out result)) { return; }
            WriteValue(result);
        }

        /// <summary>
        /// Set value of data to device
        /// </summary>
        /// <param name="value"></param>
        public void WriteValue(T value) {
            if (WriteData != null) {
                WriteData(value, DateTime.Now);
            }
        }

        /// <summary>
        /// Set value of data
        /// </summary>
        /// <param name="value"></param>
        public bool ReadValue(object value) {
            T result;
            if (!ValueTypeFix(value, out result)) { return false; }
            ReadValue(result);
            return true;
        }

        /// <summary>
        /// Set value of data
        /// </summary>
        /// <param name="value"></param>
        public bool ReadValue(T value) {
            lock (_lock) {
                TimeStamp = DateTime.Now;
                Quality = QualityEnum.Good;
                if (Equals((T)_value, value)) { return true; }
                if (QueueCount != 0) { MessageBox.Push(new IndustryDataMessage(FullName, Value.ToString(), DataType, TimeStamp, Description.ToString(), Quality)); }
                DataChangeTrigger(value, TimeStamp);
                _value = value;
                return true;
            }
        }

        /// <summary>
        /// Get Database from xml config file
        /// </summary>
        /// <returns></returns>
        public override void ReadXML(XElement element) {
            base.ReadXML(element);
            int queueCount = 0;
            XML.InitStringAttr<int>(element, QueueCountPara, out queueCount);
            if (queueCount < 0) { queueCount = 0; }
            QueueCount = queueCount;
            MessageBox.MaxCount = QueueCount;
            T initValue = default(T);
            XML.InitStringAttr<T>(element, InitValuePara, out initValue);
            ReadValue(initValue);
        }

        /// <summary>
        /// Write Database to xml config file
        /// </summary>
        /// <returns></returns>
        public override XElement WriteXML() {
            XElement result = base.WriteXML();
            result.SetAttributeValue(DataTypePara, DataType.ToString());
            return result;
        }

        /// <summary>
        /// Init
        /// </summary>
        public override void Init() {
            base.Init();
            Species = SpeciesName;
            DataType = typeof(T);
            _value = default(T);
            MessageBox = new DataMessageBox();
        }

        /// <summary>
        /// Convert value type to T
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool ValueTypeFix(object value, out T result) {
            if (!Convertor.ConvertType<T>(value, out result)) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.DataTypeConvertFail + Symbol.Colon_Char + FullName + Symbol.Space_Char + value.ToString() + Symbol.Comma_Char + DataType.ToString()); return false; }
            return true;
        }

        /// <summary>
        /// Trigger Datachange event
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="triggerTime"></param>
        private void DataChangeTrigger(T newValue, DateTime triggerTime) {
            if (DataChange != null) {
                try { DataChange(this, new DataChangeEventArgs(Value, newValue, triggerTime, FullName)); }
                catch { }
            }
        }

        #endregion Function
    }

    /// <summary>
    /// Handler for DataChange event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DataChangeHandler(object sender, DataChangeEventArgs e);

    /// <summary>
    /// Handler for WriteData event
    /// </summary>
    /// <param name="value"></param>
    /// <param name="timeStamp"></param>
    public delegate void WriteDataHandler(object value, DateTime timeStamp);

}

