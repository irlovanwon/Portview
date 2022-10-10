///Copyright(c) 2015,HIT All rights reserved.
///Summary:Recorder interface
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using Irlovan.Database;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Recorder
{
    public interface IRecorder : IDisposable
    {

        #region Property

        /// <summary>
        /// A property shows if all the properties of recorder has been initiated
        /// </summary>
        bool InitState { get; set; }

        /// <summary>
        /// A list of attributes whose initiation are not successfull
        /// </summary>
        List<string> ErrorAttr { get; set; }

        /// <summary>
        /// Name of the record,Uniq
        /// </summary>
        string RecorderName { get; }

        /// <summary>
        /// All Datas 
        /// </summary>
        Catalog Source { get; set; }

        /// <summary>
        /// Config file for the very recorder
        /// </summary>
        XElement Config { get; set; }

        /// <summary>
        /// Data list map coordinated by data name
        /// </summary>
        Dictionary<string, IRecorderData> NameDictionary { get; }

        /// <summary>
        /// Data list map coordinated by data id
        /// </summary>
        Dictionary<int, IRecorderData> IDDictionary { get; }

        /// <summary>
        /// Data List
        /// </summary>
        List<IRecorderData> DataList { get; set; }

        /// <summary>
        /// mode for Recorder 
        /// more than 0 mean Hybrid mode
        /// less than 0 mean Interval mode
        /// </summary>
        int Interval { get; set; }

        /// <summary>
        /// Exception reconnection timeout
        /// </summary>
        int ExceptionTimeout { get; set; }

        /// <summary>
        /// Communication Init failed then auto detecting mode is on
        /// </summary>
        int CommAutoDetectingInterval { get; set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Start to Run Recorder
        /// </summary>
        void Run();

        /// <summary>
        /// Init properties for Recorder
        /// </summary>
        void Init();

        #endregion Function

    }
}
