//Copyright(c) 2013,HIT All rights reserved.
//Des:DockMenu
//Author:Irlovan   
//Date:2013-04-18
//modification :

/*
*using
*CSS:
<link rel="stylesheet" type="text/css" href="Script/Control/DockMenu/Lib/PellucidCarousel/fbisCarousel.css" />
*JQueryBase:
<script src="Script/Lib/jquery-1.9.1.js" type="text/javascript"></script>
<script src="Script/Lib/jquery-migrate-1.1.1.js" type="text/javascript"></script>
*JQueryExtention:
<script src="Script/Control/DockMenu/Lib/PellucidCarousel/fbisCarousel.js" type="text/javascript"></script>
*UserDefine
<script src="Script/Lib/IrlovanHelper.js" type="text/javascript"></script>
<script src="Script/Lib/ControlHelper.js" type="text/javascript"></script>
<script src="Script/Control/DockMenu/Lib/PellucidCarousel/FbisCarouselDockMenu.js" type="text/javascript"></script>
*/

Irlovan.FbisCarouselDockMenu = function () {
    Irlovan.ControlHelper.CreateControlByStr(
        "<div class='fbisCarousel' id='FbisCarouselDockMenu' style='position: absolute; right: 180px; top: 0px;float:right;'>" +
        "<div class='arrows'>"+
        "<a class='slideright'><span>&lt;</span></a>" +
        "<a class='slideleft' ><span>&gt;</span></a>" +
        "</div>"+
        "<ul>"+
        "<li onclick='Irlovan.MainDockMenu.OnAdd()'><img src='Images/Add.png' alt='Add' /></li>" +
        "<li onclick='Irlovan.MainDockMenu.OnSave()'><img src='Images/Save.png' alt='Save' /></li>" +
        "<li onclick='Irlovan.MainDockMenu.OnLoad()'><img src='Images/Load.png' alt='Load' /></li>" +
        "<li onclick='Irlovan.MainDockMenu.OnPropertyGrid()'><img src='Images/PropertyGrid.png' alt='Load' /></li>" +
        "<li onclick='Irlovan.MainDockMenu.OnLoad()'><img src='Images/Add.png' alt='Load' /></li>" +
        "<li onclick='Irlovan.MainDockMenu.OnLoad()'><img src='Images/Add.png' alt='Load' /></li>" +
        "</ul>" +
        "</div>", "body");
    $("div.fbisCarousel").fbisCarousel({ noToDisplay: 5, arrowSelector: 'div.extraarrows a' });
    this.Reader;
    this.Recorder;
    this.PropertyGrid;
}
Irlovan.FbisCarouselDockMenu.OnAdd = function () {
    if ($('#controlselector') != []) { $('#controlselector').toggle('slide', {}, 500); }
}
Irlovan.FbisCarouselDockMenu.OnSave = function () {
}
Irlovan.FbisCarouselDockMenu.OnLoad = function () {
    this.Reader = new Irlovan.WebReader();
}
Irlovan.FbisCarouselDockMenu.OnPropertyGrid = function () {
    this.PropertyGrid = new Irlovan.PropertyGrid('body', 'DivGrid', null, 'Grid', 'Paper');
}
