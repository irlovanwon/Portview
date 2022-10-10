///Copyright(c) 2015,HIT All rights reserved.
///Summary：PLC Simulator
///Author：Irlovan
///Date：2015-03-06
///Description：
///Modification：


using Irlovan.Database;
using System;
using System.Timers;

namespace Irlovan
{
    public class PLCMap
    {


        #region Structure

        public PLCMap(Catalog source) {
            _source = source;

            new R_TRIG(MM01AG_HANDHELD, MM16AL_HHTRIG);
            new TON(UVAI, MOV_OUR_OFF_DELAY, 20000);
            TON_9 = new TON(NOESTOP_AUX, 2000);
            SetInterval(_interval, (object o, ElapsedEventArgs e) => {
                HCMD.ReadValue(HUPCMD_CMS.Value || HDNCMD_CMS.Value);
                GCMD.ReadValue(GLCMD_CMS.Value || GRCMD_CMS.Value);
                FCFSH.ReadValue((HCMD.Value || HBCI_CMS.Value) && (!GCMD.Value) && (!FCFSG.Value));
                FCFSG.ReadValue((GCMD.Value || GBCI_CMS.Value) && (!HCMD.Value) && (!FCFSH.Value));
                OHDETCT.ReadValue(DELAYOFF_FLT7620.Value || HGT7FLT_FLT7621.Value);
                //*********************************modify_reserved*********************************************
                H_CMDLOST.ReadValue(false);

                MM16AG_HH_TMOVEIN.ReadValue((MM16AL_HHTRIG.Value && MM04FG_DE_CCT_MOVE_IN_CMD.Value) || (MM01AG_HANDHELD.Value && ((MX1_4102_3.Value && (!MM16AG_HH_TMOVEIN.Value)) || ((!MX1_4102_5.Value) && MM16AG_HH_TMOVEIN.Value))) && MM777.Value);
                MM16AG_HH_TMOVEOUT.ReadValue((MM16AL_HHTRIG.Value && MM04FG_CCT_MOVE_OUT_CMD.Value) || (MM01AG_HANDHELD.Value && ((MX1_4102_5.Value && (!MM16AG_HH_TMOVEOUT.Value)) || ((!MX1_4102_3.Value) && (MM16AG_HH_TMOVEOUT.Value)))) && MM777.Value);
                MM01CG_MMFRA.ReadValue(MM01AG_CPURUN.Value);
                GES_TT.ReadValue(MM01CG_MMFRA.Value && GTTES.Value && (!SPBYSW_CMS.Value));
                HIWINDFLT.ReadValue(!HIGHW.Value);
                MM07DG_G4MCCHK.ReadValue(MM21CG_FLT3062.Value);
                MM07DG_GBCFIMCCHK.ReadValue(MM21CG_FLT3063.Value);
                MM07DG_GBCAIMCCHK.ReadValue(MM21CG_FLT3064.Value);
                Catalog sourcse=new Catalog();
                IIndustryData<String> data = sourcse.AcquireIndustryData<string>("");
                DateTime.Now.ToString();

            }, out _timer);
        }

        #endregion Structure

        #region Field

        //Data source in WebArc side
        private Catalog _source;
        //Scan Interval
        private int _interval = 200;
        private Timer _timer;
        private TON TON_9;


        #endregion Field

        #region PLCData_M

        //First Come First Service Hoist **set
        private IIndustryData<bool> FCFSH;
        //OVER HEIGHT PROTECT ACTION  **set
        private IIndustryData<bool> OHDETCT;
        // *********very interest you must record the logic here,please check the plc logic***************
        private IIndustryData<bool> H_CMDLOST;
        //HOIST BRAKE CONTACTOR 1 CHECK
        //private IIndustryData<bool> HBCCHK1;
        //HOIST BRAKE CONTACTOR 2 CHECK   
        //private IIndustryData<bool> HBCCHK2;


        private IIndustryData<bool> SMART_ATL3;
        //GANTRY COMMAND   **set
        private IIndustryData<bool> GCMD;



        //GANTRY LOCK CMD BY APTT   **set
        private IIndustryData<bool> G_LOCK_CMD;


        //GANTRY END STOP(TOW TROLLEY)**set
        private IIndustryData<bool> GES_TT;
        //SPREADER SLACK**set
        private IIndustryData<bool> SLACK_POD058;
        //HIGH WIND FAULT**set
        private IIndustryData<bool> HIWINDFLT;
        //?????????????????????????????????????????????????????????????????????
        private IIndustryData<bool> SPDLCK;
        //First Come First Service Gantry   **set
        private IIndustryData<bool> FCFSG;
        //GANTRY EMERGENCY STOP **set
        private IIndustryData<bool> GES;
        //ALWAYS ON**set
        private bool ALW_ON = true;
        //GANTRY RIGHT ONLY CMD**set
        private IIndustryData<bool> GR_ONLY_CMD;
        //GANTRY LEFT ONLY CMD**set
        private IIndustryData<bool> GL_ONLY_CMD;
        //SX BUS OK******************************set
        private IIndustryData<bool> SX_BUS_OK_POD024;
        //NO ESTOP**********************set
        private IIndustryData<bool> NOESTOP;
        //MAIN POWER ON******************set
        private IIndustryData<bool> POWERON;
        //POWER STATUS OK
        private IIndustryData<bool> POWEROK;
        //DRIVE NO FAULT
        private IIndustryData<bool> DRVOK;
        //CONTROL ON RELAY CHECK
        private IIndustryData<bool> CONCCHK;
        //TEMPERATURE OK
        private IIndustryData<bool> TEMPOK;
        //HOIST BRAKE OK
        private IIndustryData<bool> HBOK;
        //CONTROL ON PERMIT
        private IIndustryData<bool> CTLONPRM;




        //********************************************************************************UpFinished********************************

        //HOIST COMMAND   **set
        private IIndustryData<bool> HCMD;


        //TEMP          **set
        private IIndustryData<bool> MM777;

        //TEMP COIL     **set
        private IIndustryData<bool> MM778;

        //              **set
        private IIndustryData<bool> MM779;

        //SELECTED LOCAL              **set
        private IIndustryData<bool> MM01AL_LOCAL;

        //SELECTED LOCAL           **set
        private IIndustryData<bool> MM01AG_LOCAL;


        //SEMIAUTO OFF SELECTED           **set
        private IIndustryData<bool> MM01AG_SEMIAT_OFF;

        //SELECTED HANDHELD                 **set
        private IIndustryData<bool> MM01AL_HANDHELD;

        //SELECTED HANDHELD                 **set
        private IIndustryData<bool> MM01AG_HANDHELD;


        //%IX52.*.* INA0096    **set
        private IIndustryData<bool> INA0096;

        //%IX52.*.* INA0097  **set
        private IIndustryData<bool> INA0097;

        //%IX52.*.* INA0098  **set
        private IIndustryData<bool> INA0098;

        //%IX52.*.* INA0099  **set
        private IIndustryData<bool> INA0099;

        //%IX52.*.* INA0100  **set
        private IIndustryData<bool> INA0100;

        //%IX52.*.* INA0101  **set
        private IIndustryData<bool> INA0101;

        //%IX52.*.* INA00102  **set
        private IIndustryData<bool> INA0102;

        //%IX52.*.* INA0103  **set
        private IIndustryData<bool> INA0103;

        //%IX52.*.* INA0104  **set
        private IIndustryData<bool> INA0104;

        //%IX52.*.* INA0105  **set
        private IIndustryData<bool> INA0105;

        //%IX52.*.* INA0106  **set
        private IIndustryData<bool> INA0106;

        //%IX52.*.* INA0107  **set
        private IIndustryData<bool> INA0107;

        //%IX52.*.* INA0108  **set
        private IIndustryData<bool> INA0108;

        //%IX52.*.* INA0109  **set
        private IIndustryData<bool> INA0109;

        //%IX52.*.* INA0110  **set
        private IIndustryData<bool> INA0110;

        //%IX52.*.* INA0111  **set
        private IIndustryData<bool> INA0111;

        //%IX52.*.* INA0112  **set
        private IIndustryData<bool> INA0112;

        //%IX52.*.* INA0113  **set
        private IIndustryData<bool> INA0113;

        //%IX52.*.* INA0114  **set
        private IIndustryData<bool> INA0114;

        //%IX52.*.* INA0115  **set
        private IIndustryData<bool> INA0115;

        //%IX52.*.* INA0116  **set
        private IIndustryData<bool> INA0116;

        //%IX52.*.* INA0117  **set
        private IIndustryData<bool> INA0117;

        //%IX52.*.* INA0118  **set
        private IIndustryData<bool> INA0118;

        //%IX52.*.* INA0119  **set
        private IIndustryData<bool> INA0119;

        //%IX52.*.* INA0120  **set
        private IIndustryData<bool> INA0120;

        //%IX52.*.* INA0121  **set
        private IIndustryData<bool> INA0121;

        //%IX52.*.* INA0122  **set
        private IIndustryData<bool> INA0122;

        //%IX52.*.* INA0123  **set
        private IIndustryData<bool> INA0123;

        //%IX52.*.* INA0124  **set
        private IIndustryData<bool> INA0124;

        //%IX52.*.* INA0125  **set
        private IIndustryData<bool> INA0125;

        //%IX52.*.* INA0126  **set
        private IIndustryData<bool> INA0126;

        //%IX52.*.* INA0127  **set
        private IIndustryData<bool> INA0127;

        //TROLLEY(APPT) MOVE IN CMD ON HANDHELD ***set
        private IIndustryData<bool> MM16AG_HH_TMOVEIN;

        //         *************************set
        private IIndustryData<bool> MM16AL_HHTRIG;

        //CURRECTOR TROLLEY MOVE IN SIGNAL CMD SH.9F1  *************************set
        private IIndustryData<bool> DE_CCT_MOVE_IN_CMD;

        //CURRECTOR TROLLEY MOVE OUT SIGNAL CMD SH.9F1  *************************set
        private IIndustryData<bool> MM04FG_CCT_MOVE_OUT_CMD;

        //TROLLEY(APPT) MOVE OUT CMD ON HANDHELD********************************set
        private IIndustryData<bool> MM16AG_HH_TMOVEOUT;

        //CURRECTOR TROLLEY MOVE OUT SIGNAL CMD SH.9F1*****set
        private IIndustryData<bool> CCT_MOVE_OUT_CMD;

        //ENGINE SIDE CURRENT COLLECTOR TROLLEY OUT PREMIT******set
        private IIndustryData<bool> DE_CCT_OUT_PMT;

        //ENGINE SIDE CURRENT COLLECTOR TROLLEY IN PREMIT ******set
        private IIndustryData<bool> DE_CCT_IN_PMT;

        //APTT MOVE OUT TIME DELAY OFF   ******set
        private IIndustryData<bool> MOV_OUR_OFF_DELAY;

        //ENGINE SIDE CONTINUOUS SIGNAL******set
        private IIndustryData<bool> DE_CONTINUOUS;

        //ENGINE SIDE AUTO PLUG TOW TROLLEY OUT POSITION******set
        private IIndustryData<bool> DE_APTT_OUT_POS;

        //ENGINE SIDE GANTRY RIGHT ONLY*****set
        private IIndustryData<bool> DE_GLK_CMD;

        //ELECTRICITIY SELECTED*****set
        private IIndustryData<bool> MM04FG_ELECT_SELECTED;

        //ELECTRICITIY SELECTED SH.9F1*****set
        private IIndustryData<bool> ELECT_SELECTED;

        //ENGINE SIDE SWITCH ENERGY ZONE LAMP**set
        private IIndustryData<bool> DE_SW_ZONE;

        //CURRENT COLLECTOR TROLLEY IN LAMP**set
        private IIndustryData<bool> CCT_IN_POS;

        //ELECTRICITIY SELECTED FAULT****set
        private IIndustryData<bool> ELECT_SEL_FLT;

        //ENGINE SIDE APTT OUT SELECTION FAULT****set
        private IIndustryData<bool> DE_OUT_SEL_FAULT;

        //AUTO PLUG  MOVE OUT INCORRECT FAULT****set
        private IIndustryData<bool> APTT_MOV_OUT_FLT;

        //FAULT SIGNAL FOR AUTO PLUG TOW TROLLEY (00011111)***set
        private IIndustryData<bool> APTT_FAULT;

        //MHIMT'S MODIFICATION FOR REMOTE AUTOMATION**set
        private IIndustryData<bool> MM01CG_MMFRA;

        //GSP CHECK**set
        private IIndustryData<bool> GSPCHK_POD088;

        //LIMIT SWITCH BYPASS 2 POS. SW(SPRING) SH.+LC/9E23P**set
        private IIndustryData<bool> LSBYSW_POD008;

        //GANTRY 4 MC ANSWER BACK CHECK
        private IIndustryData<bool> MM07DG_G4MCCHK;

        //F SIDE GANTRY BRAKE ANSWER BACK CHECK
        private IIndustryData<bool> MM07DG_GBCFIMCCHK;


        //A SIDE GANTRY BRAKE ANSWER BACK CHECK
        private IIndustryData<bool> MM07DG_GBCAIMCCHK;

        //ENGINE SIDE GANTRY RIGHT ONLY
        private IIndustryData<bool> DE_GR_ONLY;

        //ENGINE SIDE GANTRY LEFT ONLY
        private IIndustryData<bool> DE_GL_ONLY;

        //NO ESTOP AUX
        private IIndustryData<bool> NOESTOP_AUX;





        #endregion PLCData_M

        #region PLCData_Fixed

        //Hoist brake contactor******************************%MX1.3002.2
        private IIndustryData<bool> HBCI_CMS;

        //Hoist up command***********************************%MX1.3002.13
        private IIndustryData<bool> HUPCMD_CMS;

        //Hoist down command*********************************%MX1.3002.15
        private IIndustryData<bool> HDNCMD_CMS;

        //Gantry right command*******************************%MX1.3018.2
        private IIndustryData<bool> GRCMD_CMS;

        //Gantry left command********************************%MX1.3018.0
        private IIndustryData<bool> GLCMD_CMS;

        //Gantry brake contactor*****************************%MX1.3016.2
        private IIndustryData<bool> GBCI_CMS;

        //Smart HEIGHT FAULT*********************************%MX1.4008.8            
        private IIndustryData<bool> HGT7FLT_FLT7621;

        //Overheight Sensor Fault****************************%MX1.4008.7           
        private IIndustryData<bool> DELAYOFF_FLT7620;

        //Hoist brake contactor check************************%MX1.3008.2           
        private IIndustryData<bool> HBCCHK_HESX_CMS;

        //HOIST OVER SPEED FAULT*****************************%MX1.4005.6 
        private IIndustryData<bool> HOSFLT_FLT4308_2;

        //Wheel Turning at Normal Position*******************%MX1.505.9
        private IIndustryData<bool> WTPOSOK_POD090;

        //GANTRY1 INVERTER DC LINK VOLTAGE ESTABLISHED*******%MX1.504.15
        private IIndustryData<bool> G1INVRUNX_POD080;

        //GANTRY2 INVERTER DC LINK VOLTAGE ESTABLISHED*******%MX1.505.0
        private IIndustryData<bool> G2INVRUNX_POD081;

        //GANTRY3 INVERTER DC LINK VOLTAGE ESTABLISHED*******%MX1.505.1
        private IIndustryData<bool> G3INVRUNX_POD082;

        //GANTRY4 INVERTER DC LINK VOLTAGE ESTABLISHED*******%MX1.505.2
        private IIndustryData<bool> G4INVRUNX_POD083;

        //LIMIT SWITCH BYPASS 2 POS. SW(SPRING)SH.+LC/9DD08P*%MX1.3042.11
        private IIndustryData<bool> BYPASS_LSBYSW_CMS;

        //***************************************************%MX10.0.0
        private IIndustryData<bool> MM01AG_CPURUN;

        //CURRECTOR TROLLEY MOVE IN SIGNAL CMD SH.9F1********%IX66.0.11
        private IIndustryData<bool> MM04FG_DE_CCT_MOVE_IN_CMD;

        //%IX10.0.4
        private IIndustryData<bool> IX10_0_4;

        //E-STOP(HANDHELD)********************************%IX10.0.6
        private IIndustryData<bool> IX10_0_6;

        //%IX10_0_3
        private IIndustryData<bool> IX10_0_3;

        //%IX52.*.* INA0096
        private IIndustryData<bool> IX52_2_0;

        //%IX52.*.* INA0097
        private IIndustryData<bool> IX52_2_1;

        //%IX52.*.* INA0098
        private IIndustryData<bool> IX52_2_2;

        //%IX52.*.* INA0099
        private IIndustryData<bool> IX52_2_3;

        //%IX52.*.* INA00100
        private IIndustryData<bool> IX52_2_4;

        //%IX52.*.* INA00101
        private IIndustryData<bool> IX52_2_5;

        //%IX52.*.* INA00102
        private IIndustryData<bool> IX52_2_6;

        //%IX52.*.* INA00103
        private IIndustryData<bool> IX52_2_7;

        //%IX52.*.* INA00104
        private IIndustryData<bool> IX52_2_8;

        //%IX52.*.* INA00105
        private IIndustryData<bool> IX52_2_9;

        //%IX52.*.* INA00106
        private IIndustryData<bool> IX52_2_10;

        //%IX52.*.* INA00107
        private IIndustryData<bool> IX52_2_11;

        //%IX52.*.* INA00108
        private IIndustryData<bool> IX52_2_12;

        //%IX52.*.* INA00109
        private IIndustryData<bool> IX52_2_13;

        //%IX52.*.* INA00110
        private IIndustryData<bool> IX52_2_14;

        //%IX52.*.* INA00111
        private IIndustryData<bool> IX52_2_15;

        //%IX52.*.* INA00112
        private IIndustryData<bool> IX52_3_0;

        //%IX52.*.* INA00113
        private IIndustryData<bool> IX52_3_1;

        //%IX52.*.* INA00114
        private IIndustryData<bool> IX52_3_2;

        //%IX52.*.* INA00115
        private IIndustryData<bool> IX52_3_3;

        //%IX52.*.* INA00116
        private IIndustryData<bool> IX52_3_4;

        //%IX52.*.* INA00117
        private IIndustryData<bool> IX52_3_5;

        //%IX52.*.* INA00118
        private IIndustryData<bool> IX52_3_6;

        //%IX52.*.* INA00119
        private IIndustryData<bool> IX52_3_7;

        //%IX52.*.* INA00120
        private IIndustryData<bool> IX52_3_8;

        //%IX52.*.* INA00121
        private IIndustryData<bool> IX52_3_9;

        //%IX52.*.* INA00122
        private IIndustryData<bool> IX52_3_10;

        //%IX52.*.* INA00123
        private IIndustryData<bool> IX52_3_11;

        //%IX52.*.* INA00124
        private IIndustryData<bool> IX52_3_12;

        //%IX52.*.* INA00125
        private IIndustryData<bool> IX52_3_13;

        //%IX52.*.* INA00126
        private IIndustryData<bool> IX52_3_14;

        //%IX52.*.* INA00127
        private IIndustryData<bool> IX52_3_15;

        //GANTRY END STOP (TOW TROLLEY) SH.+ERM/3Q19P*****%IX22.0.3
        private IIndustryData<bool> GTTES;

        private IIndustryData<bool> IX66_0_8;
        private IIndustryData<bool> IX66_0_10;
        private IIndustryData<bool> IX66_0_11;
        private IIndustryData<bool> IX66_0_12;
        private IIndustryData<bool> IX66_0_13;
        private IIndustryData<bool> IX66_0_14;
        private IIndustryData<bool> IX66_0_15;


        private IIndustryData<bool> MX1_4100_0;
        private IIndustryData<bool> MX1_4100_1;
        private IIndustryData<bool> MX1_4100_2;
        private IIndustryData<bool> MX1_4100_3;
        private IIndustryData<bool> MX1_4100_4;
        private IIndustryData<bool> MX1_4100_5;
        private IIndustryData<bool> MX1_4100_6;
        private IIndustryData<bool> MX1_4100_7;
        private IIndustryData<bool> MX1_4100_8;
        private IIndustryData<bool> MX1_4100_9;
        private IIndustryData<bool> MX1_4100_10;
        private IIndustryData<bool> MX1_4100_11;
        private IIndustryData<bool> MX1_4100_12;
        private IIndustryData<bool> MX1_4100_13;
        private IIndustryData<bool> MX1_4100_14;
        private IIndustryData<bool> MX1_4100_15;

        private IIndustryData<bool> MX1_4101_0;
        private IIndustryData<bool> MX1_4101_1;
        private IIndustryData<bool> MX1_4101_2;
        private IIndustryData<bool> MX1_4101_3;
        private IIndustryData<bool> MX1_4101_4;
        private IIndustryData<bool> MX1_4101_5;
        private IIndustryData<bool> MX1_4101_6;
        private IIndustryData<bool> MX1_4101_7;
        private IIndustryData<bool> MX1_4101_8;
        private IIndustryData<bool> MX1_4101_9;
        private IIndustryData<bool> MX1_4101_10;
        private IIndustryData<bool> MX1_4101_11;
        private IIndustryData<bool> MX1_4101_12;
        private IIndustryData<bool> MX1_4101_13;
        private IIndustryData<bool> MX1_4101_14;
        private IIndustryData<bool> MX1_4101_15;

        private IIndustryData<bool> MX1_4102_0;
        private IIndustryData<bool> MX1_4102_1;
        private IIndustryData<bool> MX1_4102_2;
        private IIndustryData<bool> MX1_4102_3;
        private IIndustryData<bool> MX1_4102_4;
        private IIndustryData<bool> MX1_4102_5;
        private IIndustryData<bool> MX1_4102_6;
        private IIndustryData<bool> MX1_4102_7;
        private IIndustryData<bool> MX1_4102_8;
        private IIndustryData<bool> MX1_4102_9;
        private IIndustryData<bool> MX1_4102_10;
        private IIndustryData<bool> MX1_4102_11;
        private IIndustryData<bool> MX1_4102_12;
        private IIndustryData<bool> MX1_4102_13;

        //ENGINE AUTO PLUG TOW TROLLEY ZONE***********************************%IX32.0.8
        private IIndustryData<bool> DE_TT_ZONE;

        //TROLLEY REV SLOWDOWN AREA
        private IIndustryData<bool> TRSDA_CMS;

        //UVA CONTACTOR FBI SH.+ERM/3E26P****************************%IX2.0.5
        private IIndustryData<bool> UVAI;

        //ENGINE SIDE AUTO PLUG TOW TROLLEY LOCK SIGNAL*******%IX32.0.15
        private IIndustryData<bool> DE_APTT_LK;

        //ENGINE CHANGE ZONE***********************%IX32.0.5
        private IIndustryData<bool> DE_CHG_ZONE;

        //ENGINE SIDE SPEED ZONE********%IX32.0.4
        private IIndustryData<bool> DE_SPD_ZONE;

        private bool MIS_OPS = false;

        //ENGINE SIDE AUTO PLUG TOW TROLLEY RIGHT UP SENSOR*************************%IX32.0.0
        private IIndustryData<bool> DE_APTT_RU;

        //ENGINE SIDE AUTO PLUG TOW TROLLEY LEFT UP SENSOR           ************ %IX32.0.1
        private IIndustryData<bool> DE_APTT_LU;

        //AUTO PLUG TOW TROLLEY RIGHT DOWN SENSOR*******************%IX32.0.2
        private IIndustryData<bool> DE_APTT_RD;

        //AUTO PLUG TOW TROLLEY LEFT DOWN SENSOR**********************%IX32.0.3
        private IIndustryData<bool> DE_APTT_LD;

        //2M CONTACTOR (ENGINE)******************************%IX33.0.11
        private IIndustryData<bool> M2_CONTACTOR;

        //ENGINE SIDE AUTO PLUG TOW TROLLEY RIGHT PROXIMITY**%IX32.0.6
        private IIndustryData<bool> DE_APTT_R_PROX;

        //ENGINE SIDE AUTO PLUG TOW TROLLEY LEFT PROXIMITY***%IX32.0.7
        private IIndustryData<bool> DE_APTT_L_PROX;

        //ENGNE SIDE AIR PERSSURE SIGNAL**********%IX32.0.10
        private IIndustryData<bool> DE_AIR_PERS;

        //ENGINE SIDE ELECTRICITIY INUSE(5M)******%IX33.0.12
        private IIndustryData<bool> DE_ELECT_INUSE;

        //AUTO PLUG TOW TROLLEY HOME SIGNAL OF ENGINE SIDE**%IX32.0.13
        private IIndustryData<bool> DE_APTT_HOME;

        //1M CONTACTOR (ELECTRICITY)****%IX32.0.11
        private IIndustryData<bool> M1_CONTACTOR;

        //ENGINE SIDE CURRENT COLLECTOR TROLLEY IN CMD SH.3S*****%QX24.0.6
        private IIndustryData<bool> DE_CCT_IN_CMD;

        //ENGINE SIDE LOCAL CONTROL SELECTED********************%IX32.0.9
        private IIndustryData<bool> DE_LOCAL_SEL;

        //SPREADER BYPASS OFF/ON 2 POS SW SPRING SH.+LC/9H114P ***MX1.3043.3
        private IIndustryData<bool> SPBYSW_CMS;

        //HIGH WIND ALARM SH.+CAB/9H31P***************%IX68.0.6
        private IIndustryData<bool> HIGHW;

        //GANTRY 4 MC ANSWER BACK CHECK********************%MX1.4013.11
        private IIndustryData<bool> MM21CG_FLT3062;

        //F SIDE GANTRY BRAKE ANSWER BACK CHECK****%MX1.4013.12
        private IIndustryData<bool> MM21CG_FLT3063;

        //A SIDE GANTRY BRAKE ANSWER BACK CHECK*****%MX1.4013.13
        private IIndustryData<bool> MM21CG_FLT3064;

        //GANTRY RUN TIMER************************%MX1.504.14
        private IIndustryData<bool> GRNTMR_POD079;


        //GANTRY BRAKE CONTACTOR CHECK**********%MX1.504.13
        private IIndustryData<bool> GBCHK_POD078;

        //GANTRY BRAKE RELEASE LS #1 CHECK*****%MX1.504.9
        private IIndustryData<bool> GBRCHK1_POD074;

        //GANTRY BRAKE RELEASE LS #2 CHECK******%MX1.504.10
        private IIndustryData<bool> GBRCHK2_POD075;

        //GANTRY BRAKE RELEASE CHECK #3**********%MX1.504.11
        private IIndustryData<bool> GBRCHK3_POD076;

        //GANTRY BRAKE RELEASE LS #4 CHECK********%MX1.504.12
        private IIndustryData<bool> GBRCHK4_POD077;

        //GANTRY #1 INVERTER ALARM***********************%MX1.505.3
        private IIndustryData<bool> G1INVALM_POD084;

        //GANTRY #2 INVERTER ALARM*********************%MX1.505.4
        private IIndustryData<bool> G2INVALM_POD085;

        //GANTRY #3 INVERTER ALARM**********************%MX1.505.5
        private IIndustryData<bool> G3INVALM_POD086;

        //GANTRY #4 INVERTER ALARM*********************%MX1.505.6
        private IIndustryData<bool> G4INVALM_POD087;

        //GANTRY BRAKE POWER CBI SH.+ERM/3E34P**********%IX2.0.7
        private IIndustryData<bool> GBCB;

        //TROLLEY UNDER VOLTAGE SH.+T/8K19P**********%IX42.0.3
        private IIndustryData<bool> TUVA;

        //GANTRY ANTI-COLLISION LS#1 CRANE TO CONTAINER SH.+ERM/3P30P**%IX21.0.6
        private IIndustryData<bool> GANLS1;

        //GANTRY ANTI-COLLISION LS#2 SH.+ERM/3P34P*****%IX21.0.7
        private IIndustryData<bool> GANLS2;

        //GANTRY ANTI-COLLISION LS#3 SH.+ERM/3P105P**%IX21.0.8
        private IIndustryData<bool> GANLS3;

        //GANTRY ANTI-COLLISION LS#4 SH.+ERM/3P109P****%IX21.0.9
        private IIndustryData<bool> GANLS4;

        //E-ROOM E-STOP1 SH.+ERM/3D07P*****************%IX1.0.0
        private IIndustryData<bool> ESTOP1;

        //E-ROOM SIDE E-STOP INSIDE SH.+ERM/3D11P******%IX1.0.1
        private IIndustryData<bool> ESTOP2;

        //E-ROOM SIDE E-STOP OUTSIDE SH.+ERM/3D15P****%IX1.0.2
        private IIndustryData<bool> ESTOP3;

        //D-ROOM SIDE E-STOP INSIDE SH.+ERM/3D19P******%IX1.0.3
        private IIndustryData<bool> ESTOP4;

        //D-ROOM SIDE E-STOP OUTSIDE SH.+ERM/3D23P*****%IX1.0.4
        private IIndustryData<bool> ESTOP5;

        //TROLLEY FRAME E-STOP NEAR H-MOTOR SH.+CAB/8M15P***%IX44.0.2
        private IIndustryData<bool> ESTOP6;

        //TROLLEY PANEL E-STOP SH.+CAB/8L129P*********%IX43.0.14
        private IIndustryData<bool> ESTOP7;

        //TROLLEY FRAME E-STOP NEAR T-MOTOR SH.+CAB/8L133P***%IX43.0.15
        private IIndustryData<bool> ESTOP8;

        //CAB ESTOP PB SH.+CAB/9H07P*********************%IX68.0.0
        private IIndustryData<bool> ESTOP9;

        //E-ROOM INTERNAL CONTROL POWER CBI SH.+ERM/3E19P************************************%IX2.0.3
        private IIndustryData<bool> E3ACB2;

        //TROLLEY CONTROL POWER CBI SH.+ERM/3E23P******************************************%IX2.0.4
        private IIndustryData<bool> E3ACB3;

        //CAB CONTROL POWER CBI SH.+ERM/3E31P******************%IX2.0.6
        private IIndustryData<bool> E3ACB4;

        //DRIVE MAIN POWER CB SH.+ERM/3D27P*************%IX1.0.5
        private IIndustryData<bool> E2ACB1;

        //TROLLEY POWER CBI SH.+ERM/3D34P******************%IX1.0.7
        private IIndustryData<bool> TPWRCB1;

        //TROLLEY FRAME AUX. POWER CBI SH.+T/8M109P******************%IX44.0.9
        private IIndustryData<bool> TPWRCB2;



        #endregion PLCData_Fixed

        #region Event
        #endregion Event

        #region Function

        private void APTT_TROLLEY_MOVE_IN() {
            DE_CCT_MOVE_IN_CMD.ReadValue((MM01AG_SEMIAT_OFF.Value && MM04FG_DE_CCT_MOVE_IN_CMD.Value) || (MM01AG_HANDHELD.Value && MM16AG_HH_TMOVEIN.Value));
        }

        private void APTT_TROLLEY_MOVE_OUT() {
            MM04FG_CCT_MOVE_OUT_CMD.ReadValue(IX66_0_12.Value);
            CCT_MOVE_OUT_CMD.ReadValue((MM01AG_SEMIAT_OFF.Value && MM04FG_CCT_MOVE_OUT_CMD.Value) || (MM01AG_HANDHELD.Value && MM16AG_HH_TMOVEOUT.Value));
        }

        private void AUTO_PLUG_TOW_TROLLEY_MOVE_OUT_AND_RETRACTION_LOGIC() {
            DE_CCT_OUT_PMT.ReadValue(
                (DE_TT_ZONE.Value && CCT_MOVE_OUT_CMD.Value && (!TRSDA_CMS.Value))
              || (DE_CCT_OUT_PMT.Value && (!TRSDA_CMS.Value) && (!DE_CHG_ZONE.Value))
              || (DE_CCT_OUT_PMT.Value && DE_CHG_ZONE.Value)
              || (DE_CCT_OUT_PMT.Value && DE_SPD_ZONE.Value)
              || (DE_CONTINUOUS.Value && M2_CONTACTOR.Value)
              || ((DE_APTT_LD.Value || DE_APTT_R_PROX.Value || DE_APTT_L_PROX.Value) && (!DE_CCT_MOVE_IN_CMD.Value) && (!MOV_OUR_OFF_DELAY.Value))
              && UVAI.Value && (!DE_CCT_IN_PMT.Value) && DE_AIR_PERS.Value && DE_ELECT_INUSE.Value && CCT_MOVE_OUT_CMD.Value && (!DE_CCT_MOVE_IN_CMD.Value)
                );
        }

        private void AUTO_PLUG_TOW_TROLLEY_FAULE_DETECT_LOGIC() {
            ELECT_SEL_FLT.ReadValue((!M1_CONTACTOR.Value) && M2_CONTACTOR.Value && (!ELECT_SELECTED.Value) && CCT_MOVE_OUT_CMD.Value && (!DE_SW_ZONE.Value) && (!CCT_IN_POS.Value));
            DE_OUT_SEL_FAULT.ReadValue((!DE_TT_ZONE.Value) && (!DE_APTT_OUT_POS.Value) && (!DE_SPD_ZONE.Value) && (!DE_CHG_ZONE.Value) && (!M1_CONTACTOR.Value) && M2_CONTACTOR.Value);
            APTT_MOV_OUT_FLT.ReadValue(CCT_MOVE_OUT_CMD.Value && (!DE_CCT_IN_CMD.Value) && DE_OUT_SEL_FAULT.Value && (!M1_CONTACTOR.Value) && M2_CONTACTOR.Value);
            APTT_FAULT.ReadValue(!((!ELECT_SEL_FLT.Value) && (!APTT_MOV_OUT_FLT.Value)));
        }

        private void ENGINE_SIDE_APTT_RETRACT_TO_HOME_POSITION_PERMIT_LOGIC() {
            DE_CCT_IN_PMT.ReadValue(DE_TT_ZONE.Value && (!DE_APTT_LK.Value) && DE_CCT_MOVE_IN_CMD.Value && (!CCT_MOVE_OUT_CMD.Value) && (!DE_CCT_OUT_PMT.Value) && UVAI.Value && (!MIS_OPS) && (!TRSDA_CMS.Value));
        }

        private void ENGINE_SIDE_AUTO_PLUG_TROLLEY_ON_ELECTRICITIY_POWER_SWITCH_ZONE() {
            DE_SW_ZONE.ReadValue(
                ((DE_SPD_ZONE.Value && ((DE_APTT_R_PROX.Value) || (DE_APTT_L_PROX.Value)) && (!M1_CONTACTOR.Value) && M2_CONTACTOR.Value)
                || (DE_CHG_ZONE.Value && ((DE_APTT_R_PROX.Value) || (DE_APTT_L_PROX.Value)) && M1_CONTACTOR.Value && (!M2_CONTACTOR.Value)))
                && (DE_TT_ZONE.Value)
                );
        }

        private void ENGINE_SIDE_AUTO_PLUG_TOW_TROLLEY_OUT_POSITION_DETECTION_LOGIC() {
            DE_APTT_OUT_POS.ReadValue(
                (DE_APTT_RU.Value && (
                (DE_APTT_LU.Value && DE_APTT_RD.Value)
                || (DE_APTT_LU.Value && DE_APTT_LD.Value)
                || (DE_APTT_RD.Value && DE_APTT_LD.Value)
                ))
                || (DE_APTT_RD.Value && DE_APTT_LD.Value && DE_APTT_LU.Value)
                && (!DE_APTT_HOME.Value)
                );
            DE_CONTINUOUS.ReadValue(
                (DE_APTT_RU.Value && ((DE_APTT_LD.Value) || (DE_APTT_RD.Value) || (DE_APTT_LU.Value)))
                || (DE_APTT_LU.Value && ((DE_APTT_LD.Value) || (DE_APTT_RD.Value)))
                || ((DE_APTT_LD.Value) && (DE_APTT_RD.Value))
                );
        }

        private void ENGINE_SIDE_GANTRY_LOCK_DETECT_LOGIC() {
            DE_GLK_CMD.ReadValue(
                (DE_TT_ZONE.Value && (!BYPASS_LSBYSW_CMS.Value) && DE_APTT_HOME.Value && (!DE_CCT_IN_PMT.Value))
                || (DE_TT_ZONE.Value && DE_GLK_CMD.Value && CCT_MOVE_OUT_CMD.Value && (!DE_CCT_MOVE_IN_CMD.Value) && (!DE_APTT_OUT_POS.Value) && DE_CCT_OUT_PMT.Value)
                || ((!DE_APTT_OUT_POS.Value) && (!DE_CHG_ZONE.Value) && (!DE_SPD_ZONE.Value) && (!DE_APTT_HOME.Value) && M2_CONTACTOR.Value)
                );
        }

        private void BOTH_SIDE_AUTO_PLUG_TOW_TROLLEY_IN_RETRACTED_CONDITION() {
            CCT_IN_POS.ReadValue(DE_APTT_HOME.Value && DE_APTT_LK.Value);
        }

        private void GANTRY_LOCKED_COMAND_BY_APTT() {
            G_LOCK_CMD.ReadValue(DE_GLK_CMD.Value || (!DE_LOCAL_SEL.Value) || APTT_FAULT.Value);
        }

        /// <summary>
        /// MHI'S PLC Logic :SEMIAUTO_OFF_SELECTED
        /// </summary>
        private void SEMIAUTO_OFF_SELECTED() {
            MM01AG_SEMIAT_OFF.ReadValue(MM01AL_LOCAL.Value && ((!INA0119.Value) || (!MM777.Value)));
        }

        private void ELECTRICITIY_SELECTED() {
            MM04FG_ELECT_SELECTED.ReadValue(IX66_0_10.Value);
            ELECT_SELECTED.ReadValue(MM01AG_SEMIAT_OFF.Value && MM04FG_ELECT_SELECTED.Value);
        }

        /// <summary>
        /// AUXILIARY_PANEL
        /// </summary>
        private void AUXILIARY_PANEL() {
            INA0096.ReadValue(IX52_2_0.Value);
            INA0097.ReadValue(IX52_2_1.Value);
            INA0098.ReadValue(IX52_2_2.Value);
            INA0099.ReadValue(IX52_2_3.Value);
            INA0100.ReadValue(IX52_2_4.Value);
            INA0101.ReadValue(IX52_2_5.Value);
            INA0102.ReadValue(IX52_2_6.Value);
            INA0103.ReadValue(IX52_2_7.Value);
            INA0104.ReadValue(IX52_2_8.Value);
            INA0105.ReadValue(IX52_2_9.Value);
            INA0106.ReadValue(IX52_2_10.Value);
            INA0107.ReadValue(IX52_2_11.Value);
            INA0108.ReadValue(IX52_2_12.Value);
            INA0109.ReadValue(IX52_2_13.Value);
            INA0110.ReadValue(IX52_2_14.Value);
            INA0111.ReadValue(IX52_2_15.Value);
            INA0112.ReadValue(IX52_3_0.Value);
            INA0113.ReadValue(IX52_3_1.Value);
            INA0114.ReadValue(IX52_3_2.Value);
            INA0115.ReadValue(IX52_3_3.Value);
            INA0116.ReadValue(IX52_3_4.Value);
            INA0117.ReadValue(IX52_3_5.Value);
            INA0118.ReadValue(IX52_3_6.Value);
            INA0119.ReadValue(IX52_3_7.Value);
            INA0120.ReadValue(IX52_3_8.Value);
            INA0121.ReadValue(IX52_3_9.Value);
            INA0122.ReadValue(IX52_3_10.Value);
            INA0123.ReadValue(IX52_3_11.Value);
            INA0124.ReadValue(IX52_3_12.Value);
            INA0125.ReadValue(IX52_3_13.Value);
            INA0126.ReadValue(IX52_3_14.Value);
            INA0127.ReadValue(IX52_3_15.Value);
        }

        /// <summary>
        ///  MHI'S PLC Logic :OPERATION_MODE
        /// </summary>
        private void OPERATION_MODE() {
            bool mm_Local = (IX10_0_4.Value || (!MM777.Value));
            MM01AL_LOCAL.ReadValue(mm_Local);
            MM01AG_LOCAL.ReadValue(mm_Local);

            bool mm_Handeld = IX10_0_3.Value && MM777.Value;
            MM01AL_HANDHELD.ReadValue(mm_Handeld);
            MM01AG_HANDHELD.ReadValue(mm_Handeld);

        }

        private void GANTRY_LEFT_OR_RIGHT_MOVE_ONLY_INTERLOCK_BY_ENGINE_SIDE_APTT() {
            bool common = (DE_TT_ZONE.Value)
                || (DE_CHG_ZONE.Value && (!DE_SPD_ZONE.Value) && M1_CONTACTOR.Value && (!M2_CONTACTOR.Value))
                || ((!DE_CHG_ZONE.Value) && (!M1_CONTACTOR.Value) && DE_SPD_ZONE.Value && M2_CONTACTOR.Value);
            DE_GR_ONLY.ReadValue(common && DE_APTT_L_PROX.Value);
            DE_GL_ONLY.ReadValue(common && DE_APTT_R_PROX.Value);
        }

        private void GANTRY_RIGHT_ONLY_COMMAND() {
            GR_ONLY_CMD.ReadValue(DE_GR_ONLY.Value || (!GANLS1.Value) || (!GANLS4.Value));
        }

        private void GANTRY_LEFT_ONLY_COMMAND() {
            GL_ONLY_CMD.ReadValue(DE_GL_ONLY.Value || (!GANLS2.Value) || (!GANLS3.Value));
        }

        private void GESX() {
            GES.ReadValue(
                (!GRNTMR_POD079.Value) && ((!GBCHK_POD078.Value) || MM01CG_MMFRA.Value) && MM01CG_MMFRA.Value && (!MM07DG_G4MCCHK.Value) && (!MM07DG_GBCFIMCCHK.Value) && (!MM07DG_GBCAIMCCHK.Value) && (!GBRCHK1_POD074.Value) && (!GBRCHK2_POD075.Value) && (!GBRCHK3_POD076.Value) && (!GBRCHK4_POD077.Value)
                && (!G1INVALM_POD084.Value) && (!G2INVALM_POD085.Value) && (!G3INVALM_POD086.Value) && (!G4INVALM_POD087.Value) && GBCB.Value
                && UVAI.Value && TUVA.Value && SX_BUS_OK_POD024.Value && ((!GSPCHK_POD088.Value) || (LSBYSW_POD008.Value))
                );
        }

        private void No_Estop_Occured() {
            bool result = ESTOP1.Value && ESTOP2.Value && ESTOP3.Value && ESTOP4.Value && ESTOP5.Value && ESTOP6.Value && ESTOP7.Value && ESTOP8.Value && ESTOP9.Value && MM01CG_MMFRA.Value && ((IX10_0_6.Value) || (!MM01AG_HANDHELD.Value));
            TON_9.Scan(result);
            NOESTOP.ReadValue(NOESTOP_AUX.Value && E3ACB2.Value && (!HOSFLT_FLT4308_2.Value));
        }

        private void Main_Power_On() {
            POWERON.ReadValue(E3ACB2.Value && E3ACB3.Value && E3ACB4.Value && E2ACB1.Value && TPWRCB1.Value && TPWRCB2.Value);
        }

        /// <summary>
        /// MHI'S PLC Logic :REMOTE_AUTOMATION
        /// </summary>
        private void REMOTE_AUTOMATION() {
            bool result = !MM01AG_CPURUN.Value;
            MM777.ReadValue(result);
            MM778.ReadValue(result);
            MM779.ReadValue(result);
        }

        /// <summary>
        /// Init All Data in PLC
        /// </summary>
        private void Init() {
            FCFSH = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.FCFSH");
            OHDETCT = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.OHDETCT");
            H_CMDLOST = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.H_CMDLOST");
            //HBCCHK1 = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.HBCCHK1");
            //HBCCHK2 = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.HBCCHK2");
            SMART_ATL3 = _source.AcquireIndustryData<bool>("HIT.TT628.ATL_SYS.SMART_ATL3");
            GCMD = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.GCMD");
            //WTPOSOK = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.WTPOSOK");
            //G1INVRUNX = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.G1INVRUNX");
            //G2INVRUNX = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.G2INVRUNX");
            //G3INVRUNX = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.G3INVRUNX");
            //G4INVRUNX = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.G4INVRUNX");
            G_LOCK_CMD = _source.AcquireIndustryData<bool>("HIT.TT628.AUTO_PLUG_TOW_TROLLEY.G_LOCK_CMD");
            GES_TT = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.GES_TT");
            SLACK_POD058 = _source.AcquireIndustryData<bool>("HIT.TT628.POD.POD058");
            HIWINDFLT = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.HIWINDFLT");
            SPDLCK = _source.AcquireIndustryData<bool>("HIT.TT628.ATL_SYS.SPDLCK");
            FCFSG = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.FCFSG");
            GES = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.GES");
            GR_ONLY_CMD = _source.AcquireIndustryData<bool>("HIT.TT628.AUTO_PLUG_TOW_TROLLEY.GR_ONLY_CMD");
            GL_ONLY_CMD = _source.AcquireIndustryData<bool>("HIT.TT628.AUTO_PLUG_TOW_TROLLEY.GL_ONLY_CMD");
            SX_BUS_OK_POD024 = _source.AcquireIndustryData<bool>("HIT.TT628.POD.POD024");
            NOESTOP = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.NOESTOP");
            POWERON = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.POWERON");
            POWEROK = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.POWEROK");
            DRVOK = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.DRVOK");
            CONCCHK = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.CONCCHK");
            TEMPOK = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.TEMPOK");
            HBOK = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.HBOK");
            CTLONPRM = _source.AcquireIndustryData<bool>("HIT.TT628.Global_Variables.CTLONPRM");
        }

        private void SetInterval(int interval, Action<object, ElapsedEventArgs> action, out System.Timers.Timer timer) {
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(action);
            timer.Interval = interval;
            timer.Enabled = true;
        }




        #endregion Function

        #region InterClass
        #endregion InterClass

    }
}
