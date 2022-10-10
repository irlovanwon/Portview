///Copyright(c) 2015,HIT All rights reserved.
///Summary:EventQuantum
///Author:Irlovan
///Date:2015-11-13
///Description:
///Modification:

using Irlovan.Database;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Irlovan.Quantum
{
    public class EventQuantum
    {

        #region Structure

        /// <summary>
        /// Event Quantum
        /// </summary>
        /// <param name="source"></param>
        public EventQuantum(Catalog source) {
            Source = source;
        }

        #endregion Structure

        #region Field

        private const string ProcessorNamePara = "Processor";
        private const string QuantumNamePara = "Quantum";
        private const string ProcessorTypePara = "Type";
        private const string TriggerNamePara = "Trigger";
        private const string IDPara = "ID";
        private const string NamePara = "Name";
        private const string DescriptsionPara = "Descriptsion";
        private Dictionary<string, Type> _processorDic = new Dictionary<string, Type>() { { DotNetScriptProcessor.ProcessorName, typeof(DotNetScriptProcessor) } };

        #endregion Field

        #region Property

        /// <summary>
        /// an event will have a butterfly effect
        /// </summary>
        public Dictionary<int, EventQuantum> Causation { get; private set; }

        /// <summary>
        /// what cause an event
        /// </summary>
        //public Dictionary<int, EventQuantum> Primer { get; private set; }

        /// <summary>
        /// the unique identification of a quantum
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Root
        /// </summary>
        public EventQuantum Root { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Logically Event Processor
        /// </summary>
        public Dictionary<int, IProcessorQuantum> Processors { get; private set; }

        /// <summary>
        /// Trigger of the Quantum
        /// </summary>
        public ITriggerQuantum Trigger { get; private set; }

        /// <summary>
        /// Source
        /// </summary>
        public Catalog Source { get; private set; }

        #endregion Property

        #region Delegate

        private delegate void ProcessorHandler();
        private delegate void TriggerHandler();

        #endregion Delegate

        #region Function

        /// <summary>
        /// Active the triggerAmplifier
        /// </summary>
        public void TriggerAmplifier() {
            Trigger.SwitchOn += SwitchOnHandler;
            Trigger.SwitchOff += SwitchOffHandler;
            Trigger.Trigger();
        }

        /// <summary>
        /// SwitchOnHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchOnHandler(object sender, EventArgs e) {
            for (int i = 0; i < Processors.Count; i++) {
                if (Processors[i].IsSyn) {
                    Processors[i].Run();
                }
                else {
                    ProcessorHandler _processorHandler = new ProcessorHandler(Processors[i].Run);
                    _processorHandler.BeginInvoke(null, null);
                }
                if (Processors[i].Next != null) {
                    foreach (var item in Processors[i].Next) {
                        EventQuantum quantum = Root.AcquireQuantum(item);
                        if (quantum.Trigger.Activator == 0) {
                            TriggerHandler trigger = new TriggerHandler(Root.AcquireQuantum(item).TriggerAmplifier);
                            trigger.BeginInvoke(null, null);
                        }
                    }
                }
            }
            Burst();
        }

        /// <summary>
        /// SwitchOffHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchOffHandler(object sender, EventArgs e) {
            foreach (var item in Causation) {
                item.Value.StopAmplifier();
            }
            StopAmplifier();
        }

        /// <summary>
        /// StopAmplifier
        /// </summary>
        public void StopAmplifier() {
            Trigger.StopTrigger();
            if (Trigger.Activator == 0) {
                Trigger.SwitchOn -= SwitchOnHandler;
                Trigger.SwitchOff -= SwitchOffHandler;
                foreach (var Processor in Processors) {
                    Processor.Value.Stop();
                }
                if ((Causation != null) && (Causation.Count != 0)) {
                    foreach (var item in Causation) {
                        item.Value.StopAmplifier();
                    }
                }
            }
        }

        /// <summary>
        /// Burst
        /// </summary>
        private void Burst() {
            if ((Causation != null) && (Causation.Count != 0)) {
                foreach (var item in Causation) {
                    TriggerHandler trigger = new TriggerHandler(item.Value.TriggerAmplifier);
                    trigger.BeginInvoke(null, null);
                }
            }
        }

        /// <summary>
        /// read from xml file
        /// </summary>
        /// <returns></returns>
        public void ReadXML(XElement element) {
            Processors = new Dictionary<int, IProcessorQuantum>();
            Trigger = new TriggerQuantum(Source);
            Causation = new Dictionary<int, EventQuantum>();
            ID = int.Parse(element.Attribute(IDPara).Value);
            Name = element.Attribute(NamePara).Value;
            if (element.Attribute(Description) != null) {
                Description = element.Attribute(Description).Value;
            }
            Trigger.ReadXML(element.Element(TriggerNamePara));
            foreach (var item in element.Elements(ProcessorNamePara)) {
                IProcessorQuantum result = (IProcessorQuantum)Activator.CreateInstance(_processorDic[item.Attribute(ProcessorTypePara).Value], new Object[] { Source });
                result.ReadXML(item);
                Processors.Add(result.ID, result);
            }
            foreach (var item in element.Elements(QuantumNamePara)) {
                EventQuantum result = new EventQuantum(Source);
                result.ReadXML(item);
                result.Root = this;
                Causation.Add(result.ID, result);
            }
        }

        /// <summary>
        /// Acquire Quantum by ID
        /// </summary>
        /// <param name="id"></param>
        public EventQuantum AcquireQuantum(int id) {
            if (ID == id) { return this; }
            foreach (var item in Causation) {
                item.Value.AcquireQuantum(id);
            }
            return null;
        }

        #endregion Function

    }
}
