///Copyright(c) 2013,HIT All rights reserved.
///Summary:SQLHelper
///Author:Irlovan
///Date:2013-04-03
///Description:
///Modification:

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Irlovan.Lib.SQLServer
{
    public static class SQLServer
    {

        #region Field

        public const string ArrayL = " (";

        public const string ArrayR = ") ";

        public const string LabelL = " [";

        public const string LabelR = "] ";

        public const string Dbo = "dbo";

        public const string ArraySplitChar = ",";

        public const string Where = " WHERE ";

        public const string And = " AND ";

        public const string OrderBy = " ORDER BY ";

        public const string DESC = " DESC ";

        public const string In = " in ";

        public const string LessThan = "<";

        public const string EqualTo = "=";

        public const string Password = " password ";

        public const string Database = " database ";

        public const string Server = " server ";

        public const string User = " user ";

        public const string ID = " id ";

        public const string SemiColon = ";";

        public const string RefChar = "'";

        public const string MoreThan = ">";

        public const string SpaceChar = " ";

        public const string CatalogSplitChar = ".";

        #endregion Field

        #region Function

        /// <summary>
        /// after a datetime 
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static string DateAfter(string columnName, string timeStamp) {
            StringBuilder result = new StringBuilder();
            result.Append(SpaceChar);
            result.Append(columnName);
            result.Append(SpaceChar);
            result.Append(MoreThan);
            result.Append(EqualTo);
            result.Append(SpaceChar);
            result.Append(RefChar);
            result.Append(timeStamp);
            result.Append(RefChar);
            result.Append(SpaceChar);
            return result.ToString();
        }


        /// <summary>
        /// before a datetime 
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static string DateBefore(string columnName, string timeStamp) {
            StringBuilder result = new StringBuilder();
            result.Append(SpaceChar);
            result.Append(columnName);
            result.Append(SpaceChar);
            result.Append(LessThan);
            result.Append(EqualTo);
            result.Append(SpaceChar);
            result.Append(RefChar);
            result.Append(timeStamp);
            result.Append(RefChar);
            result.Append(SpaceChar);
            return result.ToString();
        }

        /// <summary>
        /// Create Command
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="connectionString"></param>
        public static void CreateSQLSeverCommand(string queryString, string connectionString) {
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// create data table
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnDic"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static DataTable CreateDataTable(string tableName, Dictionary<string, string> columnDic, string keyName = null) {
            DataTable dataTable = new DataTable(tableName);
            foreach (var item in columnDic) {
                DataColumn column = new DataColumn();
                column.DataType = System.Type.GetType(item.Value);
                column.ColumnName = item.Key;
                dataTable.Columns.Add(column);
                if (keyName == null) {
                    continue;
                }
                if (item.Key == keyName) {
                    dataTable.PrimaryKey = new DataColumn[1] { column };
                }
            }
            return dataTable;
        }

        /// <summary>
        /// Select Column From Table
        /// </summary>
        /// <param name="columnList"></param>
        /// <param name="database"></param>
        /// <param name="tableName"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static string SelectColumn(string[] columnList, string database, string tableName, object amount = null) {
            int selectAmount = 0;
            if ((amount != null) && (!int.TryParse(amount.ToString(), out selectAmount))) {
                selectAmount = 1000;
            }
            StringBuilder tableStringBuilder = new StringBuilder();
            for (int i = 0; i < columnList.Length; i++) {
                tableStringBuilder.Append("[");
                tableStringBuilder.Append(columnList[i]);
                tableStringBuilder.Append("]");
                tableStringBuilder.Append(",");
            }
            string tableString = tableStringBuilder.ToString().TrimEnd(',');
            string result = "SELECT" + Symbol.Symbol.Space_Char + ((selectAmount <= 0) ? "" : ("Top" + Symbol.Symbol.Space_Char + selectAmount.ToString())) +
            Symbol.Symbol.Space_Char + tableString +
            " FROM [" + database + "].[dbo].[" + tableName + "]" + Symbol.Symbol.Space_Char;
            return result;
        }

        /// <summary>
        /// Create Connect String
        /// </summary>
        /// <returns></returns>
        public static string CreateConnectString(string dataSource, string dataBase, string userID, string password) {
            return SQLServer.Server + SQLServer.EqualTo + dataSource + SQLServer.SemiColon + SQLServer.Database + SQLServer.EqualTo + dataBase + SQLServer.SemiColon + SQLServer.User + SQLServer.EqualTo + userID + SQLServer.SemiColon + SQLServer.Password + SQLServer.EqualTo + password + SQLServer.SemiColon;
        }

        #endregion Function

    }
}
