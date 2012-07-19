using System;
using System.Diagnostics;
using System.Linq;
using Server.Classes;

namespace Server
{
    class Program
    {
        private static readonly int GridWidth = Console.WindowWidth;
        private static readonly int GridHeight = Console.WindowHeight-2;

        //static void Main(string[] args)
        static void Main()
        {
            var game = new Game(GridWidth, GridHeight);
            var sw = new Stopwatch();

            Console.SetWindowSize(GridWidth, GridHeight + 1);

            // 20% initial population density
            game.Randomize(20);
            //game.GenerateShape(Shape.Exploder);

            // Initial population statistics
            var total = GridWidth * GridHeight;
            var alive = game.GridOfLife.Rows.SelectMany(x => x.Cells).Count(x => x.Alive);
            var percentage = (decimal)alive / total * 100;

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
                    
                    Console.Write("Generation: " + game.CurrentGeneration.ToString() + 
                                             " Tick Time: " + sw.Elapsed + 
                                             " Game Over?: " + (game.GameOver ? "Yes.  Final Gen: " + game.FinalGeneration : "No"));

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
                    if (game.CurrentGeneration == 0 || game.GridOfLife[x,y].Alive != game.PreviousGenerations.Peek()[x,y].Alive)
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
