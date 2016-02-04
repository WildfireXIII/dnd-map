using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNDMapMaker
{
	class Map
	{
		// member variables
		private int m_sizeX = 0;
		private int m_sizeY = 0;

		private int m_squaresX = 20;
		private int m_squaresY = 20;

		private int m_gridSize = 40; // one side

		private List<GridSpace> m_grid = new List<GridSpace>();

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

		public int getGridSize() { return m_gridSize; }
		public void setGridSize(int size)
		{
			if (size <= 10) { return; } // don't let it go below certain size, otherwise multiplication scaling wont' work!
			m_gridSize = size;
			scaleMap();
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
		}
		public int getOriginX() { return m_mapOriginX; }
		public int getOriginY() { return m_mapOriginY; }

		// functions

		public void addResource(string resName)
		{
			Entity e = new Entity(resName);
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

	

		private void scaleMap()
		{
			Master.mapLog("Scaling map");
			foreach (GridSpace g in m_grid)
			{
				g.setGridSize(m_gridSize);
			}
		}
	}
}
