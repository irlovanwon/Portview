///Copyright(c) 2016,HIT All rights reserved.
///Summary:Text recorder base class
///Author:Irlovan
///Date:2016-06-12
///Description:
///Modification:       

using HIT.Layer;
using Irlovan.Database;
using Irlovan.Lib.XML;
using System.IO;
using System.Xml.Linq;

namespace Irlovan.Recorder.TextRecorder
{
    public class TextRecorder : Recorder, ITextRecorder
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="source"></param>
        /// <param name="config"></param>
        public TextRecorder(Catalog source, XElement config) : base(source, config) { }

        #endregion Structure

        #region Field

        private const string LocationPara = "Location";
        private string _recorderPath;

        #endregion Field

        #region Property

        /// <summary>
        /// the path of the recorder
        /// </summary>
        public string RecorderPath {
            get { return _recorderPath; }
            private set {
                if (value != _recorderPath) {
                    _recorderPath = value;
                }
            }
        }

        /// <summary>
        /// Date info
        /// </summary>
        public IInfo Info { get; private set; }

        /// <summary>
        /// Date Layer
        /// </summary>
        public ILayer DateLayer { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Init
        /// </summary>
        public override void Init() {
            base.Init();
            if (!XML.InitStringAttr<string>(Config, LocationPara, out _recorderPath)) { ErrorAttr.Add(LocationPara); InitState = false; }
            if (!Directory.Exists(_recorderPath)) { Directory.CreateDirectory(_recorderPath); }
            XElement layerInfo = Config.Element(HIT.Layer.Info.TagName);
            if (layerInfo == null) { InitState = false; return; }
            Info = new Info();
            Info.ReadXML(layerInfo);
            if (!Info.IsValid) { InitState = false; return; }
            DirectoryInfo directory = new DirectoryInfo(_recorderPath);
            DateLayer = new Layer(directory, Info);
            DateLayer.Refresh();
            DateLayer.CheckSize();
        }

        #endregion Function

    }
}
