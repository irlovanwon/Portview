///Copyright(c) 2015,HIT All rights reserved.
///Summary:Helper for array
///Author:Irlovan
///Date:2015-03-23
///Description:
///Modification:

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Irlovan.Lib.Array
{
    public static class Array
    {

        /// <summary>
        /// Combine two array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        public static T[] Combin<T>(T[] array1, T[] array2) {
            T[] result = new T[array1.Length + array2.Length];
            System.Array.Copy(array1, result, array1.Length);
            System.Array.Copy(array2, 0, result, array1.Length, array2.Length);
            return result;
        }

        /// <summary>
        /// Combine two IEnumerable
        /// </summary>
        /// <param name="col1"></param>
        /// <param name="col2"></param>
        /// <returns></returns>
        public static IEnumerable<T> Combine<T>(IEnumerable<T> col1, IEnumerable<T> col2) {
            foreach (T item in col1)
                yield return item;
            foreach (T item in col2)
                yield return item;
        }

        /// <summary>
        /// ICollection to array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T[] ToArray<T>(ICollection<T> collection) {
            T[] result = new T[collection.Count];
            int index = 0;
            foreach (var item in collection) {
                result[index] = item;
                index++;
            }
            return result;
        }

        /// <summary>
        /// ArrayToType
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T1[] ArrayToType<T2, T1>(T2[] array) {
            T1[] result = new T1[array.Length];
            for (int i = 0; i < array.Length; i++) {
                result[i] = (T1)Convert.ChangeType(array[i], typeof(T1));
            }
            return result;
        }

        /// <summary>
        /// List<string> to string
        /// </summary>
        /// <returns></returns>
        public static string ListToString(List<string> list, char splitChar) {
            StringBuilder result = new StringBuilder();
            foreach (var item in list) {
                result.Append(item);
                result.Append(splitChar);
            }
            return result.ToString().TrimEnd(splitChar);
        }

        /// <summary>
        /// Create Serial Array
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static int[] CreateSerialArray(int start, int end) {
            if (start == end) { return new int[] { start }; }
            int length = Math.Abs(start - end) + 1;
            int[] result = new int[length];
            for (int i = 0; i < length; i++) {
                result[i] = (start > end) ? (start - i) : (start + i);
            }
            return result;
        }

        /// <summary>
        /// convert array to list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static List<T> ArrayToList<T>(System.Array target) {
            return target.OfType<T>().ToList();
        }

        /// <summary>
        /// Get Child Array by range
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T[] Range<T>(T[] source, int startIndex, int length) {
            T[] result = new T[length];
            for (int i = 0; i < length; i++) {
                result[i] = source[startIndex + i];
            }
            return result;
        }

    }
}
