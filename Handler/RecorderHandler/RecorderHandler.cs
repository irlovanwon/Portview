///Copyright(c) 2015,HIT All rights reserved.
///Summary:Recorder handler
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using Irlovan.Canal;
using Irlovan.Lib.XML;
using Irlovan.Log;
using Irlovan.Recorder;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Handlers
{
    public class RecorderHandler : Handler
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="session"></param>
        /// <param name="local"></param>
        public RecorderHandler(IServerSession session, LocalInterface.LocalInterface local)
            : base(session, local) {
            InitFactory();
        }

        #endregion Structure

        #region Field

        //SQL Factories
        private Dictionary<string, IRecorderHandler> _factories = new Dictionary<string, IRecorderHandler>();
        internal const string TimeStampAttr = "TimeStamp";
        internal const string RecorderNameAttr = "Name";
        internal const string StartTimeAttr = "StartTime";
        internal const string EndTimeAttr = "EndTime";
        internal const string DataNameAttr = "DataName";
        internal const string EventLevelAttr = "EventLevel";
        internal const string ColumnsAttr = "Columns";
        internal const string IsDescAttr = "IsDesc";
        internal const string CountAttr = "Count";
        internal const string ValueAttr = "Value";
        internal const string IntervalAttr = "Interval";
        internal const char EventLevelSplitChar = ',';
        internal const char ColumnSplitChar = ',';

        #endregion Field

        #region Function

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool Handle(IServerSession session, string message) {
            if (!base.Handle(session, message)) { return false; }
            if (LocalInterface.Recorder == null) { return false; }
            XElement config = XML.Parse(message);
            if (config == null) { return false; }
            string factoryName = config.Name.ToString();
            if (!_factories.ContainsKey(factoryName)) { return false; }
            string recorderName;
            if (!XML.InitStringAttr<string>(config, RecorderNameAttr, out recorderName)) { return false; }
            IRecorder recorder = GetRecorder(recorderName);
            if (recorder == null) { Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.RecorderHandlerFailed + recorderName); Session.Send(CreateEmptyMessage(factoryName)); return false; }
            if (!_factories[factoryName].LoadRecorder(recorder)) { return false; }
            return _factories[factoryName].Handle(Session, config);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            foreach (var item in _factories) {
                item.Value.Dispose();
            }
        }

        /// <summary>
        /// Init Message Factory
        /// </summary>
        private void InitFactory() {
            _factories.Add(StatisticHandler.Name, new StatisticHandler(Session, LocalInterface));
            _factories.Add(EventHandler.Name, new EventHandler(Session, LocalInterface));
            _factories.Add(DataHandler.Name, new DataHandler(Session, LocalInterface));
            _factories.Add(VURHandler.Name, new VURHandler(Session, LocalInterface));
            _factories.Add(MatrixHandler.Name, new MatrixHandler(Session, LocalInterface));
        }

        /// <summary>
        /// GetEvent Recorder by recorder name
        /// </summary>
        /// <returns></returns>
        private IRecorder GetRecorder(string recorderName) {
            return (!LocalInterface.Recorder.RecorderList.ContainsKey(recorderName)) ? null : LocalInterface.Recorder.RecorderList[recorderName];
        }

        /// <summary>
        /// Create Empty Message
        /// </summary>
        /// <returns></returns>
        private string CreateEmptyMessage(string factoryName) {
            XElement result = new XElement(factoryName);
            return result.ToString();
        }

        #endregion Function

    }
}
