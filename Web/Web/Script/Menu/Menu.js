//Copyright(c) 2015,HIT All rights reserved.
//Des:Client Side Menu
//Author:Irlovan   
//Date:2015-04-01

if ((Irlovan.Global.Edition == Irlovan.Global.EditionMode.Operator)) { Irlovan.Include.Using("Script/Menu/OperatorMenu.js"); } else { Irlovan.Include.Using("Script/Menu/AdminMenu.js"); }
Irlovan.Include.Using("Script/Menu/ControlSelector/ControlSelector.js");
Irlovan.Include.Using("Script/Menu/VURMenu.js");

Irlovan.Menu = {}