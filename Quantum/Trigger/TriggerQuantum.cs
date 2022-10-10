///Copyright(c) 2015,HIT All rights reserved.
///Summary:Trigger quantum
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      

using Irlovan.Database;
using System.Timers;
using System.Xml.Linq;

namespace Irlovan.Quantum
{
    public class TriggerQuantum : ITriggerQuantum
    {

        #region Structure

        /// <summary>
        /// Trigger Quantum
        /// </summary>
        /// <param name="source"></param>
        public TriggerQuantum(Catalog source) {
            _source = source;
            Activator = 0;
            Mode = 0;
        }

        #endregion Structure

        #region Field

        private const string ExpressionPara = "Expression";
        private const string ModePara = "Mode";
        //refresh interval
        private int _interval = 500;
        private Catalog _source;
        private Irlovan.Expression.Expression _expression;
        private enum RefreshModeEnum { Interval, DataChange };

        private object _dataChangeLock = new object();
        private object _triggerLock = new object();
        private bool _switch = false;
        public event SwitchHandler SwitchOn;
        public event SwitchHandler SwitchOff;
        Timer _timer;

        #endregion Field

        #region Property

        /// <summary>
        /// Switch
        /// </summary>
        public bool Switch {
            get { return _switch; }
            set {
                if (_switch != value) {
                    if (value) {
                        if (SwitchOn != null) {
                            SwitchOn(this, null);
                        }
                    }
                    else {
                        if (SwitchOff != null) {
                            SwitchOff(this, null);
                        }
                    }
                    _switch = value;
                }
            }
        }

        /// <summary>
        /// Mode
        /// </summary>
        public short Mode { get; private set; }

        /// <summary>
        /// the num of activator
        /// </summary>
        public int Activator { get; set; }

        #endregion Property

        #region Function

        /// <summary>
        /// start the trigger
        /// </summary>
        public void Trigger() {
            lock (_triggerLock) {
                if (this.Activator > 0) {
                    if (Switch) { SwitchOn(null, null); }
                    return;
                }
                Switch = (bool)_expression.Eval();
                ModeSelect();
                this.Activator++;
            }
        }

        /// <summary>
        /// select the mode to refresh the data
        /// </summary>
        private void ModeSelect() {
            switch (Mode) {
                case (short)RefreshModeEnum.Interval:
                    IntervalMode();
                    break;
                case (short)RefreshModeEnum.DataChange:
                    DataChangeMode();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// StopTrigger
        /// </summary>
        public void StopTrigger() {
            lock (_triggerLock) {
                this.Activator--;
            }
            if (this.Activator != 0) { return; }
            switch (Mode) {
                case (short)RefreshModeEnum.Interval:
                    if (_timer != null) {
                        _timer.Enabled = false;
                        _timer.Stop();
                        _timer.Close();
                        _timer.Dispose();
                    }
                    break;
                case (short)RefreshModeEnum.DataChange:
                    foreach (var item in _expression.DataList.Values) {
                        item.DataChange -= DataChangeHandler;
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// trigger when datachange 
        /// </summary>
        /// <param name="dataList"></param>
        private void DataChangeMode() {
            foreach (var item in _expression.DataList.Values) {
                item.DataChange += DataChangeHandler;
            }
        }

        /// <summary>
        /// DataChangeHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataChangeHandler(object sender, DataChangeEventArgs e) {
            lock (_dataChangeLock) {
                Switch = (bool)_expression.Eval();
            }
        }

        /// <summary>
        /// trigger at interval
        /// </summary>
        private void IntervalMode() {
            Lib.Timer.Timer.SetInterval((object o, ElapsedEventArgs e) => {
                Switch = (bool)_expression.Eval();
            }, ref _timer, _interval);
        }

        /// <summary>
        /// Read XML
        /// </summary>
        /// <param name="element"></param>
        public void ReadXML(XElement element) {
            _expression = new Irlovan.Expression.Expression(element.Attribute(ExpressionPara).Value, _source);
            Mode = short.Parse(element.Attribute(ModePara).Value);
        }

        #endregion Function

    }
}
