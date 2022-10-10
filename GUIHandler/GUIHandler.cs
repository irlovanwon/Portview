///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:GUI Handler
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:      

using Irlovan.Canal;
using Irlovan.Lib.XML;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Handlers
{
    public class GUIHandler : Handler
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="session"></param>
        /// <param name="local"></param>
        public GUIHandler(IServerSession session, LocalInterface.LocalInterface local)
            : base(session, local) {
            _guiSource = new GUISource(LocalInterface, Session);
            InitFactory();
        }

        #endregion Structure

        #region Field

        private GUISource _guiSource;
        //GUI Factories
        private Dictionary<string, IHandler> _factories = new Dictionary<string, IHandler>();

        #endregion Field

        #region Function

        /// <summary>
        /// Handler GUI message
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool Handle(IServerSession session, string message) {
            if (!base.Handle(session, message)) { return false; }
            XElement config = XML.Parse(message);
            if (config == null) { return false; }
            string factoryName = config.Name.ToString();
            if (!_factories.ContainsKey(factoryName)) { return false; }
            return _factories[factoryName].Handle(Session, config);
        }

        /// <summary>
        /// Init Message Factory
        /// </summary>
        private void InitFactory() {
            _factories.Add(Loader.Name, new Loader(Session, LocalInterface, _guiSource));
            _factories.Add(Recorder.Name, new Recorder(Session, LocalInterface, _guiSource));
        }

        /// <summary>
        /// Dispose for the handler
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            foreach (var item in _factories) {
                item.Value.Dispose();
            }
            _guiSource.Dispose();
        }

        #endregion Function

    }
}
