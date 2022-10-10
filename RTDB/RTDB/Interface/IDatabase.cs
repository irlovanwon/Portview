///Copyright(c) 2015,HIT All rights reserved.
///Summary:Basic features for realtime database
///Author:Irlovan
///Date:2015-01-29
///Description:
///Modification:2015-08-04
///Modification:2015-11-05

using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Database
{
    public interface IDatabase
    {

        #region Property

        /// <summary>
        /// Catalog for the data
        /// </summary>
        Catalog Parent { get; set; }

        /// <summary>
        /// Full name of the data
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Name of the data
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Description of the data
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// InitState to show if the data inited in proper way from xml file
        /// </summary>
        bool InitState { get; set; }

        /// <summary>
        /// Data Species
        /// </summary>
        string Species { get; set; }

        /// <summary>
        ///List to collect error property para
        /// </summary>
        List<string> ErrorParaList { get; set; }

        /// <summary>
        /// Child Element(Calalog/Data)
        /// </summary>
        Dictionary<string, IDatabase> ChildDic { get; set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Get Database from xml config file
        /// </summary>
        /// <param name="element"></param>
        void ReadXML(XElement element);

        /// <summary>
        /// Write Database to xml config file
        /// </summary>
        /// <returns></returns>
        XElement WriteXML();

        /// <summary>
        /// Init
        /// </summary>
        void Init();

        /// <summary>
        /// Remove this data
        /// </summary>
        bool Remove();

        #endregion Function

    }

}
