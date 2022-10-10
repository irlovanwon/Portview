///Copyright(c) 2015,Irlovan All rights reserved.
///Summary:Layer
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:
      
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Irlovan.Structure
{
    public class Layer : ILayer
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="clockwork"></param>
        /// <param name="index"></param>
        public Layer(string name, Clockwork clockwork, int index, ILayer parent = null) {
            Name = name;
            Parent = parent;
            Clockwork = clockwork;
            Index = index;
            Init();
        }

        #endregion Structure

        #region Field

        private DateTime _timeStamp;

        #endregion Field

        #region Property

        /// <summary>
        /// The name of the layer
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Path of the layer
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Parent folder of the layer
        /// </summary>
        public ILayer Parent { get; private set; }

        /// <summary>
        /// Time stamp of the layer
        /// </summary>
        public DateTime TimeStamp {
            get { return _timeStamp; }
            private set {
                if (value != _timeStamp) {
                    _timeStamp = value;
                }
            }
        }

        /// <summary>
        /// DateFormat of the layer
        /// </summary>
        public string DateFormat { get; private set; }

        /// <summary>
        /// InitState
        /// </summary>
        public bool InitState { get; private set; }

        /// <summary>
        /// Clockwork of the layer
        /// </summary>
        public Clockwork Clockwork { get; private set; }

        /// <summary>
        /// Layer Index
        /// </summary>
        public int Index { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// If the layer exists
        /// </summary>
        /// <returns></returns>
        public virtual bool Exist() { return false; }

        /// <summary>
        /// Delete the layer
        /// </summary>
        /// <returns></returns>
        public virtual bool Delete() { return false; }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose() { }

        /// <summary>
        /// Init Layer
        /// </summary>
        public virtual void Init() {
            InitState = true;
            InitPath();
            InitTimeStamp();
            InitClockwork();
        }

        /// <summary>
        /// Init Clockwork
        /// </summary>
        private void InitClockwork() {
            DateFormat = Clockwork[Index].DateFormat;
        }

        /// <summary>
        /// Init Path
        /// </summary>
        private void InitPath() {
            string parentPath = (Parent == null) ? string.Empty : Parent.Path;
            try { Path = System.IO.Path.Combine(parentPath, Name); }
            catch (Exception) { InitState = false; }
        }

        /// <summary>
        /// Init timeStamp
        /// </summary>
        private void InitTimeStamp() {
            if (!DateTime.TryParseExact(Name, DateFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out _timeStamp)) { InitState = false; }
        }

        /// <summary>
        /// Append All Lines
        /// </summary>
        /// <returns></returns>
        public virtual bool AppendAllLines(DateTime timeStamp, IEnumerable<string> lines) { return false; }

        /// <summary>
        /// Append All Text
        /// </summary>
        /// <returns></returns>
        public virtual bool AppendAllText(DateTime timeStamp, string text) { return false; }

        /// <summary>
        /// Read All Lines
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public virtual IEnumerable<string> ReadAllLines(DateTime timeStamp) { return null; }

        /// <summary>
        /// Read All Text
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public virtual string ReadAllText(DateTime timeStamp) { return null; }

        #endregion Function

    }
}
