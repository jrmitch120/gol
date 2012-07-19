using System;

using KellermanSoftware.CompareNetObjects;

namespace Server.Classes
{

    /// <summary>
    /// Conway's Game of Life 
    /// 
    /// http://en.wikipedia.org/wiki/Conway's_Game_of_Life
    /// </summary>
    public class Game
    {
        /// <summary>
        /// The game grid
        /// </summary>
        public Grid GridOfLife { get; private set; }
        
        /// <summary>
        /// Generational history of the grid
        /// </summary>
        public Tunnel<Grid> PreviousGenerations { get; private set; }
        
        /// <summary>
        /// The current generation of the game
        /// </summary>
        public int CurrentGeneration { get; private set; }

        /// <summary>
        /// The generation in which a Game Over state is achieved
        /// </summary>
        public int FinalGeneration { get; private set; }

        /// <summary>
        /// Is the game "over"?  IE, will the state no longer advance or is it stuck in an infinate pattern loop.
        /// This decision is limited by the size of PreviousGeneration history.
        /// </summary>
        public bool GameOver { get; private set; }

        /// <summary>
        /// A Conway's Game of Life!
        /// </summary>
        /// <param name="x">Grid width</param>
        /// <param name="y">Grid height</param>
        public Game(int x, int y)
        {
            GridOfLife = new Grid(x, y);
            PreviousGenerations = new Tunnel<Grid>(10);
            CurrentGeneration = 0;
        }

        /// <summary>
        /// Look at the game grid and try to make a determination if the game is over
        /// </summary>
        private void Analyze()
        {
            // Don't analyze games that are complete
            if (GameOver)
                return;

            var objCmp = new CompareObjects();
            foreach(Grid previousGen in PreviousGenerations)
            {
                if (objCmp.Compare(previousGen.Rows, GridOfLife.Rows))
                {
                    GameOver = true;
                    FinalGeneration = CurrentGeneration;
                    break;
                }
            }
        }

        public void GenerateShape(Shape shape)
        {
            if (GridOfLife == null || GridOfLife.Rows.Count < 10 || GridOfLife.Rows[0].Cells.Count < 10)
                throw new GridException("Grid must be at least 10 x 10");

            GridOfLife.Reset();

            int x = GridOfLife.Rows[0].Cells.Count/2;
            int y = GridOfLife.Rows.Count/2;

            switch(shape)
            {
                case Shape.Blinker:
                    GridOfLife[x, y].Alive = true;
                    GridOfLife[x, y-1].Alive = true;
                    GridOfLife[x, y+1].Alive = true;
                    break;

                case Shape.Exploder:
                    GridOfLife[x,y-1].Alive = true;
                    GridOfLife[x -1,y].Alive = true;
                    GridOfLife[x,y].Alive = true;
                    GridOfLife[x+1,y].Alive = true;
                    GridOfLife[x-1,y+1].Alive = true;
                    GridOfLife[x+1,y+1].Alive = true;
                    GridOfLife[x,y+2].Alive = true;
                    break;

                case Shape.SuperExploder:
                   GridOfLife[x,y].Alive = true;
                   GridOfLife[x+1,y-1].Alive = true;
                   GridOfLife[x+2,y-1].Alive = true;
                   GridOfLife[x+3,y].Alive = true;
                   GridOfLife[x+2,y+1].Alive = true;
                   GridOfLife[x+1,y+1].Alive = true;
                   GridOfLife[x,y-2].Alive = true;
                   GridOfLife[x-9,y].Alive = true;
                   GridOfLife[x-8,y-1].Alive = true;
                   GridOfLife[x-7,y-1].Alive = true;
                   GridOfLife[x-6,y].Alive = true;
                   GridOfLife[x-7,y+1].Alive = true;
                   GridOfLife[x-8,y+1].Alive = true;
                   GridOfLife[x-6,y+2].Alive = true;
                   GridOfLife[x-3,y-3].Alive = true;
                   GridOfLife[x-4,y-4].Alive = true;
                   GridOfLife[x-4,y-5].Alive = true;
                   GridOfLife[x-3,y-6].Alive = true;
                   GridOfLife[x-2,y-5].Alive = true;
                   GridOfLife[x-2,y-4].Alive = true;
                   GridOfLife[x-5,y-3].Alive = true;
                   GridOfLife[x-3,y+6].Alive = true;
                   GridOfLife[x-4,y+5].Alive = true;
                   GridOfLife[x-4,y+4].Alive = true;
                   GridOfLife[x-3,y+3].Alive = true;
                   GridOfLife[x-2,y+4].Alive = true;
                   GridOfLife[x-2,y+5].Alive = true;
                   GridOfLife[x-1,y+3].Alive = true;
                   break;

                // 10 cell oscilator
                /* 
                 game.GridOfLife[50,30].Alive = true;
                 game.GridOfLife[51,30].Alive = true;
                 game.GridOfLife[52,30].Alive = true;
                 game.GridOfLife[53,30].Alive = true;
                 game.GridOfLife[54,30].Alive = true;
                 game.GridOfLife[55,30].Alive = true;
                 game.GridOfLife[56,30].Alive = true;
                 game.GridOfLife[57,30].Alive = true;
                 game.GridOfLife[58,30].Alive = true;
                 game.GridOfLife[59,30].Alive = true;
                 */

                /*
                // Beehive
                game.GridOfLife[15,10].Alive = true;
                game.GridOfLife[15,11].Alive = true;
                game.GridOfLife[16,9].Alive = true;
                game.GridOfLife[16,12].Alive = true;
                game.GridOfLife[17,10].Alive = true;
                game.GridOfLife[17,11].Alive = true;
                */

                /*
                // Beacon
                game.GridOfLife[40,10].Alive = true;
                game.GridOfLife[41,10].Alive = true;
                game.GridOfLife[40,11].Alive = true;
                game.GridOfLife[43,12].Alive = true;
                game.GridOfLife[42,13].Alive = true;
                game.GridOfLife[43,13].Alive = true;
                */
            }
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
            CurrentGeneration++;

            // Run analysis
            Analyze();
        }

        /// <summary>
        /// Randomly populate the grid with live cells
        /// </summary>
        /// <param name="percentage">Percentage chance that a cell will be alive.</param>
        public void Randomize(int percentage = 50)
        {
            if (percentage < 1 || percentage > 100)
                throw (new ArgumentOutOfRangeException("percentage", "Percentage must be between 1 - 100."));
            
            var rand = new Random();
            foreach(Row row in GridOfLife.Rows)
            {
                foreach(Cell cell in row.Cells)
                    cell.Alive = rand.Next(1, 101) <= percentage;
            }
        }
    }

    public enum Shape
    {
        Blinker,
        Exploder,
        SuperExploder
    }
}
