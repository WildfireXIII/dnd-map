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

			log("Log initialized!");
		}

		// FUNCTIONS

		public void log(string msg) { lblDebug.Content += msg + "\n"; }
		public void clearLog() { lblDebug.Content = ""; }

		// EVENT HANDLERS
	}
}
