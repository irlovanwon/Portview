///Copyright(c) 2013,Irlovan All rights reserved.
///Summary：
///Author：Irlovan
///Date：2013-11-16
///Description：
///Modification：2014-06-25


using Newtonsoft.Json.Linq;
using SuperWebSocket;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace Irlovan.Handlers
{
    public class UserManagerHandler : Handler
    {

        #region Structure

        public UserManagerHandler(WebSocketSession session, LocalInterface.LocalInterface local) : base(session, local) { }


        #endregion Structure

        #region Field

        public const string Name = "UserManagerHandler";
        private const string LoginTag = "Login";
        private const string UpdateUserInfoTag = "UpdateUserInfo";
        private const string SQLDataSourceTag = "SQLDataSource";
        private const string SQLDatabaseTag = "SQLDatabase";
        private const string SQLUserIDTag = "SQLUserID";
        private const string SQLPasswordTag = "SQLPassword";
        private const string UserInfoTag = "Data";
        private const string UserManagerTag = "UserManager";
        private const string UserNameTag = "UserName";
        private const string AddTag = "Add";
        private const string DeleteTag = "Delete";
        private const string PasswordTag = "Password";
        private const string RefreshTag = "Refresh";
        private const string LevelTag = "Level";
        private const string WarnTag = "Warn";
        private string TableName = "User";
        private int MaxUserNumber = 1000;



        #endregion Field

        #region Property

        public string UserID { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public string DataSource { get; set; }

        #endregion Property

        #region Delegate
        #endregion Delegate

        #region Event
        #endregion Event

        #region Function

        public override bool Handle(WebSocketSession session, string message) {
            if (session.SessionID != Session.SessionID) { return false; }
            JObject root;
            try {
                root = JObject.Parse(message);
            }
            catch {
                return false;
            }
            if (root == null) { return false; }
            LoginHandler(root);
            EditHandler(root);
            return true;
        }

        /// <summary>
        /// handler for login sqldatabase
        /// </summary>
        /// <param name="root"></param>
        private void LoginHandler(JObject root) {
            JObject login = (JObject)root[LoginTag];
            if (login != null) {
                Object dataSource = (JValue)login[SQLDataSourceTag];
                Object database = (JValue)login[SQLDatabaseTag];
                Object userID = (JValue)login[SQLUserIDTag];
                Object password = (JValue)login[SQLPasswordTag];
                if (IsLogin(dataSource.ToString(), database.ToString(), userID.ToString(), password.ToString())) {
                    SendIsLogin("true");
                    GlobalBase.Global.LogRecorder.Log(Log.Config.LogTypeEnum.Warn, GlobalBase.Properties.Resources.UserManagerLogin);
                    SendRefreshData(dataSource.ToString(), database.ToString(), userID.ToString(), password.ToString());
                }
                else {
                    SendWarning(GlobalBase.Properties.Resources.UserManagerLoginFail);
                    GlobalBase.Global.LogRecorder.Log(Log.Config.LogTypeEnum.Warn, GlobalBase.Properties.Resources.UserManagerLoginFail);
                }
            }
        }

        /// <summary>
        /// operation for add remove
        /// </summary>
        /// <param name="root"></param>
        private void EditHandler(JObject root) {
            JObject login = (JObject)root[UpdateUserInfoTag];
            if (login != null) {
                Object dataSource = (JValue)login[SQLDataSourceTag];
                Object database = (JValue)login[SQLDatabaseTag];
                Object sqlUserID = (JValue)login[SQLUserIDTag];
                Object sqlPassword = (JValue)login[SQLPasswordTag];
                JObject userInfo = (JObject)login[UserInfoTag];
                AddHandler(dataSource.ToString(), database.ToString(), sqlUserID.ToString(), sqlPassword.ToString(), userInfo);
                DeleteHandler(dataSource.ToString(), database.ToString(), sqlUserID.ToString(), sqlPassword.ToString(), userInfo);
            }
        }

        private void AddHandler(string dataSource, string database, string sqlUserID, string sqlPassword, JObject root) {
            JObject addInfo = (JObject)root[AddTag];
            if (addInfo != null) {
                Object userName = (JValue)addInfo[UserNameTag];
                Object password = (JValue)addInfo[PasswordTag];
                Object level = (JValue)addInfo[LevelTag];
                try {
                    using (SqlConnection connection = new SqlConnection("server=" + dataSource + ";database=" + database + ";user id =" + sqlUserID + ";password=" + sqlPassword + ";")) {
                        SqlCommand command = new SqlCommand(
                            "INSERT INTO [" + database + "].[dbo].[" + TableName +
                            "] VALUES ('" + userName + "','" + password + "','" + level + "')", connection);
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception e) {
                    GlobalBase.Global.LogRecorder.Log(Log.Config.LogTypeEnum.Error, GlobalBase.Properties.Resources.UserManagerEditFailed + e.ToString());
                    return;
                }
                SendRefreshData(dataSource.ToString(), database.ToString(), sqlUserID.ToString(), sqlPassword.ToString());
            }
        }

        private void DeleteHandler(string dataSource, string database, string sqlUserID, string sqlPassword, JObject root) {
            JArray deleteInfo = (JArray)root[DeleteTag];
            if (deleteInfo != null) {
                string resultStr = "";
                foreach (var item in deleteInfo) {
                    resultStr += "'";
                    resultStr += item.ToString();
                    resultStr += "',";
                }
                resultStr = resultStr.TrimEnd(',');
                try {
                    using (SqlConnection connection = new SqlConnection("server=" + dataSource + ";database=" + database + ";user id =" + sqlUserID + ";password=" + sqlPassword + ";")) {
                        SqlCommand command = new SqlCommand(
                            "DELETE [" + database + "].[dbo].[" + TableName +
                            "] WHERE NAME IN (" + resultStr + ")", connection);
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception e) {
                    GlobalBase.Global.LogRecorder.Log(Log.Config.LogTypeEnum.Error, GlobalBase.Properties.Resources.UserManagerEditFailed + e.ToString());
                    return;
                }
                SendRefreshData(dataSource.ToString(), database.ToString(), sqlUserID.ToString(), sqlPassword.ToString());
            }
        }

        private bool IsLogin(string dataSource, string database, string userID, string password) {
            try {
                using (SqlConnection connection = new SqlConnection("server=" + dataSource + ";database=" + database + ";user id =" + userID + ";password=" + password + ";")) {
                    connection.Open();
                    if (connection.State == ConnectionState.Open) {
                        return true;
                    }
                    return false;
                };
            }
            catch (Exception e) {
                GlobalBase.Global.LogRecorder.Log(Log.Config.LogTypeEnum.Error, GlobalBase.Properties.Resources.UserManagerLoginFail + GlobalBase.Global.NewLineStr + e.ToString());
                return false;
            }
        }
        private void SendWarning(string message) {
            XElement result = new XElement("UserManagerHandler");
            result.SetAttributeValue("Name", WarnTag);
            result.SetAttributeValue("Message", message);
            Session.Send(result.ToString());
        }
        private void SendIsLogin(string message) {
            XElement result = new XElement("UserManagerHandler");
            result.SetAttributeValue("Name", "Login");
            result.SetAttributeValue("IsLogin", message);
            Session.Send(result.ToString());
        }
        private void SendRefreshData(string dataSource, string database, string userID, string password) {
            try {
                using (SqlConnection connection = new SqlConnection("server=" + dataSource + ";database=" + database + ";user id =" + userID + ";password=" + password + ";")) {
                    SqlCommand command = new SqlCommand(
                       "SELECT Top " + MaxUserNumber.ToString() + " [Name],[Password],[Level]" +
                       "FROM [" + database + "].[dbo].[" + TableName + "] "
                    , connection);
                    command.Connection.Open();
                    XElement result = GetData(command, RefreshTag);
                    Session.Send(result.ToString());
                }
            }
            catch (Exception e) {
                GlobalBase.Global.LogRecorder.Log(Log.Config.LogTypeEnum.Error, e.ToString());
            }
        }
        private XElement GetData(SqlCommand command, string name) {
            SqlDataReader reader = command.ExecuteReader();
            XElement result = new XElement("RealtimeData");
            result.SetAttributeValue("Name", name);
            while (reader.Read()) {
                XElement child = new XElement("User");
                child.SetAttributeValue("Name", reader["Name"]);
                child.SetAttributeValue("Password", reader["Password"]);
                child.SetAttributeValue("Level", reader["Level"]);
                result.Add(child);
            }
            reader.Close();
            return result;
        }

        #endregion Function

        #region InterClass
        #endregion InterClass

    }
}
