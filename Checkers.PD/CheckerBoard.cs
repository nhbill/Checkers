using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.PD
{
    public enum Player {  Player1, Player2, King, NoPlayer, DeadSpace }; 
    public class CheckerBoard
    {
        Player _player1 = Player.Player1 ;
        Player _player2 = Player.Player2;
        Player[,] _board = new Player[8, 8] ; 
        public CheckerBoard ()
        {

        }
        public Player Winner { get; set;  }

        public void ResetBoard()
        {
            for( int row = 0; row < 8; row++)
            {
                for (int column = 0 ; column < 8; column ++)
                {
                    _board[column, row] = Player.DeadSpace; 
                }
            }

            // player 1
            for ( int row = 0; row < 3; row ++)
            {
                int column = 1;
                if (row % 2 == 1)
                {
                    column = 0;
                }

                for (; column < 8; )
                {
                    _board[column, row] = Player.Player1;
                    column += 2;
                }

            }

            // rows 3, 4, need to be setup to allow users
            
            for (int column = 0; column < 8;)
            {
                _board[column, 3] = Player.NoPlayer;
                column += 2;
            }

            for (int column = 1; column < 8;)
            {
                _board[column, 4] = Player.NoPlayer;
                column += 2;
            }

            // player 2 
            for (int row = 5; row < 8; row++)
            {
                int column = 1;
                if (row % 2 == 1)
                {
                    column = 0; 
                }

                for (; column < 8; column += 2)
                {
                    _board[column, row] = Player.Player2;
                }
            }

            ShowBoard(); 
        }

        public void ShowBoard()
        {
            Console.WriteLine(); 

            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    if (_board[column, row] == Player.Player2)
                        Console.Write("2");
                    else if (_board[column, row] == Player.Player1)
                        Console.Write("1");
                    else if (_board[column, row] == Player.NoPlayer)
                        Console.Write(" ");
                    else if (_board[column, row] == Player.DeadSpace)
                        Console.Write("*");
                }
                Console.WriteLine(""); 
            }
        }

        Player _currentPlayer = Player.Player1; 
        public bool MakeMove() // keeps track of who went last
        {
            bool tryMove = DoPlayerMove(_currentPlayer);

            if (tryMove == false)
            {
                if (_currentPlayer == Player.Player1)
                    Winner = Player.Player2;
                else
                    Winner = Player.Player1;

                return false;
            }
            
            if (_currentPlayer == _player1)
                _currentPlayer = _player2;
            else
                _currentPlayer = _player1;

            return true; 
        }

        private Move PickMove(List<Move> moves)
        {
            if (moves.Any())
                return moves[0];
            else 
                return null;
        }

        private Jump PickJump(List<Jump> jumps)
        {
            if (jumps.Any())
            {
                Jump jump = jumps.FirstOrDefault(s => s.JumpCount == jumps.Max(t => t.JumpCount));
                return jump; 
            }
            else
                return null; 
        }

        public bool DoPlayerMove(Player player)
        {
        List<Move> moves = new List<Move>();
        List<Jump> jumps = new List<Jump>(); 
        
            for ( int row = 0; row < 8; row ++)
            {
                for ( int column = 0; column < 8; column ++)
                {
                    if (_board[column, row] == player)
                    {
                        moves.AddRange(GetMoves(player, new Point() { X = column, Y = row }));
                        jumps.AddRange(GetJumps(player, new Point() { X = column, Y = row }));
                    }
                }
            }

            if (jumps.Any() == false && moves.Any() == false)
                return false; 

            if ( jumps.Any())
                return MakeJump(player, PickJump(jumps)); 
            else
                return MakeMove(player, PickMove(moves));
        }

        public bool MakeMove(Player play, Move move)
        {
            _board[move.Destination.X, move.Destination.Y] = play;
            _board[move.Start.X, move.Start.Y] = Player.NoPlayer;
            return true;
        }

        public bool MakeJump(Player play, Jump jump)
        {
            _board[jump.Destination.X, jump.Destination.Y] = play;
            _board[jump.Start.X, jump.Start.Y] = Player.NoPlayer;
            _board[jump.Jumped.X, jump.Jumped.Y] = Player.NoPlayer;

            if ( jump.ChildJumps.Any())
            {
                foreach ( Jump child in jump.ChildJumps)
                {
                    _board[child.Destination.X, child.Destination.Y] = play;
                    _board[child.Start.X, child.Start.Y] = Player.NoPlayer;
                    _board[child.Jumped.X, child.Jumped.Y] = Player.NoPlayer;
                }
            }


            return true; 
        }

        List<Move> GetMoves( Player player, Point point )
        {
            List<Move> moves = new List<Move>(); 

            if ( player == Player.Player1)
            {
                // x + 1, y + 1
                GetTestPoint(point, 1, 1, moves);
                // x - 1 , y + 1 
                GetTestPoint(point, -1, 1, moves);
            }
            else if (player == Player.Player2)
            {
                // x + 1, y -1
                GetTestPoint(point, 1, -1, moves);
                // x - 1 , y -1 
                GetTestPoint(point, -1, -1, moves);
            }
            else if (player == Player.King)
            {
                // x + 1, y -1
                GetTestPoint(point, 1, -1, moves);
                // x - 1 , y -1 
                GetTestPoint(point, -1, -1, moves);
                // x + 1, y + 1
                GetTestPoint(point, 1, 1, moves);
                // x - 1 , y + 1 
                GetTestPoint(point, -1, 1, moves);
            }

            // do it backwards on check in case need to delete
            for(int index = moves.Count() - 1;  index >= 0; index -- )
            {
                Move move = moves[index];

                if (IsValid(move) == false)
                    moves.Remove(move); 
            }

            return moves;
        }

        private void GetTestPoint ( Point point, int x, int y, List<Move> moves)
        {
            Point testPoint = new Point(point);

            testPoint.X += x;
            testPoint.Y += y;

            if (Point.CheckPoint(testPoint))
            {
                if ( _board [testPoint.X, testPoint.Y] == Player.NoPlayer)
                    moves.Add(new Move(point, testPoint));
            }
        }

        private void GetTestPoint(Point point, int jumpedXOffset, int jumpedYOffset, int destinationXOffset, int destinationYOffset, List<Jump> jumps, Player player)
        {
            Point jumpedTest = new Point(point);
            jumpedTest.X += jumpedXOffset;
            jumpedTest.Y += jumpedYOffset;

            Point destinationTest = new Point(point);
            destinationTest.X += destinationXOffset;
            destinationTest.Y += destinationYOffset;

            Player jumpedPlayer;
            if (player == Player.Player1)
                jumpedPlayer = Player.Player2;
            else
                jumpedPlayer = Player.Player1; 

            if (Point.CheckPoint(jumpedTest) && Point.CheckPoint(destinationTest))
            {
                if ( _board[jumpedTest.X, jumpedTest.Y] == jumpedPlayer && _board[destinationTest.X, destinationTest.Y] == Player.NoPlayer)
                    jumps.Add(new Jump(point, jumpedTest, destinationTest));
            }
        }

        private bool IsValid( Move move )
        {
            if (_board[move.Destination.X, move.Destination.Y] != Player.NoPlayer)
                return false;
            else
                return true; 
        }

        private bool IsValid(Jump move, Player player)
        {
            Player testPlayer = Player.NoPlayer ;

            if (player == Player.Player1)
                testPlayer = _player2;
            else
                testPlayer = _player1; 

            if (_board[move.Destination.X, move.Destination.Y] != Player.NoPlayer && _board[move.Jumped.X, move.Jumped.Y] != testPlayer)
                return false;
            else
                return true;
        }

        List<Jump> GetJumps(Player player, Point point, Jump parent = null)
        {
            if (_board[point.X, point.Y] == Player.King)
                player = Player.King; 

            List<Jump> jumps = new List<Jump>();

            if (player == Player.Player1)
            {
                GetTestPoint(point, 1, 1, 2,2, jumps, player);
                GetTestPoint(point, -1, 1, -2, 2, jumps, player);
            }
            else if (player == Player.Player2)
            {
                GetTestPoint(point, 1, -1, 2, -2, jumps, player);
                GetTestPoint(point, -1, -1, -2, -2, jumps, player);
            }
            else if (player == Player.King)
            {
                GetTestPoint(point, 1, 1, 2, 2, jumps, player);
                GetTestPoint(point, -1, 1, -2, 2, jumps, player);
                GetTestPoint(point, 1, -1, 2, -2, jumps, player);
                GetTestPoint(point, -1, -1, -2, -2, jumps, player);
            }

            // does this have a first valid jump?
            if ( parent != null)
            {
                parent.ChildJumps.AddRange(jumps); 
            }

            List<Jump> childJumps = new List<Jump>();

            // do it backwards on check in case need to delete
            for (int index = jumps.Count() - 1; index >= 0; index--)
            {
                GetJumps(player, jumps[index].Destination, jumps[index]);

                if ( jumps[index].ChildJumps.Any())
                {
                    foreach (Jump childJump in jumps[index].ChildJumps)
                    {
                        GetJumps(player, childJump.Destination, childJump);
                    }
                }
            }

            return jumps;
        }
    }
}
 