///Copyright(c) 2015,HIT All rights reserved.
///Summary:Each client will generate a session during communication
///Author:Irlovan
///Date:2015-11-10
///Description:
///Modification:

namespace Irlovan.Canal
{
    public class ServerSession : IServerSession
    {

        #region Structure

        /// <summary>
        /// Construction
        /// </summary>
        public ServerSession(string id, string path) {
            ID = id;
            Path = path;
        }

        #endregion Structure

        #region Property

        /// <summary>
        /// Session ID
        /// </summary>
        /// <param name="messsage"></param>
        public string ID { get; private set; }

        /// <summary>
        /// Session Path
        /// </summary>
        public string Path { get; private set; }

        #endregion Property

        #region Function

        /// <summary>
        /// Send message
        /// </summary>
        /// <param name="messsage"></param>
        public virtual void Send(string messsage) { }

        /// <summary>
        /// close and dispose for the session
        /// </summary>
        public virtual void Dispose() { }

        #endregion Function

    }
}
