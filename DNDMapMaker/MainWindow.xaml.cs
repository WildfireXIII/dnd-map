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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DNDMapMaker
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			if (Master.DISPLAY_DEBUG) { lblDebug.IsEnabled = true; lblDebug.Visibility = Visibility.Visible; }

			// get map window
			MapDesignWindow mapWin = new MapDesignWindow();
			mapWin.ShowDialog();

			log("Log initialized!");
		}

		// FUNCTIONS

		public void log(string msg) { lblDebug.Content += msg + "\n"; svDebug.ScrollToBottom(); }
		

		// EVENT HANDLERS
	}
}
