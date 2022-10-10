///Copyright(c) 2015,HIT All rights reserved.
///Summary:Singularity
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Irlovan.Register
{
    [Serializable]
    public class Singularity : Unit, ISingularity
    {

        #region Structure

        /// <summary>
        /// Construct from parameter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="root"></param>
        /// <param name="parent"></param>
        public Singularity(string id, IRegister root, IUnit parent = null)
            : base(id, root, parent) {
        }

        /// <summary>
        /// Construct from config file
        /// </summary>
        /// <param name="config"></param>
        /// <param name="root"></param>
        /// <param name="parent"></param>
        public Singularity(XElement config, IRegister root, IUnit parent = null)
            : base(config, root, parent) {
            InitXML();
        }

        /// <summary>
        /// The special constructor is used to deserialize values.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public Singularity(SerializationInfo info, StreamingContext context)
            : base(info, context) {
            foreach (SerializationEntry entry in info) {
                string name = (string)entry.Name;
                string[] coordinate = name.Split(Unit.SerializeSplitString);
                IAddress address = new Address();
                address.Parse(coordinate[0]);
                Deserialize(address, Root, Parent);
            }
        }

        /// <summary>
        /// Deserialize States
        /// </summary>
        /// <param name="id"></param>
        /// <param name="root"></param>
        /// <param name="parent"></param>
        public Singularity(SerializationInfo info, StreamingContext context, IAddress address, IRegister root, IUnit parent = null)
            : base(info, context) {
            Deserialize(address, root, parent);
        }

        #endregion Structure

        #region Property

        /// <summary>
        /// Singularity of the Universe
        /// </summary>
        public IUniverse Container { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Init
        /// </summary>
        internal override void Init() {
            base.Init();
            Container = new Universe(this);
        }

        /// <summary>
        /// Deserialize States
        /// </summary>
        /// <param name="id"></param>
        /// <param name="root"></param>
        /// <param name="parent"></param>
        public override bool Deserialize(IAddress address, IRegister root, IUnit parent = null) {
            if (!base.Deserialize(address, root, parent)) { return false; }
            if (address.Tags.Length <= 1) { return true; }
            return Container.Deserialize(SInfo, SContext, address.GetChild(), root);
        }



        /// <summary>
        /// Init ID
        /// </summary>
        public override bool UpdateID(string newID) {
            if (!base.UpdateID(newID)) { return false; }
            Container.UpdateAddress(newID);
            return true;
        }

        /// <summary>
        /// CheckDuplicateID to avoid the same id in one universe
        /// </summary>
        internal override bool IsDuplicateID(string id) {
            if (base.IsDuplicateID(id)) { return true; }
            if (Container.Contain(id)) { return true; }
            return false;
        }

        /// <summary>
        /// Update Root
        /// </summary>
        public override void UpdateRoot(IRegister register) {
            base.UpdateRoot(register);
            Container.UpdateRoot(register);
        }

        /// <summary>
        /// Update index
        /// </summary>
        public override void UpdateIndex() {
            base.UpdateIndex();
            Container.UpdateIndex();
        }

        /// <summary>
        /// Init the address of a unit
        /// </summary>
        public override void UpdateAddress() {
            base.UpdateAddress();
            Container.UpdateAddress(ID);
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
            info.AddValue(Address.FullName + SerializeSplitString + Flag, typeof(Singularity).ToString(), typeof(string));
            Container.Serialize(info, context);
        }

        /// <summary>
        /// Wite info to Singularity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override bool Write<T>(IAddress address, T data) {
            if (!base.Write<T>(address, data)) { return false; }
            if (address.Tags.Length <= 1) { return false; }
            IAddress childAddress = address.GetChild();
            if (childAddress == null) { return false; }
            if (Container.Contain(childAddress.LastName)) {
                return Container.GetUnit(childAddress.LastName).Write<T>(childAddress, data);
            }
            return false;
        }

        /// <summary>
        /// Read info From Singularity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool Read<T>(IAddress address, out T result) {
            if (!base.Read<T>(address, out result)) { return false; }
            if (address.Tags.Length <= 1) { return false; }
            IAddress childAddress = address.GetChild();
            if (childAddress == null) { return false; }
            if (Container.Contain(childAddress.LastName)) {
                return Container.GetUnit(childAddress.LastName).Read<T>(childAddress, out result);
            }
            return false;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            base.Dispose();
            Container.Dispose();
        }

        #endregion Function

    }
}
