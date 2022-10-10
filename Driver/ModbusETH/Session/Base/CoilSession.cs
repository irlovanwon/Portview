///Copyright(c) 2015,HIT All rights reserved.
///Summary：Holding Register Session
///Author：Irlovan
///Date：2015-06-14
///Description：
///Modification：      

using Modbus.Device;
using System.Collections.Generic;

namespace Irlovan.Driver
{
    internal class CoilSession : Session
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        internal CoilSession(ModbusIpMaster master, int startAddress, int sessionLength)
            : base(master, startAddress, sessionLength) {
        }

        #endregion Structure


        #region Field

        internal new const int MaxSessionLength = 1968;
        internal new const int MinSessionLength = 1;

        #endregion Field

        #region Property

        internal List<Coil> MDataList = new List<Coil>();

        #endregion Property

        #region Function

        /// <summary>
        /// Check if the session is valid
        /// </summary>
        internal override void CheckValid() {
            base.CheckValid();
        }

        /// <summary>
        /// Read Received Data
        /// </summary>
        internal void ReadReceivedData(bool[] nResult) {
            foreach (var item in MDataList) {
                item.Read(nResult[item.StartAddress - StartAddress]);
            }
        }

        /// <summary>
        /// Quality Bad
        /// </summary>
        internal void QualityBad() {
            foreach (var item in MDataList) {
                item.Data.Quality = DataQuality.QualityEnum.Bad;
            }
        }

        #endregion Function

    }
}
