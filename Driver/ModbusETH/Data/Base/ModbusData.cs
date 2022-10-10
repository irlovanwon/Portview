///Copyright(c) 2015,HIT All rights reserved.
///Summary：Modbus Data
///Author：Irlovan
///Date：2015-06-12
///Description：
///Modification：

using Irlovan.Driver;
using System;

namespace Irlovan.Driver
{
    internal class ModbusData : DriverData
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
        internal ModbusData(IDriverData driverData, int modbusAddress, Type modbusType)
            : base(driverData.Data, driverData.GroupName, driverData.Readonly, driverData.Address, driverData.Expression) {
            StartAddress = modbusAddress;
            ModbusType = modbusType;
        }

        #endregion Structure

        #region Field

        //Modbus Property000001-065536 
        internal const int MaxAddress = 65536;
        internal const int MinAddress = 1;
        internal const char AddressFlag = '1';

        #endregion Field

        #region Property

        /// <summary>
        /// Start Address
        /// </summary>
        /// <returns></returns>
        internal int StartAddress { get; private set; }

        /// <summary>
        /// IsValid
        /// </summary>
        /// <returns></returns>
        internal bool IsValid { get; set; }

        /// <summary>
        /// Modbus Type
        /// </summary>
        /// <returns></returns>
        internal Type ModbusType { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Check if the Modbus Data is valid
        /// </summary>
        internal virtual void CheckValie() {
            IsValid = true;
            if ((StartAddress > MaxAddress) || (StartAddress < MinAddress) || (Address[0] != AddressFlag)) { IsValid = false; }
        }

        /// <summary>
        /// Bit Converter for modbus data
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        internal static object BitConvert(Type type, byte[] data, int startIndex) {
            try {
                if (type == typeof(Int16)) {
                    return (object)BitConverter.ToInt16(data, startIndex);
                } else if (type == typeof(Int32)) {
                    return (object)BitConverter.ToInt32(data, startIndex);
                } else if (type == typeof(Int64)) {
                    return (object)BitConverter.ToInt64(data, startIndex);
                } else if (type == typeof(UInt16)) {
                    return (object)BitConverter.ToUInt16(data, startIndex);
                } else if (type == typeof(UInt32)) {
                    return (object)BitConverter.ToUInt32(data, startIndex);
                } else if (type == typeof(UInt64)) {
                    return (object)BitConverter.ToUInt64(data, startIndex);
                } else if (type == typeof(Double)) {
                    return (object)BitConverter.ToDouble(data, startIndex);
                } else if (type == typeof(float)) {
                    return (object)BitConverter.ToSingle(data, startIndex);
                }
            }
            catch (Exception) { }
            return null;
        }

        /// <summary>
        /// Get Bytes for modbus data
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static byte[] GetBytes(Type type, object value) {
            try {
                if (type == typeof(Int16)) {
                    return BitConverter.GetBytes((Int16)value);
                } else if (type == typeof(Int32)) {
                    return BitConverter.GetBytes((Int32)value);
                } else if (type == typeof(Int64)) {
                    return BitConverter.GetBytes((Int64)value);
                } else if (type == typeof(UInt16)) {
                    return BitConverter.GetBytes((UInt16)value);
                } else if (type == typeof(UInt32)) {
                    return BitConverter.GetBytes((UInt32)value);
                } else if (type == typeof(UInt64)) {
                    return BitConverter.GetBytes((UInt64)value);
                } else if (type == typeof(Double)) {
                    return BitConverter.GetBytes((Double)value);
                } else if (type == typeof(float)) {
                    return BitConverter.GetBytes((float)value);
                }
            }
            catch (Exception) { }
            return null;
        }

        #endregion Function
    }
}
