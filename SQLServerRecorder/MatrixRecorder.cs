///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Record matrix data
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using Irlovan.Database;
using Irlovan.DataQuality;
using Irlovan.Lib.Array;
using Irlovan.Lib.Convertor;
using Irlovan.Lib.SQLServer;
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
    public class MatrixRecorder : SQLRecorder, IMatrixRecorder
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="source"></param>
        /// <param name="config"></param>
        public MatrixRecorder(Catalog source, XElement config)
            : base(source, config) { }

        #endregion Structure

        #region Field

        private MatrixArray<IIndustryData> _matrixDataList;
        private string _datetimeColumn;
        private Timer _timer;
        private const string DateNameAttr = "DateName";
        private const string TitleAttr = "Title";

        #endregion Field

        #region Property


        #endregion Property

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
        public string[] Read(DateTime timeStampobject) {
            return null;
        }

        /// <summary>
        /// Get data from database
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public MatrixArray<string> Read(DateTime startTime, DateTime endTime, object amount = null, string[] columns = null) {
            if (State != ConnectionState.Open) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.DatabaseConnectFailed + RecorderName); return null; }
            try {
                using (SqlConnection connection = new SqlConnection(ConnectionString)) {
                    SqlCommand command = InitReadCommand(connection, startTime, endTime, amount, columns);
                    command.Connection.Open();
                    return ParseDataFromSQL(command, columns);
                }
            }
            catch (Exception e) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, e.ToString());
                return null;
            }
        }

        /// <summary>
        /// dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            Lib.Timer.Timer.DisposeTimer(_timer);
        }

        /// <summary>
        /// Init SQLReadCommand
        /// </summary>
        private SqlCommand InitReadCommand(SqlConnection connection, DateTime startTime, DateTime endTime, object amount, string[] columns) {
            StringBuilder commandString = new StringBuilder();
            string[] title = Lib.Array.Array.Combin<string>(new string[] { _datetimeColumn }, (columns == null) ? _matrixDataList.Title : columns);
            commandString.Append(SQLServer.SelectColumn(title, Database, TableName, amount));
            commandString.Append(SQLServer.Where);
            commandString.Append(SQLServer.DateAfter(_datetimeColumn, startTime.ToString(TimeFormat)));
            commandString.Append(SQLServer.And);
            commandString.Append(SQLServer.DateBefore(_datetimeColumn, endTime.ToString(TimeFormat)));
            return new SqlCommand(commandString.ToString(), connection);
        }


        /// <summary>
        /// get data string which read from database
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private MatrixArray<string> ParseDataFromSQL(SqlCommand command, string[] columns) {
            SqlDataReader reader = command.ExecuteReader();
            string[] title = Lib.Array.Array.Combin<string>(new string[] { _datetimeColumn }, (columns == null) ? _matrixDataList.Title : columns);
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
            DateTime timeStamp;
            if (!Convertor.ConvertType<DateTime>(reader[_datetimeColumn], out timeStamp)) { return; }
            row[0] = timeStamp.ToString(TimeFormat);
            for (int i = 1; i < row.Length; i++) {
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
            if (!XML.InitStringAttr<string>(Config, DateNameAttr, out _datetimeColumn)) { ErrorAttr.Add(DateNameAttr); InitState = false; return; }
            List<string> titleList = new List<string>();
            List<IIndustryData> rowDataList = new List<IIndustryData>();
            foreach (var item in DataList) {
                string title;
                if (!XML.InitStringAttr<string>(item.Config, TitleAttr, out title)) { ErrorAttr.Add(DateNameAttr); InitState = false; continue; }
                titleList.Add(title);
                rowDataList.Add(item.Data);
            }
            if (rowDataList.Count == 0) { InitState = false; return; }
            _matrixDataList = new MatrixArray<IIndustryData>(titleList.ToArray());
            _matrixDataList.AddRow(rowDataList.ToArray());
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
                    foreach (var item in _matrixDataList.Rows) {
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
            for (int i = 0; i < _matrixDataList.Title.Length; i++) {
                AddColumn(dataTable, _matrixDataList.Title[i], _matrixDataList.Rows[0][i].DataType);
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
            object[] array = new object[_matrixDataList.Title.Length + 1];
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
