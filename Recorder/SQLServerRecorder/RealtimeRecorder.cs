///Copyright(c) 2015,HIT All rights reserved.
///Summary:Record realtime recorder
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      

using Irlovan.Database;
using Irlovan.DataQuality;
using Irlovan.Lib.Convertor;
using Irlovan.Lib.SQLServer;
using Irlovan.Log;
using Irlovan.Message;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Xml.Linq;

namespace Irlovan.Recorder.SQLServerRecorder
{
    public class RealtimeRecorder : SQLRecorder, IRealtimeDataRecorder
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="source"></param>
        /// <param name="config"></param>
        public RealtimeRecorder(Catalog source, XElement config)
            : base(source, config) { }

        #endregion Structure

        #region Field

        private const string ColumnIDPara = "ID";
        private const string ColumnIDType = "System.Int32";
        private const string ColumnTimeStamp = "TimeStamp";
        private const string ColumnTimeStampType = "System.DateTime";
        private const string ColumnValue = "Value";
        private const string ColumnValueType = "System.Object";

        private Timer _timer;
        private Dictionary<string, object> _dataValueCache = new Dictionary<string, object>();

        #endregion Field

        #region Function

        /// <summary>
        /// Init properties for recorder
        /// </summary>
        public override void Init() {
            base.Init();
        }

        /// <summary>
        /// run recorder
        /// </summary>
        public override void Run() {
            base.Run();
            if (!InitState) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.AttributeNotInit + RecorderName + ":" + Lib.Array.Array.ListToString(ErrorAttr, ErrorInfoSplitChar)); return; }
            ModeSelect();
        }

        /// <summary>
        /// Get data from database
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="amount"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<IIndustryDataMessage> Read(DateTime startTime, DateTime endTime, object amount, object name = null, bool isDesc = true) {
            if (State != ConnectionState.Open) { Global.Info.LogRecorder.Log(LogLevelEnum.Warn, Lib.Properties.Resources.DatabaseConnectFailed + RecorderName); return null; }
            try {
                List<int> queryIDList = new List<int>();
                if (name != null) { queryIDList = GetQueryIDList(name.ToString()); }
                using (SqlConnection connection = new SqlConnection(ConnectionString)) {
                    SqlCommand command = InitReadCommand(connection, startTime, endTime, amount, queryIDList, name, isDesc);
                    command.Connection.Open();
                    return ParseDataFromSQL(command);
                }
            }
            catch (Exception e) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, e.ToString());
                return null;
            }
        }

        /// <summary>
        /// record datalist to database
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public bool Record(IEnumerable<IRecorderData> dataList, DateTime timeStamp) {
            if (State != ConnectionState.Open) { return false; }
            try {
                using (var bulkCopy = new SqlBulkCopy(ConnectionString)) {
                    List<DataRow> dataRows = new List<DataRow>();
                    DataTable dataTable = SQLServer.CreateDataTable(this.TableName, new Dictionary<string, string>() { { ColumnIDPara, ColumnIDType }, { ColumnTimeStamp, ColumnTimeStampType }, { ColumnValue, ColumnValueType } }, ColumnIDPara);
                    foreach (var item in dataList) {
                        if (item.Value == null) { continue; }
                        DataRow row = dataTable.NewRow();
                        row[ColumnIDPara] = item.ID;
                        row[ColumnTimeStamp] = timeStamp;
                        row[ColumnValue] = (item.Quality == QualityEnum.Good) ? item.Value : null;
                        dataRows.Add(row);
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
            Lib.Timer.Timer.DisposeTimer(_timer);
        }

        /// <summary>
        /// Get Query ID List
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<int> GetQueryIDList(string name) {
            List<int> result = new List<int>();
            Regex expression = new Regex("(" + name + ")");
            foreach (var item in DataList) {
                if (expression.Matches(item.Data.FullName).Count == 0) { continue; }
                result.Add(item.ID);
            }
            return result;
        }

        /// <summary>
        /// Init SQLReadCommand
        /// </summary>
        private SqlCommand InitReadCommand(SqlConnection connection, DateTime startTime, DateTime endTime, object amount, List<int> queryIDList, object name = null, bool isDesc = true) {
            StringBuilder commandString = new StringBuilder();
            commandString.Append(SQLServer.SelectColumn(new string[] { ColumnIDPara, ColumnTimeStamp, ColumnValue }, Database, TableName, amount));
            commandString.Append(SQLServer.Where);
            commandString.Append(SQLServer.DateAfter(ColumnTimeStamp, startTime.ToString(TimeFormat)));
            commandString.Append(SQLServer.And);
            commandString.Append(SQLServer.DateBefore(ColumnTimeStamp, endTime.ToString(TimeFormat)));
            commandString.Append(((name == null) || (queryIDList.Count == 0)) ? string.Empty : (SQLServer.And + ColumnIDPara + SQLServer.In + IDString(queryIDList)));
            commandString.Append(SQLServer.SpaceChar);
            commandString.Append(SQLServer.OrderBy);
            commandString.Append(ColumnTimeStamp);
            if (isDesc) { commandString.Append(SQLServer.DESC); }
            return new SqlCommand(commandString.ToString(), connection);
        }

        /// <summary>
        /// Select SQLServer RecordMode
        /// </summary>
        /// <param name="config"></param>
        private void ModeSelect() {
            HybridMode();
            IntervalMode();
        }

        /// <summary>
        /// get data string which read from database
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private List<IIndustryDataMessage> ParseDataFromSQL(SqlCommand command) {
            SqlDataReader reader = command.ExecuteReader();
            List<IIndustryDataMessage> result = new List<IIndustryDataMessage>();
            while (reader.Read()) {
                int id;
                DateTime timeStamp;
                string value;
                if (!Convertor.ConvertType<int>(reader[ColumnIDPara], out id)) { continue; }
                if (!Convertor.ConvertType<DateTime>(reader[ColumnTimeStamp], out timeStamp)) { continue; }
                if (!Convertor.ConvertType<string>(reader[ColumnValue], out value)) { continue; }
                if (!IDDictionary.ContainsKey(id)) { continue; }
                IRecorderData data = IDDictionary[id];
                IIndustryDataMessage message = new IndustryDataMessage(data.Name, value, data.DataType, timeStamp, data.Description, QualityEnum.Good);
                result.Add(message);
            }
            reader.Close();
            return result;
        }

        /// <summary>
        /// record datas by interval
        /// </summary>
        /// <param name="config"></param>
        private void IntervalMode() {
            if (Interval >= 0) { return; }
            int interval = Math.Abs(Interval);
            Irlovan.Lib.Timer.Timer.SetInterval((object o, ElapsedEventArgs e) => {
                RecordHistoryInterval();
            }, ref _timer, interval);
        }

        /// <summary>
        /// record datas whose value changed by interval
        /// </summary>
        /// <param name="config"></param>
        private void HybridMode() {
            if (Interval <= 0) { return; }
            FillDataCache();
            Irlovan.Lib.Timer.Timer.SetInterval((object o, ElapsedEventArgs e) => {
                RecordHistoryHybrid();
            }, ref _timer, Interval);
        }

        /// <summary>
        /// Record History Hybridly
        /// </summary>
        private void RecordHistoryHybrid() {
            List<IRecorderData> result = new List<IRecorderData>();
            foreach (var item in NameDictionary) {
                if (item.Value.Value.Equals(_dataValueCache[item.Key])) { continue; }
                result.Add(item.Value);
                _dataValueCache[item.Key] = item.Value.Value;
            }
            if (result.Count == 0) { return; }
            Record(result, DateTime.Now);
        }

        /// <summary>
        /// Record History Intervally
        /// </summary>
        private void RecordHistoryInterval() {
            Record(DataList, DateTime.Now);
        }

        /// <summary>
        /// FillDataCache
        /// </summary>
        private void FillDataCache() {
            foreach (var item in NameDictionary) {
                _dataValueCache.Add(item.Key, item.Value.Value);
            }
        }

        #endregion Function

    }
}
