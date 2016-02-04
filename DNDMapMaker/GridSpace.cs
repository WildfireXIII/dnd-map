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

		// for when dragging around entire map
		private int m_offsetX = 0;
		private int m_offsetY = 0;

		private int m_currentX = 0;
		private int m_currentY = 0;

		// construction
		public GridSpace(int size, int x, int y, int xIndex, int yIndex)
		{
			m_size = size;
			m_xIndex = xIndex;
			m_yIndex = yIndex;

			m_currentX = x;
			m_currentY = y;

			createDrawing(x, y);
			updateGraphics();
		}

		// properties
		public void setOffsetX(int x) { m_offsetX = x; }
		public void setOffsetY(int y) { m_offsetY = y; }

		// functions

		public void setGridSize(int size)
		{
			m_size = size;
			updatePosition(m_currentX, m_currentY);
		}

		private void updatePosition(int x, int y) // pass in MOUSE POS, this function will handle offsets
		{
			m_currentX = m_size * m_xIndex + (x - m_offsetX);
			m_currentY = m_size * m_yIndex + (y - m_offsetY);
			Canvas.SetLeft(m_body, m_currentX);
			Canvas.SetTop(m_body, m_currentY); 
		}

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
