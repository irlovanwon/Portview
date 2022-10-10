///Copyright(c) 2013,Irlovan All rights reserved.
///Summary：SQLHandler
///Author：Irlovan
///Date：2013-05-13
///Description：
///Modification：2015-04-17

using Irlovan.Helper;
using SuperWebSocket;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Handlers
{
    public class SQLHandler : Handler
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="session"></param>
        /// <param name="local"></param>
        public SQLHandler(WebSocketSession session, LocalInterface.LocalInterface local)
            : base(session, local) {
            InitFactory();
        }

        #endregion Structure

        #region Field

        //SQL Factories
        private Dictionary<string, IHandler> _factories = new Dictionary<string, IHandler>();

        #endregion Field

        #region Function

        /// <summary>
        /// Handler GUI message
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool Handle(WebSocketSession session, string message) {
            if (!base.Handle(session, message)) { return false; }
            if (LocalInterface.Recorder == null) { return false; }
            XElement config = XML.Parse(message);
            if (config == null) { return false; }
            string factoryName = config.Name.ToString();
            if (!_factories.ContainsKey(factoryName)) { return false; }
            return _factories[factoryName].Handle(Session, config);
        }

        /// <summary>
        /// Dispose for the handler
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            foreach (var item in _factories) {
                item.Value.Dispose();
            }
        }

        /// <summary>
        /// Init Message Factory
        /// </summary>
        private void InitFactory() {
            _factories.Add(DataHandler.Name, new DataHandler(Session, LocalInterface));
            //_factories.Add(EventHandler.Name, new EventHandler(Session, LocalInterface));
            //_factories.Add(StatisticHandler.Name, new StatisticHandler(Session, LocalInterface));
        }

        #endregion Function

        #region InterClass
        #endregion InterClass

    }
}
