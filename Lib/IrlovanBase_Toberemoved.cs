/////Copyright(c) 2013,HIT All rights reserved.
/////Summary:Helper
/////Author:Irlovan
/////Date:2013-04-01
/////Description:
/////Modification:


//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading;
//using System.Timers;
//using System.Xml.Linq;

//namespace Irlovan.Helper
//{
//    public static class Helper
//    {
//        /*
//         * sample:
//         * int[] data = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
//         * int[] sub = data.SubArray(3, 4); // contains {3,4,5,6}
//         */
//        public static T[] SubArray<T>(this T[] data, int index, int length) {
//            T[] result = new T[length];
//            System.Array.Copy(data, index, result, 0, length);
//            return result;
//        }

//        //get the list of a named attibute from the child of the xelement
//        public static System.Array GetChildAttributeArray(XElement element, string attrName, bool opc = false) {
//            List<string> result = new List<string>();
//            if (opc) { result.Add(""); }
//            foreach (var item in element.Elements()) {
//                result.Add(item.Attribute(attrName).Value);
//            }
//            return result.ToArray();
//        }




//        //Rename a key in the Dictionary
//        public static void RenameKey<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey fromKey, TKey toKey) {
//            TValue value = dic[fromKey];
//            dic.Remove(fromKey);
//            dic[toKey] = value;
//        }







//        /// <summary>
//        ///IsLocalIpAddress("localhost");        // true (loopback name)
//        ///IsLocalIpAddress("127.0.0.1");        // true (loopback IP)
//        ///IsLocalIpAddress("MyNotebook");       // true (my computer name)
//        ///IsLocalIpAddress("192.168.0.1");      // true (my IP)
//        ///IsLocalIpAddress("NonExistingName");  // false (non existing computer name)
//        ///IsLocalIpAddress("99.0.0.1");         // false (non existing IP in my net)
//        /// </summary>
//        /// <param name="host"></param>
//        /// <returns></returns>
//        public static bool IsLocalIpAddress(string host) {
//            try { // get host IP addresses
//                IPAddress[] hostIPs = Dns.GetHostAddresses(host);
//                // get local IP addresses
//                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

//                // test if any host IP equals to any local IP or to localhost
//                foreach (IPAddress hostIP in hostIPs) {
//                    // is localhost
//                    if (IPAddress.IsLoopback(hostIP)) return true;
//                    // is local address
//                    foreach (IPAddress localIP in localIPs) {
//                        if (hostIP.Equals(localIP)) return true;
//                    }
//                }
//            } catch { }
//            return false;
//        }
//        /// <summary>
//        /// get default value of a Type
//        /// </summary>
//        /// <param name="type"></param>
//        /// <returns></returns>
//        public static object GetDefault(Type type) {
//            if (type.IsValueType) {
//                return Activator.CreateInstance(type);
//            }
//            return null;
//        }







//        /// <summary>
//        /// convert list to string 
//        /// </summary>
//        /// <returns></returns>
//        public static string ListToString(List<string> list) {
//            if ((list == null) || (list.Count == 0)) { return string.Empty; }
//            StringBuilder result = new StringBuilder();
//            foreach (var item in list) {
//                result.Append(item);
//                result.Append(",");
//            }
//            return result.ToString();
//        }
//    }
//}
