///Copyright(c) 2015,HIT All rights reserved.
///Summary:Register
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using Irlovan.Lib.Symbol;
using Irlovan.Log;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Timers;

namespace Irlovan.Register
{
    public class Register : IRegister
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="registerPath"></param>
        /// <param name="fileName"></param>
        /// <param name="interval"></param>
        public Register(string registerPath, string fileName, int interval = 1000) {
            _registerPath = registerPath;
            Interval = interval;
            Name = fileName;
            if (!Init()) { InitState = false; return; }
        }

        #endregion Structure

        #region Field

        private string _registerPath;
        //define the default depth of the dimention
        private int _depth = 50;
        //formatter for Serialization
        IFormatter _formatter = new BinaryFormatter();
        //lock for Serialization
        private object _sLock = new object();
        //timer for record hd
        private Timer _timer;

        #endregion Field

        #region Property

        /// <summary>
        /// name of a register
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// Hardware path
        /// </summary>
        public String HDPath { get; private set; }

        /// <summary>
        /// Init State shows if Register inits properly
        /// </summary>
        public bool InitState { get; private set; }

        /// <summary>
        /// Interval For Mode Selection :-1 meams datachange mode >0 means interval mode =0 meams not record to HD
        /// </summary>
        public int Interval { get; private set; }


        /// <summary>
        /// Max Count of the Group dimension
        /// </summary>
        public int Depth {
            get { return _depth; }
            private set {
                if (value != _depth) {
                    _depth = value;
                }
            }
        }

        /// <summary>
        /// Random Access Memory
        /// </summary>
        public ISingularity RAM { get; set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Init register
        /// </summary>
        /// <returns></returns>
        public bool Init() {
            InitState = true;
            if (!InitHDPath()) { return false; }
            return true;
        }

        /// <summary>
        /// Read data from Recorder
        /// </summary>
        /// <returns></returns>
        public bool AsyncRead<T>(string address, out T value) {
            value = default(T);
            if (RAM == null) { return false; }
            if (string.IsNullOrEmpty(address)) { return false; }
            IAddress ramAddress = new Address();
            ramAddress.Parse(address);
            return RAM.Read<T>(ramAddress, out value);
        }

        /// <summary>
        /// Write data from Recorder
        /// </summary>
        /// <returns></returns>
        public bool AsyncWrite<T>(string address, T data) {
            if (RAM == null) { return false; }
            if (string.IsNullOrEmpty(address)) { return false; }
            IAddress ramAddress = new Address();
            ramAddress.Parse(address);
            return RAM.Write<T>(ramAddress, data);
        }

        /// <summary>
        /// Remove data from Recorder(Not Finished)
        /// </summary>
        /// <returns></returns>
        public bool AsyncRemove(IAddress address) { return false; }

        /// <summary>
        /// Dispose Memeory for register
        /// </summary>
        public void Dispose() {
            if (RAM != null) { RAM.Dispose(); }
            if (_timer != null) { Lib.Timer.Timer.DisposeTimer(_timer); }
        }

        /// <summary>
        /// Record register to hardware
        /// </summary>
        /// <returns></returns>
        public bool RecordtoHD() {
            lock (_sLock) {
                try {
                    if (!InitState) { return false; }
                    FileStream fs = new FileStream(HDPath, FileMode.Create);
                    _formatter.Serialize(fs, RAM);
                    fs.Close();
                }
                catch (Exception e) {
                    Global.Info.LogRecorder.Log(LogLevelEnum.Error, e.ToString());
                }
                return true;
            }
        }

        /// <summary>
        /// Init register from hardware
        /// </summary>
        /// <returns></returns>
        public bool InitFromHD() {
            lock (_sLock) {
                try {
                    if (!InitState) { return false; }
                    FileStream fs = new FileStream(HDPath, FileMode.Open);
                    RAM = (ISingularity)_formatter.Deserialize(fs);
                    RAM.UpdateRoot(this);
                    return true;
                }
                catch (Exception e) {
                    Global.Info.LogRecorder.Log(LogLevelEnum.Error, e.ToString());
                    return false;
                }
            }
        }

        /// <summary>
        /// Select mode for record hd
        /// </summary>
        public void Mode() {
            if (Interval == 0) { return; }
            IntervalMode();
        }

        /// <summary>
        /// Init Hardware path
        /// </summary>
        /// <param name="path"></param>
        private bool InitHDPath() {
            try {
                string path = _registerPath + Name;
                if (!File.Exists(path)) { File.Create(path); }
                HDPath = path;
                return true;
            }
            catch (Exception e) {
                InitState = false;
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, Irlovan.Lib.Properties.Resources.RegisterInitFailed + Symbol.NewLine_Symbol + e.ToString());
                return false;
            }
        }

        /// <summary>
        /// interval record
        /// </summary>
        private void IntervalMode() {
            if (Interval <= 0) { return; }
            Irlovan.Lib.Timer.Timer.SetInterval((object o, ElapsedEventArgs e) => {
                RecordtoHD();
            }, ref _timer, Interval);
        }

        #endregion Function

    }
}
