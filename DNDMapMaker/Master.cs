﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DNDMapMaker
{
	class Master
	{
		// "constants"
		public static bool DISPLAY_DEBUG = false;
		public static string RES_FOLDER = "C:\\dwl\\tmp\\DNDRES\\lib";
		public static string MAP_FOLDER = "C:\\dwl\\tmp\\DNDRES\\maps";
		public static string ICON_FOLDER = "C:\\dwl\\tmp\\DNDRES\\icons";

		// variables
		private static MainWindow win;
		private static MapDesignWindow mapWin;

		private static string mode = "design";
		
		public static Canvas activeCanvas;
		//public static bool isMapActive; // true if mapDesignWindow open, false if not

		// properties
		//public static Canvas getMapCanvas() { return mapWin.getCanvas(); }

		public static string Mode { get { return mode; } set { mode = value; } }
		
		public static void setDraggingEntity(bool isDragging, Entity draggingEntity) { mapWin.setDraggingEntity(isDragging, draggingEntity); }
		public static void setDraggingIcon(bool isDragging, Icon draggingIcon) { mapWin.setDraggingIcon(isDragging, draggingIcon); }
		public static void setMapOffsetX(double off) { mapWin.setMapOffsetX(off); }
		public static void setMapOffsetY(double off) { mapWin.setMapOffsetY(off); }
		//public static int getMapOffsetX() { return mapWin.getMapOffsetX(); }
		//public static int getMapOffsetY() { return mapWin.getMapOffsetY(); }

		// just for properties window, that's ALL it should be used for
		public static void setMapSelectedEntity(Entity e) { mapWin.setSelectedEntity(e); }
		public static void setMapSelectedIcon(Icon pIcon) { mapWin.setSelectedIcon(pIcon); }

		// attempts on map design first, but if not up, use main window instead
		public static void addResource(string resName)
		{
			if (mapWin != null) { mapWin.addResource(resName); }
			// else { win.addResource(resName); } // DON'T KNOW IF THIS IS NECESSARY
			// (will win ever even need add resource?
		}

		// functions
		public static void assignMainWin(MainWindow win) { Master.win = win; }
		public static void assignMapWin(MapDesignWindow win) { Master.mapWin = win; }

		public static void log(string message)
		{
			try { win.log(message); }
			catch (Exception e) { }
		}
		public static void mapLog(string message)
		{
			try { mapWin.log(message); }
			catch (Exception e) { }
		}
	}
}
