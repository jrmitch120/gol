using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Classes
{
    [Serializable]
    public class Grid
    {
        public List<Row> Rows { get; set; }

        public Cell this[int x, int y]
        {
            get { if (Rows.Count <= y || Rows[y].Cells.Count <= x) throw new ArgumentOutOfRangeException(); return Rows[y].Cells[x]; }
            set { if (Rows.Count <= y || Rows[y].Cells.Count <= x) throw new ArgumentOutOfRangeException(); Rows[y].Cells[x] = value; }
        }

        public Grid(int x, int y)
        {
            Rows = new List<Row>();

            for (int i = 0; i < y; i++)
                Rows.Add(new Row(x)); 
        }

        /// <summary>
        /// Reset the grid to all dead cells
        /// </summary>
        public void Reset()
        {
            foreach (Cell cell in Rows.SelectMany(x => x.Cells))
                cell.Alive = false;
        }

        /// <summary>
        /// Process the grid for Conway's game of life
        /// </summary>
        public void ProcessGrid()
        {
            // Copy of the existing grid
            var work = this.Clone();

            Parallel.For(0, Rows.Count, y =>
                Parallel.For(0, Rows[y].Cells.Count, x =>
                {
                    lock (work)
                    {
                        work.Rows[y].Cells[x].Alive = CalculateHealth(x, y);
                    }
                })
            );

            Rows = work.Rows;
        }

        /// <summary>
        /// Determine if a specific cell is deemed to be alive in the context of the grid
        /// </summary>
        /// <param name="column">Column of the cell (X)</param>
        /// <param name="row">Row of the cell (Y)</param>
        /// <returns>Is the requested cell alive?</returns>
        private bool  CalculateHealth(int column, int row)
        {
            int startRow = row - 1;
            int startCell = column - 1;

            int neighbors = 0;

            // Check neighbors
            for (int y = startRow; y < startRow + 3; y++)
            {
                for (int x = startCell; x < startCell + 3; x++)
                {
                    if (y == row && x == column)
                        continue;

                    // Out of bounds row
                    if (y < 0 || y >= Rows.Count)
                        continue;

                    // Out of bounds cell
                    if (x < 0 || x >= Rows[y].Cells.Count)
                        continue;

                    if(this[x, y].Alive)
                        neighbors++;
                }
            }

            // Stay alive
            if (this[column,row].Alive && (neighbors == 2 || neighbors == 3))
                return (true);

            // Come back to life
            if (!this[column, row].Alive && neighbors == 3)
                return (true);

            // You're dead
            return (false);
        }
    }
}
