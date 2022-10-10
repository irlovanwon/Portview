
using Irlovan.Lib.XML;
///Copyright(c) 2015,HIT All rights reserved.
///Summary：
///Author：Irlovan
///Date：2015-06-11
///Description：
///Modification：
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Driver
{
    internal class MGroup
    {

        #region Structure

        /// <summary>
        /// Modbus Device
        /// </summary>
        internal MGroup(IGroup group, ModbusIpMaster master) {
            _master = master;
            Group = group;
            Init();
        }

        #endregion Structure

        #region Field


        private ModbusIpMaster _master;
        private List<HoldingRegisterSession> _holdingRegisterSessionList = new List<HoldingRegisterSession>();
        private List<InternalRegisterSession> _internalRegisterSessionList = new List<InternalRegisterSession>();
        private List<InputCoilSession> _inputCoilSessionList = new List<InputCoilSession>();
        private List<OutputCoilSession> _outputCoilSessionList = new List<OutputCoilSession>();
        private SortedDictionary<int, List<HoldingRegister>> _sortedHoldingRegisterDataList = new SortedDictionary<int, List<HoldingRegister>>();
        private SortedDictionary<int, List<InternalRegister>> _sortedInternalRegisterDataList = new SortedDictionary<int, List<InternalRegister>>();
        private SortedDictionary<int, List<InputCoil>> _sortedInputCoilDataList = new SortedDictionary<int, List<InputCoil>>();
        private SortedDictionary<int, List<OutputCoil>> _sortedOutputCoilDataList = new SortedDictionary<int, List<OutputCoil>>();

        #endregion Field

        #region Property



        /// <summary>
        /// Initstate of the device
        /// </summary>
        internal bool InitState { get; private set; }

        /// <summary>
        /// Initstate of the device
        /// </summary>
        internal IGroup Group { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Init
        /// </summary>
        private void Init() {
            ClassifyDataList();
        }

        /// <summary>
        /// Classify DataList
        /// </summary>
        private void ClassifyDataList() {
            IEnumerable<XElement> dataConfigList = Group.Config.Elements(Driver.DataTagPara);
            foreach (var item in dataConfigList) {
                ParseMDataConfig(item);
            }
            AddSession();
        }

        /// <summary>
        /// Parse Modbus Data Config
        /// </summary>
        /// <param name="dataConfig"></param>
        private void ParseMDataConfig(XElement dataConfig) {
            string dataName, modbusTypeStr; Type modbusType; int offset, modbusAddress;
            if (!XML.InitStringAttr<string>(dataConfig, Driver.RealtimeDataPara, out dataName)) { return; }
            if (!XML.InitStringAttr<string>(dataConfig, ModbusETH.ModbusTypeAttr, out modbusTypeStr)) { return; }
            try { modbusType = Type.GetType(modbusTypeStr); }
            catch (Exception) { return; }
            if (!XML.InitStringAttr<int>(dataConfig, ModbusETH.OffsetAttr, out offset)) { return; }
            IDriverData driverData = Group.GetData(dataName);
            if (!int.TryParse(driverData.Address, out modbusAddress)) { return; }
            if (!CheckOffset(modbusType, offset)) { return; }
            AddHoldingRegister(driverData, modbusAddress, modbusType, offset);
            AddInputCoil(driverData, modbusAddress, modbusType, offset);
            AddOutputCoil(driverData, modbusAddress, modbusType, offset);
            AddInternalRegister(driverData, modbusAddress, modbusType, offset);
        }

        /// <summary>
        /// Add Session
        /// </summary>
        private void AddSession() {
            AddHoldingRegisterSession();
            AddInputCoilSession();
            AddOutputCoilSession();
            AddInternalRegisterSession();
        }

        /// <summary>
        /// Check offset 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private bool CheckOffset(Type type, int offset) {
            if (type == typeof(Int16)) {
                return (offset == 2);
            } else if (type == typeof(Int32)) {
                return (offset == 4);
            } else if (type == typeof(Int64)) {
                return (offset == 8);
            } else if (type == typeof(UInt16)) {
                return (offset == 2);
            } else if (type == typeof(UInt32)) {
                return (offset == 4);
            } else if (type == typeof(UInt64)) {
                return (offset == 8);
            } else if (type == typeof(Double)) {
                return (offset == 8);
            } else if (type == typeof(float)) {
                return (offset == 4);
            } else if (type == typeof(Boolean)) {
                return (offset == 1);
            }
            return false;
        }

        /// <summary>
        /// Add Holding Register
        /// </summary>
        private void AddHoldingRegister(IDriverData driverData, int address, Type modbusType, int offset) {
            if ((address > HoldingRegister.MaxAddress) || (address < HoldingRegister.MinAddress)) { return; }
            HoldingRegister mData = new HoldingRegister(driverData, address, modbusType, offset);
            if (!_sortedHoldingRegisterDataList.ContainsKey(address)) {
                _sortedHoldingRegisterDataList.Add(address, new List<HoldingRegister> { mData });
            } else {
                _sortedHoldingRegisterDataList[address].Add(mData);
            }
        }

        private void AddHoldingRegisterSession() {
            List<HoldingRegister> driverDataList = new List<HoldingRegister>();
            int sessionLength = 0;
            int startAddress = 0;
            foreach (var item in _sortedHoldingRegisterDataList) {
                foreach (var child in item.Value) {
                    if (driverDataList.Count == 0) {
                        startAddress = item.Key;
                        driverDataList.Add(child);
                        sessionLength = child.DataLength / 2;
                    } else if ((child.StartAddress <= (startAddress + sessionLength)) && (child.StartAddress >= startAddress) && ((child.StartAddress - startAddress + child.DataLength / 2) <= Register.MaxDataLength)) {
                        driverDataList.Add(child);
                        int childFixedLength = (child.StartAddress - startAddress + child.DataLength / 2);
                        if (childFixedLength > sessionLength) {
                            sessionLength = childFixedLength;
                        }
                    } else {
                        HoldingRegisterSession session = new HoldingRegisterSession(_master, startAddress, sessionLength, driverDataList);
                        _holdingRegisterSessionList.Add(session);
                        driverDataList = new List<HoldingRegister>();
                        sessionLength = 0;
                        startAddress = 0;
                        startAddress = child.StartAddress;
                        driverDataList.Add(child);
                        sessionLength = child.DataLength / 2;
                    }
                }
            }
            if (driverDataList.Count != 0) {
                HoldingRegisterSession session = new HoldingRegisterSession(_master, startAddress, sessionLength, driverDataList);
                _holdingRegisterSessionList.Add(session);
                driverDataList = new List<HoldingRegister>();
                sessionLength = 0;
                startAddress = 0;
            }
        }

        private void AddInternalRegisterSession() {
            List<InternalRegister> driverDataList = new List<InternalRegister>();
            int sessionLength = 0;
            int startAddress = 0;
            foreach (var item in _sortedInternalRegisterDataList) {
                foreach (var child in item.Value) {
                    if (driverDataList.Count == 0) {
                        startAddress = item.Key;
                        driverDataList.Add(child);
                        sessionLength = child.DataLength / 2;
                    } else if ((child.StartAddress <= (startAddress + sessionLength)) && (child.StartAddress >= startAddress) && ((child.StartAddress - startAddress + child.DataLength / 2) <= RegisterSession.MaxSessionLength)) {
                        driverDataList.Add(child);
                        int childFixedLength = (child.StartAddress - startAddress + child.DataLength / 2);
                        if (childFixedLength > sessionLength) {
                            sessionLength = childFixedLength;
                        }
                    } else {
                        InternalRegisterSession session = new InternalRegisterSession(_master, startAddress, sessionLength, driverDataList);
                        _internalRegisterSessionList.Add(session);
                        driverDataList = new List<InternalRegister>();
                        sessionLength = 0;
                        startAddress = 0;
                        startAddress = child.StartAddress;
                        driverDataList.Add(child);
                        sessionLength = child.DataLength / 2;
                    }
                }
            }
            if (driverDataList.Count != 0) {
                InternalRegisterSession session = new InternalRegisterSession(_master, startAddress, sessionLength, driverDataList);
                _internalRegisterSessionList.Add(session);
                driverDataList = new List<InternalRegister>();
                sessionLength = 0;
                startAddress = 0;
            }
        }

        private void AddInputCoilSession() {
            List<InputCoil> driverDataList = new List<InputCoil>();
            int sessionLength = 0;
            int startAddress = 0;
            foreach (var item in _sortedInputCoilDataList) {
                foreach (var child in item.Value) {
                    if (driverDataList.Count == 0) {
                        startAddress = item.Key;
                        driverDataList.Add(child);
                        sessionLength = 1;
                    } else if ((child.StartAddress <= (startAddress + sessionLength + 1)) && ((child.StartAddress - startAddress + 1) < CoilSession.MaxSessionLength)) {
                        driverDataList.Add(child);
                        int childFixedLength = (child.StartAddress - startAddress + 1);
                        if (childFixedLength > sessionLength) {
                            sessionLength = childFixedLength;
                        }
                    } else {
                        InputCoilSession session = new InputCoilSession(_master, startAddress, sessionLength, driverDataList);
                        _inputCoilSessionList.Add(session);
                        driverDataList = new List<InputCoil>();
                        sessionLength = 0;
                        startAddress = 0;
                        startAddress = child.StartAddress;
                        driverDataList.Add(child);
                        sessionLength = 1;
                    }
                }
            }
            if (driverDataList.Count != 0) {
                InputCoilSession session = new InputCoilSession(_master, startAddress, sessionLength, driverDataList);
                _inputCoilSessionList.Add(session);
                driverDataList = new List<InputCoil>();
                sessionLength = 0;
                startAddress = 0;
            }
        }

        private void AddOutputCoilSession() {
            List<OutputCoil> driverDataList = new List<OutputCoil>();
            int sessionLength = 0;
            int startAddress = 0;
            foreach (var item in _sortedOutputCoilDataList) {
                foreach (var child in item.Value) {
                    if (driverDataList.Count == 0) {
                        startAddress = item.Key;
                        driverDataList.Add(child);
                        sessionLength = 1;
                    } else if ((child.StartAddress <= (startAddress + sessionLength + 1)) && ((child.StartAddress - startAddress + 1) < CoilSession.MaxSessionLength)) {
                        driverDataList.Add(child);
                        int childFixedLength = (child.StartAddress - startAddress + 1);
                        if (childFixedLength > sessionLength) {
                            sessionLength = childFixedLength;
                        }
                    } else {
                        OutputCoilSession session = new OutputCoilSession(_master, startAddress, sessionLength, driverDataList);
                        _outputCoilSessionList.Add(session);
                        driverDataList = new List<OutputCoil>();
                        sessionLength = 0;
                        startAddress = 0;
                        startAddress = child.StartAddress;
                        driverDataList.Add(child);
                        sessionLength = 1;
                    }
                }
            }
            if (driverDataList.Count != 0) {
                OutputCoilSession session = new OutputCoilSession(_master, startAddress, sessionLength, driverDataList);
                _outputCoilSessionList.Add(session);
                driverDataList = new List<OutputCoil>();
                sessionLength = 0;
                startAddress = 0;
            }
        }

        /// <summary>
        /// AddInternalRegister
        /// </summary>
        private void AddInternalRegister(IDriverData driverData, int address, Type modbusType, int offset) {
            if ((address > InternalRegister.MaxAddress) || (address < InternalRegister.MinAddress)) { return; }
            InternalRegister mData = new InternalRegister(driverData, address, modbusType, offset);
            if (!_sortedInternalRegisterDataList.ContainsKey(address)) {
                _sortedInternalRegisterDataList.Add(address, new List<InternalRegister> { mData });
            } else {
                _sortedInternalRegisterDataList[address].Add(mData);
            }
        }

        /// <summary>
        /// AddInputCoil
        /// </summary>
        private void AddInputCoil(IDriverData driverData, int address, Type modbusType, int offset) {
            if ((address > InputCoil.MaxAddress) || (address < InputCoil.MinAddress)) { return; }
            InputCoil mData = new InputCoil(driverData, address, modbusType);
            if (!_sortedInputCoilDataList.ContainsKey(address)) {
                _sortedInputCoilDataList.Add(address, new List<InputCoil> { mData });
            } else {
                _sortedInputCoilDataList[address].Add(mData);
            }
        }

        /// <summary>
        /// AddOutputCoil
        /// </summary>
        private void AddOutputCoil(IDriverData driverData, int address, Type modbusType, int offset) {
            if ((address > OutputCoil.MaxAddress) || (address < OutputCoil.MinAddress)) { return; }
            OutputCoil mData = new OutputCoil(driverData, address, modbusType);
            if (!_sortedOutputCoilDataList.ContainsKey(address)) {
                _sortedOutputCoilDataList.Add(address, new List<OutputCoil> { mData });
            } else {
                _sortedOutputCoilDataList[address].Add(mData);
            }
        }



        /// <summary>
        /// Read Device
        /// </summary>
        internal void Read() {
            if (Group.RWMode != RWModeEnum.Input) { return; }
            foreach (var item in _inputCoilSessionList) {
                try {
                    item.Read();
                }
                catch (Exception) {
                    item.QualityBad();
                    continue;
                }
            }
            System.Threading.Thread.Sleep(10);
            foreach (var item in _outputCoilSessionList) {
                try {
                    item.Read();
                }
                catch (Exception) {
                    item.QualityBad();
                    continue;
                }
            }
            System.Threading.Thread.Sleep(10);
            foreach (var item in _internalRegisterSessionList) {
                try {
                    item.Read();
                }
                catch (Exception) {
                    item.QualityBad();
                    continue;
                }
            }
            System.Threading.Thread.Sleep(10);
            foreach (var item in _holdingRegisterSessionList) {
                try {
                    item.Read();
                }
                catch (Exception) {
                    item.QualityBad();
                    continue;
                }
            }
        }

        /// <summary>
        /// Write Device
        /// </summary>
        internal void Write() {
            if (Group.RWMode != RWModeEnum.Output) { return; }
            foreach (var item in _holdingRegisterSessionList) {
                try {
                    item.Write();
                }
                catch (Exception) {
                    continue;
                }
            }
            foreach (var item in _outputCoilSessionList) {
                try {
                    item.Write();
                }
                catch (Exception) {
                    continue;
                }
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        internal void Dispose() {

        }

        #endregion Function

    }
}
