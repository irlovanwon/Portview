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
    internal class InternalRegisterSession : RegisterSession
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        internal InternalRegisterSession(ModbusIpMaster master, int startAddress, int sessionLength, List<InternalRegister> mDataList)
            : base(master, startAddress, sessionLength) {
                foreach (var item in mDataList) {
                    MDataList.Add((Register)item);
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
            ushort[] nResult = Master.ReadInputRegisters((ushort)(StartAddress - InternalRegister.MinAddress), (ushort)SessionLength);
            ReadReceivedData(nResult);
        }
       
        #endregion Function

    }
}
