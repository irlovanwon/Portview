using Irlovan.Database;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Irlovan
{
    public class LastAlive
    {
        #region Structure


        public LastAlive(Catalog source) {

            string[] craneIDList = new string[] {

                "TT670", "TT671", "TT672", "TT673", "TT674", "TT675", "TT676", "TT677", "TT678", "TT679",
                "TT680", "TT681", "TT682", "TT683", "TT684", "TT685", "TT686", "TT687", "TT688", "TT689",
                "TT01", "TT02", "TT04", "TT05", "TT06", "TT07", "TT42", "TT43", "TT46", "TT49",
                "TT59", "TT38", "TT39", "TT40", "TT47", "TT48", "TT51", "TT53", "TT54", "TT55",
                "TT56", "TT58", "TT68", "TT611", "TT601", "TT602", "TT603", "TT604", "TT605", "TT606",
                "TT607", "TT608", "TT609", "TT610", "TT612", "TT613", "TT614", "TT615", "TT616", "TT617",
                "TT619", "TT624", "TT631", "TT637", "TT638", "TT646", "TT647", "TT648", "TT649", "TT651",
                "TT652", "TT653", "TT654", "TT655", "TT656", "TT657", "TT658", "TT659", "TT661", "TT662",
                "TT669", "TT702", "TT703", "TT704", "TT705", "TT706", "TT707", "TT708", "TT709", "TT710",
                "TT711", "TT712"

            };

            List<IIndustryData<String>> lastAliveList = new List<IIndustryData<string>>();
            List<IIndustryData<Boolean>> pingList = new List<IIndustryData<bool>>();
            foreach (var item in craneIDList) {
                lastAliveList.Add(source.AcquireIndustryData<String>("HIT.FUEL." + item + "_Last"));
                pingList.Add(source.AcquireIndustryData<Boolean>("HIT.ETHComm.FUELM." + item + "F"));
            }

            foreach (var item in lastAliveList) {
                _lastAliveStrArray.Add(item);
            }

            foreach (var item in pingList) {
                _pingResultArray.Add(item);
            }

            for (int i = 0; i < _pingResultArray.Count; i++) {
                _pingCacheArray.Add(_pingResultArray[i].Value);
            }

            Timer timer;

            for (int i = 0; i < _pingResultArray.Count; i++) {
                Boolean pingResult = _pingResultArray[i].Value;
                if (pingResult) {
                    _lastAliveStrArray[i].ReadValue(_onlineStr);
                } else {
                    _lastAliveStrArray[i].ReadValue(@"'" + DateTime.Now.ToString() + @"'");
                }
                _pingCacheArray[i] = pingResult;
                //System.IO.File.AppendAllText(@"e://test.mm",tt670_LastAlive.Value);
            }

            SetInterval(1000, (object o, ElapsedEventArgs e) => {
                for (int i = 0; i < _pingResultArray.Count; i++) {
                    Boolean pingResult = _pingResultArray[i].Value;
                    if (pingResult) {
                        _lastAliveStrArray[i].ReadValue(_onlineStr);
                        _pingCacheArray[i] = pingResult;
                        continue;
                    }
                    if (pingResult == _pingCacheArray[i]) { continue; }
                    if (!pingResult) {
                        _lastAliveStrArray[i].ReadValue("'" + DateTime.Now.ToString() + "'");
                    }
                    _pingCacheArray[i] = pingResult;
                }
            }, out timer);

        }

        #endregion Structure

        private List<IIndustryData<Boolean>> _pingResultArray = new List<IIndustryData<Boolean>>();
        private List<IIndustryData<String>> _lastAliveStrArray = new List<IIndustryData<string>>();
        private List<Boolean> _pingCacheArray = new List<bool>();
        private String _onlineStr = "'ONLINE'";

        #region Function

        private void SetInterval(int interval, Action<object, ElapsedEventArgs> action, out System.Timers.Timer timer) {
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(action);
            timer.Interval = interval;
            timer.Enabled = true;
        }

        #endregion Function

    }
}