///Copyright(c) 2015,HIT All rights reserved.
///Summary:Recorder handler base class
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:       

using Irlovan.Canal;
using Irlovan.Recorder;

namespace Irlovan.Handlers
{
    internal class BaseHandler : Handler, IRecorderHandler
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="session"></param>
        /// <param name="local"></param>
        internal BaseHandler(IServerSession session, LocalInterface.LocalInterface local) : base(session, local) { }

        #endregion Structure

        #region Property

        /// <summary>
        /// Recorder
        /// </summary>
        public IRecorder Recorder { get; set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Load Recorder
        /// </summary>
        /// <returns></returns>
        public virtual bool LoadRecorder(IRecorder recorder) { return (recorder != null); }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
        }

        #endregion Function

    }
}
