///Copyright(c) 2015,HIT All rights reserved.
///Summary:Statistic recorder handler
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:      

using Irlovan.Canal;
using Irlovan.Lib.Array;
using Irlovan.Lib.XML;
using Irlovan.Recorder;
using System;
using System.Globalization;
using System.Xml.Linq;

namespace Irlovan.Handlers
{
    internal class StatisticHandler : BaseHandler
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="session"></param>
        /// <param name="local"></param>
        internal StatisticHandler(IServerSession session, LocalInterface.LocalInterface local)
            : base(session, local) { }

        #endregion Structure

        #region Field

        internal const string Name = "StatisticRecorderHandler";

        #endregion Field

        #region Function

        /// <summary>
        /// Handler String
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool Handle(IServerSession session, XElement element) {
            if (!base.Handle(session, element)) { return false; }
            string timeStampStr;
            if (!XML.InitStringAttr<string>(element, RecorderHandler.TimeStampAttr, out timeStampStr)) { return false; }
            IStatisticRecorder statisticRecorder = (IStatisticRecorder)Recorder;
            DateTime datetime;
            if (!DateTime.TryParse(timeStampStr, LocalInterface.Config.RecorderQueryCulture, DateTimeStyles.None, out datetime)) { return false; }
            MatrixArray<string> statisticArray = statisticRecorder.Read(datetime);
            if ((statisticArray == null) || (statisticArray.Rows.Count == 0)) { return false; }
            Session.Send(CreateMessage(statisticArray));
            return true;
        }

        /// <summary>
        /// Load Recorder
        /// </summary>
        /// <param name="recorder"></param>
        /// <returns></returns>
        public override bool LoadRecorder(IRecorder recorder) {
            if (!base.LoadRecorder(recorder)) { return false; }
            if (!(recorder is IStatisticRecorder)) { return false; }
            Recorder = recorder;
            return true;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
        }

        /// <summary>
        /// Create SQL Result Message
        /// </summary>
        private string CreateMessage(MatrixArray<string> matrix) {
            XElement result = new XElement(Name);
            XElement matrixInfo = matrix.ToXML(true);
            result.Add(matrixInfo);
            return result.ToString();
        }

        #endregion Function

    }
}
