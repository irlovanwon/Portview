///Copyright(c) 2013,HIT All rights reserved.
///Summary:
///Author:Irlovan
///Date:2013-12-10
///Description:
///Modification:

using Irlovan.Database;
using Irlovan.Driver;
using Irlovan.Lib.Symbol;
using Irlovan.Log;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace Irlovan.LocalInterface
{
    public class Driver
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="source"></param>
        /// <param name="driverAssemblyList"></param>
        public Driver(Catalog source, Dictionary<string, Assembly> driverAssemblyList) {
            if (source == null) { return; }
            _driverAssemblyList = driverAssemblyList;
            DriverList = new List<IDriver>();
            InitDriver(source);
        }

        #endregion Structure

        #region Field

        //container of driver
        private Dictionary<string, Assembly> _driverAssemblyList;
        private const string DriverDllPath = "\\Driver\\";
        private const string DriverNameSpace = "Irlovan.Driver.";
        private const string DriverFilePath = "\\Core\\Driver";
        private const string DriverRootName = "Driver";
        private const string DriverFileType = ".dll";

        #endregion Field

        #region Property

        /// <summary>
        /// Driver Instance List
        /// </summary>
        internal List<IDriver> DriverList { get; private set; }

        #endregion Property

        #region Delegate

        private delegate void RunDriverHanlder();

        #endregion Delegate

        #region Function

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() {
            foreach (var item in DriverList) {
                item.Dispose();
            }
        }

        /// <summary>
        /// Run Driver
        /// </summary>
        internal void Run() {
            if (DriverList == null) { return; }
            foreach (var item in DriverList) {
                try {
                    RunDriverHanlder runDriver = new RunDriverHanlder(item.Run);
                    runDriver.BeginInvoke(null, null);
                }
                catch (Exception e) {
                    Global.Info.LogRecorder.Log(LogLevelEnum.Error, e.ToString());
                    continue;
                }
            }
        }

        /// <summary>
        /// run driver
        /// </summary>
        /// <param name="source"></param>
        private void InitDriver(Catalog source) {
            XDocument driverDoc;
            string path = Global.Info.ProjectPath + DriverFilePath;
            if (!System.IO.File.Exists(path)) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Warn, Lib.Properties.Resources.DriverNoSource);
                return;
            }
            driverDoc = LoadDriverConfig(path);
            if (driverDoc == null) { return; }
            if (driverDoc.Document.Root.Name != DriverRootName) { return; }
            foreach (var item in driverDoc.Root.Elements()) {
                try {
                    RunEachDriver(item, source);
                }
                catch (Exception e) {
                    Global.Info.LogRecorder.Log(LogLevelEnum.Error, e.ToString());
                    continue;
                }
            }

        }

        /// <summary>
        /// Run each driver
        /// </summary>
        /// <param name="item"></param>
        /// <param name="source"></param>
        private void RunEachDriver(XElement item, Catalog source) {
            string driverName = item.Name.ToString();
            string driverFullName = Chip.ChipLoader.NameSpaceValue_Driver + driverName;
            if ((!_driverAssemblyList.ContainsKey(driverName)) || (_driverAssemblyList.Count == 0)) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.DriverNoSource + driverName); return; }
            Type driverType = _driverAssemblyList[driverName].GetType(driverFullName);
            if (driverType == null) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.DriverNoSource); return; }
            IDriver driver = (IDriver)Activator.CreateInstance(driverType, new Object[] { source, item });
            driver.Init();
            if (!driver.InitState) { return; }
            DriverList.Add(driver);
        }

        /// <summary>
        /// load config file of drivers
        /// </summary>
        private XDocument LoadDriverConfig(string path) {
            try {
                return XDocument.Load(path);
            }
            catch (Exception e) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.DriverNoSource + Symbol.NewLine_Symbol + e.ToString());
                return null;
            }
        }

        #endregion Function

    }
}
