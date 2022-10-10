///Copyright(c) 2016,HIT All rights reserved.
///Summary:Record history data as text format.
///Author:Irlovan
///Date: 2016-06-12
///Description:
///Modification:

using HIT.Layer;

namespace Irlovan.Recorder.TextRecorder
{
    public interface ITextRecorder
    {

        #region Property

        /// <summary>
        /// the path of the recorder
        /// </summary>
        string RecorderPath { get; }

        /// <summary>
        /// Engine of the recorder
        /// </summary>
        IInfo Info { get; }

        /// <summary>
        /// Date Layer
        /// </summary>
        ILayer DateLayer { get; }

        #endregion Property

    }
}
