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

		private int m_gridSize = 40; // one side

		private List<GridSpace> grid = new List<GridSpace>();

		// construction
		public Map()
		{
			// set up initial sizes
			m_sizeX = m_gridSize * 20;
			m_sizeY = m_gridSize * 20;
			createGrid();
		}

		// properties

		// functions

		private void createGrid()
		{
			int numX = (int)(m_sizeX / m_gridSize);
			int numY = (int)(m_sizeY / m_gridSize);

			// initialize grid spaces
			for (int x = 0; x < numX; x++)
			{
				for (int y = 0; y < numY; y++)
				{
					GridSpace s = new GridSpace(m_gridSize, x * m_gridSize, y * m_gridSize, x, y);
					grid.Add(s);
				}
			}
		}
	}
}
