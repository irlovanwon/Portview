///Copyright(c) 2015,HIT All rights reserved.
///Summary:GUI Source
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using Irlovan.Canal;
using Irlovan.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Irlovan.Handlers
{
    public class GUISource
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="localInterface"></param>
        /// <param name="session"></param>
        internal GUISource(LocalInterface.LocalInterface localInterface, IServerSession session) {
            _localInterface = localInterface;
            _session = session;
        }

        #endregion Structure

        #region Field

        private object _lock = new object();
        private Dictionary<string, XElement> _source = new Dictionary<string, XElement>();
        private IServerSession _session;
        private LocalInterface.LocalInterface _localInterface;
        internal const string PagePara = "Page";
        private const string PathSplitStr = @"\";
        private const string PathSplitStrFixed = @"/";

        #endregion Field

        #region Property

        /// <summary>
        /// Source of GUI info
        /// </summary>
        internal Dictionary<string, XElement> Source {
            get { return _source; }
            private set {
                if (value != _source) {
                    _source = value;
                }
            }
        }

        #endregion Property

        #region Function

        /// <summary>
        /// Init GUI Source
        /// </summary>
        internal void Update(List<string> pathList) {
            lock (_lock) {
                Dictionary<string, XElement> source = InitSource(pathList);
                Dispose();
                _source = source;
            }
        }

        /// <summary>
        /// Refresh GUI Source
        /// </summary>
        internal void Refresh() {
            lock (_lock) {
                Dictionary<string, XElement> source = RefreshSource();
                Dispose();
                _source = source;
            }
        }


        /// <summary>
        /// Get GUI Source
        /// </summary>
        internal Dictionary<string, XElement> GetSource() {
            lock (_lock) {
                Dictionary<string, XElement> result = new Dictionary<string, XElement>();
                foreach (var item in _source) {
                    result.Add(item.Key, item.Value);
                }
                return result;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        internal void Dispose() {
            _source.Clear();
        }

        /// <summary>
        /// Refresh GUI Source
        /// </summary>
        /// <returns>GUI Source</returns>
        private Dictionary<string, XElement> RefreshSource() {
            Dictionary<string, XElement> result = new Dictionary<string, XElement>();
            foreach (var item in _source) {
                XElement pageElement = LoadGUIFromHD(item.Key);
                if (pageElement == null) { continue; }
                if (pageElement.Name != PagePara) { continue; }
                result.Add(item.Key, pageElement);
            }
            return result;
        }

        /// <summary>
        /// Create GUI Source
        /// </summary>
        /// <returns>GUI Source</returns>
        private Dictionary<string, XElement> InitSource(List<string> pathList) {
            Dictionary<string, XElement> result = new Dictionary<string, XElement>();
            foreach (var path in pathList) {
                XElement pageElement = LoadGUIFromCache(path);
                if (pageElement == null) { pageElement = LoadGUIFromHD(path); }
                if (pageElement == null) { continue; }
                if (pageElement.Name != PagePara) { continue; }
                result.Add(path, pageElement);
            }
            return result;
        }

        /// <summary>
        /// Load GUI From Cache
        /// </summary>
        private XElement LoadGUIFromCache(string path) {
            return _source.ContainsKey(path) ? _source[path] : null;
        }

        /// <summary>
        /// Load GUI From HD
        /// </summary>
        private XElement LoadGUIFromHD(string path) {
            string filePath = Info.GUIPath + PathSplitStrFixed + path;
            if (!File.Exists(filePath)) { return null; }
            try {
                return XElement.Load(new StringReader(File.ReadAllText(filePath, Encoding.UTF8)));
            }
            catch (Exception) {
                return null;
            }
        }

        #endregion Function

    }
}
