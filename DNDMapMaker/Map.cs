﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DNDMapMaker
{
	public class Map
	{
		// member variables
		private int m_sizeX = 0;
		private int m_sizeY = 0;

		private int m_squaresX = 20;
		private int m_squaresY = 20;

		private int m_lastGridSize = 40;
		private int m_gridSize = 40; // one side

		private string m_color = "Black";
		
		private List<GridSpace> m_grid = new List<GridSpace>();
		private List<Entity> m_entities = new List<Entity>();
		private List<Icon> m_icons = new List<Icon>();

		private Entity m_selectedEntity = null;
		private Icon m_selectedIcon = null;

		// upper left corner of map in comparison to canvas upper left
		private double m_mapOriginX = 0;
		private double m_mapOriginY = 0;

		// construction
		public Map()
		{
			// set up initial sizes
			configureGridSizing();
			createGrid();
		}

		// properties
		public List<Icon> Icons { get { return m_icons; } set { m_icons = value; } }
		
		public int getGridSquaresX() { return m_squaresX; }
		public int getGridSquaresY() { return m_squaresY; }

		public Entity getSelectedEntity() { return m_selectedEntity; }
		public void setSelectedEntity(Entity entity) 
		{
			if (m_selectedEntity != null) { m_selectedEntity.setSelected(false); }
			m_selectedEntity = entity;
			m_selectedEntity.setSelected(true);
			Master.setMapSelectedEntity(m_selectedEntity);
		}
		public Icon getSelectedIcon() { return m_selectedIcon; }
		public void setSelectedIcon(Icon pIcon)
		{
			if (m_selectedIcon != null) { m_selectedIcon.setSelected(false); }
			m_selectedIcon = pIcon;
			m_selectedIcon.setSelected(true);
			Master.setMapSelectedIcon(m_selectedIcon);
		}

		public void deselectAllEntities() { if (m_selectedEntity != null) { m_selectedEntity.setSelected(false); } }

		public int getLastGridSize() { return m_lastGridSize; }
		public int getGridSize() { return m_gridSize; }
		public void setGridSize(int size)
		{
			if (size <= 10) { return; } // don't let it go below certain size, otherwise multiplication scaling wont' work!

			//int gridDiff = size - m_gridSize;
			//int totalDiffX = gridDiff * m_squaresX;
			//int totalDiffY = gridDiff * m_squaresY;
			m_lastGridSize = m_gridSize;
			m_gridSize = size;
			scaleMap(2, 2);
		}
		public void setGridPos(double x, double y)
		{
			//Master.mapLog("Setting map pos to (" + x + "," + y + ")"); // DEBUG
			m_mapOriginX = x;
			m_mapOriginY = y;

			// move all gridspaces 
			foreach (GridSpace g in m_grid)
			{
				g.updatePosition();
			}

			// move all entities
			foreach (Entity e in m_entities)
			{
				if (e.isLocked())
				{
					e.move(x + e.getGridX(), y + e.getGridY());
				}
			}

			// move all icons
			foreach (Icon pIcon in m_icons)
			{
				pIcon.updatePosFromSpace();
			}
		}
		public double getOriginX() { return m_mapOriginX; }
		public double getOriginY() { return m_mapOriginY; }
		public void setGridSquareCount(int x, int y)
		{
			m_squaresX = x;
			m_squaresY = y;
			configureGridSizing();
			createGrid();
		}

		public void setColor(string color)
		{
			m_color = color;
			Color actualColor = Colors.Red;
			if (m_color.ToLower() == "black") { actualColor = Colors.Black; }
			if (m_color.ToLower() == "white") { actualColor = Colors.White; }
			
			// set color on each grid space
			foreach (GridSpace gs in m_grid)
			{
				gs.setColor(actualColor);
			}
		}
		public string getColor() { return m_color; }

		// functions

		public GridSpace getClosestGridSpace(double dGoalX, double dGoalY)
		{
			GridSpace pClosest = null;
			double dClosestX = 1000;
			double dClosestY = 1000;
			
			foreach (GridSpace pGridSpace in m_grid)
			{
				double difX = Math.Abs(dGoalX - pGridSpace.getCurrentX());
				double difY = Math.Abs(dGoalY - pGridSpace.getCurrentY());

				if (difX <= dClosestX && difY <= dClosestY)
				{
					pClosest = pGridSpace;
					dClosestX = difX;
					dClosestY = difY;
				}
			}

			return pClosest;
		}

		public GridSpace getGridSpace(int iX, int iY)
		{
			foreach (GridSpace pGridSpace in m_grid)
			{
				if (pGridSpace.XSpace == iX && pGridSpace.YSpace == iY) { return pGridSpace; }
			}
			return null;
		}

		// returns created entity so can be added to listbox
		public Entity addResource(string resName)
		{
			Entity e = new Entity(this, resName);
			e.setLocked(true);
			m_entities.Add(e);
			return e;
		}

		public Icon addIcon(string sIconName, int iWidth, int iHeight, string sName)
		{
			Icon pIcon = new Icon(this, sIconName, iWidth, iHeight, sName);
			m_icons.Add(pIcon);
			return pIcon;
		}

		private void configureGridSizing()
		{
			m_sizeX = m_gridSize * m_squaresX;
			m_sizeY = m_gridSize * m_squaresY;
		}
		private void createGrid()
		{
			// clear any current instances of the grid
			foreach (GridSpace s in m_grid) { s.deleteGraphics(); }
			m_grid.Clear();

			int numX = (int)(m_sizeX / m_gridSize);
			int numY = (int)(m_sizeY / m_gridSize);

			// initialize grid spaces
			for (int x = 0; x < numX; x++)
			{
				for (int y = 0; y < numY; y++)
				{
					GridSpace s = new GridSpace(this, m_gridSize, x * m_gridSize, y * m_gridSize, x, y);
					m_grid.Add(s);
				}
			}
		}

		// x and y only affect entities
		private void scaleMap(int x, int y)
		{
			Master.mapLog("Scaling map");
			foreach (GridSpace g in m_grid)
			{
				g.setGridSize(m_gridSize);
			}
			foreach (Entity e in m_entities)
			{
				if (e.isLocked())
				{
					e.scale();
				}
			}
		}

		public void saveMap(string fileName)
		{
			string savedData = "";

			// first save map attributes
			savedData += "x=" + m_squaresX.ToString();
			savedData += "\ny=" + m_squaresY.ToString();
			savedData += "\ncolor=" + m_color;
			
			// save all entities
			foreach (Entity e in m_entities)
			{
				savedData += "\n|ENTITY|";
				savedData += "\nres=" + e.getResName();
				savedData += "\ngridx=" + e.getGridX();
				savedData += "\ngridy=" + e.getGridY();
				savedData += "\nscalex=" + e.getScaleX();
				savedData += "\nscaley=" + e.getScaleY();
				savedData += "\nz=" + e.getZIndex();
				savedData += "\nangle=" + e.getAngle();
			}

			savedData += "\n|ENDDATA|";

			StreamWriter file = new StreamWriter(fileName);
			file.WriteLine(savedData);
			file.Close();
		}
		public void openMap(string fileName)
		{
			bool readingEntity = false;

			string line;

			StreamReader file = new StreamReader(fileName);
			//Entity curEntity = null; // don't need, because can just access last added entity
			while ((line = file.ReadLine()) != null)
			{
				// check important command characters
				if (line.Substring(0, 1) == "|")
				{
					if (line == "|ENTITY|") 
					{
						readingEntity = true;
						//curEntity = null;
						// if was already working on entity, add to list
						//if (curEntity != null) { m_entities.Add(curEntity); }
						continue;
					}
				}

				// non-entity values
				if (!readingEntity)
				{
					if (line.Substring(0, 1) == "x")
					{
						m_squaresX = Int32.Parse(getValue(line));
						setGridSquareCount(m_squaresX, m_squaresY);
					}
					else if (line.Substring(0, 1) == "y")
					{
						m_squaresY = Int32.Parse(getValue(line));
						setGridSquareCount(m_squaresX, m_squaresY);
					}
					else if (line.StartsWith("color"))
					{
						m_color = getValue(line);
						setColor(m_color);	
					}
				}
				else //if reading entity
				{
					if (line.Length > 3 && line.Substring(0, 3) == "res")
					{
						Master.addResource(getValue(line));
					}
					else if (line.Length > 5 && line.Substring(0, 5) == "gridx")
					{
						double x = Double.Parse(getValue(line));
						m_entities[m_entities.Count - 1].move(x, m_entities[m_entities.Count - 1].getGridY());
					}
					else if (line.Length > 5 && line.Substring(0, 5) == "gridy")
					{
						double y = Double.Parse(getValue(line));
						m_entities[m_entities.Count - 1].move(m_entities[m_entities.Count - 1].getGridX(), y);
					}
					else if (line.Length > 6 && line.Substring(0, 6) == "scalex")
					{
						double x = Double.Parse(getValue(line));
						m_entities[m_entities.Count - 1].scaleVerbatim(x, m_entities[m_entities.Count - 1].getScaleY());
					}
					else if (line.Length > 6 && line.Substring(0, 6) == "scaley")
					{
						double y = Double.Parse(getValue(line));
						m_entities[m_entities.Count - 1].scaleVerbatim(m_entities[m_entities.Count - 1].getScaleX(), y);
					}
					else if (line.Substring(0, 1) == "z")
					{
						int z = Int32.Parse(getValue(line));
						m_entities[m_entities.Count - 1].setZIndex(z);
					}
					else if (line.Length > 5 && line.Substring(0, 5) == "angle")
					{
						int angle = Int32.Parse(getValue(line));
						m_entities[m_entities.Count - 1].setAngle(angle);
					}
				}		
			}

			file.Close();
		}

		private string getValue(string line) { return line.Substring(line.IndexOf("=") + 1); }

		public static Map LoadMap(string mapName)
		{
			Map map = new Map();
			map.openMap(mapName);
			return map; 
		}
	}
}
