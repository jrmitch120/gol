using System;
using System.Diagnostics;
using System.Linq;
using Server.Classes;

namespace Server
{
    class Program
    {
        //private static readonly int GridWidth = Console.LargestWindowWidth - 20;
        //private static readonly int GridHeight = Console.LargestWindowHeight - 20;

        private static readonly int GridWidth = Console.WindowWidth;
        private static readonly int GridHeight = Console.WindowHeight-1;

        //static void Main(string[] args)
        static void Main()
        {
            var game = new Game(GridWidth, GridHeight);
            var sw = new Stopwatch();

            Console.SetWindowSize(GridWidth, GridHeight + 1);

            // 20% initial population density
            game.Randomize(20);

            // Initial population statistics
            var alive = game.GridOfLife.Rows.SelectMany(x => x.Cells).Count(x => x.Alive);
            var total = game.GridOfLife.Rows.SelectMany(x => x.Cells).Count();
            var percentage = (decimal)alive / total * 100;

            #region Playing w/Shapes

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

            // Super Exploder

            /*
            game.GridOfLife[73,24].Alive = true;
            game.GridOfLife[74,23].Alive = true;
            game.GridOfLife[75,23].Alive = true;
            game.GridOfLife[76,24].Alive = true;
            game.GridOfLife[75,25].Alive = true;
            game.GridOfLife[74,25].Alive = true;
            game.GridOfLife[73,22].Alive = true;
            game.GridOfLife[64,24].Alive = true;
            game.GridOfLife[65,23].Alive = true;
            game.GridOfLife[66,23].Alive = true;
            game.GridOfLife[67,24].Alive = true;
            game.GridOfLife[66,25].Alive = true;
            game.GridOfLife[65,25].Alive = true;
            game.GridOfLife[67,26].Alive = true;
            game.GridOfLife[70,21].Alive = true;
            game.GridOfLife[69,20].Alive = true;
            game.GridOfLife[69,19].Alive = true;
            game.GridOfLife[70,18].Alive = true;
            game.GridOfLife[71,19].Alive = true;
            game.GridOfLife[71,20].Alive = true;
            game.GridOfLife[68,21].Alive = true;
            game.GridOfLife[70,30].Alive = true;
            game.GridOfLife[69,29].Alive = true;
            game.GridOfLife[69,28].Alive = true;
            game.GridOfLife[70,27].Alive = true;
            game.GridOfLife[71,28].Alive = true;
            game.GridOfLife[71,29].Alive = true;
            game.GridOfLife[72,27].Alive = true;
            */

            // Exploder!
            /*
            game.GridOfLife[22,8].Alive = true;
            game.GridOfLife[21,9].Alive = true;
            game.GridOfLife[22,9].Alive = true;
            game.GridOfLife[23,9].Alive = true;
            game.GridOfLife[21,10].Alive = true;
            game.GridOfLife[23,10].Alive = true;
            game.GridOfLife[22,11].Alive = true;
            */

            /*
            // Blinker
            game.GridOfLife[1,1].Alive = true;
            game.GridOfLife[2,1].Alive = true;
            game.GridOfLife[3,1].Alive = true;
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

            #endregion

            DisplayGrid(game);

            Console.Write("** Initial Population: " + alive +  " of " + total + " : " + Math.Round(percentage,2) + "% **");
            Console.ReadKey();

            // Game loop
            do 
            {
                while (!Console.KeyAvailable)
                {
                    sw.Restart();                    
                    
                    game.Tick();

                    DisplayGrid(game);
                    
                    Console.Write("Generation: " + game.GenerationCount.ToString() + " Tick Time: " + sw.Elapsed + "        ");

                    if(sw.Elapsed.TotalMilliseconds <= 100)
                        System.Threading.Thread.Sleep(100);
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }

        static void DisplayGrid(Game game)
        {
            // Only update the parts of the screen that have changed since the previous generation
            for (int y = 0; y < game.GridOfLife.Rows.Count; y++)
            {
                for (int x = 0; x < game.GridOfLife.Rows[y].Cells.Count; x++)
                {
                    if (game.GenerationCount == 0 || game.GridOfLife[x,y].Alive != game.PreviousGenerations.Peek()[x,y].Alive)
                    {
                        Console.SetCursorPosition(x,y);
                        Console.ForegroundColor = SetColor(game.GridOfLife[x,y]);
                        Console.Write(game.GridOfLife[x,y].Alive ? "x" : ".");
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(0,game.GridOfLife.Rows.Count);
        }

        static ConsoleColor SetColor(Cell cell)
        {
            switch(cell.Alive)
            {
                case true:
                    return ConsoleColor.Yellow;

                default:
                    return ConsoleColor.Gray;
            }
        }
    }
}
