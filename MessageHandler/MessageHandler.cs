///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Handler for handling Message exchange
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
    public class MessageHandler : Handler
    {

        #region Structure

        /// <summary>
        /// Construct
        /// </summary>
        /// <param name="session"></param>
        /// <param name="local"></param>
        public MessageHandler(IServerSession session, LocalInterface.LocalInterface local)
            : base(session, local) {
            InitFactory();
        }

        #endregion Structure

        #region Field

        //Message Factories
        private Dictionary<string, IHandler> _factories = new Dictionary<string, IHandler>();

        #endregion Field

        #region Function

        /// <summary>
        /// handle sql command
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool Handle(IServerSession session, string message) {
            if (!base.Handle(session, message)) { return false; }
            if (LocalInterface.Source == null) { return false; }
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
            _factories.Add(DataMessage.Name, new DataMessage(Session, LocalInterface));
            _factories.Add(DatabaseMessage.Name, new DatabaseMessage(Session, LocalInterface));
        }

        /// <summary>
        /// Dispose for the handler
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            DisposeFactory();
        }

        /// <summary>
        /// Dispose all factories
        /// </summary>
        private void DisposeFactory() {
            foreach (var item in _factories) {
                item.Value.Dispose();
            }
        }

        #endregion Function

    }
}
