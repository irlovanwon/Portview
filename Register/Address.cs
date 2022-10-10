///Copyright(c) 2015,HIT All rights reserved.
///Summary:Register address
///Author:Irlovan
///Date:2015-11-12
///Description:
///Modification:      

using System.Linq;
using System.Text;

namespace Irlovan.Register
{
    public class Address : IAddress
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        public Address() { }

        #endregion Structure

        #region Field

        private IAddress _parentAddress;
        private string _fullName;
        internal const char SplitChar = '.';

        #endregion Field

        #region Property

        /// <summary>
        /// Full Name of the address
        /// </summary>
        public string FullName {
            get { return _fullName; }
            set {
                if (value != _fullName) {
                    _fullName = value;
                }
            }
        }

        /// <summary>
        /// First Name of the address
        /// </summary>
        public string FirstName { get; private set; }

        /// <summary>
        /// Last Name of the address
        /// </summary>
        public string LastName { get; private set; }


        /// <summary>
        /// Tags of the address
        /// </summary>
        public string[] Tags { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() { }

        /// <summary>
        /// Parse from string
        /// </summary>
        /// <param name="address"></param>
        public bool Parse(string address) {
            if (!ParseText(address)) { return false; }
            if (string.IsNullOrEmpty(address)) { return false; }
            Tags = address.Split(SplitChar);
            LastName = Tags[0];
            FirstName = Tags[Tags.Length - 1];
            FullName = address;
            return true;
        }

        /// <summary>
        /// Parse from parent
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentAddress"></param>
        public bool Parse(string id, IAddress parentAddress = null) {
            if (!ParseText(id)) { return false; }
            FirstName = id;
            _parentAddress = parentAddress;
            if (_parentAddress != null) {
                FullName = parentAddress.FullName + SplitChar + FirstName;
            }
            else {
                FullName = FirstName;
            }
            Tags = FullName.Split(SplitChar);
            LastName = Tags[0];
            return true;
        }

        /// <summary>
        /// Parse tags
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentAddress"></param>
        public bool Parse(string[] tags) {
            if (tags.Length == 0) { return false; }
            StringBuilder fullName = new StringBuilder();
            foreach (var item in tags) {
                if (!ParseText(item)) { return false; }
                fullName.Append(item);
                fullName.Append(SplitChar);
            }
            FullName = fullName.ToString().TrimEnd(SplitChar);
            Tags = tags;
            LastName = Tags[0];
            FirstName = Tags[Tags.Length - 1];
            return true;
        }

        /// <summary>
        /// Parse Text
        /// </summary>
        /// <param name="addressText"></param>
        /// <returns></returns>
        private bool ParseText(string addressText) {
            if (string.IsNullOrEmpty(addressText)) { return false; }
            if (addressText.Contains(Unit.SerializeSplitString)) { return false; }
            return true;
        }

        /// <summary>
        /// Get Child Address
        /// </summary>
        /// <returns></returns>
        public IAddress GetChild() {
            if (Tags.Length <= 1) { return null; }
            IAddress address = new Address();
            address.Parse(Tags.ToList().GetRange(1, Tags.Length - 1).ToArray());
            return address;
        }

        /// <summary>
        /// GetAddressFromRange
        /// </summary>
        /// <returns></returns>
        public IAddress GetRange(int index, int length) {
            if (Tags.Length <= 1) { return null; }
            IAddress address = new Address();
            address.Parse(Tags.ToList().GetRange(index, length).ToArray());
            return address;
        }

        #endregion Function

    }
}
