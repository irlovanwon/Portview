///Copyright(c) 2015,HIT All rights reserved.
///Summary:Loader for loading GUI
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using Irlovan.Canal;
using Irlovan.Lib.Symbol;
using Irlovan.Lib.XML;
using Irlovan.Log;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;

namespace Irlovan.Handlers
{
    internal class Loader : GUI
    {

        #region Structure

        /// <summary>
        /// Handler for realtime Event
        /// </summary>
        internal Loader(IServerSession session, LocalInterface.LocalInterface local, GUISource source)
            : base(session, local, source) {
        }

        #endregion Structure

        #region Field

        internal const string Name = "Loader";

        #endregion Field

        #region Function

        /// <summary>
        /// Handler String
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool Handle(IServerSession session, XElement element) {
            ParseSubcription(element);
            return true;
        }

        /// <summary>
        /// Parse Subcription List
        /// </summary>
        private void ParseSubcription(XElement element) {
            XElement sbc = element.Element(SubcriptionPara);
            if (sbc == null) { return; }
            List<string> pathList = new List<string>();
            foreach (var item in sbc.Elements(PagePara)) {
                string pagePath = ParseSubcriptionPage(item);
                if (string.IsNullOrEmpty(pagePath)) { Global.Info.LogRecorder.Log(LogLevelEnum.Warn, Irlovan.Lib.Properties.Resources.LoadGUIFailed + Symbol.NewLine_Symbol + item.ToString()); continue; }
                pathList.Add(pagePath);
            }
            if (pathList.Count == 0) { return; }
            Source.Update(pathList);
            SendGUISource();
        }

        /// <summary>
        /// Send GUI Source
        /// </summary>
        private void SendGUISource() {
            Dictionary<string, XElement> source = Source.GetSource();
            foreach (var item in source) {
                Session.Send(CreateGUIMessage(GetRelativePath(item.Key), item.Value).ToString());
                Thread.Sleep(SendPageDelay);
            }
        }

        /// <summary>
        /// Create GUI Message
        /// </summary>
        /// <returns></returns>
        private XElement CreateGUIMessage(string path, XElement message) {
            XElement result = new XElement(Name);
            XElement sbc = new XElement(SubcriptionPara);
            XElement page = message;
            page.SetAttributeValue(PathAttr, path);
            sbc.Add(page);
            result.Add(sbc);
            return result;
        }

        /// <summary>
        /// Parse Subcription Page
        /// </summary>
        /// <param name="page"></param>
        private string ParseSubcriptionPage(XElement page) {
            string pagePath;
            if (!XML.InitStringAttr<string>(page, PathAttr, out pagePath)) { return null; }
            return pagePath;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
        }

        #endregion Function

    }
}
