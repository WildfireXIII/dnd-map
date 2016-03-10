using System;
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
		public static bool DISPLAY_DEBUG = true;
		public static string RES_FOLDER = "C:\\dwl\\tmp\\DNDRES\\lib";

		// variables
		private static MainWindow win;
		private static MapDesignWindow mapWin;

		public static Canvas activeCanvas;
		//public static bool isMapActive; // true if mapDesignWindow open, false if not

		// properties
		//public static Canvas getMapCanvas() { return mapWin.getCanvas(); }
		
		public static void setDraggingEntity(bool isDragging, Entity draggingEntity) { mapWin.setDraggingEntity(isDragging, draggingEntity); }
		public static void setMapOffsetX(double off) { mapWin.setMapOffsetX(off); }
		public static void setMapOffsetY(double off) { mapWin.setMapOffsetY(off); }
		//public static int getMapOffsetX() { return mapWin.getMapOffsetX(); }
		//public static int getMapOffsetY() { return mapWin.getMapOffsetY(); }

		// just for properties window, that's ALL it should be used for
		public static void setMapSelectedEntity(Entity e) { mapWin.setSelectedEntity(e); }

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
