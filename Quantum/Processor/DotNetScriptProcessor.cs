///Copyright(c) 2015,HIT All rights reserved.
///Summary:DotNetScriptProcessor
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      

using Irlovan.Database;
using Irlovan.Script;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace Irlovan.Quantum
{
    public class DotNetScriptProcessor : IProcessorQuantum
    {

        #region Structure

        /// <summary>
        /// DotNetScriptProcessor
        /// </summary>
        /// <param name="source"></param>
        public DotNetScriptProcessor(Catalog source) {
            Source = source;
        }

        #endregion Structure

        #region Field

        private const string ScriptRootName = "Script";
        private const string ScriptFilePath = "\\Script\\Script";
        private const string IDPara = "ID";
        private const string NextPara = "Next";
        private const string ClassNamePara = "ClassName";
        private const string RunMethodNamePara = "RunMethodName";
        private const string StopMethodNamePara = "StopMethodName";
        private const string SynPara = "IsSyn";
        private const string CatchEventPara = "CatchEvent";
        public const string ProcessorName = "DotNetScriptProcessor";
        private Type _sciptClass;
        public Dictionary<string, Type> ScriptDictionary = new Dictionary<string, Type>(){ 
            {DotNetFrameworkScript.ScriptName, typeof(DotNetFrameworkScript)}
        };

        #endregion Field

        #region Property

        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Next
        /// </summary>
        public int[] Next { get; set; }

        /// <summary>
        /// Source
        /// </summary>
        public Catalog Source { get; private set; }

        /// <summary>
        /// CatchEvent
        /// </summary>
        private bool CatchEvent { get; set; }

        /// <summary>
        /// ClassName
        /// </summary>
        public string ClassName { get; private set; }

        /// <summary>
        /// RunMethodName
        /// </summary>
        public string RunMethodName { get; private set; }

        /// <summary>
        /// StopMethodName
        /// </summary>
        public string StopMethodName { get; private set; }

        /// <summary>
        /// the processor is syn or asyn!
        /// </summary>
        public bool IsSyn { get; set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Run
        /// </summary>
        public void Run() {
            if (CatchEvent) {
                try { RunScript(); }
                catch { }
            }
            else {
                RunScript();
            }
        }

        /// <summary>
        /// Stop
        /// </summary>
        public void Stop() {
            if (StopMethodName == null) { return; }
            MethodInfo stopMethod = _sciptClass.GetMethod(StopMethodName);
            dynamic obj = Activator.CreateInstance(_sciptClass, new object[] { Source });
            stopMethod.Invoke(obj, null);
        }

        /// <summary>
        /// RunScript
        /// </summary>
        private void RunScript() {
            XDocument scriptDoc = XDocument.Load(Global.Info.ProjectPath + ScriptFilePath);
            if (scriptDoc.Document.Root.Name != ScriptRootName) { return; }
            var element = scriptDoc.Root.Element(DotNetFrameworkScript.ScriptName);
            DotNetFrameworkScript result = new DotNetFrameworkScript(Source, element);
            _sciptClass = result.ScriptAssembly.GetType(ClassName);
            MethodInfo runMethod = _sciptClass.GetMethod(RunMethodName);
            dynamic obj = Activator.CreateInstance(_sciptClass, new object[] { Source });
            runMethod.Invoke(obj, null);
        }

        /// <summary>
        /// ReadXML
        /// </summary>
        /// <param name="element"></param>
        public void ReadXML(XElement element) {
            ID = int.Parse(element.Attribute(IDPara).Value);
            if (element.Attribute(NextPara) != null) {
                Next = Lib.Array.Array.ArrayToType<string, int>(element.Attribute(NextPara).Value.Split(','));
            }
            ClassName = element.Attribute(ClassNamePara).Value;
            RunMethodName = element.Attribute(RunMethodNamePara).Value;
            StopMethodName = (element.Attribute(StopMethodNamePara) == null) ? null : element.Attribute(StopMethodNamePara).Value;
            if ((element.Attribute(CatchEventPara) != null)) { CatchEvent = bool.Parse(element.Attribute(CatchEventPara).Value); }
            IsSyn = Boolean.Parse(element.Attribute(SynPara).Value);
        }

        #endregion Function

    }
}
