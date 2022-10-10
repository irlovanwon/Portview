///Copyright(c) 2015,HIT All rights reserved.
///Summary:Matrix Array
///Author:Irlovan
///Date:2015-11-15
///Description:To store a group of data sorted in matrix with titles of string list
///Modification:

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Irlovan.Lib.Array
{
    public class MatrixArray<T>
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        public MatrixArray(string[] title, string key = null) {
            _key = key;
            Rows = new List<T[]>();
            RowDic = new Dictionary<T, T[]>();
            InitState = true;
            if (!InitTitle(title)) { throw new Exception(TitleNotValidExceptionStr); };
            _width = title.Length;
        }

        #endregion Structure

        #region Field

        //All the exception should be uniformed so this is temprarorily!!!!**************
        private const string TitleNotValidExceptionStr = "The title of the Matrix Array is not valid";

        private string[] _title;
        private int _width;
        private int _keyIndex = -1;
        private string _key;

        public const string MatrixTag = "MatrixArray";
        public const string MatrixRowItemTag = "MatrixRow";
        public const char RowSplitChar = ';';
        public const char ColumnSplitChar = ',';

        #endregion Field

        #region Property

        /// <summary>
        /// Title of the matrix array
        /// </summary>
        public string[] Title {
            get { return _title; }
        }

        /// <summary>
        /// Colum count
        /// </summary>
        public int Width {
            get { return _width; }
        }

        /// <summary>
        /// Rows
        /// </summary>
        public List<T[]> Rows { get; private set; }

        /// <summary>
        /// Rows Dictionary
        /// </summary>
        public Dictionary<T, T[]> RowDic { get; private set; }

        /// <summary>
        /// Init State
        /// </summary>
        public bool InitState { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Add row to the matrix
        /// </summary>
        /// <param name="columnList"></param>
        public bool AddRow(T[] columnList) {
            if (columnList.Length != _width) { return false; }
            if ((!string.IsNullOrEmpty(_key)) && (_keyIndex != -1) && (columnList[_keyIndex] != null) && (!RowDic.ContainsKey(columnList[_keyIndex]))) { RowDic.Add(columnList[_keyIndex], columnList); }
            Rows.Add(columnList);
            return true;
        }

        /// <summary>
        /// Get Column by title 
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public T[] GetColumn(string title) {
            int index = GetTitleIndex(title);
            if (index == -1) { return null; }
            T[] result = new T[Rows.Count];
            for (int i = 0; i < result.Length; i++) {
                result[i] = Rows[i][index];
            }
            return result;
        }

        /// <summary>
        /// Write data message to xml
        /// </summary>
        /// <returns></returns>
        public XElement ToXML(bool desc = false) {
            XElement result = new XElement(MatrixTag);
            foreach (var item in Rows) {
                if (desc) { result.AddFirst(CreateRowXElement(item)); }
                else { result.Add(CreateRowXElement(item)); }
            }
            return result;
        }

        /// <summary>
        /// dispose
        /// </summary>
        public void Dispose() { }

        /// <summary>
        /// Create Row XElement Message
        /// </summary>
        /// <returns></returns>
        public XElement CreateRowXElement(T[] rowInfo) {
            XElement result = new XElement(MatrixRowItemTag);
            for (int i = 0; i < rowInfo.Length; i++) {
                try { result.SetAttributeValue(Title[i], rowInfo[i].ToString()); }
                catch (Exception) { continue; }
            }
            return result;
        }

        /// <summary>
        /// Write data message to string
        /// </summary>
        /// <returns></returns>
        public new string ToString() {
            StringBuilder result = new StringBuilder();
            foreach (var item in Rows) {
                result.Append(CreateRowString(item));
                result.Append(RowSplitChar);
            }
            return result.ToString().TrimEnd(RowSplitChar);
        }

        /// <summary>
        /// Create Row XElement Message
        /// </summary>
        /// <returns></returns>
        private string CreateRowString(T[] rowInfo) {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < rowInfo.Length; i++) {
                result.Append(rowInfo[i]);
                result.Append(ColumnSplitChar);
            }
            return result.ToString().TrimEnd(ColumnSplitChar);
        }

        /// <summary>
        /// if a matrix init successfull
        /// </summary>
        /// <returns></returns>
        private bool InitTitle(string[] title) {
            if (title.Length == 0) { return false; }
            //check duplicate items from a list using LINQ ... 
            var result = title.GroupBy(s => s).SelectMany(grp => grp.Skip(1));
            if (result.Count() != 0) { return false; }
            _title = title;
            if (!string.IsNullOrEmpty(_key)) { _keyIndex = GetTitleIndex(_key); }
            return true;
        }

        /// <summary>
        /// Get Title Index
        /// </summary>
        /// <returns></returns>
        private int GetTitleIndex(string title) {
            return System.Array.FindIndex(_title, s => s == title);
        }

        #endregion Function

    }
}
