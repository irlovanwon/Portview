///Copyright(c) 2013,HIT All rights reserved.
///Summary：
///Author：Irlovan
///Date：2013-12-24
///Description：
///Modification：


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Irlovan
{
    public class AutoBooleanGenerate
    {

        #region Structure

        public AutoBooleanGenerate() {
            //GenerateTagLampO();
            GenerateAllInOne();
        }

        #endregion Structure

        #region Field

        private string _driverFilePath = @"E:\WebArchitecture\WebArcServer\bin\Debug\Project\Core\Drivers";
        private int _index = 1;
        private List<string> _addrs = new List<string>();
        private List<string> _meters = new List<string>();
        private List<string> _allinonebools = new List<string>();
        private List<string> _descs = new List<string>();
        private string _fileOutputPath = "d:/acts.txt";
        private string _headString = "CW.CRANE3.State.";
        private string _name = "State";
        private string _filePath = "E:/WebArchitecture/WebArcServer/bin/Debug/Project/Core/Driver";
        private string[] _sourceLines;
        private string _tagStart = "<Control Name=\"Tag\" ID=\"";
        private string _tagMid = "\" ContainerID=\"MainControlContainer\" GridContainerID=\"controlgrid\" Class=\"null\" Config=\"{'tagName':{'Attributes':'tagName','Value':'";
        private string _tagEnd = "'},'fontSize':{'Attributes':'fontSize','Value':'15'},'color':{'Attributes':'color','Value':'blue'},'backgroundColor':{'Attributes':'backgroundColor','Value':'transparent'},'borderColor':{'Attributes':'borderColor','Value':'transparent 1px solid'},'width':{'Attributes':'width','Value':'180'},'height':{'Attributes':'height','Value':'15'},'left':{'Attributes':'left','Value':'927.9794921875px','Description':'The Control Pos:Left'},'top':{'Attributes':'top','Value':'621px','Description':'The Control Pos:Top'},'zIndex':{'Attributes':'zIndex','Value':'auto','Description':'The Control ZIndex'},'id':{'Attributes':'id','Value':'GridControl_Tag_1_paste','Description':'The Control Id'},'isLock':{'Attributes':'isLock','Value':'true','Description':'Show if the control is Draggable'}}\" Left=\"927.9794921875px\" Top=\"1024px\" ZIndex=\"10\" IsLock=\"true\" />";
        private string _lampStart = "<Control Name=\"RoundLamp\" ID=\"";
        private string _lampMid = "\" ContainerID=\"MainControlContainer\" GridContainerID=\"controlgrid\" Class=\"null\" Config=\"{'value':{'Attributes':'value','Value':'false','Expression':'";
        private string _lampEnd = "'},'color':{'Attributes':'color','Value':'1','Description':'1:green2:red'},'size':{'Attributes':'size','Value':'20'},'left':{'Attributes':'left','Value':'1188px','Description':'The Control Pos:Left'},'top':{'Attributes':'top','Value':'857px','Description':'The Control Pos:Top'},'zIndex':{'Attributes':'zIndex','Value':'auto','Description':'The Control ZIndex'},'id':{'Attributes':'id','Value':'GridControl_RoundLamp_0_paste_paste','Description':'The Control Id'},'isLock':{'Attributes':'isLock','Value':'true','Description':'Show if the control is Draggable'}}\" Left=\"927px\" Top=\"1024px\" ZIndex=\"10\" IsLock=\"true\" />";

        private string _recLamp_1 = "<Control Name=\"RecLamp\" ContainerID=\"MainControlContainer\" GridContainerID=\"controlgrid\" Class=\"null\" Config=\"{'tagName':{'Attributes':'tagName','Value':'";
        private string _recLamp_2 = "'},'value':{'Attributes':'value','Value':false,'Expression':'#";
        private string _recLamp_3 = "#'},'fontSize':{'Attributes':'fontSize','Value':'18'},'borderColor':{'Attributes':'borderColor','Value':'transparent 1px solid'},'size':{'Attributes':'size','Value':'16px'},'fontColor':{'Attributes':'fontColor','Value':'black'},'posFix':{'Attributes':'posFix','Value':'500px'},'onColor':{'Attributes':'onColor','Value':'lime'},'offColor':{'Attributes':'offColor','Value':'white'},'left':{'Attributes':'left','Value':'1678.89px','Description':'The Control Pos:Left'},'top':{'Attributes':'top','Value':'308.86px','Description':'The Control Pos:Top'},'zIndex':{'Attributes':'zIndex','Value':'auto','Description':'The Control ZIndex'},'id':{'Attributes':'id','Value':'GridControl_RecLamp_0','Description':'The Control Id'},'isLock':{'Attributes':'isLock','Value':'false','Description':'Show if the control is Draggable'}}\" Left=\"1678.89px\" Top=\"308.86px\" ZIndex=\"auto\" IsLock=\"true\" ID=\"BoolState_";
        private string _recLamp_4 = "\" />";

        private string _meter_1 = "<Control Name=\"Meter\" IsLock=\"true\" ZIndex=\"auto\" Top=\"227.73px\" Left=\"2000.62px\" Config=\"{'tagName':{'Attributes':'tagName','Value':'";
        private string _meter_2 = ":'},'value':{'Attributes':'value','Value':0,'Expression':'#";
        private string _meter_3 = "#'},'namePosFix':{'Attributes':'namePosFix','Value':5},'valuePosFix':{'Attributes':'valuePosFix','Value':'220'},'width':{'Attributes':'width','Value':'400'},'height':{'Attributes':'height','Value':'40'},'maxValue':{'Attributes':'maxValue','Value':'10000'},'minValue':{'Attributes':'minValue','Value':'-10000'},'majorTicks':{'Attributes':'majorTicks','Value':5,'Description':''},'minorTicks':{'Attributes':'minorTicks','Value':5,'Description':''},'hMargin':{'Attributes':'hMargin','Value':20},'vMargin':{'Attributes':'vMargin','Value':10},'vAlignment':{'Attributes':'vAlignment','Value':'bottom'},'hAlignment':{'Attributes':'hAlignment','Value':'left'},'fontColor':{'Attributes':'fontColor','Value':'#111111','Description':''},'fontSize':{'Attributes':'fontSize','Value':9,'Description':''},'stepColor':{'Attributes':'stepColor','Value':'#000000','Description':''},'majorTicksStrokeWidth':{'Attributes':'majorTicksStrokeWidth','Value':2,'Description':''},'majorTicksLength':{'Attributes':'majorTicksLength','Value':12,'Description':''},'minorTicksStrokeWidth':{'Attributes':'minorTicksStrokeWidth','Value':1,'Description':''},'minorTicksLength':{'Attributes':'minorTicksLength','Value':6,'Description':''},'meterStrokeWidth':{'Attributes':'meterStrokeWidth','Value':3,'Description':''},'meterOpacity':{'Attributes':'meterOpacity','Value':1,'Description':''},'meterColor':{'Attributes':'meterColor','Value':'#00ffff','Description':''},'speed':{'Attributes':'speed','Value':400,'Description':''},'easing':{'Attributes':'easing','Value':'','Description':''},'left':{'Attributes':'left','Value':'2000.62px','Description':'The Control Pos:Left'},'top':{'Attributes':'top','Value':'227.73px','Description':'The Control Pos:Top'},'zIndex':{'Attributes':'zIndex','Value':'auto','Description':'The Control ZIndex'},'id':{'Attributes':'id','Value':'GridControl_Meter_0','Description':'The Control Id'},'isLock':{'Attributes':'isLock','Value':'true','Description':'Show if the control is Draggable'}}\" Class=\"null\" GridContainerID=\"controlgrid\" ContainerID=\"MainControlContainer\" ID=\"_Meter_AutoGenerate_";
        private string _meter_4 = "\" />";

        #endregion Field

        #region Property
        #endregion Property

        #region Delegate
        #endregion Delegate

        #region Event
        #endregion Event

        #region Function

        private void GenerateTagLampO() {
            _sourceLines = File.ReadAllLines(_filePath);
            foreach (var item in _sourceLines) {
                //if (item.ToLower().Contains(_fileterString)) {
                if (true) {
                    string addr = "#" + _headString + item.Split(',')[0] + "#";
                    string desc = item.Split(',')[4];
                    _addrs.Add(_tagStart + _name + _index.ToString() + _tagMid + desc + _tagEnd);
                    _index++;
                    _descs.Add(_lampStart + _name + _index.ToString() + _lampMid + addr + _lampEnd);
                    _index++;
                }
            }
            File.AppendAllLines(_fileOutputPath, _addrs);
            File.AppendAllLines(_fileOutputPath, _descs);
        }

        private void GenerateAllInOne() {
            XDocument doc = XDocument.Load(_driverFilePath);
            var ele = doc.Root.Element("OPCClient").Elements("Group");
            foreach (var item in ele) {
                foreach (var child in item.Elements()) {
                    string name = child.Attribute("RealtimeDataName").Value;
                    string addr = child.Attribute("Addr").Value;
                    XAttribute descAttr = child.Attribute("Description");
                    if ((descAttr != null) && (descAttr.Value.Contains("%M"))) {
                        _allinonebools.Add(_recLamp_1 + descAttr.Value.Replace(" ", "_").Replace("-", "_").Replace("%", "_%") + _recLamp_2 + name + _recLamp_3 + _index.ToString() + _recLamp_4);
                         _index++;
                    }
                    else {
                        _allinonebools.Add(_meter_1 + name.Split('.')[name.Split('.').Length - 1] + _meter_2 + name + _meter_3 + _index.ToString() + _meter_4); 
                        _index++;
                    }
                   
                   
                }
            }
            File.AppendAllLines(_fileOutputPath, _allinonebools);
            File.AppendAllLines(_fileOutputPath, _meters);
        }


        #endregion Function

        #region InterClass
        #endregion InterClass

    }
}
