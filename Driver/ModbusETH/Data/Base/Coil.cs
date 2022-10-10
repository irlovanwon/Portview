///Copyright(c) 2015,HIT All rights reserved.
///Summary：Modbus Coil Data
///Author：Irlovan
///Date：2015-06-13
///Description：
///Modification：

using Irlovan.Driver;
using System;

namespace Irlovan.Driver
{
    internal class Coil : ModbusData
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
        internal Coil(IDriverData driverData, int modbusAddress, Type modbusType)
            : base(driverData, modbusAddress, modbusType) {
        }

        #endregion Structure

        #region Function

        /// <summary>
        /// Read
        /// </summary>
        /// <returns></returns>
        internal void Read(bool result) {
            ReadValue(result);
        }

        /// <summary>
        /// Check if the Modbus Data is valid
        /// </summary>
        internal override void CheckValie() {
            base.CheckValie();
            if (ModbusType!=typeof(bool)) {IsValid = false;}
        }

        #endregion Function

    }
}
