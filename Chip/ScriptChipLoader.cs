///Copyright(c) 2015,HIT All rights reserved.
///Summary:Script chip loader
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      

using Irlovan.Global;
using Irlovan.Lib.Symbol;
using Irlovan.Log;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace Irlovan.Chip
{
    public class ScriptChipLoader
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        internal ScriptChipLoader() {
            if (!Load()) { return; }
            if (!Check()) { return; }
            ScriptAssemblyList = new Dictionary<string, Assembly>();
            LoadScriptDll();
        }

        #endregion Structure

        #region Field

        private const string FileType = ".dll";
        private const string ScriptDllPath = "\\Script\\";
        private const string ScriptFilePath = "\\Script";
        private const string RootTag = "Script";

        private XDocument _configDoc;

        #endregion Field

        #region Property

        public Dictionary<string, Assembly> ScriptAssemblyList { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// load handlers
        /// </summary>
        private void LoadScriptDll() {
            try {
                foreach (var item in _configDoc.Root.Elements()) {
                    string name = item.Name.ToString();
                    Assembly assem = Assembly.LoadFile(System.Environment.CurrentDirectory + ScriptDllPath + name + FileType);
                    ScriptAssemblyList.Add(name, assem);
                    AppDomain.CurrentDomain.Load(assem.GetName());
                }
            }
            catch (Exception e) {
                Runtime.ServerShutDown(e.ToString() + Symbol.NewLine_Symbol + Global.Info.ProjectPath);
            }
        }

        /// <summary>
        /// load config of script file
        /// </summary>
        /// <returns></returns>
        private bool Load() {
            try {
                _configDoc = XDocument.Load(Global.Info.ProjectPath + ScriptFilePath);
                return true;
            }
            catch (Exception e) {
                Runtime.ServerShutDown(Lib.Properties.Resources.ScriptFail + e.ToString());
                return false;
            }
        }

        /// <summary>
        /// check file
        /// </summary>
        /// <returns></returns>
        private bool Check() {
            if (_configDoc.Root.Name != RootTag) { return false; }
            Global.Info.LogRecorder.Log(LogLevelEnum.Warn, Lib.Properties.Resources.ScriptLoad);
            return true;
        }

        #endregion Function

    }
}
