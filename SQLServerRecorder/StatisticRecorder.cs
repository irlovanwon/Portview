///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Record statistic data
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      

using Irlovan.Database;
using Irlovan.DataQuality;
using Irlovan.Lib.Array;
using Irlovan.Lib.Convertor;
using Irlovan.Lib.SQLServer;
using Irlovan.Lib.Symbol;
using Irlovan.Lib.XML;
using Irlovan.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Timers;
using System.Xml.Linq;

namespace Irlovan.Recorder.SQLServerRecorder
{
    public class StatisticRecorder : SQLRecorder, IStatisticRecorder
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="source"></param>
        /// <param name="config"></param>
        public StatisticRecorder(Catalog source, XElement config)
            : base(source, config) { }

        #endregion Structure

        #region Field

        private const string GroupTagPara = "Group";
        private const char GroupSplitChar = ',';
        private const string DataTagPara = "Data";
        private const string ColumnTitlePara = "Title";
        private const string ColumnTypePara = "Type";
        private const string DateNamePara = "DateName";
        private const string DeviceCountPara = "DeviceCount";
        private string _datetimeColumn;
        private int _count;
        private Timer _timer;
        private Dictionary<string, Type> _title = new Dictionary<string, Type>();
        private MatrixArray<IIndustryData> _dataList;

        #endregion Field

        #region Function

        /// <summary>
        /// Init properties for recorder
        /// </summary>
        public override void Init() {
            base.Init();
            DataListInit();
        }

        /// <summary>
        /// run recorder
        /// </summary>
        public override void Run() {
            base.Run();
            if (!InitState) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.AttributeNotInit + RecorderName + ":" + Lib.Array.Array.ListToString(ErrorAttr, ErrorInfoSplitChar)); return; }
            IntervalMode();
        }

        /// <summary>
        /// Get data from database
        /// </summary>
        /// <returns></returns>
        public MatrixArray<string> Read(DateTime timeStampobject) {
            if (State != ConnectionState.Open) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.DatabaseConnectFailed + RecorderName); return null; }
            return ReadTime(timeStampobject);
        }

        /// <summary>
        /// Get data from database
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public MatrixArray<string> Read(DateTime startTime, DateTime endTime, object amount) {
            return Read(startTime);
        }

        /// <summary>
        /// dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            Lib.Timer.Timer.DisposeTimer(_timer);
        }

        /// <summary>
        /// Get data from database
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="amount"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private MatrixArray<string> ReadTime(DateTime timeStamp) {
            try {
                using (SqlConnection connection = new SqlConnection(ConnectionString)) {
                    SqlCommand command = InitReadCommand(connection, timeStamp);
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
        /// Init SQLReadCommand
        /// </summary>
        private SqlCommand InitReadCommand(SqlConnection connection, DateTime timeStamp) {
            StringBuilder commandString = new StringBuilder();
            string[] title = Lib.Array.Array.Combin<string>(new string[] { _datetimeColumn }, _dataList.Title);
            commandString.Append(SQLServer.SelectColumn(title, Database, TableName, _count));
            commandString.Append(SQLServer.Where);
            commandString.Append(SQLServer.DateBefore(_datetimeColumn, timeStamp.ToString(TimeFormat)));
            commandString.Append(SQLServer.OrderBy);
            commandString.Append(_datetimeColumn);
            commandString.Append(SQLServer.DESC);
            return new SqlCommand(commandString.ToString(), connection);
        }


        /// <summary>
        /// get data string which read from database
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private MatrixArray<string> ParseDataFromSQL(SqlCommand command) {
            SqlDataReader reader = command.ExecuteReader();
            string[] title = Lib.Array.Array.Combin<string>(new string[] { _datetimeColumn }, _dataList.Title);
            MatrixArray<string> message = new MatrixArray<string>(title);
            while (reader.Read()) {
                ReadRowInfo(reader, message);
            }
            reader.Close();
            return message;
        }

        /// <summary>
        /// Fill Column from SQL
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="message"></param>
        private void ReadRowInfo(SqlDataReader reader, MatrixArray<string> message) {
            string[] row = new string[message.Title.Length];
            for (int i = 0; i < row.Length; i++) {
                string data;
                if (!Convertor.ConvertType<string>(reader[message.Title[i]], out data)) { return; }
                row[i] = data;
            }
            message.AddRow(row);
        }

        /// <summary>
        /// datalist init
        /// </summary>
        /// <param name="source"></param>
        /// <param name="config"></param>
        private void DataListInit() {
            if (!XML.InitStringAttr<string>(Config, DateNamePara, out _datetimeColumn)) { ErrorAttr.Add(DateNamePara); InitState = false; return; }
            if (!XML.InitStringAttr<int>(Config, DeviceCountPara, out _count)) { ErrorAttr.Add(DeviceCountPara); InitState = false; return; }
            if (Config.Element(DataTagPara) == null) { return; }
            IEnumerable<XElement> configList = Config.Elements(DataTagPara);
            try {
                _dataList = CreateDataList(configList);
            }
            catch (Exception) {
                throw;
            }
            if (_dataList == null) { InitState = false; }
        }

        /// <summary>
        ///  CreateDataList Parsing from XML config
        /// </summary>
        /// <param name="configList"></param>
        /// <returns></returns>
        private MatrixArray<IIndustryData> CreateDataList(IEnumerable<XElement> configList) {
            List<string> title;
            List<IIndustryData[]> columnDataList;
            List<IIndustryData[]> rowDataList;
            ParseDataListFromXML(configList, out title, out columnDataList);
            rowDataList = CreateRowDataList(columnDataList);
            if (title.Count == 0) { return null; }
            MatrixArray<IIndustryData> result = new MatrixArray<IIndustryData>(title.ToArray());
            foreach (var item in rowDataList) {
                result.AddRow(item);
            }
            if (result.Rows.Count == 0) { return null; }
            return result;
        }

        /// <summary>
        /// Create Row DataList
        /// </summary>
        private List<IIndustryData[]> CreateRowDataList(List<IIndustryData[]> columnDataList) {
            List<IIndustryData[]> rowDataList = new List<IIndustryData[]>();
            for (int i = 0; i < _count; i++) {
                IIndustryData[] row = CreateRow(columnDataList, i);
                rowDataList.Add(row);
            }
            return rowDataList;
        }

        /// <summary>
        /// CreateRow
        /// </summary>
        private IIndustryData[] CreateRow(List<IIndustryData[]> columnDataList, int rowIndex) {
            IIndustryData[] result = new IIndustryData[columnDataList.Count];
            for (int i = 0; i < columnDataList.Count; i++) {
                result[i] = columnDataList[i][rowIndex];
            }
            return result;
        }

        /// <summary>
        /// Parse title from xml config
        /// </summary>
        /// <returns></returns>
        private void ParseDataListFromXML(IEnumerable<XElement> configList, out List<string> title, out List<IIndustryData[]> columnDataList) {
            title = new List<string>();
            columnDataList = new List<IIndustryData[]>();
            foreach (var item in configList) {
                string columnName;
                if (!XML.InitStringAttr<string>(item, ColumnTitlePara, out columnName)) { continue; }
                string columnTypeStr;
                if (!XML.InitStringAttr<string>(item, ColumnTypePara, out columnTypeStr)) { continue; }
                Type columnType = Type.GetType(columnTypeStr);
                if (columnType == null) { continue; }
                string columnGroup;
                if (!XML.InitStringAttr<string>(item, GroupTagPara, out columnGroup)) { continue; }
                string[] dataNameList = columnGroup.Split(GroupSplitChar);
                if (dataNameList.Length != _count) { continue; }
                List<IIndustryData> columnData = CreateColumnDataList(dataNameList, columnType);
                if ((columnData == null) || (columnData.Count != _count)) { continue; }
                _title.Add(columnName, columnType);
                title.Add(columnName);
                columnDataList.Add(columnData.ToArray());
            }
        }

        /// <summary>
        /// Create ColumnData List
        /// </summary>
        private List<IIndustryData> CreateColumnDataList(string[] dataNameList, Type columnType) {
            List<IIndustryData> columnData = new List<IIndustryData>();
            foreach (var dataName in dataNameList) {
                IIndustryData data = Source.AcquireIndustryData(dataName);
                if ((data == null) || (data.DataType != columnType)) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.StatisticRecorderError + RecorderName + Symbol.NewLine_Symbol + dataName); return null; }
                columnData.Add(data);
            }
            return columnData;
        }

        /// <summary>
        /// record datas by interval
        /// </summary>
        /// <param name="config"></param>
        private void IntervalMode() {
            Irlovan.Lib.Timer.Timer.SetInterval((object o, ElapsedEventArgs e) => {
                Record(DateTime.Now);
            }, ref _timer, Interval);
            Record(DateTime.Now);
        }

        /// <summary>
        /// record datalist to database
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        private bool Record(DateTime timeStamp) {
            if (State != ConnectionState.Open) { return false; }
            try {
                using (var bulkCopy = new SqlBulkCopy(ConnectionString)) {
                    List<DataRow> dataRows = new List<DataRow>();
                    DataTable dataTable = CreateDataTable();
                    foreach (var item in _dataList.Rows) {
                        dataRows.Add(CreateDataRow(item, dataTable, timeStamp));
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
        /// create data table
        /// </summary>
        private DataTable CreateDataTable() {
            DataTable dataTable = new DataTable(TableName);
            AddColumn(dataTable, _datetimeColumn, typeof(DateTime));
            foreach (var item in _title) {
                AddColumn(dataTable, item.Key, item.Value);
            }
            return dataTable;
        }

        /// <summary>
        /// CreateDataRow
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="dataTable"></param>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        private DataRow CreateDataRow(IIndustryData[] dataList, DataTable dataTable, DateTime timeStamp) {
            DataRow result = dataTable.NewRow();
            object[] array = new object[_dataList.Title.Length + 1];
            array[0] = timeStamp;
            for (int i = 1; i < array.Length; i++) {
                IIndustryData data = dataList[i - 1];
                array[i] = (data.Quality != QualityEnum.Good) ? null : data.Value;
            }
            result.ItemArray = array;
            return result;
        }

        /// <summary>
        /// Add column to datatable
        /// </summary>
        private void AddColumn(DataTable dataTable, string name, Type type) {
            DataColumn column = new DataColumn();
            column.DataType = type;
            column.ColumnName = name;
            dataTable.Columns.Add(column);
        }

        #endregion Function

    }

}
