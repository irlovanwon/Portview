///Copyright(c) 2015,HIT All rights reserved.
///Summary:CMD handler
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using Irlovan.Canal;
using Irlovan.Lib.CMD;
using Irlovan.Lib.Symbol;
using Irlovan.Lib.XML;
using System.Xml.Linq;

namespace Irlovan.Handlers
{

    public class CMDHandler : Handler
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="session"></param>
        /// <param name="local"></param>
        public CMDHandler(IServerSession session, LocalInterface.LocalInterface local)
            : base(session, local) {
        }

        #endregion Structure

        #region Field

        private const string RootTag = "MSCMD";
        private const string CommandAttr = "Command";

        #endregion Field

        #region Function

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool Handle(IServerSession session, string message) {
            if (!base.Handle(session, message)) { return false; }
            XElement config = XML.Parse(message);
            if (config == null) { return false; }
            if (config.Name != RootTag) { return false; }
            string commandString;
            if (!XML.InitStringAttr<string>(config, CommandAttr, out commandString)) { return false; }
            CMD.ExecuteCommandSync(commandString.Replace(Symbol.Quot_Symbol, Symbol.Quot_Char.ToString()));
            return true;
        }

        #endregion Function

    }
}
