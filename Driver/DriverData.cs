///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Driver data
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using Irlovan.Database;
using Irlovan.DataQuality;
using System;

namespace Irlovan.Driver
{
    public class DriverData : IDriverData
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="data"></param>
        /// <param name="groupName"></param>
        /// <param name="readOnly"></param>
        /// <param name="address"></param>
        /// <param name="expression"></param>
        public DriverData(IIndustryData data, string groupName, bool readOnly, string address, Irlovan.Expression.Expression expression = null) {
            Data = data;
            Expression = expression;
            Address = address;
            GroupName = groupName;
            Readonly = readOnly;
            EventInit();
        }

        #endregion Structure

        #region Property


        /// <summary>
        /// Industry Data Source
        /// </summary>
        public IIndustryData Data { get; private set; }

        /// <summary>
        /// Expression of the Driver Data
        /// </summary>
        public Irlovan.Expression.Expression Expression { get; private set; }

        /// <summary>
        /// Address to the client
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        /// Group Name In the driver
        /// </summary>
        public string GroupName { get; private set; }

        /// <summary>
        /// data set permittion to the client 
        /// </summary>
        public bool Readonly { get; private set; }

        /// <summary>
        /// Quality of the driver data
        /// </summary>
        public QualityEnum Quality { get; private set; }

        #endregion Property

        #region Event

        public event WriteDriverDataHandler WriteDriverData;

        #endregion Event

        #region Function

        /// <summary>
        /// Get expressioned value of driver data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public object Value(object data) {
            return (Expression == null) ? data : Expression.Eval(new object[] { data });
        }

        /// <summary>
        /// read value for data
        /// </summary>
        /// <param name="data"></param>
        public void ReadValue(object data, QualityEnum quality = QualityEnum.Good) {
            Data.ReadValue(Value(data));
            SetQuality(quality);
        }

        /// <summary>
        /// set the quality of the driver data
        /// </summary>
        /// <param name="data"></param>
        public void SetQuality(QualityEnum quality) {
            Data.Quality = quality;
        }

        /// <summary>
        /// write value to data
        /// </summary>
        /// <param name="data"></param>
        public void WriteValue(object data) {
            Data.WriteValue(data);
        }

        /// <summary>
        /// Init Event for Write data
        /// </summary>
        private void EventInit() {
            if (Readonly) { return; }
            Data.WriteData += WriteDataHandler;
        }

        /// <summary>
        /// handler for write data event
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeStamp"></param>
        private void WriteDataHandler(object value, DateTime timeStamp) {
            if (WriteDriverData != null) {
                WriteDriverData(GroupName, Data.FullName, Address, value, timeStamp);
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() {
            Data.WriteData -= WriteDataHandler;
        }

        #endregion Function

    }

    /// <summary>
    /// WriteDriverDataHandler
    /// </summary>
    /// <param name="groupName"></param>
    /// <param name="name"></param>
    /// <param name="addr"></param>
    /// <param name="value"></param>
    /// <param name="timeStamp"></param>
    public delegate void WriteDriverDataHandler(string groupName, string name, string addr, object value, DateTime timeStamp);

}
