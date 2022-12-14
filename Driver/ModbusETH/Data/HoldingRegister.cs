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
    internal class HoldingRegister : Register
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
        internal HoldingRegister(IDriverData driverData, int modbusAddress, Type modbusType, int dataLength, bool isModiconBitOrdering = false)
            : base(driverData,modbusAddress,modbusType,dataLength,isModiconBitOrdering) {
        }

        #endregion Structure

        #region Field

        //Modbus Property000001-065536 
        internal new const int MaxAddress = 465536;
        internal new const int MinAddress = 400001;
        internal new const char AddressFlag = '4';

        #endregion Field

        #region Function

        /// <summary>
        /// Check if the Modbus Data is valid
        /// </summary>
        internal override void CheckValie() {
            base.CheckValie();
        }

        /// <summary>
        /// Get Write Data
        /// </summary>
        /// <returns></returns>
        internal byte[] GetWriteData() {
            byte[] result=ModbusData.GetBytes(ModbusType,Value(Data.Value));
            if (result.Length!=DataLength) {return null;}
            return result;
        }

        #endregion Function

    }
}
