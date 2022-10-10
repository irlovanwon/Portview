///Copyright(c) 2015,HIT All rights reserved.
///Summary:Register ether
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using Irlovan.Lib.Symbol;
using Irlovan.Log;
using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Irlovan.Register
{
    [Serializable]
    public class Ether<T> : Unit, IEther<T>
    {

        #region Structure

        /// <summary>
        /// Construct from parameter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="root"></param>
        /// <param name="parent"></param>
        public Ether(string id, IRegister root, IUnit parent = null)
            : base(id, root, parent) {
        }

        /// <summary>
        /// Construct from config file
        /// </summary>
        /// <param name="config"></param>
        /// <param name="root"></param>
        /// <param name="parent"></param>
        public Ether(XElement config, IRegister root, IUnit parent = null)
            : base(config, root, parent) {
            InitXML();
        }

        /// <summary>
        /// Deserialize States
        /// </summary>
        /// <param name="id"></param>
        /// <param name="root"></param>
        /// <param name="parent"></param>
        public Ether(SerializationInfo info, StreamingContext context, IAddress address, IRegister root, IUnit parent = null)
            : base(info, context) {
            Deserialize(address, root, parent);
        }

        #endregion Structure

        #region Field

        private const string DataPara = "Data";
        public const string TypePara = "Type";

        #endregion Field

        #region Property

        /// <summary>
        /// Data info
        /// </summary>
        public T Data { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
        }

        /// <summary>
        /// Wite info to Ether
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override bool Write<Tw>(IAddress address, Tw data) {
            if (!base.Write<Tw>(address, data)) { return false; }
            if (address.Tags.Length != 1) { return false; }
            if (typeof(T) != typeof(Tw)) { return false; }
            Data = (T)(object)data;
            return true;
        }

        /// <summary>
        /// Read info From Ether
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool Read<Tr>(IAddress address, out Tr result) {
            if (!base.Read<Tr>(address, out result)) { return false; }
            if (address.Tags.Length != 1) { return false; }
            if (typeof(T) != typeof(Tr)) { return false; }
            result = (Tr)(object)Data;
            return true;
        }

        /// <summary>
        /// Init info from xml
        /// </summary>
        /// <param name="element"></param>
        public override void InitXML() {
            base.InitXML();
        }

        /// <summary>
        /// Serialize
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            // Use the AddValue method to specify serialized values.
            base.GetObjectData(info, context);
            info.AddValue(Address.FullName + SerializeSplitString + Flag, typeof(Ether<>).ToString(), typeof(string));
            try {
                info.AddValue(Address.FullName + Unit.SerializeSplitString + DataPara, Data, typeof(T));
                info.AddValue(Address.FullName + Unit.SerializeSplitString + TypePara, typeof(T).ToString(), typeof(string));
            }
            catch (Exception e) {
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, Lib.Properties.Resources.RegisterInitFailed + DataPara + Symbol.NewLine_Symbol + e.ToString());
            }
        }

        /// <summary>
        /// Deserialize States
        /// </summary>
        /// <param name="id"></param>
        /// <param name="root"></param>
        /// <param name="parent"></param>
        public override bool Deserialize(IAddress address, IRegister root, IUnit parent = null) {
            if (!base.Deserialize(address, root, parent)) { return false; }
            try {
                Data = (T)SInfo.GetValue(Address.FullName + Unit.SerializeSplitString + DataPara, typeof(T));
            }
            catch (Exception e) {
                string message = Lib.Properties.Resources.RegisterInitFailed + DataPara + Symbol.NewLine_Symbol + e.ToString();
                Global.Info.LogRecorder.Log(LogLevelEnum.Error, message);
            }

            return true;
        }

        #endregion Function

    }
}
