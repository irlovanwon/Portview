///Copyright(c) 2014,HIT All rights reserved.
///Summary:
///Author:Irlovan
///Date:2014-11-21
///Description:Control the level of log
///Modification:2015-11-06

using Irlovan.Lib.Symbol;
using Irlovan.Lib.XML;
using Irlovan.Structure;
using System;
using System.Xml.Linq;

namespace Irlovan.Log
{
    internal class Choker
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        internal Choker() { }

        #endregion Structure

        #region Field

        private LogLevelEnum _level;
        private bool _enable;
        private const string LevelNameAttr = "Name";
        private const string LevelEnableAttr = "Enable";

        #endregion Field

        #region Property

        /// <summary>
        /// Level of the log
        /// </summary>
        internal LogLevelEnum Level {
            get { return _level; }
            private set {
                if (value != _level) {
                    _level = value;
                }
            }
        }

        /// <summary>
        /// If the Choker opens
        /// </summary>
        internal bool Enable {
            get { return _enable; }
            set {
                if (value != _enable) {
                    _enable = value;
                }
            }
        }

        /// <summary>
        /// Engine of the recorder
        /// </summary>
        public Clockwork Engine { get; private set; }

        /// <summary>
        /// Date Container
        /// </summary>
        public IFolderLayer DateContainer { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Parse from XML
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        internal bool Parse(XElement config) {
            if (!XML.InitStringAttr<LogLevelEnum>(config, LevelNameAttr, out _level)) { return false; }
            if (!XML.InitStringAttr<bool>(config, LevelEnableAttr, out _enable)) { return false; }
            XElement clockworkConfig = config.Element(Clockwork.RootTag);
            if (clockworkConfig == null) { return false; }
            Engine = new Clockwork(clockworkConfig);
            string recorderPath = Environment.CurrentDirectory + Logger.LogPath + Symbol.Catalog_Char + _level;
            DateContainer = new FolderLayer(recorderPath, Engine, 0, null);
            DateContainer.Refresh();
            return true;
        }

        /// <summary>
        /// Push new text to log
        /// </summary>
        /// <param name="text"></param>
        internal void Push(string text) {
            if (!Enable) { return; }
            DateTime timeStamp = DateTime.Now;
            string[] message = new string[] { timeStamp.ToString(), text };
            DateContainer.AppendAllLines(DateTime.Now, message);
        }

        #endregion Function

    }
}
