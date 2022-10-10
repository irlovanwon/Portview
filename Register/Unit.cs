///Copyright(c) 2015,HIT All rights reserved.
///Summary:Unit
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:

using Irlovan.Lib.XML;
using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Irlovan.Register
{

    [Serializable]
    public class Unit : IUnit
    {

        #region Structure

        /// <summary>
        /// Construct from parameter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="root"></param>
        /// <param name="parent"></param>
        internal Unit(string id, IRegister root, IUnit parent = null)
            : this(root, parent) {
            ID = id;
        }

        /// <summary>
        /// Construct from config file
        /// </summary>
        /// <param name="config"></param>
        internal Unit(XElement config, IRegister root, IUnit parent = null)
            : this(root, parent) {
            Config = config;
        }

        /// <summary>
        /// The special constructor is used to deserialize values.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        internal Unit(SerializationInfo info, StreamingContext context)
            : this() {
            SInfo = info;
            SContext = context;
        }

        internal Unit() {
            Init();
        }

        /// <summary>
        /// Construct Para
        /// </summary>
        /// <param name="root"></param>
        /// <param name="parent"></param>
        internal Unit(IRegister root, IUnit parent = null)
            : this() {
            Parent = parent;
            Root = root;
            if (Root == null) { InitState = false; }
        }

        #endregion Structure

        #region Field

        private string _id;
        private IUnit _parent;
        // Start Index Number for dimention 
        private const int StartIndex = 0;
        internal const char SerializeSplitString = ':';
        internal const string IDPara = "ID";
        internal const string Flag = "Flag";

        #endregion Field

        #region Property

        /// <summary>
        /// Init
        /// </summary>
        internal virtual void Init() {
            InitState = true;
            Index = StartIndex;
        }

        /// <summary>
        /// id of a unit
        /// </summary>
        public String ID {
            get { return _id; }
            internal set {
                if (value != _id) {
                    UpdateID(value);
                }
            }
        }

        /// <summary>
        /// Parent of a unit
        /// </summary>
        public IUnit Parent {
            get { return _parent; }
            set {
                if (value != _parent) {
                    _parent = value;
                    UpdateAddress();
                    UpdateIndex();
                    CheckDepth();
                }
            }
        }

        /// <summary>
        /// Address of a unit
        /// </summary>
        public IAddress Address { get; internal set; }

        /// <summary>
        /// Dimention Index of the Unit
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Root of the Unit
        /// </summary>
        public IRegister Root { get; private set; }

        /// <summary>
        /// Shows if the Unit Init Successfully
        /// </summary>
        public bool InitState { get; private set; }

        /// <summary>
        /// Config file info
        /// </summary>
        public XElement Config { get; private set; }

        /// <summary>
        /// Serialization Info
        /// </summary>
        public SerializationInfo SInfo { get; private set; }

        /// <summary>
        /// Serialization StreamingContext
        /// </summary>
        public StreamingContext SContext { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Init info from xml
        /// </summary>
        /// <param name="element"></param>
        public virtual void InitXML() {
            if (Config == null) { InitState = false; return; }
            if (!XML.InitStringAttr<string>(Config, IDPara, out _id)) { InitState = false; }
        }

        /// <summary>
        /// Init ID
        /// </summary>
        public virtual bool UpdateID(string newID) {
            if (IsDuplicateID(newID)) { return false; }
            if (Parent != null) {
                ((ISingularity)Parent).Container.ChangeID(_id, newID);
            }
            _id = newID;
            UpdateAddress();
            return true;
        }

        /// <summary>
        /// Init the address of a unit
        /// </summary>
        public virtual void UpdateAddress() {
            if (Address != null) { Address.Dispose(); }
            Address = new Address();
            Address.Parse(ID, (Parent == null) ? null : Parent.Address);
        }

        /// <summary>
        /// CheckDuplicateID to avoid the same id in one universe
        /// </summary>
        internal virtual bool IsDuplicateID(string id) {
            if (Parent == null) { return false; }
            return false;
        }

        /// <summary>
        /// Update index
        /// </summary>
        public virtual void UpdateIndex() {
            Index = (Parent == null) ? StartIndex : (Parent.Index + 1);
        }

        /// <summary>
        /// Update Root
        /// </summary>
        public virtual void UpdateRoot(IRegister register) {
            Root = register;
            CheckDepth();
        }

        /// <summary>
        /// Check if over index
        /// </summary>
        /// <returns></returns>
        private void CheckDepth() {
            if (Root == null) { return; }
            if (Index <= Root.Depth) { return; }
            InitState = false;
            //**********************************lock myselft oh fukky!!!!
            //if (Parent == null) { return; }
            //if (((ISingularity)Parent).Container.Contain(ID)) {
            //    ((ISingularity)Parent).Container.Remove(ID);
            //}
        }

        /// <summary>
        /// Wite info to Unit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual bool Write<T>(IAddress address, T data) {
            if (address.LastName != ID) { return false; }
            return true;
        }

        /// <summary>
        /// Read info From Unit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public virtual bool Read<T>(IAddress address, out T result) {
            result = default(T);
            if (address.LastName != ID) { return false; }
            return true;
        }

        /// <summary>
        /// Serialize
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) {
            // Use the AddValue method to specify serialized values.
            info.AddValue(Address.FullName + SerializeSplitString + IDPara, ID, typeof(string));
        }

        /// <summary>
        /// Deserialize States
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <param name="address"></param>
        /// <param name="root"></param>
        /// <param name="parent"></param>
        public virtual bool Deserialize(IAddress address, IRegister root, IUnit parent = null) {
            if (address == null) { return false; }
            ID = address.Tags[0];
            if (root != null) {
                UpdateRoot(root);
            }
            if (parent != null) {
                Parent = parent;
            }
            return true;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose() {
            Address.Dispose();
        }

        #endregion Function

    }
}
