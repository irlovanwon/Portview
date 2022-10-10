///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Notification interface
///Author:Irlovan
///Date:
///Description:
///Modification:      

using Irlovan.Database;
using Irlovan.Register;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Notification
{
    public interface INotification : IDisposable
    {

        #region Property

        /// <summary>
        /// A property shows if all the properties of Notification has been initiated
        /// </summary>
        bool InitState { get; set; }

        /// <summary>
        /// A list of attributes whose initiation are not successfull
        /// </summary>
        List<string> ErrorAttr { get; set; }

        /// <summary>
        /// ID of the Notification
        /// </summary>
        string ID { get; set; }

        /// <summary>
        /// All Datas 
        /// </summary>
        Catalog Source { get; set; }

        /// <summary>
        /// Config file of Notification
        /// </summary>
        XElement Config { get; set; }

        /// <summary>
        /// Data list of notification
        /// </summary>
        Dictionary<string, IIndustryData> DataList { get; set; }

        /// <summary>
        /// When message is triggered to notice by a IIndustryData variable
        /// </summary>
        IIndustryData<bool> Notice { get; set; }

        /// <summary>
        /// Register
        /// </summary>
        IRegister Register { get; }

        #endregion Property

        #region Function

        /// <summary>
        /// Start to Run Notification
        /// </summary>
        void Run();

        /// <summary>
        /// Init properties for Notification
        /// </summary>
        void Init();

        #endregion Function

    }
}
