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

namespace DNDMapMaker
{
	class Entity
	{
		private Rectangle m_body = new Rectangle();
		private ImageBrush m_bgImage;

		public Entity(string resName)
		{
			Master.mapLog("Entity created " + resName);
			loadImageBrush(resName);
			addToCanvas();
		}

		private void loadImageBrush(string resName)
		{
			m_bgImage = new ImageBrush(new BitmapImage(new Uri(Master.RES_FOLDER + "\\" + resName)));
			m_body.Fill = m_bgImage;
			m_body.Height = m_bgImage.ImageSource.Height;
			m_body.Width = m_bgImage.ImageSource.Width;
		}

		private void addToCanvas()
		{
			Master.activeCanvas.Children.Add(m_body);
		}
	}
}
