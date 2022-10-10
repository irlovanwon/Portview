///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:SQLRecorder base class
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using Irlovan.Database;
using Irlovan.Lib.SQLServer;
using Irlovan.Lib.XML;
using Irlovan.Log;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Timers;
using System.Xml.Linq;

namespace Irlovan.Recorder.SQLServerRecorder
{
    public class SQLRecorder : Recorder, ISQLRecorder
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="source"></param>
        /// <param name="config"></param>
        public SQLRecorder(Catalog source, XElement config)
            : base(source, config) { }

        #endregion Structure

        #region Field

        public string TimeFormat = "MM/dd/yyyy HH:mm:ss.fff";
        public const string UserIDPara = "UserID";
        public const string PasswordPara = "Password";
        public const string DatabasePara = "DatabaseName";
        public const string DataSourcePara = "DataSource";
        public const string TableNamePara = "TableName";

        /**SQLSERVER PARA**/
        private string _dataSource;
        private string _database;
        private string _userID;
        private string _password;
        private string _tableName;

        // Timer for communication auto detection mode
        public Timer _autoDetectingTimer;

        #endregion Field

        #region Property

        /// <summary>
        /// userID for SQLServer
        /// </summary>
        public string UserID {
            get { return _userID; }
            set {
                if (value != _userID) {
                    _userID = value;
                }
            }
        }

        /// <summary>
        /// password for SQLServer
        /// </summary>
        public string Password {
            get { return _password; }
            set {
                if (value != _password) {
                    _password = value;
                }
            }
        }

        /// <summary>
        /// database string for SQLServer
        /// </summary>
        public string Database {
            get { return _database; }
            set {
                if (value != _database) {
                    _database = value;
                }
            }
        }

        /// <summary>
        /// datasource string for SQLServer
        /// </summary>
        public string DataSource {
            get { return _dataSource; }
            set {
                if (value != _dataSource) {
                    _dataSource = value;
                }
            }
        }

        /// <summary>
        /// table name for SQLServer
        /// </summary>
        public string TableName {
            get { return _tableName; }
            set {
                if (value != _tableName) {
                    _tableName = value;
                }
            }
        }

        /// <summary>
        /// Connection State 
        /// </summary>
        public ConnectionState State { get; set; }

        /// <summary>
        /// connection string for SQLServer
        /// </summary>
        public string ConnectionString { get; set; }

        #endregion Property

        #region Delegate

        private delegate void ReconnectHandler();

        #endregion Delegate

        #region Function

        /// <summary>
        /// dispose for recorder
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            Lib.Timer.Timer.DisposeTimer(_autoDetectingTimer);
        }

        /// <summary>
        /// Start to Run the Recorder
        /// </summary>
        public override void Run() {
            base.Run();
            Connect();
        }

        /// <summary>
        /// init all properties for recorder
        /// </summary>
        public override void Init() {
            base.Init();
            State = ConnectionState.Closed;
            if (!XML.InitStringAttr<string>(Config, SQLRecorder.DataSourcePara, out _dataSource)) { ErrorAttr.Add(SQLRecorder.DataSourcePara); InitState = false; }
            if (!XML.InitStringAttr<string>(Config, SQLRecorder.DatabasePara, out _database)) { ErrorAttr.Add(SQLRecorder.DatabasePara); InitState = false; }
            if (!XML.InitStringAttr<string>(Config, SQLRecorder.UserIDPara, out _userID)) { ErrorAttr.Add(SQLRecorder.UserIDPara); InitState = false; }
            if (!XML.InitStringAttr<string>(Config, SQLRecorder.PasswordPara, out _password)) { ErrorAttr.Add(SQLRecorder.PasswordPara); InitState = false; }
            if (!XML.InitStringAttr<string>(Config, SQLRecorder.TableNamePara, out _tableName)) { ErrorAttr.Add(SQLRecorder.TableNamePara); InitState = false; }
            ConnectionString = SQLServer.CreateConnectString(DataSource, Database, UserID, Password);
        }

        /// <summary>
        /// id list to string for SQL querying
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        internal string IDString(IEnumerable<int> dataList) {
            StringBuilder result = new StringBuilder();
            result.Append(SQLServer.ArrayL);
            foreach (var item in dataList) {
                result.Append(item);
                result.Append(SQLServer.ArraySplitChar);
            }
            result.Remove(result.Length - 1, 1);
            result.Append(SQLServer.ArrayR);
            return result.ToString();
        }

        /// <summary>
        /// Connect to local sqlserver database
        /// </summary>
        private void Connect() {
            try {
                using (SqlConnection connection = new SqlConnection(ConnectionString)) {
                    connection.Open();
                    State = connection.State;
                };
                Global.Info.LogRecorder.Log(LogLevelEnum.Warn, Lib.Properties.Resources.SQLDatabaseConnected + ":" + RecorderName);
            }
            catch {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.SQLDatabaseConnectedFail + ":" + RecorderName);
                Reconnect();
            }
        }

        /// <summary>
        /// when communication to database init failed,then reconnect
        /// </summary>
        private void Reconnect() {
            //check if reconnection is needed
            if ((State == ConnectionState.Open) && (CommAutoDetectingInterval != 0)) { return; }
            Timer reconnetTimer = new Timer();
            Lib.Timer.Timer.SetTimeout((object o, ElapsedEventArgs e) => {
                Connect();
            }, ref reconnetTimer, CommAutoDetectingInterval);
        }

        #endregion Function

    }
}
