///Copyright(c) 2015,HIT All rights reserved.
///Summary:VUR handler
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:      

using Irlovan.Canal;
using Irlovan.Lib.XML;
using Irlovan.Recorder;
using Irlovan.VUR;
using System;
using System.Globalization;
using System.Xml.Linq;

namespace Irlovan.Handlers
{
    internal class VURHandler : BaseHandler
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="session"></param>
        /// <param name="local"></param>
        internal VURHandler(IServerSession session, LocalInterface.LocalInterface local)
            : base(session, local) { }

        #endregion Structure

        #region Field

        internal const string Name = "VURHandler";
        internal const string CommandAttr = "Command";
        private Cache _cache;
        private enum CommandEnum { Play, Pause }

        #endregion Field

        #region Delegate

        /// <summary>
        /// For replay operation like Play and Pause
        /// </summary>
        /// <param name="command"></param>
        /// <param name="element"></param>
        private delegate void ReplayHandler(CommandEnum command, XElement element);

        #endregion Delegate

        #region Function

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="session"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public override bool Handle(IServerSession session, XElement element) {
            if (!base.Handle(session, element)) { return false; }
            CommandEnum command;
            DisposeVURCache();
            if (!XML.InitStringAttr<CommandEnum>(element, CommandAttr, out command)) { return false; }
            ReplayHandler play = new ReplayHandler(Play);
            play.BeginInvoke(command, element, null, null);
            Pause(command, element);
            return true;
        }

        /// <summary>
        /// Load Recorder
        /// </summary>
        /// <param name="recorder"></param>
        /// <returns></returns>
        public override bool LoadRecorder(IRecorder recorder) {
            if (!base.LoadRecorder(recorder)) { return false; }
            if (!(recorder is IVURRecorder)) { return false; }
            DisposeVURCache();
            Recorder = recorder;
            return true;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            DisposeVURCache();
        }

        /// <summary>
        /// Dispose VUR Cache
        /// </summary>
        private void DisposeVURCache() {
            if (_cache == null) { return; }
            _cache.DataReplay -= DataReplayHandler;
            _cache.Dispose();
        }

        /// <summary>
        /// Play VUR
        /// </summary>
        /// <param name="command"></param>
        /// <param name="element"></param>
        private void Play(CommandEnum command, XElement element) {
            if (command != CommandEnum.Play) { return; }
            string timeStampStr; DateTime timeStamp; double interval;
            if (!XML.InitStringAttr<string>(element, RecorderHandler.TimeStampAttr, out timeStampStr)) { return; }
            if (!XML.InitStringAttr<double>(element, RecorderHandler.IntervalAttr, out interval)) { return; }
            if ((!DateTime.TryParse(timeStampStr, LocalInterface.Config.RecorderQueryCulture, DateTimeStyles.None, out timeStamp)) && (!DateTime.TryParseExact(timeStampStr, ((IVURRecorder)Recorder).DateFormat, LocalInterface.Config.RecorderQueryCulture, DateTimeStyles.None, out timeStamp))) { return; }
            DisposeVURCache();
            InitVURCache();
            _cache.Play(timeStamp, (int)interval);
        }

        /// <summary>
        /// Pause VUR
        /// </summary>
        /// <param name="command"></param>
        /// <param name="element"></param>
        private void Pause(CommandEnum command, XElement element) {
            if (command != CommandEnum.Pause) { return; }
            if (_cache == null) { return; }
            _cache.Pause();
        }

        /// <summary>
        /// Init VUR Cache
        /// </summary>
        /// <param name="vurCache"></param>
        /// <param name="currentIndex"></param>
        private void InitVURCache() {
            _cache = new Cache((IVURRecorder)Recorder, LocalInterface.Config.RecorderQueryCulture);
            _cache.DataReplay += DataReplayHandler;
        }

        /// <summary>
        /// Data Replay Handler
        /// </summary>
        /// <param name="replayCache"></param>
        private void DataReplayHandler(XElement replayCache) {
            Session.Send(CreateVURMessage(replayCache).ToString());
        }

        /// <summary>
        /// Create VUR Message
        /// </summary>
        /// <returns></returns>
        private XElement CreateVURMessage(XElement message) {
            XElement result = new XElement(Name);
            result.Add(message);
            return result;
        }

        #endregion Function

    }
}
