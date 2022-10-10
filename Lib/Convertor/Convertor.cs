///Copyright(c) 2015,HIT All rights reserved.
///Summary:
///Author:Irlovan
///Date:2015-01-31
///Description:
///Modification:

using System;

namespace Irlovan.Lib.Convertor
{
    public class Convertor
    {

        /// <summary>
        /// Change object to a type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool ConvertType<T>(object value, out T result) {
            result = default(T);
            if (value == null) { return false; }
            try {
                result = (T)Convert.ChangeType(value, typeof(T));
                return true;
            } catch {
                return false;
            }
        }

        /// <summary>
        /// Change object to a type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="resultType"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool ConvertType(object value, Type resultType, out object result) {
            try {
                result = Convert.ChangeType(value, resultType);
                return true;
            } catch {
                result = null;
                return false;
            }
        }

    }
}
