///Copyright(c) 2015,HIT All rights reserved.
///Summary:
///Author:Irlovan
///Date:2015-08-12
///Description:
///Modification:

using Irlovan.Lib.XML;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Structure
{
    public class Clockwork
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="layerInfos"></param>
        /// <param name="fileNameExtention"></param>
        public Clockwork(List<LayerInfo> layerInfos, string fileNameExtention) {
            _layerInfos = layerInfos;
            _fileNameExtention = fileNameExtention;
        }

        /// <summary>
        /// Construction from xml
        /// </summary>
        /// <param name="config"></param>
        public Clockwork(XElement config) {
            _layerInfos = new List<LayerInfo>();
            InitState = true;
            ParseXML(config);
        }

        #endregion Structure

        #region Field

        private List<LayerInfo> _layerInfos;
        private string _fileNameExtention;

        public const string RootTag = "Clockwork";
        private const string FileNameExtentionAttr = "FileNameExtention";
        private const string LayerTag = "Layer";
        private const string MaxChildCountAttr = "MaxChildCount";
        private const string DateFormatAttr = "DateFormat";

        #endregion Field

        #region Property

        /// <summary>
        /// Wind the Colockwork
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public LayerInfo this[int index] {
            get {
                index = ((index >= (_layerInfos.Count - 1)) || (index < 0)) ? (_layerInfos.Count - 1) : index;
                return _layerInfos[index];
            }
        }

        /// <summary>
        /// file name extention of file layer
        /// </summary>
        public string FileNameExtention {
            get { return _fileNameExtention; }
            private set {
                if (value != _fileNameExtention) {
                    _fileNameExtention = value;
                }
            }
        }

        /// <summary>
        /// Init state
        /// </summary>
        public bool InitState { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// If the current layer is  file group
        /// </summary>
        public bool IsFileGroup(int index) {
            return index == (_layerInfos.Count - 2);
        }

        /// <summary>
        /// if the current layer if file layer
        /// </summary>
        public bool IsFileLayer(int index) {
            return index == (_layerInfos.Count - 1);
        }

        /// <summary>
        /// Parse from XML
        /// </summary>
        private void ParseXML(XElement config) {
            if (config.Name != RootTag) { InitState = false; return; }
            if (!XML.InitStringAttr<string>(config, FileNameExtentionAttr, out _fileNameExtention)) { InitState = false; return; }
            XElement layer = config.Element(LayerTag);
            ParseLayerFromXML(layer);
            if (_layerInfos.Count < 2) { InitState = false; }
        }

        /// <summary>
        /// Parse Layer From XML
        /// </summary>
        /// <param name="config"></param>
        private void ParseLayerFromXML(XElement config) {
            if (config.Name != LayerTag) { InitState = false; return; }
            int maxChildCount; string dateFormat;
            if (!XML.InitStringAttr<int>(config, MaxChildCountAttr, out maxChildCount)) { InitState = false; return; }
            if (!XML.InitStringAttr<string>(config, DateFormatAttr, out dateFormat)) { InitState = false; return; }
            _layerInfos.Add(new LayerInfo(maxChildCount, dateFormat));
            XElement nextLayer = config.Element(LayerTag);
            if (nextLayer == null) { return; }
            ParseLayerFromXML(nextLayer);
        }

        #endregion Function

    }
}
