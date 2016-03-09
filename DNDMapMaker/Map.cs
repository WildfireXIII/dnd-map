using System;
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

		private int m_lastGridSize = 40;
		private int m_gridSize = 40; // one side

		private List<GridSpace> m_grid = new List<GridSpace>();
		private List<Entity> m_entities = new List<Entity>();

		private Entity m_selectedEntity;

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
			foreach (GridSpace g in m_grid)
			{
				g.updatePosition();
			}
			foreach (Entity e in m_entities)
			{
				if (e.isLocked())
				{
					e.move(x + e.getGridX(), y + e.getGridY());
				}
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

		// functions

		// returns created entity so can be added to listbox
		public Entity addResource(string resName)
		{
			Entity e = new Entity(this, resName);
			e.setLocked(true);
			m_entities.Add(e);
			return e;
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
	}
}
