///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Record events
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using Irlovan.Database;
using Irlovan.Lib.Convertor;
using Irlovan.Lib.SQLServer;
using Irlovan.Log;
using Irlovan.Message;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Timers;
using System.Xml.Linq;

namespace Irlovan.Recorder.SQLServerRecorder
{
    public class EventRecorder : SQLRecorder, IEventRecorder
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="source"></param>
        /// <param name="config"></param>
        public EventRecorder(Catalog source, XElement config)
            : base(source, config) { }

        #endregion Structure

        #region Field

        private const string ColumnIDPara = "ID";
        private const string ColumnIDType = "System.Int32";
        private const string ColumnStartTime = "StartTime";
        private const string ColumnStartTimeType = "System.DateTime";
        private const string ColumnEndTime = "EndTime";
        private const string ColumnEndTimeType = "System.DateTime";
        private const string ColumnEventLevel = "EventLevel";
        private const string ColumnEventLevelType = "System.String";
        private Timer _historyEventTimer;
        private Dictionary<string, IEventData> _dataNameDic;
        private Dictionary<int, IEventData> _dataIDDic;
        private Dictionary<string, bool> _dataValueCache;

        #endregion Field

        #region Function

        /// <summary>
        /// Init properties for recorder
        /// </summary>
        public override void Init() {
            base.Init();
            InitDataList();
            _dataValueCache = new Dictionary<string, bool>();
        }

        /// <summary>
        /// run recorder
        /// </summary>
        public override void Run() {
            base.Run();
            if (!InitState) { Irlovan.Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.AttributeNotInit + RecorderName + ":" + Lib.Array.Array.ListToString(ErrorAttr, ErrorInfoSplitChar)); return; }
            HybridMode();
        }

        /// <summary>
        /// get data from sqlserver
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="amount"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<IEventDataMessage> Read(DateTime startTime, DateTime endTime, object amount, object name = null, string[] eventLevel = null, bool isDesc = true) {
            if (State != ConnectionState.Open) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.DatabaseConnectFailed + RecorderName); return null; }
            try {
                List<IEventData> eventList = (name == null) ? new List<IEventData>() : AcquireEventsByName(_dataNameDic.Values, name.ToString());
                using (SqlConnection connection = new SqlConnection(ConnectionString)) {
                    SqlCommand command = InitReadCommand(connection, startTime, endTime, amount, eventList, name, eventLevel, isDesc);
                    command.Connection.Open();
                    return ParseDataFromSQL(command);
                }
            }
            catch (Exception e) {
                Irlovan.Global.Info.LogRecorder.Log(LogLevelEnum.Error, e.ToString());
                return null;
            }
        }

        /// <summary>
        /// AcquireEventsByName
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<IEventData> AcquireEventsByName(IEnumerable<IEventData> dataList, string name) {
            List<IEventData> result = new List<IEventData>();
            foreach (var item in dataList) {
                if (item.FullName.Contains(name)) {
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// record to sqlserver
        /// </summary>
        /// <param name="eventList"></param>
        /// <returns></returns>
        public bool Record(List<IEventDataMessage> eventList) {
            if (State != ConnectionState.Open) { return false; }
            try {
                using (var bulkCopy = new SqlBulkCopy(ConnectionString)) {
                    List<DataRow> dataRows = new List<DataRow>();
                    DataTable dataTable = SQLServer.CreateDataTable(this.TableName, new Dictionary<string, string>() { { ColumnIDPara, ColumnIDType }, { ColumnStartTime, ColumnStartTimeType }, { ColumnEndTime, ColumnEndTimeType }, { ColumnEventLevel, ColumnEventLevelType } }, ColumnIDPara);
                    for (int i = 0; i < eventList.Count; i++) {
                        FillDataRow(eventList[i], dataRows, dataTable);
                    }
                    bulkCopy.BulkCopyTimeout = 0;
                    bulkCopy.DestinationTableName = SQLServer.LabelL + SQLServer.Dbo + SQLServer.LabelR + SQLServer.CatalogSplitChar + SQLServer.LabelL + TableName + SQLServer.LabelR;
                    bulkCopy.WriteToServer(dataRows.ToArray());
                }
            }
            catch (Exception e) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, e.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            Lib.Timer.Timer.DisposeTimer(_historyEventTimer);
        }

        /// <summary>
        /// Init SQLReadCommand
        /// </summary>
        private SqlCommand InitReadCommand(SqlConnection connection, DateTime startTime, DateTime endTime, object amount, List<IEventData> eventList, object name = null, string[] eventLevel = null, bool isDesc = true) {
            StringBuilder commandString = new StringBuilder();
            commandString.Append(SQLServer.SelectColumn(new string[] { ColumnIDPara, ColumnStartTime, ColumnEndTime }, Database, TableName, amount));
            commandString.Append(SQLServer.Where);
            commandString.Append(SQLServer.DateAfter(ColumnStartTime, startTime.ToString(TimeFormat)));
            commandString.Append(SQLServer.And);
            commandString.Append(SQLServer.DateBefore(ColumnEndTime, endTime.ToString(TimeFormat)));
            commandString.Append(((name == null) || (eventList.Count == 0)) ? string.Empty : (SQLServer.And + ColumnIDPara + SQLServer.In + IDString(_dataIDDic.Keys)));
            commandString.Append(SQLServer.SpaceChar);
            commandString.Append((eventLevel == null) ? SQLServer.SpaceChar : (SQLServer.And + ColumnEventLevel + SQLServer.In + EventLevelString(eventLevel)));
            commandString.Append(SQLServer.SpaceChar);
            commandString.Append(SQLServer.OrderBy);
            commandString.Append(ColumnStartTime);
            if (isDesc) { commandString.Append(SQLServer.DESC); }
            return new SqlCommand(commandString.ToString(), connection);
        }

        /// <summary>
        /// Fill data row in SQL
        /// </summary>
        private void FillDataRow(IEventDataMessage eventMessage, List<DataRow> dataRows, DataTable dataTable) {
            if ((eventMessage.StartTime == null) || (eventMessage.EndTime == null)) { return; }
            DataRow row = dataTable.NewRow();
            row[ColumnIDPara] = NameDictionary[eventMessage.Name].ID;
            row[ColumnStartTime] = eventMessage.StartTime;
            row[ColumnEndTime] = eventMessage.EndTime;
            row[ColumnEventLevel] = eventMessage.EventLevel;
            dataRows.Add(row);
        }

        /// <summary>
        /// HybridMode
        /// </summary>
        /// <param name="interval"></param>
        private void HybridMode() {
            //Hybrid Mode
            if (Interval <= 0) { return; }
            FillDataValueCache();
            Lib.Timer.Timer.SetInterval((object o, ElapsedEventArgs e) => {
                RecordHistoryMessage();
            }, ref _historyEventTimer, Interval);
        }

        /// <summary>
        /// Record History Message
        /// </summary>
        private void RecordHistoryMessage() {
            List<IEventDataMessage> historyList = new List<IEventDataMessage>();
            foreach (var item in _dataNameDic) {
                if (item.Value.Value.Equals(_dataValueCache[item.Key])) { continue; }
                List<IDataMessage> messages = item.Value.EventMessageBox.ToDataMessages(1);
                if ((!item.Value.Value) && (messages != null) && (messages.Count == 1)) { historyList.Add((IEventDataMessage)messages[0]); }
                _dataValueCache[item.Key] = item.Value.Value;
            }
            if (historyList.Count == 0) { return; }
            Record(historyList);
        }

        /// <summary>
        /// eventlevel list to string
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        private string EventLevelString(string[] dataList) {
            StringBuilder result = new StringBuilder();
            result.Append(SQLServer.ArrayL);
            foreach (var item in dataList) {
                result.Append(SQLServer.RefChar);
                result.Append(item);
                result.Append(SQLServer.RefChar);
                result.Append(SQLServer.ArraySplitChar);
            }
            result.Remove(result.Length - 1, 1);
            result.Append(SQLServer.ArrayR);
            return result.ToString();
        }

        /// <summary>
        /// Parse data from database
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private List<IEventDataMessage> ParseDataFromSQL(SqlCommand command) {
            SqlDataReader reader = command.ExecuteReader();
            List<IEventDataMessage> result = new List<IEventDataMessage>();
            while (reader.Read()) {
                int id;
                DateTime startTime;
                DateTime endTime;
                if (!Convertor.ConvertType<int>(reader[ColumnIDPara], out id)) { continue; }
                if (!Convertor.ConvertType<DateTime>(reader[ColumnStartTime], out startTime)) { continue; }
                if (!Convertor.ConvertType<DateTime>(reader[ColumnEndTime], out endTime)) { continue; }
                if (!_dataIDDic.ContainsKey(id)) { continue; }
                IEventData eventData = _dataIDDic[id];
                if (eventData == null) { continue; }
                IEventDataMessage message = new EventDataMessage(eventData.FullName, startTime, endTime, eventData.EventLevel, eventData.Description, eventData.Indication);
                result.Add(message);
            }
            reader.Close();
            return result;
        }

        /// <summary>
        /// Fill Data Value Cache
        /// </summary>
        /// <param name="dataList"></param>
        private void FillDataValueCache() {
            List<bool> result = new List<bool>();
            foreach (var item in _dataNameDic) {
                _dataValueCache.Add(item.Key, item.Value.Value);
            }
        }

        /// <summary>
        /// Init Data List
        /// </summary>
        private void InitDataList() {
            _dataNameDic = new Dictionary<string, IEventData>();
            _dataIDDic = new Dictionary<int, IEventData>();
            foreach (var item in DataList) {
                if (!(item.Data is IEventData)) { continue; }
                _dataNameDic.Add(item.Data.FullName, (IEventData)item.Data);
                _dataIDDic.Add(item.ID, (IEventData)item.Data);
            }
        }

        #endregion Function

    }
}



