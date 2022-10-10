///Copyright(c) 2013,HIT All rights reserved.
///Summary:
///Author:Irlovan
///Date:2014-07-10
///Description:
///Modification:2015-04-30

using Irlovan.Chip;
using Irlovan.Database;
using System;

namespace Irlovan.LocalInterface
{
    public class LocalInterface : IDisposable
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="chip"></param>
        public LocalInterface(ChipLoader chip) {
            Chip = chip;
        }

        #endregion Structure

        #region Field

        private const string RegisterPath = "/Register/";
        private const string FileName = "RAM";

        #endregion Field

        #region Property

        /// <summary>
        /// Local database
        /// </summary>
        public Catalog Source { get; private set; }

        /// <summary>
        /// Recorder interface
        /// </summary>
        public Recorder Recorder { get; private set; }

        /// <summary>
        /// Driver interface
        /// </summary>
        public Driver Driver { get; private set; }

        /// <summary>
        /// Notification interface
        /// </summary>
        public Notification Notification { get; private set; }

        /// <summary>
        /// config interface
        /// </summary>
        public Config Config { get; private set; }

        /// <summary>
        /// script interface
        /// </summary>
        public Script Script { get; private set; }

        /// <summary>
        /// chip interface
        /// </summary>
        public ChipLoader Chip { get; private set; }

        /// <summary>
        /// Register
        /// </summary>
        public Register Register { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Init local interface
        /// </summary>
        public void Init() {
            InitRegister();
            InitConfig();
            InitSource();
            if (Source == null) { return; }
            InitDriver();
            InitRecorder();
            InitNotification();
            InitScript();
        }

        /// <summary>
        /// Init Register
        /// </summary>
        private void InitRegister() {
            Register = new Register();
        }

        /// <summary>
        /// get config
        /// </summary>
        private void InitConfig() {
            Config = new Config(Chip);
        }

        /// <summary>
        /// get source
        /// </summary>
        private void InitSource() {
            RealtimeData data = new RealtimeData();
            Source = data.Source;
        }

        /// <summary>
        /// Init Recorder
        /// </summary>
        private void InitRecorder() {
            Recorder = new Recorder(Source, Chip.Recorder.AssemblyList);
        }

        /// <summary>
        /// get script
        /// </summary>
        private void InitScript() {
            Script = new Script(Source, Chip.Script.ScriptAssemblyList);
        }

        /// <summary>
        /// Init driver
        /// </summary>
        private void InitDriver() {
            Driver = new Driver(Source, Chip.Driver.AssemblyList);
        }

        /// <summary>
        /// Init notification
        /// </summary>
        private void InitNotification() {
            Notification = new Notification(Source, Register.SysRegister, Chip.Notification.AssemblyList);
        }

        /// <summary>
        /// Run local interface
        /// </summary>
        public void Run() {
            if (Source == null) { return; }
            Driver.Run();
            Recorder.Run();
            Notification.Run();
        }

        /// <summary>
        /// Dispose for LocalInterface 
        /// </summary>
        public void Dispose() {
            DisposeDriver();
            DisposeRecorder();
            DisposeScript();
            Register.Dispose();
        }

        /// <summary>
        /// Dispose Drivers
        /// </summary>
        private void DisposeDriver() {
            foreach (var item in Driver.DriverList) {
                item.Dispose();
            }
        }

        /// <summary>
        /// Dispose Recorders
        /// </summary>
        private void DisposeRecorder() {
            foreach (var item in Recorder.RecorderList) {
                item.Value.Dispose();
            }
        }

        /// <summary>
        /// Dispose Script
        /// </summary>
        private void DisposeScript() {
            //anything is ready?
        }

        #endregion Function

    }
}
