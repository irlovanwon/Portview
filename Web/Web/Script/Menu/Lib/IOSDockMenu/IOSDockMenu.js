//Copyright(c) 2013,HIT All rights reserved.
//Des:DockMenu
//Author:Irlovan   
//Date:2013-04-18
//modification :

/*
*using
*CSS:
<link rel="stylesheet" type="text/css" href="Script/Control/DockMenu/Lib/DockMenuStyle.css" />
<!--[if lt IE 7]>
 <style type="text/css">
 .dock img { behavior: url(iepngfix.htc) }
 </style>
<![endif]-->
*JQueryBase:
<script src="Script/Lib/jquery-1.9.1.js" type="text/javascript"></script>
<script src="Script/Lib/jquery-migrate-1.1.1.js" type="text/javascript"></script>
*JQueryExtention:
<script src="Script/Control/DockMenu/Lib/interface.js" type="text/javascript"></script>
*UserDefine
<script src="Script/Lib/IrlovanHelper.js" type="text/javascript"></script>
<script src="Script/Lib/ControlHelper.js" type="text/javascript"></script>
<script src="Script/Control/DockMenu/MainDockMenu.js" type="text/javascript"></script>
*/

Irlovan.MainDockMenu = function () {
    Irlovan.ControlHelper.CreateControlByStr(
        "<div class='dock' id='MainDockMenu' style='position: absolute; right: 180px; top: 0px;float:right;'>" +
        "<div class='dock-container'>" +
        "<a class='dock-item' onclick='Irlovan.MainDockMenu.OnAdd()'><img src='Images/Add.png' alt='Add' /><span>Add New Control</span></a>" +
        "<a class='dock-item' onclick='Irlovan.MainDockMenu.OnSave()'><img src='Images/Save.png' alt='Save' /><span>Save Project</span></a>" +
        "<a class='dock-item' onclick='Irlovan.MainDockMenu.OnLoad()'><img src='Images/Load.png' alt='Load' /><span>Load Project</span></a>" +
        "<a class='dock-item' onclick='Irlovan.MainDockMenu.OnPropertyGrid()'><img src='Images/PropertyGrid.png' alt='Load' /><span>PropertyGrid</span></a>" +
        "<a class='dock-item' onclick='Irlovan.MainDockMenu.OnLoad()'><img src='Images/Add.png' alt='Load' /><span>Load Project</span></a>" +
        "<a class='dock-item' onclick='Irlovan.MainDockMenu.OnLoad()'><img src='Images/Add.png' alt='Load' /><span>Load Project</span></a>" +
        "</div>" +                                                                         
        "</div>", "body");
		    $('#MainDockMenu').Fisheye({
		        maxWidth: 32,
		        items: 'a',
		        itemsText: 'span',
		        container: '.dock-container',
		        itemWidth: 32,
		        itemHeight:32,
		        proximity: 90,
		        halign: 'center'
		    })
    this.Reader;
    this.Recorder;
    this.PropertyGrid;
}
Irlovan.MainDockMenu.OnAdd = function () {
    if ($('#controlselector')!=[]) {$('#controlselector').toggle('slide', {}, 500);} 
}
Irlovan.MainDockMenu.OnSave = function () {
}
Irlovan.MainDockMenu.OnLoad = function () {
    this.Reader = new Irlovan.WebReader();
}
Irlovan.MainDockMenu.OnPropertyGrid = function () {
    this.PropertyGrid = new Irlovan.PropertyGrid('body', 'DivGrid', null, 'Grid', 'Paper');
}
