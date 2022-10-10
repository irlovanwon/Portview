///Copyright(c) 2015,HIT All rights reserved.
///Summary:RealtimeData
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using Irlovan.Database;
using System.Xml.Linq;

namespace Irlovan.LocalInterface
{
    public class RealtimeData
    {

        #region Structure

        /// <summary>
        /// RealtimeData
        /// </summary>
        public RealtimeData() {
            Source = (Catalog)RunRealtimeDatabase();
        }

        #endregion Structure

        #region Field

        private const string RealtimeDataFilePath = "\\Core\\RealtimeData";

        #endregion Field

        #region Property

        //the RealtimeData Module
        public Catalog Source { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// get custom database
        /// </summary>
        /// <returns></returns>
        private ICatalog RunRealtimeDatabase() {
            try {
                ICatalog result = new Catalog();
                result.ReadXML(XElement.Load(Global.Info.ProjectPath + RealtimeDataFilePath));
                return result;
            }
            catch {
                return null;
            }
        }

        #endregion Function

    }
}
