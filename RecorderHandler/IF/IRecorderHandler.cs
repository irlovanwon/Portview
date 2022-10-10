//Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Recorder handler interface
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using Irlovan.Recorder;

namespace Irlovan.Handlers
{
    internal interface IRecorderHandler : IHandler
    {

        #region Property

        /// <summary>
        /// Recorder
        /// </summary>
        IRecorder Recorder { get; set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Load Recorder
        /// </summary>
        /// <returns></returns>
        bool LoadRecorder(IRecorder recorder);

        #endregion Function

    }
}
