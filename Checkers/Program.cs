using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    class Program
    {
        static void Main(string[] args)
        {
            Checkers.PD.CheckerBoard board = new PD.CheckerBoard();
            board.ResetBoard();

            while (true)
            {
                if (board.MakeMove() == false)
                {
                    Console.WriteLine("Game over - the winner is " + board.Winner.ToString());
                    board.ResetBoard();
                }

                board.ShowBoard();
            }
        }
    }
}
