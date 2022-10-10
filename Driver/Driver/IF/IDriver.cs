///Copyright(c) 2015,HIT All rights reserved.
///Summary:Driver interface
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using Irlovan.Database;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Driver
{
    public interface IDriver : IDisposable
    {

        #region Property

        /// <summary>
        /// Driver Data List<groupName,IDriverDataBox>
        /// </summary>
        Dictionary<string, IGroup> DataList { get; }

        /// <summary>
        /// All datas in realtime data table
        /// </summary>
        Catalog Source { get; set; }

        /// <summary>
        /// Config file for driver
        /// </summary>
        XElement Config { get; set; }

        /// <summary>
        /// Unique Name for each driver
        /// </summary>
        string DriverName { get; set; }

        /// <summary>
        /// Comm data to indicate if the device is connected
        /// </summary>
        string Comm { get; set; }

        /// <summary>
        /// bool to indicate if init success
        /// </summary>
        bool InitState { get; set; }

        /// <summary>
        /// reconnect time out when device disconnected
        /// </summary>
        int ReconnectTimeout { get; set; }

        /// <summary>
        /// Write Data Mode
        /// </summary>
        WriteDataModeEnum WriteDataMode { get; set; }

        /// <summary>
        /// Communication Data to show if the driver is connected to the device
        /// </summary>
        IIndustryData CommData { get; set; }

        #endregion Property

        #region Event

        /// <summary>
        /// server disconnected
        /// </summary>
        event ServerShutDownHandler ServerShutDown;

        /// <summary>
        /// server connected
        /// </summary>
        event ServerConnectedHandler ServerConnected;

        #endregion Event

        #region Function

        /// <summary>
        /// Properties int for driver
        /// </summary>
        void Init();

        /// <summary>
        /// Start runging driver
        /// </summary>
        void Run();

        /// <summary>
        /// Stop driver before reconnect
        /// </summary>
        void Stop();

        /// <summary>
        /// Disconnect with device
        /// </summary>
        void Disconnect();


        /// <summary>
        /// Reconnect to the device
        /// </summary>
        void Reconnect();

        /// <summary>
        /// device disconnected
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="timeStamp"></param>
        void ServerShutDownEventHandler(string reason, DateTime timeStamp);

        /// <summary>
        /// device connected
        /// </summary>
        /// <param name="timeStamp"></param>
        void ServerConnectedHandler(DateTime timeStamp);

        /// <summary>
        /// write single data handler
        /// </summary>
        /// <param name="timeStamp"></param>
        void WriteDataHandler(string groupName, string name, string addr, object value, DateTime timeStamp);

        /// <summary>
        /// write data list handler
        /// </summary>
        /// <param name="timeStamp"></param>
        void WriteDataListHandler(string groupName, Dictionary<string, object> dataList, DateTime timeStamp);

        #endregion Function

    }

}
