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
using System.IO;

namespace DNDMapMaker
{


	public partial class MapDesignWindow : Window
	{
		// member variables

		// states
		private bool m_isDraggingMap = false;
		private bool m_isDraggingEntity = false;
		private bool m_isDraggingIcon = false;
		private Entity m_draggingEntity = null;
		private Icon m_draggingIcon = null;

		private double m_initialScaleX = 0.0;
		private double m_initialScaleY = 0.0;

		private double m_draggingOffsetX = 0;
		private double m_draggingOffsetY = 0;

		//private Entity m_hoverEntity = null;
		private Entity m_selectedEntity = null; // ONLY USED FOR PROPERTY STUFF
		private Icon m_selectedIcon = null;

		private Map m_currentMap;

		private ScaleTransform m_scale = new ScaleTransform();

		// construction
		public MapDesignWindow()
		{
			InitializeComponent();

			if (Master.DISPLAY_DEBUG) { lblDebug.IsEnabled = true; lblDebug.Visibility = Visibility.Visible; }

			// make sure canvas has a background
			cnvsWorld.Background = Brushes.White;

			Master.activeCanvas = cnvsWorld;
			Master.assignMapWin(this);

			Map m = new Map();
			m_currentMap = m;

			log("Log initialized!");
			log("Map created!");

			m.setGridSize(15);
			m.setGridPos(20, 20);

			fillIconList();
			fillResourceList();
			fillMapProperties();
			fillMapList();

			cnvsWorld.RenderTransform = m_scale;
			m_initialScaleX = m_scale.ScaleX;
			m_initialScaleY = m_scale.ScaleY;

			disableProperties();

			cnvsWorld.Focusable = true;

			//mediaElement1.Source = new Uri("C:\\trol.mp3");
			//mediaElement1.Play();
		}

		// PROPERTIES
		public Canvas getCanvas() { return cnvsWorld; }
		public void setDraggingEntity(bool isDragging, Entity draggingEntity)
		{
			m_isDraggingEntity = true;
			m_draggingEntity = draggingEntity;
		}
		public void setDraggingIcon(bool isDragging, Icon draggingIcon)
		{
			m_isDraggingIcon = true;
			m_draggingIcon = draggingIcon;
		}

		public void setMapOffsetX(double off) { m_draggingOffsetX = off; }
		public void setMapOffsetY(double off) { m_draggingOffsetY = off; }
		//public int getMapOffsetX() { return m_draggingOffsetX; }
		//public int getMapOffsetY() { return m_draggingOffsetY; }

		public void setSelectedEntity(Entity e) { m_selectedEntity = e; fillPropList(); }

		public void setSelectedIcon(Icon pSelectedIcon)
		{
			m_selectedIcon = pSelectedIcon;

			// select it in the playing icons listbox
			foreach (StackPanel pPanel in lbPlayingIcons.Items)
			{
				if (((Label)pPanel.Children[1]).Content.ToString() == pSelectedIcon.Name) { lbPlayingIcons.SelectedItem = pPanel; }
			}
		}

		// FUNCTIONS

		public void addResource(string resName)
		{
			Entity e = m_currentMap.addResource(resName);
			lbEntities.Items.Add(e);
			e.move(200, 50); // make it not in the corner of the window!
		}
		public void addIcon(string sIconName, int iWidth, int iHeight, string sName)
		{
			Icon pIcon = m_currentMap.addIcon(sIconName, iWidth, iHeight, sName);

			// update the list of playing icons
			//StackPanel pMainPanel = pnlPlayingIcons;

			StackPanel pIconStack = new StackPanel();
			pIconStack.Orientation = Orientation.Horizontal;

			Rectangle pImage = new Rectangle();
			pImage.Fill = pIcon.Image;
			pImage.Height = 20;
			pImage.Width = 20;

			Label lblIconName = new Label();
			lblIconName.Content = pIcon.Name;

			pIconStack.Children.Add(pImage);
			pIconStack.Children.Add(lblIconName);
			lbPlayingIcons.Items.Add(pIconStack);
			//pMainPanel.Children.Add(pIconStack);
		}

		private void setPreviewPaneImage(string resName)
		{
			ImageBrush paneImage = new ImageBrush(new BitmapImage(new Uri(Master.RES_FOLDER + "\\" + resName)));
			paneImage.Stretch = Stretch.Uniform;
			rPreviewPane.Fill = paneImage;
		}
		private void setIconPreviewPaneImage(string icoName)
		{
			ImageBrush paneImage = new ImageBrush(new BitmapImage(new Uri(Master.ICON_FOLDER + "\\" + icoName)));
			paneImage.Stretch = Stretch.Uniform;
			rIconPreviewPane.Fill = paneImage;
		}

		private void fillIconList()
		{
			lbIcons.Items.Clear();
			List<string> pFileList = Directory.EnumerateFiles(Master.ICON_FOLDER).ToList();
			foreach (string sFileName in pFileList)
			{
				ListBoxItem item = new ListBoxItem();
				item.Content = sFileName.Substring(sFileName.LastIndexOf('\\') + 1);
				lbIcons.Items.Add(item);
			}
		}

		private void fillResourceList()
		{
			List<string> fileList = Directory.EnumerateFiles(Master.RES_FOLDER).ToList();
			foreach (string fileName in fileList)
			{
				ListBoxItem item = new ListBoxItem();
				item.Content = fileName.Substring(fileName.LastIndexOf('\\') + 1);
				lbRes.Items.Add(item);
			}
		}

		private void fillMapProperties()
		{
			txtSquaresX.Text = m_currentMap.getGridSquaresX().ToString();
			txtSquaresY.Text = m_currentMap.getGridSquaresY().ToString();
		}

		private void fillPropList()
		{
			enableProperties();

			txtScaleX.Text = m_selectedEntity.getScaleX().ToString();
			txtScaleY.Text = m_selectedEntity.getScaleY().ToString();
			txtZIndex.Text = m_selectedEntity.getZIndex().ToString();
			txtScale.Text = "";
			txtAngle.Text = m_selectedEntity.getAngle().ToString();
		}

		private void fillMapList()
		{
			lbAvailableMaps.Items.Clear();
			List<string> maps = Directory.GetFiles("C:\\dwl\\tmp\\DNDRES\\maps", "*.map").Select(path => System.IO.Path.GetFileName(path)).ToList();
			foreach (string map in maps)
			{
				string curMap = map;
				// remove the .map extension
				if (!map.EndsWith(".map")) { continue; }

				curMap = map.Substring(0, map.IndexOf(".map"));

				lbAvailableMaps.Items.Add(curMap);
			}
		}
		
		private void enableProperties()
		{
			for (int i = 0; i < pnlProperties.Children.Count; i++)
			{
				pnlProperties.Children[i].IsEnabled = true;
			}
			
		}
		private void disableProperties()
		{
			for (int i = 0; i < pnlProperties.Children.Count; i++)
			{
				pnlProperties.Children[i].IsEnabled = false;
			}
		}

		public void log(string msg) { lblDebug.Content += msg + "\n"; svDebug.ScrollToBottom(); }

		// EVENT HANDLERS

		private void cnvsWorld_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.MiddleButton == MouseButtonState.Pressed)
			{
				m_isDraggingMap = true;

				// get point for offsets
				Point p = e.GetPosition(cnvsWorld);
				int x = (int)p.X;
				int y = (int)p.Y;

				m_draggingOffsetX = x - m_currentMap.getOriginX();
				m_draggingOffsetY = y - m_currentMap.getOriginY();
				log("Setting offsets to (" + x + "," + y + ")");
			}
			cnvsWorld.Focus();
		}

		private void cnvsWorld_MouseMove(object sender, MouseEventArgs e)
		{
			if (m_isDraggingMap)
			{
				Point p = e.GetPosition(cnvsWorld);
				double x = p.X - m_draggingOffsetX;
				double y = p.Y - m_draggingOffsetY;

				//log("Move coordinates (" + x + "," + y + ")"); // DEBUG

				m_currentMap.setGridPos(x, y);
			}
			else if (m_isDraggingEntity) // TODO: HANDLE LOCKING (or take care of updating needed stuff in mouseup, or in entity itself)
			{
				Point p = e.GetPosition(cnvsWorld);
				double x = p.X - m_draggingOffsetX;
				double y = p.Y - m_draggingOffsetY;

				m_draggingEntity.move(x, y);
			}
			else if (m_isDraggingIcon)
			{
				Point p = e.GetPosition(cnvsWorld);
				double x = p.X - m_draggingOffsetX;
				double y = p.Y - m_draggingOffsetY;

				m_draggingIcon.move(x, y);
				//m_draggingIcon.setSelected(false);
			} 
		}

		private void cnvsWorld_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (m_isDraggingMap)
			{
				m_isDraggingMap = false;
				m_draggingOffsetX = 0;
				m_draggingOffsetY = 0;
			}
			else if (m_isDraggingEntity)
			{
				if (m_draggingEntity.isSelected()) { m_draggingEntity.setHighlight(Colors.Green); }
				else { m_draggingEntity.setHighlight(Colors.Transparent); }

				m_isDraggingEntity = false;
				m_draggingOffsetX = 0;
				m_draggingOffsetY = 0;
			}
			// TODO: this is where you add gridspace checking for icon
			else if (m_isDraggingIcon)
			{
				m_isDraggingIcon = false;
				m_draggingOffsetX = 0;
				m_draggingOffsetY = 0;

				double dGoalX = m_draggingIcon.CurrentX;
				double dGoalY = m_draggingIcon.CurrentY;

				m_draggingIcon.GridSpace = m_currentMap.getClosestGridSpace(dGoalX, dGoalY);
				m_draggingIcon.updatePosFromSpace();
			}
		}

		private void cnvsWorld_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (e.Delta > 0) 
			{
				m_scale.ScaleX *= 1.1;
				m_scale.ScaleY *= 1.1;
			}
			else if (e.Delta < 0) 
			{
				m_scale.ScaleX *= 0.9;
				m_scale.ScaleY *= 0.9;
			}
		}

		// THIS IS ACTUALLY ASSIGNED TO WINDOW, because WPF has a weird thing that canvases don't get keyboard input...
		private void window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down && txtIconName.IsFocused == false)
			{
				if (Master.Mode == "play")
				{
					Icon pIcon = m_currentMap.getSelectedIcon();
					if (pIcon == null) { return; }

					int iCurrentX = pIcon.GridSpace.XSpace;
					int iCurrentY = pIcon.GridSpace.YSpace;

					if (e.Key == Key.Left)
					{
						if (iCurrentX == 0) { return; }
						iCurrentX--;
					}
					else if (e.Key == Key.Right)
					{
						if (iCurrentX == m_currentMap.getGridSquaresX() - 1) { return; }
						iCurrentX++;
					}
					else if (e.Key == Key.Up)
					{
						if (iCurrentY == 0) { return; }
						iCurrentY--;
					}
					else if (e.Key == Key.Down)
					{
						if (iCurrentY == m_currentMap.getGridSquaresY() - 1) { return; }
						iCurrentY++;
					}

					pIcon.GridSpace = m_currentMap.getGridSpace(iCurrentX, iCurrentY);
					pIcon.updatePosFromSpace();
					e.Handled = true;
				}
			}
		}

		private void lbRes_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListBoxItem selected = (ListBoxItem)lbRes.SelectedItem;

			setPreviewPaneImage(selected.Content.ToString());
		}

		private void lbIcons_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListBoxItem selected = (ListBoxItem)lbIcons.SelectedItem;
			if (selected == null) return;
			setIconPreviewPaneImage(selected.Content.ToString());
		}
		private void btnAddIcon_Click(object sender, RoutedEventArgs e)
		{
			ListBoxItem selected = (ListBoxItem)lbIcons.SelectedItem;
			addIcon(selected.Content.ToString(), Convert.ToInt32(txtIconX.Text), Convert.ToInt32(txtIconY.Text), txtIconName.Text);
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			ListBoxItem selected = (ListBoxItem)lbRes.SelectedItem;
			addResource(selected.Content.ToString());
		}

		private void lbEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Entity sel = (Entity)lbEntities.SelectedItem;
			m_currentMap.setSelectedEntity(sel);
		}
		
		private void lbAvailableMaps_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (lbAvailableMaps.SelectedItem == null) { return; } // TODO: better fix??
			string sel = lbAvailableMaps.SelectedItem.ToString();
			txtMapName.Text = sel;
		}

		private void btnSetScale_Click(object sender, RoutedEventArgs e)
		{
			if (m_selectedEntity != null)
			{
				double scaleX = Double.Parse(txtScaleX.Text);
				double scaleY = Double.Parse(txtScaleY.Text);

				m_selectedEntity.scaleVerbatim(scaleX, scaleY);

				/*System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"C:\trol.mp3");
			
				player.Play();
				m_selectedEntity.scaleVerbatim(10000,100000);*/
			}
		}

		private void btnSetZ_Click(object sender, RoutedEventArgs e)
		{
			if (m_selectedEntity != null)
			{
				int z = -1;
				try { z = Int32.Parse(txtZIndex.Text); }
				catch (Exception ex) { }

				if (z < 1 || z > 99) 
				{ 
					System.Windows.Forms.MessageBox.Show("Z-index must be between 1 and 99 (inclusive)", "Out of bounds", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
					txtZIndex.Text = m_selectedEntity.getZIndex().ToString();
					return; 
				}

				m_selectedEntity.setZIndex(z);
			}
		}

		private void btnSetScaleMultiplier_Click(object sender, RoutedEventArgs e)
		{
			if (m_selectedEntity != null)
			{
				double scale = Double.Parse(txtScale.Text);

				m_selectedEntity.scaleVerbatim(m_selectedEntity.getScaleX()*scale, m_selectedEntity.getScaleY()*scale);
				txtScaleX.Text = m_selectedEntity.getScaleX().ToString();
				txtScaleY.Text = m_selectedEntity.getScaleY().ToString();
			}
		}

		private void btnSetAngle_Click(object sender, RoutedEventArgs e)
		{
			if (m_selectedEntity != null)
			{
				int angle = -1;
				try { angle = Int32.Parse(txtAngle.Text); }
				catch (Exception ex) { }

				if (angle < 0 || angle > 360)
				{
					System.Windows.Forms.MessageBox.Show("Angle must be between 0 and 360 (inclusive)", "Out of bounds", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
					txtZIndex.Text = m_selectedEntity.getAngle().ToString();
					return;
				}

				m_selectedEntity.setAngle(angle);
			}
		}

		private void btnSetSquares_Click(object sender, RoutedEventArgs e)
		{
			int squaresX = Int32.Parse(txtSquaresX.Text);
			int squaresY = Int32.Parse(txtSquaresY.Text);
			m_currentMap.setGridSquareCount(squaresX, squaresY);
		}
		private void btnSetGridColor_Click(object sender, RoutedEventArgs e)
		{
			string gridColor = txtGridColor.Text;
			m_currentMap.setColor(gridColor);
		}

		private void btnSaveMap_Click(object sender, RoutedEventArgs e)
		{
			string mapName = txtMapName.Text;

			m_currentMap.saveMap("C:\\dwl\\tmp\\DNDRES\\maps\\" + mapName + ".map");
			fillMapList();
		}

		private void btnLoadMap_Click(object sender, RoutedEventArgs e)
		{
			lbEntities.Items.Clear();
			cnvsWorld.Children.Clear();
			m_scale.ScaleX = m_initialScaleX;
			m_scale.ScaleY = m_initialScaleY;
			
			string mapName = txtMapName.Text; // TODO: THIS NEEDS TO CHANGE OBVIOUSLY
			//m_currentMap.openMap("C:\\dwl\\tmp\\DNDRES\\maps\\" + mapName + ".map");
			m_currentMap = new Map();
			m_currentMap.setGridSize(15);
			m_currentMap.setGridPos(0, 0);
			//m_currentMap.openMap("C:\\dwl\\tmp\\DNDRES\\maps\\" + mapName + ".map");
			m_currentMap.openMap(Master.MAP_FOLDER + "\\" + mapName + ".map");
			//m_currentMap = Map.LoadMap("C:\\dwl\\tmp\\DNDRES\\maps\\" + mapName + ".map");

			m_currentMap.setGridPos(300, 50);

			// display map properties in GUI 
			txtSquaresX.Text = m_currentMap.getGridSquaresX().ToString();
			txtSquaresY.Text = m_currentMap.getGridSquaresY().ToString();
			txtGridColor.Text = m_currentMap.getColor();
		}

		// play map button! (too lazy to rename....wow)
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Panel.SetZIndex(MapPlayGrid, 11);
			MapPlayGrid.IsEnabled = true;
			MapPlayGrid.Visibility = Visibility.Visible;

			Panel.SetZIndex(MapDesignGrid, 9);
			MapDesignGrid.IsEnabled = false;
			MapDesignGrid.Visibility = Visibility.Hidden;
			this.Title = "Play Map!";
			Master.Mode = "play";
			m_currentMap.deselectAllEntities();
		}

		// go back to map design
		private void btnMapDesign_Click(object sender, RoutedEventArgs e)
		{
			Panel.SetZIndex(MapPlayGrid, 9);
			MapPlayGrid.IsEnabled = false;
			MapPlayGrid.Visibility = Visibility.Hidden;

			Panel.SetZIndex(MapDesignGrid, 11);
			MapDesignGrid.IsEnabled = true;
			MapDesignGrid.Visibility = Visibility.Visible;
			this.Title = "Map Design";
			Master.Mode = "design";
		}

		private void lbPlayingIcons_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			string sTarget = ((Label)((StackPanel)lbPlayingIcons.SelectedItem).Children[1]).Content.ToString();
			foreach (Icon pIcon in m_currentMap.Icons)
			{
				if (pIcon.Name == sTarget)
				{
					m_currentMap.setSelectedIcon(pIcon);
				}
			}
		}

		// this is the preview mouse down thing
		private void txtIconName_MouseDown(object sender, MouseButtonEventArgs e)
		{
			txtIconName.SelectAll();
			TextBox tb = (sender as TextBox);
			if (!tb.IsKeyboardFocusWithin)
			{
				e.Handled = true;
				tb.Focus();
			}
		}

		private void txtIconName_GotFocus(object sender, RoutedEventArgs e)
		{
			txtIconName.SelectAll();
		}

		private void txtIconName_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			txtIconName.SelectAll();
		}

		private void lbPlayingIcons_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				cnvsWorld.Focus();
			}
		}

		private void btnKillIcon_Click(object sender, RoutedEventArgs e)
		{
			if (m_selectedIcon == null) return;
			m_selectedIcon.kill();
			this.fillIconList();
		}
	}
}
