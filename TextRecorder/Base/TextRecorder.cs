///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Text recorder base class
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:       

using Irlovan.Database;
using Irlovan.Lib.XML;
using Irlovan.Structure;
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
        /// Init
        /// </summary>
        public override void Init() {
            base.Init();
            if (!XML.InitStringAttr<string>(Config, LocationPara, out _recorderPath)) { ErrorAttr.Add(LocationPara); InitState = false; }
            XElement clockworkConfig = Config.Element(Clockwork.RootTag);
            if (clockworkConfig == null) { InitState = false; return; }
            Engine = new Clockwork(clockworkConfig);
            if (!Engine.InitState) { InitState = false; return; }
            InitDateContainer();
        }

        /// <summary>
        /// Init DateContainer
        /// </summary>
        private void InitDateContainer() {
            DateContainer = new FolderLayer(RecorderPath, Engine, 0, null);
            DateContainer.Refresh();
        }


        #endregion Function

    }
}
