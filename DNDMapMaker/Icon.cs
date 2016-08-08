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
	public class Icon
	{
		// member variables
		private Rectangle m_pBody = new Rectangle();
		private ImageBrush m_pBGImage;
		private string m_sResName;
		private string m_sIconName; // character/monster name etc.

		// width and height (in number of grid spaces it takes up)
		private int m_iSpacesX = 0;
		private int m_iSpacesY = 0;

		// offset from grid origin
		private int m_iGridX = 0; 
		private int m_iGridY = 0;

		private double m_dCurrentX = 0;
		private double m_dCurrentY = 0;

		private Map m_pParent;

		private GridSpace m_pGridSpace = null;

		public Icon(Map pParent, string sResName, int iSpacesX, int iSpacesY, string sIconName)
		{
			m_pParent = pParent;
			m_sResName = sResName;
			m_iSpacesX = iSpacesX;
			m_iSpacesY = iSpacesY;
			m_sIconName = sIconName;
			loadImageBrush(sResName);
			addToCanvas();
		}

		// PROPERTIES
		public double CurrentX { get { return m_dCurrentX; } set { m_dCurrentX = value; } }
		public double CurrentY { get { return m_dCurrentY; } set { m_dCurrentY = value; } }
		public GridSpace GridSpace { get { return m_pGridSpace; } set { m_pGridSpace = value; } }
		public ImageBrush Image { get { return m_pBGImage; } set { m_pBGImage = value; } } 
		public string Name { get { return m_sIconName; } set { m_sIconName = value; } }
		
		// FUNCTIONS
		
		private void addToCanvas() { Master.activeCanvas.Children.Add(m_pBody);	}
		
		private void loadImageBrush(string resName)
		{
			m_pBGImage = new ImageBrush(new BitmapImage(new Uri(Master.ICON_FOLDER + "\\" + resName)));
			m_pBody.Fill = m_pBGImage;

			m_pBody.Height = m_pParent.getGridSize() * m_iSpacesY;
			m_pBody.Width = m_pParent.getGridSize() * m_iSpacesX;
			
			m_pBody.StrokeThickness = .5;
			m_pBody.Stroke = Brushes.Transparent;

			//m_pBody.LayoutTransform = m_rotate;
			//m_rotate.Angle = 0;

			Canvas.SetZIndex(m_pBody, 100);
			//m_zIndex = 1;

			//m_scaleX = (int) m_pBody.Width;
			//m_scaleY = (int) m_pBody.Height;

			m_pBody.MouseDown += new MouseButtonEventHandler(body_MouseDown);

			m_pGridSpace = m_pParent.getGridSpace(0, 0);
			updatePosFromSpace();
		}

		public void setSelected(bool bSelected)
		{
			if (!bSelected) { m_pBody.Stroke = Brushes.Transparent; }
			else { m_pBody.Stroke = Brushes.Orange; }
		}

		public void updatePosFromSpace()
		{
			m_dCurrentX = m_pGridSpace.getCurrentX();
			m_dCurrentY = m_pGridSpace.getCurrentY();

			Canvas.SetLeft(m_pBody, m_dCurrentX);
			Canvas.SetTop(m_pBody, m_dCurrentY);
		}

		// NOTE: should only be used for dragging. Any other time, the icon should be squarely within its grid space, not offset
		public void move(double dX, double dY)
		{
			m_dCurrentX = dX;
			m_dCurrentY = dY;

			Canvas.SetLeft(m_pBody, m_dCurrentX);
			Canvas.SetTop(m_pBody, m_dCurrentY);
		}
		
		// EVENT HANDLERS

		private void body_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (Master.Mode == "design") { return; }
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				m_pParent.setSelectedIcon(this);

				// highlight
				m_pBody.Stroke = Brushes.Orange;

				// get point for offsets
				Point p = e.GetPosition(Master.activeCanvas);
				int x = (int)p.X;
				int y = (int)p.Y;

				Master.setMapOffsetX(x - m_dCurrentX);
				Master.setMapOffsetY(y - m_dCurrentY);

				Master.setDraggingIcon(true, this);
			}
		}
	}
}
