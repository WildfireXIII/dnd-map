﻿using System;
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
		private Entity m_draggingEntity = null;


		private double m_draggingOffsetX = 0;
		private double m_draggingOffsetY = 0;

		private Entity m_hoverEntity = null;
		
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

			fillResourceList();

			cnvsWorld.RenderTransform = m_scale;
		}

		// PROPERTIES
		public Canvas getCanvas() { return cnvsWorld; }
		public void setDraggingEntity(bool isDragging, Entity draggingEntity)
		{
			m_isDraggingEntity = true;
			m_draggingEntity = draggingEntity;
		}

		public void setMapOffsetX(double off) { m_draggingOffsetX = off; }
		public void setMapOffsetY(double off) { m_draggingOffsetY = off; }
		//public int getMapOffsetX() { return m_draggingOffsetX; }
		//public int getMapOffsetY() { return m_draggingOffsetY; }
		
		// FUNCTIONS

		private void addResource(string resName)
		{
			Entity e = m_currentMap.addResource(resName);
			ListBoxItem item = new ListBoxItem();
			item.Content = e;
			lbEntities.Items.Add(item);
		}

		private void setPreviewPaneImage(string resName)
		{
			ImageBrush paneImage = new ImageBrush(new BitmapImage(new Uri(Master.RES_FOLDER + "\\" + resName)));
			paneImage.Stretch = Stretch.Uniform;
			rPreviewPane.Fill = paneImage;
		}

		private void fillResourceList()
		{
			//string[] fileList = Directory.GetFiles(Master.RES_FOLDER);
			List<string> fileList = Directory.EnumerateFiles(Master.RES_FOLDER).ToList();
			foreach (string fileName in fileList)
			{
				ListBoxItem item = new ListBoxItem();
				item.Content = fileName.Substring(fileName.LastIndexOf('\\') + 1);
				lbRes.Items.Add(item);
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
		}

		private void cnvsWorld_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			//if (e.Delta > 0) { m_currentMap.setGridSize(m_currentMap.getGridSize() + 1); }
			//if (e.Delta < 0) { m_currentMap.setGridSize(m_currentMap.getGridSize() - 1); }
			//int increase = m_cu
			if (e.Delta > 0) 
			{
				/*
				//int increase = (int)(m_currentMap.getGridSize() * 1.1); // how much each individual square will increase
				int increase = (int)(m_currentMap.getGridSize() + 2); // how much each individual square will increase
				//int dsize = increase - m_currentMap.getGridSize();
				//int xAdjusted = (int)(m_currentMap.getOriginX() - (dsize * m_currentMap.getGridSquaresX()) / 2);
				//int yAdjusted = (int)(m_currentMap.getOriginY() - (dsize * m_currentMap.getGridSquaresY()) / 2);
				m_currentMap.setGridSize(increase);
				//m_currentMap.setGridPos(xAdjusted, yAdjusted);
				*/

				m_scale.ScaleX *= 1.1;
				m_scale.ScaleY *= 1.1;
			}
			else if (e.Delta < 0) 
			{
				/*
				int decrease = (int)(m_currentMap.getGridSize() - 2);
				//int decrease = (int)(m_currentMap.getGridSize() * .9);
				//int dsize = m_currentMap.getGridSize() - decrease;
				//int xAdjusted = (int)(m_currentMap.getOriginX() + (dsize * m_currentMap.getGridSquaresX()) / 2);
				//int yAdjusted = (int)(m_currentMap.getOriginY() + (dsize * m_currentMap.getGridSquaresY()) / 2);
				m_currentMap.setGridSize(decrease);
				//m_currentMap.setGridPos(xAdjusted, yAdjusted);
				*/
				m_scale.ScaleX *= 0.9;
				m_scale.ScaleY *= 0.9;
			}
		}

		private void lbRes_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListBoxItem selected = (ListBoxItem)lbRes.SelectedItem;

			setPreviewPaneImage(selected.Content.ToString());
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			ListBoxItem selected = (ListBoxItem)lbRes.SelectedItem;
			addResource(selected.Content.ToString());
		}
	}
}
