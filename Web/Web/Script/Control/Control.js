//Copyright(c) 2015,HIT All rights reserved.
//Des:Control 
//Author:Irlovan   
//Date:2015-03-31
//modification :

/**Framework select for operation and admin mode**/
if (Irlovan.Global.Edition == Irlovan.Global.EditionMode.Operator) {
    Irlovan.Include.Using("Script/Control/Framework/BaseFrame.js");
} else {
    Irlovan.Include.Using("Script/Control/Framework/MenuFrame.js");
}
Irlovan.Include.Using("Script/Control/Effect.js");
Irlovan.Include.Using("Script/Control/Species/Classic.js");

/**Assigner**/
Irlovan.Include.Using("Script/Control/Controls/Assigner/ResetSwitch.js");

/**Base control**/
Irlovan.Include.Using("Script/Control/Controls/Base/Circle.js");
Irlovan.Include.Using("Script/Control/Controls/Base/Label.js");
Irlovan.Include.Using("Script/Control/Controls/Base/Rectangle.js");
Irlovan.Include.Using("Script/Control/Controls/Base/TextBox.js");

/**brakes**/
Irlovan.Include.Using("Script/Control/Controls/Brake/Brake1.js");

/**button**/
Irlovan.Include.Using("Script/Control/Controls/Button/ButtonBlue.js");
Irlovan.Include.Using("Script/Control/Controls/Button/ButtonCMD.js");
Irlovan.Include.Using("Script/Control/Controls/Button/ButtonClassic.js");
Irlovan.Include.Using("Script/Control/Controls/Button/ButtonExit.js");

/**Crane**/
Irlovan.Include.Using("Script/Control/Controls/Crane/BridgeCrane.js");
Irlovan.Include.Using("Script/Control/Controls/Crane/FrameCrane.js");
Irlovan.Include.Using("Script/Control/Controls/Crane/RTG/ZPMC_M_165_11/ZPMC_M_165_11_RTG_Skyview.js");
Irlovan.Include.Using("Script/Control/Controls/Crane/PortalJibGrabCrane.js");
Irlovan.Include.Using("Script/Control/Controls/Crane/PortalJibHookCrane.js");

/**Diagnose**/
Irlovan.Include.Using("Script/Control/Controls/Diagnose/DataViewer.js");
Irlovan.Include.Using("Script/Control/Controls/Diagnose/AlarmViewer.js");
Irlovan.Include.Using("Script/Control/Controls/Diagnose/HistoryEventViewer.js");
Irlovan.Include.Using("Script/Control/Controls/Diagnose/HistoryDataViewer.js");
Irlovan.Include.Using("Script/Control/Controls/Diagnose/StatisticViewer.js");
Irlovan.Include.Using("Script/Control/Controls/Diagnose/PowerSimulation.js");

/**Interface**/
Irlovan.Include.Using("Script/Control/Controls/Interface/HTMLIF.js");

/**Ladder**/
Irlovan.Include.Using("Script/Control/Controls/Ladder/TSwitch.js");

/**Meter**/
Irlovan.Include.Using("Script/Control/Controls/Meter/Ruler.js");

/**Switch**/
Irlovan.Include.Using("Script/Control/Controls/Switch/Arrow.js");
Irlovan.Include.Using("Script/Control/Controls/Switch/BaseLamp.js");
Irlovan.Include.Using("Script/Control/Controls/Switch/FlashLamp.js");
Irlovan.Include.Using("Script/Control/Controls/Switch/FlashRec.js");
Irlovan.Include.Using("Script/Control/Controls/Switch/GearSwitch.js");
Irlovan.Include.Using("Script/Control/Controls/Switch/MasterSwitch.js");
Irlovan.Include.Using("Script/Control/Controls/Switch/MoSwitch.js");
Irlovan.Include.Using("Script/Control/Controls/Switch/RecLamp.js");
Irlovan.Include.Using("Script/Control/Controls/Switch/RoundLamp.js");



Irlovan.Control = {};

