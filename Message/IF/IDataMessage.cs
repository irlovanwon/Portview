///Copyright(c) 2015,HIT All rights reserved.
///Summary:DataMessage interface
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      

namespace Irlovan.Message
{
    public interface IDataMessage : IMessage
    {

        /// <summary>
        /// Name of the data 
        /// </summary>s
        string Name { get; }

        /// <summary>
        /// Description for data
        /// </summary>
        string Description { get; }

    }

}
