///Copyright(c) 2015,HIT All rights reserved.
///Summary:Trigger interface
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:      

using System;
using System.Xml.Linq;

namespace Irlovan.Quantum
{
    public interface ITriggerQuantum
    {

        /// <summary>
        /// switch
        /// </summary>
        bool Switch { get; set; }

        /// <summary>
        /// stop trigger
        /// </summary>
        void StopTrigger();

        /// <summary>
        /// trigger
        /// </summary>
        void Trigger();

        /// <summary>
        /// Activator
        /// </summary>
        int Activator { get; set; }

        /// <summary>
        /// Read config file
        /// </summary>
        /// <param name="element"></param>
        void ReadXML(XElement element);

        /// <summary>
        /// Switch on
        /// </summary>
        event SwitchHandler SwitchOn;

        /// <summary>
        /// Switch off
        /// </summary>
        event SwitchHandler SwitchOff;

    }

    /// <summary>
    /// Handler for switch
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SwitchHandler(object sender, EventArgs e);

}
