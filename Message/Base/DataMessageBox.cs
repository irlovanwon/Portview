///Copyright(c) 2015,HIT All rights reserved.
///Summary:DataMessageBox
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      

using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Message
{
    public class DataMessageBox
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="maxCount"></param>
        public DataMessageBox(int maxCount = 0) {
            MaxCount = maxCount;
        }

        #endregion Structure

        #region Field

        /// <summary>
        /// Message queue
        /// </summary>
        private Queue<IDataMessage> _messages = new Queue<IDataMessage>();
        private object _lock = new object();
        private const string TagName = "Message";

        #endregion Field

        #region Property

        /// <summary>
        /// Max count of the queue
        /// </summary>
        public int MaxCount { get; set; }

        #endregion Property

        #region Function

        /// <summary>
        /// push message
        /// </summary>
        public void Push(IDataMessage message) {
            if (MaxCount == 0) { return; }
            lock (_lock) {
                while (_messages.Count >= MaxCount) { _messages.Dequeue(); }
                _messages.Enqueue(message);
            }
        }

        /// <summary>
        /// Messages to xml
        /// </summary>
        /// <returns></returns>
        public XElement ToXML(int count = -1) {
            lock (_lock) {
                if (_messages.Count == 0) { return null; }
                XElement result = new XElement(TagName);
                foreach (var item in _messages) {
                    result.AddFirst(item.ToXML());
                }
                int index = 0;
                foreach (var item in result.Elements()) {
                    if ((count >= 0) && (index >= count)) { item.Remove(); }
                    index++;
                }
                return result;
            }
        }

        /// <summary>
        /// Messages to xml
        /// </summary>
        /// <returns></returns>
        public List<IDataMessage> ToDataMessages(int count = -1) {
            lock (_lock) {
                if (_messages.Count == 0) { return null; }
                List<IDataMessage> result = new List<IDataMessage>();
                foreach (var item in _messages) {
                    result.Add(item);
                }
                result.Reverse();
                if ((count >= 0) && (result.Count > count)) {
                    result = result.GetRange(0, count);
                }
                return result;
            }
        }

        #endregion Function

    }
}
