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
    internal class InputCoilSession : CoilSession
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        internal InputCoilSession(ModbusIpMaster master, int startAddress, int sessionLength, List<InputCoil> mDataList)
            : base(master, startAddress, sessionLength) {
                foreach (var item in mDataList) {
                    MDataList.Add((Coil)item);
                }
        }

        #endregion Structure

        #region Function

        /// <summary>
        /// Check if the session is valid
        /// </summary>
        internal override void CheckValid() {
            base.CheckValid();
        }

        /// <summary>
        /// Read 
        /// </summary>
        internal override void Read() {
            bool[] nResult = Master.ReadInputs((ushort)(StartAddress-InputCoil.MinAddress), (ushort)SessionLength);
            ReadReceivedData(nResult);
        }

        #endregion Function

    }
}
