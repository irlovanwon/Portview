///Copyright(c) 2015,HIT All rights reserved.
///Summary：Modbus Data
///Author：Irlovan
///Date：2015-06-13
///Description：
///Modification：

using Irlovan.Driver;
using Irlovan.Lib.Convertor;
using System;

namespace Irlovan.Driver
{
    internal class Register : ModbusData
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="data"></param>
        /// <param name="groupName"></param>
        /// <param name="readOnly"></param>
        /// <param name="address"></param>
        /// <param name="expression"></param>
        internal Register(IDriverData driverData, int modbusAddress, Type modbusType, int dataLength, bool isModiconBitOrdering = false)
            : base(driverData, modbusAddress, modbusType) {
            DataLength = dataLength;
            IsModiconBitOrdering = isModiconBitOrdering;
        }

        #endregion Structure

        #region Field

        internal const int MaxDataLength = 246;
        internal const int MinDataLength = 2;

        #endregion Field

        #region Property

        /// <summary>
        /// DataLength
        /// </summary>
        /// <returns></returns>
        internal int DataLength { get; private set; }

        /// <summary>
        /// Is Modicon Bit Ordering
        /// </summary>
        /// <returns></returns>
        internal bool IsModiconBitOrdering { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Read
        /// </summary>
        /// <returns></returns>
        internal void Read(byte[] datas) {
            if (datas.Length != DataLength) { return; }
            byte[] fixedDatas = ModiconBitOrderingCheck(datas);
            object result = ModbusData.BitConvert(ModbusType, fixedDatas, 0);
            if (result == null) { return; }
            ReadValue(result);
        }

        /// <summary>
        /// Modicon BitOrdering Check (bit 0 is MSB)
        /// </summary>
        private byte[] ModiconBitOrderingCheck(byte[] originData) {
            if (!IsModiconBitOrdering) { return originData; }
            byte[] result = new byte[originData.Length];
            for (int i = 0; i < originData.Length; i += 2) {
                result[i] = originData[i + 1];
                result[i + 1] = originData[i];
            }
            return result;
        }

        /// <summary>
        /// Check if the Modbus Data is valid
        /// </summary>
        internal override void CheckValie() {
            base.CheckValie();
            if ((DataLength > MaxDataLength) || (DataLength < MinDataLength) || (DataLength % 2 != 0) || ((StartAddress + DataLength - 1) > MaxAddress)) { IsValid = false; }
        }

        #endregion Function

    }
}
