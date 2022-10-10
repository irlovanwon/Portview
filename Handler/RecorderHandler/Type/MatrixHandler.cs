///Copyright(c) 2015,HIT All rights reserved.
///Summary:Matrix recorder handler
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
    internal class MatrixHandler : BaseHandler
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="session"></param>
        /// <param name="local"></param>
        internal MatrixHandler(IServerSession session, LocalInterface.LocalInterface local)
            : base(session, local) { }

        #endregion Structure

        #region Field

        internal const string Name = "MatrixRecorderHandler";

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
            string startTimeStr; string endTimeStr; object amount = null; string[] columns = null;
            DateTime startTime, endTime;
            if (!XML.InitStringAttr<string>(element, RecorderHandler.StartTimeAttr, out startTimeStr)) { return false; }
            if (!XML.InitStringAttr<string>(element, RecorderHandler.EndTimeAttr, out endTimeStr)) { return false; }
            XML.InitStringAttr<object>(element, RecorderHandler.CountAttr, out amount);
            string columnsStr = string.Empty;
            XML.InitStringAttr<string>(element, RecorderHandler.ColumnsAttr, out columnsStr);
            if (!string.IsNullOrEmpty(columnsStr)) { columns = columnsStr.Split(RecorderHandler.ColumnSplitChar); }
            IMatrixRecorder matrixRecorder = (IMatrixRecorder)Recorder;
            if (!DateTime.TryParse(startTimeStr, LocalInterface.Config.RecorderQueryCulture, DateTimeStyles.None, out startTime)) { return false; }
            if (!DateTime.TryParse(endTimeStr, LocalInterface.Config.RecorderQueryCulture, DateTimeStyles.None, out endTime)) { return false; }
            MatrixArray<string> matrixArray = matrixRecorder.Read(startTime, endTime, amount, columns);
            if (matrixArray == null) { return false; }
            Session.Send(CreateMessage(matrixArray));
            return true;
        }

        /// <summary>
        /// Load Recorder
        /// </summary>
        /// <param name="recorder"></param>
        /// <returns></returns>
        public override bool LoadRecorder(IRecorder recorder) {
            if (!base.LoadRecorder(recorder)) { return false; }
            if (!(recorder is IMatrixRecorder)) { return false; }
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
