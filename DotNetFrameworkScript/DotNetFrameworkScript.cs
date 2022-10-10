///Copyright(c) 2015,HIT All rights reserved.
///Summary:.NetFramework based script
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using Irlovan.Database;
using Irlovan.Lib.Symbol;
using Irlovan.Log;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Irlovan.Script
{
    public class DotNetFrameworkScript
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="source"></param>
        /// <param name="config"></param>
        public DotNetFrameworkScript(Catalog source, XElement config) {
            //*************************for cms**************************
            _source = source;
            Init(config);
            RunScripts(Scripts(config));
        }

        #endregion Structure

        #region Field

        //****************************for cms******************
        private Catalog _source;
        public const string ScriptName = "DotNetFrameWork";
        private const string ProjectNamePara = "Name";
        private const string PreferencePara = "Preference";
        private const string ExecuteModePara = "ExecuteMode";
        private const string DataDllPara = "RTDB.dll";
        private const string DataQualityDllPara = "DataQuality.dll";
        private const string RemoteInterfaceDllPara = "RemoteInterface.dll";
        private const string GenerateInMemoryPara = "GenerateInMemory";
        private string[] Framework4_0 = new string[] { "Accessibility.dll", "CustomMarshalers.dll", "Microsoft.CSharp.dll", "Microsoft.JScript.dll", "Microsoft.VisualBasic.Compatibility.Data.dll", "Microsoft.VisualBasic.Compatibility.dll", "Microsoft.VisualBasic.dll", "Microsoft.VisualC.Dll", "mscorlib.dll", "sysglobl.dll", "System.Activities.Core.Presentation.dll", "System.Activities.dll", "System.Activities.DurableInstancing.dll", "System.Activities.Presentation.dll", "System.Addin.Contract.dll", "System.Addin.dll", "System.ComponentModel.Composition.dll", "System.ComponentModel.DataAnnotations.dll", "System.configuration.dll", "System.Configuration.Install.dll", "System.Core.dll", "System.Data.DataSetExtensions.dll", "System.Data.dll", "System.Data.Entity.dll", "System.Data.Linq.dll", "System.Data.Services.Client.dll", "System.Data.SqlXml.dll", "System.Deployment.dll", "System.Device.dll", "System.DirectoryServices.AccountManagement.dll", "System.DirectoryServices.dll", "System.DirectoryServices.Protocols.dll", "System.dll", "System.Drawing.dll", "System.EnterpriseServices.dll", "System.IdentityModel.dll", "System.IdentityModel.Selectors.dll", "System.IO.Log.dll", "System.Management.dll", "System.Management.Instrumentation.dll", "System.Messaging.dll", "System.Net.dll", "System.Numerics.dll", "System.Runtime.DurableInstancing.dll", "System.Runtime.Remoting.dll", "System.Runtime.Serialization.dll", "System.Runtime.Serialization.Formatters.Soap.dll", "System.Security.dll", "System.ServiceModel.Activities.dll", "System.ServiceModel.Channels.dll", "System.ServiceModel.Discovery.dll", "System.ServiceModel.dll", "System.ServiceModel.Routing.dll", "System.ServiceProcess.dll", "System.Transactions.dll", "System.Web.ApplicationServices.dll", "System.Web.Services.dll", "System.Windows.Forms.DataVisualization.dll", "System.Windows.Forms.dll", "System.Xaml.dll", "System.XML.dll", "System.Xml.Linq.dll" };
        private Dictionary<string, string> LangugeDic = new Dictionary<string, string>() {
        {"CSharp","CSharp"},
        {"VisualBasic","VisualBasic"},
        {"JScript","JScript"},
        {"C++","Cpp"}
        };
        private Dictionary<string, string> ExecuteModeDic = new Dictionary<string, string>() {
        {"Dll","Script.dll"},
        {"Exe","Script.exe"}
        };
        private Dictionary<string, bool> ExecutableDic = new Dictionary<string, bool>() {
        {"Dll",false},
        {"Exe",true}
        };
        private const string PathPara = "Path";

        #endregion Field

        #region Property

        /// <summary>
        /// Choose the language you like
        /// </summary>
        public string Preference { get; private set; }

        /// <summary>
        /// You can make the script executabale or just a dll
        /// </summary>
        public string ExecuteMode { get; private set; }

        /// <summary>
        /// You can make the script compiled in memory,no file will be generated
        /// </summary>
        public bool GenerateInMemory { get; private set; }

        /// <summary>
        /// show if the script is executabale or just a dll
        /// </summary>
        public bool Executable { get; private set; }

        /// <summary>
        /// If the script is generate as a dll,we get the assembly
        /// </summary>
        public Assembly ScriptAssembly { get; private set; }

        /// <summary>
        /// the compile error message
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// projectName
        /// </summary>
        public string ProjectName { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// PropertyInit
        /// </summary>
        /// <param name="config"></param>
        private void Init(XElement config) {
            ProjectName = config.Attribute(ProjectNamePara).Value;
            Executable = ExecutableDic[config.Attribute(ExecuteModePara).Value];
            ExecuteMode = ExecuteModeDic[config.Attribute(ExecuteModePara).Value];
            Preference = LangugeDic[config.Attribute(PreferencePara).Value];
            GenerateInMemory = Boolean.Parse(config.Attribute(GenerateInMemoryPara).Value);
        }

        /// <summary>
        /// Run the Script
        /// </summary>
        /// <param name="scripts"></param>
        private void RunScripts(string[] scripts) {
            try {
                CodeDomProvider codeProvider = CodeDomProvider.CreateProvider(Preference);
                System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
                if (GenerateInMemory) { parameters.GenerateInMemory = true; } else { parameters.GenerateExecutable = true; }
                foreach (var item in Framework4_0) { parameters.ReferencedAssemblies.Add(item); }
                parameters.ReferencedAssemblies.Add(DataDllPara);
                parameters.ReferencedAssemblies.Add(DataQualityDllPara);
                parameters.ReferencedAssemblies.Add(RemoteInterfaceDllPara);
                parameters.OutputAssembly = ExecuteMode;
                CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, scripts);
                CreateErrorMessage(results);
                ScriptAssembly = results.CompiledAssembly;
                //if ((!GenerateInMemory)&&(Executable)) { Process.Start(ExecuteMode); }
                //**************************************2014-08-26 FOR CMS**********************************************
                dynamic obj = Activator.CreateInstance(ScriptAssembly.GetType("Irlovan.CMS"), new object[] { _source });
            }
            catch (Exception e) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.ScriptError + e.ToString() + Symbol.NewLine_Symbol + ErrorMessage);
            }
        }

        /// <summary>
        /// Get Compile Error Message
        /// </summary>
        /// <param name="results"></param>
        private void CreateErrorMessage(CompilerResults results) {
            ErrorMessage = "";
            foreach (CompilerError CompErr in results.Errors) {
                ErrorMessage = ErrorMessage +
                            "Line number " + CompErr.Line +
                            ", Error Number: " + CompErr.ErrorNumber +
                            ", '" + CompErr.ErrorText + ";" +
                            Environment.NewLine + Environment.NewLine;
            }
            if (results.Errors.Count > 0) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.ScriptError + Symbol.NewLine_Symbol + ErrorMessage); }
        }

        /// <summary>
        /// Get All Scripts
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private string[] Scripts(XElement config) {
            string[] result = new string[config.Elements().Count()];
            int index = 0;
            foreach (var item in config.Elements()) {
                string path = Global.Info.ProjectPath + @"\" + item.Attribute(PathPara).Value;
                if (!System.IO.File.Exists(path)) {
                    Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.ScriptNotFound + Symbol.NewLine_Symbol + path);
                    continue;
                }
                result[index] = System.IO.File.ReadAllText(path);
                index++;
            }
            return result;
        }

        #endregion Function

    }
}