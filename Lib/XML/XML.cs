///Copyright(c) 2015,HIT All rights reserved.
///Summary:XML Helper
///Author:Irlovan
///Date:2015-03-21
///Description:
///Modification:

using System.ComponentModel;
using System.Xml.Linq;

namespace Irlovan.Lib.XML
{
    public static class XML
    {

        /// <summary>
        /// Parse XML From String
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static XElement Parse(string xmlStr) {
            XElement result;
            try { result = XElement.Parse(xmlStr); }
            catch { return null; }
            return result;
        }

        /// <summary>
        /// Init an appointed attribute
        /// </summary>
        public static bool InitStringAttr<T>(XElement config, string paraName, out T value) {
            value = default(T);
            if (config == null) { return false; }
            XAttribute attr = config.Attribute(paraName);
            if ((attr == null) || (!TryParseT<T>(attr.Value, out value))) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Try to parse string to type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool TryParseT<T>(string input, out T value) {
            value = default(T);
            try {
                value = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(input);
            }
            catch {
                return false;
            }
            return true;
        }

    }
}
