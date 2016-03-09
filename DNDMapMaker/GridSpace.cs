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

		private Map m_parent;

		// for when dragging around entire map
		private double m_offsetX = 0;
		private double m_offsetY = 0;

		private double m_currentX = 0;
		private double m_currentY = 0;

		// construction
		public GridSpace(Map parent, int size, int x, int y, int xIndex, int yIndex)
		{
			m_size = size;
			m_xIndex = xIndex;
			m_yIndex = yIndex;

			m_currentX = x;
			m_currentY = y;

			m_parent = parent;

			createDrawing(x, y);
			updateGraphics();
		}

		// properties
		public void setOffsetX(double x) { m_offsetX = x; }
		public void setOffsetY(double y) { m_offsetY = y; }

		public double getCurrentX() { return m_currentX; }
		public double getCurrentY() { return m_currentY; }

		public void setColor(Color c) { m_body.Stroke = new SolidColorBrush(c);  }

		// functions

		public void setGridSize(int size)
		{
			m_size = size;
			m_body.Width = size;
			m_body.Height = size;
			updatePosition();
		}

		public void updatePosition() 
		{
			m_currentX = m_size * m_xIndex + m_parent.getOriginX();
			m_currentY = m_size * m_yIndex + m_parent.getOriginY();
			Canvas.SetLeft(m_body, m_currentX);
			Canvas.SetTop(m_body, m_currentY); 
		}

		private void createDrawing(int x, int y)
		{
			m_body.StrokeThickness = .25;
			m_body.Stroke = new SolidColorBrush(Colors.Black);
			m_body.Height = m_size;
			m_body.Width = m_size;
			Canvas.SetZIndex(m_body, 100);

			Canvas.SetLeft(m_body, x);
			Canvas.SetTop(m_body, y);
			//Master.mapLog("Grid square created"); // DEBUG
		}

		private void updateGraphics() // adds to active canvas
		{
			Master.activeCanvas.Children.Add(m_body);
		}
		public void deleteGraphics()
		{
			Master.activeCanvas.Children.Remove(m_body);
		}
	}
}
