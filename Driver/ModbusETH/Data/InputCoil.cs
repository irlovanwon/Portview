///Copyright(c) 2015,HIT All rights reserved.
///Summary：Modbus Data
///Author：Irlovan
///Date：2015-06-13
///Description：
///Modification：

using Irlovan.Driver;
using System;

namespace Irlovan.Driver
{
    internal class InputCoil : Coil
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
        internal InputCoil(IDriverData driverData, int modbusAddress, Type modbusType)
            : base(driverData, modbusAddress, modbusType) {
        }

        #endregion Structure

        #region Field

        //Modbus Property000001-065536 
        internal new const int MaxAddress = 165536;
        internal new const int MinAddress = 100001;
        internal new const char AddressFlag = '1';

        #endregion Field

        #region Function

        /// <summary>
        /// Check if the Modbus Data is valid
        /// </summary>
        internal override void CheckValie() {
            base.CheckValie();
            if (Readonly==false) {IsValid = false;}
        }

        #endregion Function

    }
}
