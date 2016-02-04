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

		// properties
		//public static Canvas getMapCanvas() { return mapWin.getCanvas(); }
		


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
