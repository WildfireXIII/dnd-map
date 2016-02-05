﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNDMapMaker
{
	public class Map
	{
		// member variables
		private int m_sizeX = 0;
		private int m_sizeY = 0;

		private int m_squaresX = 20;
		private int m_squaresY = 20;

		private int m_gridSize = 40; // one side

		private List<GridSpace> m_grid = new List<GridSpace>();
		private List<Entity> m_entities = new List<Entity>();

		private Entity m_selectedEntity;

		// upper left corner of map in comparison to canvas upper left
		private int m_mapOriginX = 0;
		private int m_mapOriginY = 0;

		// construction
		public Map()
		{
			// set up initial sizes
			m_sizeX = m_gridSize * m_squaresX;
			m_sizeY = m_gridSize * m_squaresY;
			createGrid();
		}

		// properties
		public int getGridSquaresX() { return m_squaresX; }
		public int getGridSquaresY() { return m_squaresY; }

		public Entity getSelectedEntity() { return m_selectedEntity; }
		public void setSelectedEntity(Entity entity) 
		{
			if (m_selectedEntity != null) { m_selectedEntity.setSelected(false); }
			m_selectedEntity = entity;
			m_selectedEntity.setSelected(true);
		}

		public int getGridSize() { return m_gridSize; }
		public void setGridSize(int size)
		{
			if (size <= 10) { return; } // don't let it go below certain size, otherwise multiplication scaling wont' work!

			//int gridDiff = size - m_gridSize;
			//int totalDiffX = gridDiff * m_squaresX;
			//int totalDiffY = gridDiff * m_squaresY;

			m_gridSize = size;
			scaleMap(2, 2);
		}
		public void setGridPos(int x, int y)
		{
			//Master.mapLog("Setting map pos to (" + x + "," + y + ")"); // DEBUG
			m_mapOriginX = x;
			m_mapOriginY = y;
			foreach (GridSpace g in m_grid)
			{
				g.updatePosition();
			}
			foreach (Entity e in m_entities)
			{
				if (e.isLocked())
				{
					e.move(x - e.getGridX(), y - e.getGridY());
				}
			}
		}
		public int getOriginX() { return m_mapOriginX; }
		public int getOriginY() { return m_mapOriginY; }

		// functions

		// returns created entity so can be added to listbox
		public Entity addResource(string resName)
		{
			Entity e = new Entity(this, resName);
			//e.setLocked(true);
			m_entities.Add(e);
			return e;
		}

		private void createGrid()
		{
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
					//e.scale(x, y);
				}
			}
		}
	}
}
