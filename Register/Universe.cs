///Copyright(c) 2015,HIT All rights reserved.
///Summary:Register universe
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using Irlovan.Global;
using Irlovan.Lib.Symbol;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Irlovan.Register
{
    public class Universe : IUniverse
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="owner"></param>
        public Universe(IUnit owner) {
            Owner = owner;
            Init();
        }

        #endregion Structure

        #region Field

        private Dictionary<string, IUnit> _map = new Dictionary<string, IUnit>();
        private const int MaxDefaultCount = 50;
        private object _mapLock = new object();

        #endregion Field

        #region Property

        ///<summary>
        ///Map to locate units
        ///</summary>
        public Dictionary<string, IUnit> Map {
            get { return GerMap(); }
        }

        /// <summary>
        /// Max count of units
        /// </summary>
        public int MaxCount { get; private set; }

        /// <summary>
        /// Owner of the universe
        /// </summary>
        public IUnit Owner { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Init for Group
        /// </summary>
        private void Init() {
            _map = new Dictionary<string, IUnit>();
            MaxCount = MaxDefaultCount;
        }

        /// <summary>
        /// Push Unit to Group
        /// </summary>
        public bool Push(IUnit unit) {
            lock (_mapLock) {
                if (_map.ContainsKey(unit.ID)) { return false; }
                unit.Parent = Owner;
                _map.Add(unit.ID, unit);
                return true;
            }
        }

        /// <summary>
        /// remove Unit from Group by its id
        /// </summary>
        public bool Remove(string id) {
            lock (_mapLock) {
                if (!_map.ContainsKey(id)) { return true; }
                _map[id].Dispose();
                _map.Remove(id);
                return true;
            }
        }

        /// <summary>
        /// if the universe contain a unit
        /// </summary>
        public bool Contain(string id) {
            lock (_mapLock) {
                return _map.ContainsKey(id);
            }
        }

        /// <summary>
        /// get unit by id
        /// </summary>
        public IUnit GetUnit(string id) {
            lock (_mapLock) {
                return _map[id];
            }
        }

        /// <summary>
        /// dispose for the group
        /// </summary>
        public void Dispose() {
            DisposeUnit();
        }

        /// <summary>
        /// Dispose all the unit
        /// </summary>
        private void DisposeUnit() {
            lock (_mapLock) {
                foreach (var item in _map) {
                    item.Value.Dispose();
                }
            }
        }

        /// <summary>
        /// Return a new copy of Map
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, IUnit> GerMap() {
            lock (_mapLock) {
                Dictionary<string, IUnit> result = new Dictionary<string, IUnit>();
                foreach (var item in _map) {
                    result.Add(item.Key, item.Value);
                }
                return result;
            }
        }

        /// <summary>
        /// update address
        /// </summary>
        public void UpdateAddress(string parentID) {
            lock (_mapLock) {
                foreach (var item in _map) {
                    item.Value.UpdateAddress();
                }
            }
        }

        /// <summary>
        /// update root
        /// </summary>
        public void UpdateRoot(IRegister root) {
            lock (_mapLock) {
                foreach (var item in _map) {
                    item.Value.UpdateRoot(root);
                }
            }
        }

        /// <summary>
        /// update indexd
        /// </summary>
        public void UpdateIndex() {
            lock (_mapLock) {
                foreach (var item in _map) {
                    item.Value.UpdateIndex();
                }
            }
        }

        /// <summary>
        /// Change ID from idFrom to idTo
        /// </summary>
        public bool ChangeID(string idFrom, string idTo) {
            lock (_mapLock) {
                if ((!_map.ContainsKey(idFrom)) || (_map.ContainsKey(idTo))) { return false; }
                _map.Add(idTo, _map[idFrom]);
                _map.Remove(idFrom);
                return true;
            }
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        public bool Deserialize(SerializationInfo info, StreamingContext context, IAddress address, IRegister root) {
            lock (_mapLock) {
                string childID = address.Tags[0];
                if (_map.ContainsKey(childID)) {
                    _map[childID].Deserialize(address, root, Owner);
                }
                else {
                    if (address.Tags.Length < 1) { return false; }
                    string flag = string.Empty;
                    try {
                        flag = (string)info.GetValue(Owner.Address.FullName + Address.SplitChar + address.Tags[0] + Unit.SerializeSplitString + Unit.Flag, typeof(string));
                    }
                    catch (Exception e) {
                        string message = Lib.Properties.Resources.RegisterInitFailed + Owner.Root.Name + Symbol.NewLine_Symbol + e.ToString();
                        Runtime.ServerShutDown(message);
                    }

                    if (flag == typeof(Singularity).ToString()) {
                        DeserializeSingularity(info, context, address);
                    }
                    if (flag == typeof(Ether<>).ToString()) {
                        DeserializeEther(info, context, address);
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Deserialize Ether
        /// </summary>
        private bool DeserializeEther(SerializationInfo info, StreamingContext context, IAddress address) {
            if (address.Tags.Length != 1) { return false; }
            if (_map.ContainsKey(address.Tags[0])) { return false; }
            string etherType = string.Empty;
            try {
                etherType = (string)info.GetValue(Owner.Address.FullName + Address.SplitChar + address.Tags[0] + Unit.SerializeSplitString + Ether<object>.TypePara, typeof(string));
            }
            catch (Exception e) {
                string message = Lib.Properties.Resources.RegisterInitFailed + Owner.Root.Name + Symbol.NewLine_Symbol + e.ToString();
                Runtime.ServerShutDown(message);
            } 

            Type myGenericClass = typeof(Ether<>).MakeGenericType(new Type[] { Type.GetType(etherType) });
            IUnit ether = (IUnit)Activator.CreateInstance(myGenericClass, new Object[] { info, context, address, Owner.Root, Owner });
            //ether.UpdateIndex();
            _map.Add(ether.ID, ether);
            return true;
        }

        /// <summary>
        /// Deserialize Singularity
        /// </summary>
        private bool DeserializeSingularity(SerializationInfo info, StreamingContext context, IAddress address) {
            ISingularity singularity = (ISingularity)Activator.CreateInstance(typeof(Singularity), new Object[] { info, context, address, Owner.Root, Owner });
            //singularity.UpdateIndex();
            _map.Add(singularity.ID, singularity);
            return true;
        }

        /// <summary>
        /// serialize
        /// </summary>
        public void Serialize(SerializationInfo info, StreamingContext context) {
            lock (_mapLock) {
                foreach (var item in _map) {
                    item.Value.GetObjectData(info, context);
                }
            }
        }

        #endregion Function

    }
}
