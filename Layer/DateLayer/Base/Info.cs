///Copyright(c) 2015,HIT All rights reserved.
///Summary:Layer
///Author:Irlovan
///Date:2016-05-07
///Description:
///Modification:

using Irlovan.Lib.XML;
using System;
using System.Xml.Linq;

namespace HIT.Layer
{
    public class Info : IInfo
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="clockwork"></param>
        /// <param name="index"></param>
        public Info() {
            IsValid = true;
        }

        #endregion Structure

        #region Field

        private int _maxCount;
        private string _dateFormat;

        private const string DateFormat_Attr = "DateFormat";
        private const string MaxCount_Attr = "MaxCount";
        public const string TagName = "Layer";

        #endregion Field

        #region Property

        /// <summary>
        /// MaxChildCount
        /// </summary>
        public int MaxCount {
            get { return _maxCount; }
            private set {
                if (value != _maxCount) {
                    _maxCount = value;
                }
            }
        }

        /// <summary>
        /// DateFormat of the layer
        /// </summary>
        public string DateFormat {
            get { return _dateFormat; }
            private set {
                if (value != _dateFormat) {
                    _dateFormat = value;
                }
            }
        }

        /// <summary>
        /// Next Info
        /// </summary>
        public IInfo Next {
            get; private set;
        }

        /// <summary>
        /// IsValid
        /// </summary>
        public bool IsValid { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Read from xml
        /// </summary>
        /// <param name="config"></param>
        public bool ReadXML(XElement config) {
            if (config.Name != TagName) { return false; }
            if (!XML.InitStringAttr<int>(config, MaxCount_Attr, out _maxCount)) { IsValid = false; return false; }
            if (!XML.InitStringAttr<string>(config, DateFormat_Attr, out _dateFormat)) { IsValid = false; return false; }
            XElement nextConfig = config.Element(TagName);
            if (nextConfig == null) { return true; }
            Next = new Info();
            Next.ReadXML(nextConfig);
            if (!Next.IsValid) { IsValid = false; return false; }
            return true;
        }

        #endregion Function

    }
}
