///Copyright(c) 2015,HIT All rights reserved.
///Summary:Script
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      

using Irlovan.Database;
using Irlovan.Lib.Symbol;
using Irlovan.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Irlovan.LocalInterface
{
    public class Script
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="source"></param>
        /// <param name="scriptAssemblyList"></param>
        public Script(Catalog source, Dictionary<string, Assembly> scriptAssemblyList) {
            _source = source;
            if (!Load()) { return; }
            _scriptAssemblyList = scriptAssemblyList;
            RunScript();
        }

        #endregion Structure

        #region Field

        private const string ScriptFilePath = "\\Script";
        private const string ScriptNameSpace = "Irlovan.Script.";
        private Dictionary<string, Assembly> _scriptAssemblyList;
        private Catalog _source;
        private XDocument _configDoc;

        #endregion Field

        #region Function

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
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.ScriptFail + e.ToString());
                return false;
            }
        }

        /// <summary>
        /// run script
        /// </summary>
        private void RunScript() {
            if (_configDoc.Root.Elements().Count() == 0) { return; }
            foreach (var item in _configDoc.Root.Elements()) {
                if (_scriptAssemblyList.ContainsKey(item.Name.ToString())) {
                    try {
                        Activator.CreateInstance(GetScriptType(item.Name.ToString()), new Object[] { _source, item });
                    }
                    catch (Exception e) {
                        Global.Info.LogRecorder.Log(LogLevelEnum.Error, e.ToString() + Symbol.NewLine_Symbol + Lib.Properties.Resources.ScriptError + item.Name.ToString());
                    }
                }
            }

        }

        /// <summary>
        /// get the script dll type to load
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Type GetScriptType(string name) {
            Type result = _scriptAssemblyList[name].GetType(ScriptNameSpace + name);
            return result;
        }

        #endregion Function

    }
}
