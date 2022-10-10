///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:      

using Irlovan.Canal;
using Irlovan.Message;
using System.Xml.Linq;

namespace Irlovan.Handlers
{
    internal class DatabaseMessage : Handler
    {

        #region Structure

        /// <summary>
        /// Handler for realtime Event
        /// </summary>
        internal DatabaseMessage(IServerSession session, LocalInterface.LocalInterface local)
            : base(session, local) {
        }

        #endregion Structure

        #region Field

        internal const string Name = "Database";
        internal const string DatabaseItemPara = "Item";

        #endregion Field

        #region Function

        /// <summary>
        /// Handler message
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool Handle(IServerSession session, XElement message) {
            if (!base.Handle(session, message)) { return false; };
            Session.Send(GetAllData());
            return true;
        }

        /// <summary>
        /// Get All Data
        /// </summary>
        /// <returns></returns>
        private string GetAllData() {
            XElement result = new XElement(Name);
            foreach (var item in LocalInterface.Source.AcquireAll(true)) {
                string value = (item.Value == null) ? string.Empty : item.Value.ToString();
                XElement dataMessage = (new IndustryDataMessage(item.FullName, value, item.DataType, item.TimeStamp, item.Description, item.Quality)).ToXML(FormatEnum.Typic);
                dataMessage.Name = DatabaseItemPara;
                result.Add(dataMessage);
            }
            return result.ToString();
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