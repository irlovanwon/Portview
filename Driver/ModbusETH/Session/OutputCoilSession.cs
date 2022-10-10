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
    internal class OutputCoilSession : CoilSession
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        internal OutputCoilSession(ModbusIpMaster master, int startAddress, int sessionLength, List<OutputCoil> mDataList)
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
            bool[] nResult = Master.ReadCoils((ushort)(StartAddress-OutputCoil.MinAddress), (ushort)SessionLength);
            ReadReceivedData(nResult);
        }

        /// <summary>
        /// Write
        /// </summary>
        internal void Write() {
            bool[] nResult = new bool[SessionLength];
            foreach (OutputCoil item in MDataList) {
                nResult[item.StartAddress - StartAddress] = item.GetWriteData();
            }
            Master.WriteMultipleCoils((ushort)(StartAddress-OutputCoil.MinAddress), nResult);
        }

        #endregion Function

    }
}
