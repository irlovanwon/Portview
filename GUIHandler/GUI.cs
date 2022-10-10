///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:GUI Handler
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using Irlovan.Canal;
using Irlovan.Global;

namespace Irlovan.Handlers
{
    internal class GUI : Handler
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="session"></param>
        /// <param name="local"></param>
        /// <param name="source"></param>
        internal GUI(IServerSession session, LocalInterface.LocalInterface local, GUISource source)
            : base(session, local) {
            Source = source;
        }

        #endregion Structure

        #region Field

        internal const string SubcriptionPara = "SBC";
        internal const string SavePara = "Save";
        internal const string PagePara = "Page";
        internal const string ItemPara = "Item";
        internal const string PathAttr = "Path";
        internal const string InitPara = "Init";
        internal const string StateAttr = "State";

        //millisecond during sending GUI
        internal const int SendPageDelay = 20;

        #endregion Field

        #region Property

        /// <summary>
        /// GUI Source
        /// </summary>
        internal GUISource Source { get; set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            DisposeGUISource();
        }

        /// <summary>
        /// Dispose GUI Source
        /// </summary>
        internal void DisposeGUISource() {
            Source.Dispose();
        }

        /// <summary>
        /// Get Relative Path
        /// </summary>
        internal string GetRelativePath(string fullPath) {
            return fullPath.Replace(Info.GUIPath, string.Empty);
        }

        #endregion Function

    }
}
