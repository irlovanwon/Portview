///Copyright(c) 2015,HIT All rights reserved.
///Summary:
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:      

using Hardcodet.Wpf.TaskbarNotification;
using Irlovan.Chip;
using Irlovan.Global;
using Irlovan.Lib.Symbol;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Irlovan.Server
{
    /// <summary>
    /// App.xaml
    /// </summary>
    public partial class App : Application
    {

        #region Structure

        public App() {
            InitReady = false;
        }



        #endregion Structure

        #region Field

        private Irlovan.RemoteInterface.RemoteInterface _remote;
        private LocalInterface.LocalInterface _local;
        private ChipLoader _chip;
        private TaskbarIcon _notifyIcon;
        private const string NotifyIconResourceName = "NotifyIcon";
        //main function args
        private String[] _args = new String[0];

        private Mutex _mutex;

        #endregion Field

        #region Property

        /// <summary>
        /// property to show if initialize is ready
        /// </summary>
        internal static bool InitReady { get; private set; }
        
        #endregion Property

        #region Function

        /// <summary>
        /// Start Server
        /// </summary>
        private void StartServer() {
            _local.Run();
            RemoteInterface();
            InitReady = true;
        }

        /// <summary>
        ///Load the project 
        /// </summary>
        /// <param name="args"></param>
        private void LoadProject() {
            if (GeneralMode()) { return; }
            if (AppointedMode()) { return; }
            Runtime.ServerShutDown(Lib.Properties.Resources.ProjectNotFound);
        }

        /// <summary>
        /// Load chips
        /// </summary>
        private void LoadChip() {
            try {
                _chip = new ChipLoader();
            }
            catch (Exception e) {
                Runtime.ServerShutDown(e.ToString());
            }
        }

        /// <summary>
        /// Appoint to the very project
        /// </summary>
        private bool AppointedMode() {
            if (_args.Length == 0) { return false; }
            if (!SetProjectPath(_args[0])) { return false; }
            return true;
        }

        /// <summary>
        /// load project which used lase time
        /// </summary>
        private bool GeneralMode() {
            if (_args.Length != 0) { return false; }
            if (!SetProjectPath(System.Environment.CurrentDirectory + Global.Info.DefautProjectPath)) { return false; }
            return true;
        }

        /// <summary>
        /// Set current project path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool SetProjectPath(string path) {
            if (!(Directory.Exists(path))) { return false; }
            Global.Info.ProjectPath = path;
            Global.Info.GUIPath = path + Global.Info.LayoutPath;
            return true;
        }

        /// <summary>
        /// Start Remoteinterface
        /// </summary>
        private void RemoteInterface() {
            _remote = new Irlovan.RemoteInterface.RemoteInterface(_local);
        }

        /// <summary>
        /// Construct LocalInterface
        /// </summary>
        private void LocalInterface() {
            _local = new LocalInterface.LocalInterface(_chip);
            _local.Init();
        }

        /// <summary>
        /// avoid open the same project multiple times
        /// </summary>
        private void AvoidMuntipleOpen() {
            bool createNew;
            _mutex = new Mutex(true, _local.Config.ProjectName, out createNew);
            if (!createNew) {
                Runtime.ServerShutDown(Lib.Properties.Resources.ProjectAlreadyRuning + _local.Config.ProjectName);
            }
        }


        /// <summary>
        /// Trigged When UI Start
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            GetMainArgs(e);
            Global.Info.LogRecorder = new Log.Logger();
            Global.Info.Dispatcher = Dispatcher;
            LoadProject();
            LoadChip();
            LocalInterface();
            AvoidMuntipleOpen();
            StartServer();
            try {
                _notifyIcon = (TaskbarIcon)FindResource(NotifyIconResourceName);
                _notifyIcon.ToolTipText = _local.Config.ProjectName;
            }
            catch { }
            _remote.ClientNotify += (int count) => {
                Action<int> notice = new Action<int>(NoticeClient);
                Global.Info.Dispatcher.Invoke(notice, count);
            };
        }

        /// <summary>
        /// NoticeClient
        /// </summary>
        private void NoticeClient(int count) {
            _notifyIcon.ToolTipText = _local.Config.ProjectName + Symbol.NewLine_Symbol + Lib.Properties.Resources.ClientCount + count.ToString();
        }

        /// <summary>
        /// get program args for main function in WPF
        /// </summary>
        /// <param name="e"></param>
        private void GetMainArgs(StartupEventArgs e) {
            if (e.Args != null && e.Args.Count() > 0) {
                _args = e.Args;
            }
        }

        /// <summary>
        /// Trigged When UI Closed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e) {
            //the icon would clean up automatically, but this is cleaner
            _notifyIcon.Dispose();
            if (_local != null) {
                try {
                    _local.Dispose();
                }
                catch (Exception) { }
            }
            if (_remote != null) {
                try {
                    _remote.Dispose();
                }
                catch { }
            }
            DisposeMutex();
            base.OnExit(e);
            Runtime.ServerShutDown();
        }


        /// <summary>
        /// Dispose mutex
        /// </summary>
        private void DisposeMutex() {
            if (_mutex != null) {
                try {
                    _mutex.ReleaseMutex();
                    _mutex.Close();
                    _mutex.Dispose();
                    _mutex = null;
                }
                catch { }
            }
        }

        #endregion Function

    }
}
