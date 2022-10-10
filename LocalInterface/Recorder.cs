///Copyright(c) 2015,HIT All rights reserved.
///Summary:Recorder
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      

using Irlovan.Database;
using Irlovan.Lib.Symbol;
using Irlovan.Lib.XML;
using Irlovan.Log;
using Irlovan.Recorder;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace Irlovan.LocalInterface
{
    public class Recorder
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="source"></param>
        /// <param name="recorderAssemblyList"></param>
        public Recorder(Catalog source, Dictionary<string, Assembly> recorderAssemblyList) {
            _source = source;
            _recorderAssemblyList = recorderAssemblyList;
            RecorderList = new Dictionary<string, IRecorder>();
            RunRecorder();
        }

        #endregion Structure

        #region Field

        private Catalog _source;
        private Dictionary<string, Assembly> _recorderAssemblyList;
        private const string RecorderNameSpace = "Irlovan.Recorder.";
        private const string TypeName = "Type";
        private const string RecorderRootName = "Recorder";
        private const string RecorderFilePath = "\\Core\\Recorder";

        #endregion Field

        #region Property

        /// <summary>
        /// Recorder List
        /// </summary>
        public Dictionary<string, IRecorder> RecorderList { get; private set; }

        #endregion Property

        #region Delegate

        private delegate void RunRecorderHanlder();

        #endregion Delegate

        #region Function

        /// <summary>
        /// Run Recorder
        /// </summary>
        internal void Run() {
            foreach (var item in RecorderList) {
                try {
                    RunRecorderHanlder runRecorder = new RunRecorderHanlder(item.Value.Run);
                    runRecorder.BeginInvoke(null, null);
                }
                catch (Exception e) {
                    Global.Info.LogRecorder.Log(LogLevelEnum.Error, e.ToString());
                    continue;
                }
            }
        }

        /// <summary>
        /// start to run recorder module
        /// </summary>
        private void RunRecorder() {
            XDocument recorderDoc;
            string path = Global.Info.ProjectPath + RecorderFilePath;
            if (!System.IO.File.Exists(path)) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.RecorderNoResource);
                return;
            }
            try { recorderDoc = XDocument.Load(path); }
            catch (Exception e) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.RecorderNoResource + Symbol.NewLine_Symbol + e.ToString());
                return;
            }
            if (recorderDoc.Document.Root.Name != RecorderRootName) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.RecorderNoResource);
                return;
            }
            foreach (var dll in recorderDoc.Root.Elements()) {
                InitDll(dll);
            }
        }

        /// <summary>
        /// Init recorder dll
        /// </summary>
        private void InitDll(XElement dll) {
            string dllName = dll.Name.ToString();
            string recorderDllFullName = Chip.ChipLoader.NameSpaceValue_Recorder + dllName;
            if ((!_recorderAssemblyList.ContainsKey(dllName)) || (_recorderAssemblyList.Count == 0)) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.RecorderNoResource + recorderDllFullName);
                return;
            }
            foreach (var recorder in dll.Elements()) {
                CreateRecorder(recorder, dllName);
            }
        }

        /// <summary>
        /// create recorder instance
        /// </summary>
        /// <param name="config"></param>
        private void CreateRecorder(XElement config, string dllName) {
            string recorderName;
            if (!XML.InitStringAttr<string>(config, Irlovan.Recorder.Recorder.RecorderNameTag, out recorderName)) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.NoRecorderName);
                return;
            }
            string recorderDllFullName = Chip.ChipLoader.NameSpaceValue_Recorder + dllName;
            string recorderFullName = recorderDllFullName + "." + config.Name.ToString();
            Type recorderType = _recorderAssemblyList[dllName].GetType(recorderFullName);
            if (recorderType == null) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.RecorderNoResource); return; }
            IRecorder recorder = (IRecorder)Activator.CreateInstance(recorderType, new Object[] { _source, config });
            recorder.Init();
            if (!recorder.InitState) { return; }
            if (!RecorderList.ContainsKey(recorderName)) { RecorderList.Add(recorderName, recorder); }
            else { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.RecorderNameExist); }
        }

        #endregion Function

    }
}
