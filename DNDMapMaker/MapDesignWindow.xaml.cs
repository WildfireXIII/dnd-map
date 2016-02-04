using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DNDMapMaker
{


	public partial class MapDesignWindow : Window
	{
		public MapDesignWindow()
		{
			InitializeComponent();

			if (Master.DISPLAY_DEBUG) { lblDebug.IsEnabled = true; lblDebug.Visibility = Visibility.Visible; }

			// make sure canvas has a background
			cnvsWorld.Background = Brushes.White;


			Master.activeCanvas = cnvsWorld;
			Master.assignMapWin(this);

			Map m = new Map();

			log("Log initialized!");
			log("Map created!");
		}

		// PROPERTIES
		public Canvas getCanvas() { return cnvsWorld; }

		// FUNCTIONS

		public void log(string msg) { lblDebug.Content += msg + "\n"; svDebug.ScrollToBottom(); }

		// EVENT HANDLERS
	}
}
