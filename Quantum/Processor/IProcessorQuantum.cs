///Copyright(c) 2015,HIT All rights reserved.
///Summary:ProcessorQuantum interface
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      

using System.Xml.Linq;

namespace Irlovan.Quantum
{
    public interface IProcessorQuantum
    {

        /// <summary>
        /// Run
        /// </summary>
        void Run();

        /// <summary>
        /// Stop
        /// </summary>
        void Stop();

        /// <summary>
        /// ReadXML
        /// </summary>
        /// <param name="element"></param>
        void ReadXML(XElement element);

        /// <summary>
        /// IsSyn
        /// </summary>
        bool IsSyn { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        int ID { get; set; }

        /// <summary>
        /// Next
        /// </summary>
        int[] Next { get; set; }

    }
}
