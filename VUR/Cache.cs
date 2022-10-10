///Copyright(c) 2015,HIT All rights reserved.
///Summary:Virtual UI Replay
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:      

using Irlovan.Lib.Array;
using Irlovan.Lib.XML;
using Irlovan.Recorder;
using System;
using System.Globalization;
using System.Timers;
using System.Xml.Linq;

namespace Irlovan.VUR
{
    public class Cache
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        public Cache(IVURRecorder recorder, CultureInfo dateCulture) {
            Recorder = recorder;
            DateCulture = dateCulture;
        }

        #endregion Structure

        #region Field

        private bool _isPlaying = false;
        private System.Timers.Timer _queryTimer;
        internal const string Name = "VUR";
        internal const string TimeStampAttr = "TimeStamp";
        //unit:second 
        private const double _defaultReplayInterval = 1;

        #endregion Field

        #region Property

        /// <summary>
        /// IVURRecorder Recorder
        /// </summary>
        public IVURRecorder Recorder { get; private set; }

        /// <summary>
        /// Date time Culture
        /// </summary>
        public CultureInfo DateCulture { get; private set; }

        #endregion Property

        #region Delegate

        public delegate void DataReplayEventHandler(XElement cache);

        #endregion Delegate

        #region Event

        public event DataReplayEventHandler DataReplay;

        #endregion Event

        #region Function

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() {
            DisposeTimer();
            DisposeReplay();
        }

        /// <summary>
        /// Play VUR
        /// </summary>
        /// <param name="command"></param>
        /// <param name="element"></param>
        public void Play(DateTime timeStamp, int interval) {
            Dispose();
            _isPlaying = true;
            Start(timeStamp, interval);
        }

        /// <summary>
        /// Start VUR
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <param name="interval"></param>
        private void Start(DateTime timeStamp, int interval) {
            try {
                DisposeTimer();
                MatrixArray<string> vurCache = Recorder.ReadCache(timeStamp);
                QueryVURCache(vurCache, timeStamp, interval);
                PlayVURCache(vurCache, interval);
            }
            catch (Exception) {
                DisposeTimer();
            }
        }

        /// <summary>
        /// Pause VUR
        /// </summary>
        /// <param name="command"></param>
        /// <param name="element"></param>
        public void Pause() {
            Dispose();
        }

        /// <summary>
        /// Query VUR Cache
        /// </summary>
        /// <param name="vurCache"></param>
        /// <param name="timeStamp"></param>
        private void QueryVURCache(MatrixArray<string> vurCache, DateTime timeStamp, int interval) {
            Lib.Timer.Timer.DisposeTimer(_queryTimer);
            if (vurCache != null) { return; }
            if (DataReplay != null) { DataReplay(VURNotFoundMessage(timeStamp.ToString())); }
            int defaultInterval = (int)(_defaultReplayInterval * 1000);
            DateTime currentTimeStamp = timeStamp;
            Lib.Timer.Timer.SetInterval((object o, ElapsedEventArgs e) => {
                currentTimeStamp = currentTimeStamp.AddSeconds(_defaultReplayInterval);
                vurCache = ((IVURRecorder)Recorder).ReadCache(currentTimeStamp);
                if (vurCache != null) { PlayVURCache(vurCache, interval); return; }
                if (!_isPlaying) { Lib.Timer.Timer.DisposeTimer(_queryTimer); return; }
                if (DataReplay != null) { DataReplay(VURNotFoundMessage(currentTimeStamp.ToString())); }
            }, ref _queryTimer, defaultInterval);
        }

        /// <summary>
        /// Play VUR Cache
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="vurCache"></param>
        private void PlayVURCache(MatrixArray<string> vurCache, int interval) {
            if (vurCache == null) { return; }
            DisposeTimer();
            for (int i = 0; i < vurCache.Rows.Count; i++) {
                if (!_isPlaying) { return; }
                XElement message = vurCache.CreateRowXElement(vurCache.Rows[i]);
                string timeStampStr; DateTime timeStamp;
                if (!XML.InitStringAttr<string>(message, ((IVURRecorder)Recorder).DateTitle, out timeStampStr)) { continue; }
                if (!DateTime.TryParseExact(timeStampStr, Recorder.DateFormat, DateCulture, DateTimeStyles.None, out timeStamp)) { continue; }
                if (DataReplay != null) { DataReplay(VURMessage(message, timeStamp)); }
                System.Threading.Thread.Sleep(interval);
            }
            DateTime currentTimeStamp;
            if (!DateTime.TryParseExact(vurCache.Rows[vurCache.Rows.Count - 1][0], Recorder.DateFormat, DateCulture, DateTimeStyles.None, out currentTimeStamp)) { return; }
            if ((currentTimeStamp.CompareTo(DateTime.Now) >= 0) || (!_isPlaying)) { return; }
            Start(currentTimeStamp.AddSeconds(_defaultReplayInterval), interval);
        }

        /// <summary>
        /// Dispose Timer
        /// </summary>
        private void DisposeTimer() {
            Lib.Timer.Timer.DisposeTimer(_queryTimer);
        }

        /// <summary>
        /// DisposeReplay
        /// </summary>
        private void DisposeReplay() {
            _isPlaying = false;
        }

        /// <summary>
        /// Create VUR Not Found Message
        /// </summary>
        /// <returns></returns>
        private XElement VURNotFoundMessage(string timeStamp) {
            XElement result = new XElement(Name);
            result.SetAttributeValue(TimeStampAttr, timeStamp);
            return result;
        }

        /// <summary>
        /// Create VUR Message
        /// </summary>
        /// <returns></returns>
        private XElement VURMessage(XElement message, DateTime timeStamp) {
            XElement result = new XElement(Name);
            result.SetAttributeValue(TimeStampAttr, timeStamp);
            result.Add(message);
            return result;
        }

        #endregion Function

    }
}
