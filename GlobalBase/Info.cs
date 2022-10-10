///Copyright(c) 2015,HIT All rights reserved.
///Summary:
///Author:Irlovan
///Date:2015-02-26
///Description:
///Modification:2015-11-06

using Irlovan.Log;
using System.Resources;

namespace Irlovan.Global
{
    public class Info
    {

        /// <summary>
        /// Global Resources
        /// </summary>
        public static ResourceManager Resource = new ResourceManager(typeof(Irlovan.Global.Properties.Resources));

        /// <summary>
        /// UI Dispatcher
        /// </summary>
        public static System.Windows.Threading.Dispatcher Dispatcher;

        /// <summary>
        /// Project Path 
        /// </summary>
        public static string ProjectPath { get; set; }

        /// <summary>
        /// DefautProjectPath
        /// </summary>
        public const string DefautProjectPath = "\\Project";

        /// <summary>
        /// GUI Path
        /// </summary>
        public static string GUIPath { get; set; }

        /// <summary>
        /// LayoutPath*****************************
        /// </summary>
        public const string LayoutPath = "\\Web\\Layout";

        /// <summary>
        /// LogRecorder
        /// </summary>
        public static Logger LogRecorder;

        /// <summary>
        /// Supported LanguageEnum
        /// </summary>
        public enum LanguageEnum { zh_CHS, en };

        /// <summary>
        /// DefaultLanguage
        /// </summary>
        public const string DefaultLanguage = "en";

        /// <summary>
        /// SystemInfoPath
        /// </summary>
        public const string SystemInfoPath = "\\WA.ini";

    }
}
