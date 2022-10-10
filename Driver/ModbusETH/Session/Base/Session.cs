///Copyright(c) 2015,HIT All rights reserved.
///Summary：Session
///Author：Irlovan
///Date：2015-06-14
///Description：
///Modification：


using Modbus.Device;
namespace Irlovan.Driver
{
   internal class Session
    {

        #region Structure

       /// <summary>
       /// Construction
       /// </summary>
       internal Session(ModbusIpMaster master,int startAddress,int sessionLength) {
           SessionLength = sessionLength;
           StartAddress = startAddress;
           Master = master;
       }

        #endregion Structure

       #region Field

       internal const int MaxSessionLength = 123;
       internal const int MinSessionLength = 1;

       #endregion Field

        #region Property

       /// <summary>
       /// Session Length
       /// </summary>
       internal int SessionLength { get; private set; }

       /// <summary>
       /// Session Length
       /// </summary>
       internal int StartAddress { get; private set; }

       /// <summary>
       /// Is Session Valid
       /// </summary>
       internal bool IsValid { get; set; }

       /// <summary>
       /// Modbus Master
       /// </summary>
       internal ModbusIpMaster Master { get; private set; }

        #endregion Property

        #region Function

       /// <summary>
       /// Check if the session is valid
       /// </summary>
       internal virtual void CheckValid() {
           IsValid = true;
           if ((SessionLength > MaxSessionLength) || (SessionLength < MinSessionLength)) { IsValid = false; }
       }

       /// <summary>
       /// ReadAddress
       /// </summary>
       internal virtual void Read() { }


        #endregion Function  

    }
}
