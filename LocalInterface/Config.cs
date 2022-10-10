///Copyright(c) 2015,HIT All rights reserved.
///Summary:
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      

using Irlovan.Chip;
using Irlovan.Global;
using Irlovan.Lib.Symbol;
using Irlovan.Lib.XML;
using Irlovan.Log;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml.Linq;

namespace Irlovan.LocalInterface
{
    public class Config
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        public Config(ChipLoader chip) {
            _chip = chip;
            Init();
        }

        #endregion Structure

        #region Field

        private const string CultureFileName = "\\Config";
        private const string RootName = "Config";
        private const string Lauguage_Para = "Language";
        private const string RecorderQueryCulture_Para = "RecorderQueryCulture";
        private const string Culture_Attr = "Culture";
        private const string Port_Para = "Port";
        private const string Port_Value_Para = "Value";
        private const string ProjectName_Para = "ProjectName";
        private const string ProjectName_Name_Para = "Name";
        private int _port;
        private string _projectName;
        private ChipLoader _chip;

        #endregion Field

        #region Property

        /// <summary>
        /// Recorder Query Culture
        /// </summary>
        public CultureInfo RecorderQueryCulture { get; private set; }

        /// <summary>
        /// Culture Info of WebArc
        /// </summary>
        public CultureInfo Language { get; private set; }

        /// <summary>
        /// the client allowed to access (* means allow all of clients to access if the firewall not denied)
        /// </summary>
        public int Port {
            get { return _port; }
            private set {
                if (value != _port) {
                    _port = value;
                }
            }
        }

        /// <summary>
        /// The Uniq name of a Project
        /// </summary>
        public string ProjectName {
            get { return _projectName; }
            private set {
                if (value != _projectName) {
                    _projectName = value;
                }
            }
        }

        #endregion Property

        #region Function

        /// <summary>
        /// Init
        /// </summary>
        private void Init() {
            XDocument doc = ReadConfig();
            if (doc.Root.Name != RootName) { Runtime.ServerShutDown(Lib.Properties.Resources.ConfigFileError); }
            XElement config = doc.Root;
            InitLanguage(config.Element(Lauguage_Para));
            InitRecorderQueryCulture(config.Element(RecorderQueryCulture_Para));
            InitPort(config.Element(Port_Para));
            InitProjectName(config.Element(ProjectName_Para));
        }

        /// <summary>
        /// Init language
        /// </summary>
        private void InitLanguage(XElement config) {
            if (config == null) { return; }
            string cultureLanguage;
            XML.InitStringAttr<string>(config, Culture_Attr, out cultureLanguage);
            Language = CultureConfig(cultureLanguage);
            Thread.CurrentThread.CurrentCulture = Language;
            Thread.CurrentThread.CurrentUICulture = Language;
        }

        /// <summary>
        /// Init Recorder Query Culture
        /// </summary>
        private void InitRecorderQueryCulture(XElement config) {
            if (config == null) { return; }
            string culture;
            XML.InitStringAttr<string>(config, Culture_Attr, out culture);
            RecorderQueryCulture = CultureConfig(culture);
        }

        /// <summary>
        /// Get Port Config
        /// </summary>
        /// <param name="config"></param>
        private void InitPort(XElement config) {
            if ((config == null) || (!XML.InitStringAttr<int>(config, Port_Value_Para, out _port))) { Runtime.ServerShutDown(Lib.Properties.Resources.ConfigFileError); }
        }

        /// <summary>
        /// Set Uniq name for project
        /// </summary>
        private void InitProjectName(XElement config) {
            if ((config == null) || (!XML.InitStringAttr<string>(config, ProjectName_Name_Para, out _projectName))) { Runtime.ServerShutDown(Lib.Properties.Resources.ConfigFileError); }
        }

        /// <summary>
        /// set culture property
        /// </summary>
        /// <param name="config"></param>
        private CultureInfo CultureConfig(string culture) {
            switch (culture) {
                case "en":
                case "en-US":
                case "zh-CHS":
                case "zh-CHT":
                    return CultureInfo.GetCultureInfo(culture);
                default:
                    Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.LanguageUnsupported);
                    return CultureInfo.GetCultureInfo(Global.Info.DefaultLanguage);
            }
        }

        /// <summary>
        /// read config file to xdoc
        /// </summary>
        /// <returns></returns>
        private XDocument ReadConfig() {
            string configFilePath = Global.Info.ProjectPath + CultureFileName;
            if (!File.Exists(configFilePath)) {
                Runtime.ServerShutDown(Lib.Properties.Resources.ConfigFileError + Symbol.NewLine_Symbol + Global.Info.ProjectPath);
            }
            try {
                XDocument doc = XDocument.Load(configFilePath);
                return doc;
            }
            catch (Exception e) {
                Runtime.ServerShutDown(Lib.Properties.Resources.ConfigFileError + Symbol.NewLine_Symbol + e.ToString());
                return null;
            }
        }

        #endregion Function

    }
}
