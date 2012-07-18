using System;

namespace Server.Classes
{

    /// <summary>
    /// Conway's Game of Life 
    /// 
    /// http://en.wikipedia.org/wiki/Conway's_Game_of_Life
    /// </summary>
    public class Game
    {
        public Grid GridOfLife { get; private set; }
        public Tunnel<Grid> PreviousGenerations { get; private set; }
        public int GenerationCount { get; private set; }

        public Game(int x, int y)
        {
            GridOfLife = new Grid(x, y);
            PreviousGenerations = new Tunnel<Grid>(50);
            GenerationCount = 0;
        }
        
        /// <summary>
        /// Run a calculation round for the game
        /// </summary>
        public void Tick()
        {
            // Make a copy of the grid to tack onto our generation history
            PreviousGenerations.Push(GridOfLife.Clone());
            
            // Process the grid
            GridOfLife.ProcessGrid();
            
            // Add a generation
            GenerationCount++;
        }

        /// <summary>
        /// Percentage chance that a cell will be alive.
        /// </summary>
        /// <param name="percentage"></param>
        public void Randomize(int percentage = 50)
        {
            if (percentage < 0 || percentage > 100)
                throw (new ArgumentOutOfRangeException("percentage", "Percentage must be between 1 - 100."));
            
            var rand = new Random();
            foreach(Row row in GridOfLife.Rows)
            {
                foreach(Cell cell in row.Cells)
                    cell.Alive = rand.Next(1, 101) <= percentage;
            }
        }
    }
}
