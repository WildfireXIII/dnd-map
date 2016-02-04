using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace DNDMapMaker
{
	class GridSpace
	{
		// member variables
		private Rectangle m_body = new Rectangle();
		private int m_size = 0;
		private int m_xIndex = 0;
		private int m_yIndex = 0;

		// construction
		public GridSpace(int size, int x, int y, int xIndex, int yIndex)
		{
			m_size = size;
			m_xIndex = xIndex;
			m_yIndex = yIndex;
			createDrawing(x, y);
			updateGraphics();
		}

		// properties

		// functions

		private void createDrawing(int x, int y)
		{
			m_body.StrokeThickness = .5;
			m_body.Stroke = new SolidColorBrush(Colors.Black);
			m_body.Height = m_size;
			m_body.Width = m_size;

			Canvas.SetLeft(m_body, x);
			Canvas.SetTop(m_body, y);
			Master.mapLog("Grid square created");
		}

		private void updateGraphics() // adds to active canvas
		{
			Master.activeCanvas.Children.Add(m_body);
		}
	}
}
