///Copyright(c) 2014,HIT All rights reserved.
///Summary:Logger class
///Author:Irlovan
///Date:2014-11-20
///Description:Record log to local file
///Modification:2015-11-09

using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Log
{
    public class Logger
    {

        #region Structure

        /// <summary>
        /// Logger construction
        /// </summary>
        /// <param name="choker"></param>
        /// <param name="dataChangeMode"></param>
        public Logger() {
            Parse();
        }

        #endregion Structure

        #region Field

        private const string LevelTag = "Level";
        private const string LogTag = "Log";
        private const string LogConfigPath = "\\WA.ini";
        private Dictionary<LogLevelEnum, Choker> _levelDic = new Dictionary<LogLevelEnum, Choker>();
        public const string LogPath = "\\Log";

        #endregion Field

        #region Function

        /// <summary>
        /// Parse from XML
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private void Parse() {
            XElement config = XElement.Load(System.Environment.CurrentDirectory + LogConfigPath);
            if (config == null) { return; }
            XElement logConfig = config.Element(LogTag);
            if (logConfig == null) { return; }
            IEnumerable<XElement> levelConfig = logConfig.Elements(LevelTag);
            foreach (var item in levelConfig) {
                Choker choker = new Choker();
                if (!choker.Parse(item)) { continue; }
                _levelDic.Add(choker.Level, choker);
            }
        }

        /// <summary>
        /// record log
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public void Log(LogLevelEnum level, string message) {
            if (!_levelDic.ContainsKey(level)) { return; }
            _levelDic[level].Push(message);
        }

        #endregion Function

    }
}
