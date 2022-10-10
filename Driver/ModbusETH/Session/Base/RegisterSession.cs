///Copyright(c) 2015,HIT All rights reserved.
///Summary：Holding Register Session
///Author：Irlovan
///Date：2015-06-14
///Description：
///Modification：      

using Modbus.Device;
using System;
using System.Collections.Generic;

namespace Irlovan.Driver
{
    internal class RegisterSession : Session
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        internal RegisterSession(ModbusIpMaster master, int startAddress, int sessionLength)
            : base(master, startAddress, sessionLength) {
        }

        #endregion Structure


        #region Field

        internal new const int MaxSessionLength = 123;
        internal new const int MinSessionLength = 1;

        #endregion Field

        #region Property

        internal List<Register> MDataList = new List<Register>();

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
        internal void ReadReceivedData(ushort[] nResult) {
            byte[] fixedResult = new byte[nResult.Length * 2];
            Buffer.BlockCopy(nResult, 0, fixedResult, 0, nResult.Length * 2);
            foreach (var item in MDataList) {
                item.Read(Lib.Array.Array.Range<byte>(fixedResult, (item.StartAddress - StartAddress)*2, item.DataLength));
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
