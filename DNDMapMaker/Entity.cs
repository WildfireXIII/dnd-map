using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Input;

namespace DNDMapMaker
{
	public class Entity
	{
		// member variables
		private Rectangle m_body = new Rectangle();
		private ImageBrush m_bgImage;
		private string m_resName;

		private double m_currentX = 0;
		private double m_currentY = 0;

		private double m_gridX = 0; // offset from grid origin
		private double m_gridY = 0;

		private int m_gridSpaceX = 0; // relative gridspace position closest to top left (set when locked)
		private int m_gridSpaceY = 0; 

		private bool m_isSelected = false;
		private bool m_isLocked = false;

		private Map m_parent; // unsure if I even need this

		// construction
		public Entity(Map parent, string resName)
		{
			Master.mapLog("Entity created " + resName);
			m_parent = parent;
			m_resName = resName;
			loadImageBrush(resName);
			addToCanvas();
		}

		// properties
		public double getCurrentX() { return m_currentX; }
		public double getCurrentY() { return m_currentY; }

		public double getGridX() { return m_gridX; }
		public double getGridY() { return m_gridY; }

		public bool isLocked() { return m_isLocked; }
		public void setLocked(bool locked) 
		{
			m_isLocked = locked;
			m_gridX = m_currentX - m_parent.getOriginX();
			m_gridY = m_currentY - m_parent.getOriginY();
			//m_gridSpaceX = (int)(m_gridX / m_parent.getGridSize());
		}

		public bool isSelected() { return m_isSelected; }
		public void setSelected(bool selected) 
		{
			m_isSelected = selected;
			if (selected) { setHighlight(Colors.Green); }
			else { setHighlight(Colors.Transparent); }
		}

		// functions
		private void loadImageBrush(string resName)
		{
			m_bgImage = new ImageBrush(new BitmapImage(new Uri(Master.RES_FOLDER + "\\" + resName)));
			m_body.Fill = m_bgImage;
			m_body.Height = m_bgImage.ImageSource.Height;
			m_body.Width = m_bgImage.ImageSource.Width;
			m_body.StrokeThickness = 4;
			m_body.Stroke = Brushes.Transparent;

			m_body.MouseDown += new MouseButtonEventHandler(body_MouseDown);
		}

		public void scaleVerbatim(int width, double height)
		{
			m_body.Width = width;
			m_body.Height = height;
		}

		public void scale() 
		{ 
			// find place to move left point
			//int gridSize = m_parent.getGridSize();
			//int originX = m_parent.getOriginX();

			// get original ratio
			int oldGridSizeX = m_parent.getLastGridSize() * m_parent.getGridSquaresX();
			int newGridSizeX = m_parent.getGridSize() * m_parent.getGridSquaresX();
			int oldGridSizeY = m_parent.getLastGridSize() * m_parent.getGridSquaresY();
			int newGridSizeY = m_parent.getGridSize() * m_parent.getGridSquaresY();

			double leftRatio = ((double)m_gridX / (double)oldGridSizeX);
			m_gridX = newGridSizeX * leftRatio;

			double rightRatio = ((double)(m_gridX + m_body.Width) / (double)oldGridSizeX);
			m_body.Width = newGridSizeX * rightRatio - m_gridX;
			
			double topRatio = ((double)m_gridY / (double)oldGridSizeY);
			m_gridY = newGridSizeY * leftRatio;

			double bottomRatio = ((double)(m_gridY + m_body.Height) / (double)oldGridSizeY);
			m_body.Height = newGridSizeY * bottomRatio - m_gridY;


			/*Master.mapLog("newgridx: " + m_gridX);
			Master.mapLog("ratio: " + leftRatio);
			Master.mapLog("OldTotalGridSizeX: " + oldGridSizeX);
			Master.mapLog("newTotalGridSizeX: " + newGridSizeX);
			Master.mapLog("lastGridSize: " + m_parent.getLastGridSize());
			Master.mapLog("GridSize: " + m_parent.getGridSize());*/


			//int newX = (int)(m_gridSpaceX*gridSize + originX);

			//m_gridX = (newX - originX);

			move(m_gridX + m_parent.getOriginX(), m_gridY + m_parent.getOriginY());

			//int originY = m

		}

		public void move(double x, double y)
		{
			m_currentX = x;
			m_currentY = y;
			Canvas.SetLeft(m_body, x);
			Canvas.SetTop(m_body, y);
		}
		public void setHighlight(Color c)
		{
			m_body.Stroke = new SolidColorBrush(c);
		}

		private void addToCanvas()
		{
			Master.activeCanvas.Children.Add(m_body);
		}

		public override string ToString()
		{
			return m_resName.Substring(0, m_resName.LastIndexOf('.'));
		}

		// EVENT HANDLERS

		private void body_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				m_parent.setSelectedEntity(this);

				// highlight
				m_body.Stroke = Brushes.Blue;

				// get point for offsets
				Point p = e.GetPosition(Master.activeCanvas);
				int x = (int)p.X;
				int y = (int)p.Y;

				Master.setMapOffsetX(x - m_currentX);
				Master.setMapOffsetY(y - m_currentY);

				Master.setDraggingEntity(true, this);
			}
		}
	}
}
