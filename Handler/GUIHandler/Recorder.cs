///Copyright(c) 2015,HIT All rights reserved.
///Summary:Recorder
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:      

using Irlovan.Canal;
using Irlovan.Lib.Symbol;
using Irlovan.Lib.XML;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Handlers
{
    class Recorder : GUI
    {

        #region Structure

        /// <summary>
        /// Handler for realtime Event
        /// </summary>
        internal Recorder(IServerSession session, LocalInterface.LocalInterface local, GUISource source)
            : base(session, local, source) {
        }

        #endregion Structure

        #region Field

        internal const string Name = "Recorder";

        #endregion Field

        #region Function

        /// <summary>
        /// Handler String
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool Handle(IServerSession session, XElement element) {
            if (element.Name != Name) { return false; }
            ParseSave(element);
            return true;
        }

        /// <summary>
        /// Save GUI to HD
        /// </summary>
        private void ParseSave(XElement element) {
            XElement save = element.Element(SavePara);
            if (save == null) { return; }
            IEnumerable<XElement> elements = save.Elements(PagePara);
            Dictionary<string, bool> saveResult = new Dictionary<string, bool>();
            foreach (var item in elements) {
                SaveElement(item, saveResult);
            }
            Session.Send(CreateSaveMessage(saveResult).ToString());
            Source.Refresh();
        }

        /// <summary>
        /// save element to HD
        /// </summary>
        private void SaveElement(XElement element, Dictionary<string, bool> saveResult) {
            string pagePath;
            if (!XML.InitStringAttr<string>(element, PathAttr, out pagePath)) { return; }
            try {
                System.IO.File.WriteAllText(Global.Info.GUIPath + Symbol.Catalog_Char + pagePath, element.ToString());
                saveResult.Add(pagePath, true);
            }
            catch (System.Exception) {
                saveResult.Add(pagePath, false);
            }
        }

        /// <summary>
        /// Create save result state Message
        /// </summary>
        /// <param name="saveResult">page save success or fail</param>
        private XElement CreateSaveMessage(Dictionary<string, bool> saveResult) {
            XElement result = new XElement(Name);
            XElement save = new XElement(SavePara);
            foreach (var item in saveResult) {
                XElement page = new XElement(PagePara);
                page.SetAttributeValue(StateAttr, item.Value);
                page.SetAttributeValue(PathAttr, GetRelativePath(item.Key));
                save.Add(page);
            }
            result.Add(save);
            return result;
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
