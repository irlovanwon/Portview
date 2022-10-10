///Copyright(c) 2015,HIT All rights reserved.
///Summary：Holding Register Session
///Author：Irlovan
///Date：2015-06-14
///Description：
///Modification：      

using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace Irlovan.Driver
{
    internal class HoldingRegisterSession : RegisterSession
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        internal HoldingRegisterSession(ModbusIpMaster master, int startAddress, int sessionLength, List<HoldingRegister> mDataList)
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
        [HandleProcessCorruptedStateExceptions]
        internal override void Read() {
            try {
                ushort[] nResult = Master.ReadHoldingRegisters((ushort)(StartAddress - HoldingRegister.MinAddress), (ushort)SessionLength);
                ReadReceivedData(nResult);
            }
            catch (Exception) {
            }

        }

        /// <summary>
        /// Write
        /// </summary>
        internal void Write() {
            byte[] fixedResult = new byte[SessionLength * 2];
            foreach (HoldingRegister item in MDataList) {
                byte[] witeBytes = item.GetWriteData();
                for (int i = 0; i < witeBytes.Length; i++) { fixedResult[(item.StartAddress - StartAddress)*2 + i] = witeBytes[i]; }
            }
            ushort[] nResult = new ushort[SessionLength];
            Buffer.BlockCopy(fixedResult, 0, nResult, 0, fixedResult.Length);
            Master.WriteMultipleRegisters((ushort)(StartAddress - HoldingRegister.MinAddress), nResult);
        }

        #endregion Function

    }
}
