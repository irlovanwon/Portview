///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Textrecorder interface
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using Irlovan.Structure;

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
        Clockwork Engine { get; }

        /// <summary>
        /// Date Container
        /// </summary>
        IFolderLayer DateContainer { get; }

        #endregion Property

    }
}
