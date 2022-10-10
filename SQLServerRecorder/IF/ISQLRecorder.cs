///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:SQLServer recorder interface
///Author:Irlovan
///Date:2015-11-13
///Description:

using System.Data;

namespace Irlovan.Recorder.SQLServerRecorder
{
    internal interface ISQLRecorder : IRecorder
    {

        #region Property

        /// <summary>
        /// connection string for SQLServer
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// userID for SQLServer
        /// </summary>
        string UserID { get; set; }

        /// <summary>
        /// table name for SQLServer
        /// </summary>
        string TableName { get; set; }

        /// <summary>
        /// password for SQLServer
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// database string for SQLServer
        /// </summary>
        string Database { get; set; }

        /// <summary>
        /// datasource string for SQLServer
        /// </summary>
        string DataSource { get; set; }

        /// <summary>
        /// Connection State 
        /// </summary>
        ConnectionState State { get; set; }

        #endregion Property

    }
}
