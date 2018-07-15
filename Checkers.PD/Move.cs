using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.PD
{
    public class Move
    {
        protected Point _start;
        protected Point _destination;

        public Move(Point start, Point end)
        {
            _start = start ; 
            _destination = end ; 
        }

        public Move ( int x, int y, int endX, int endY)
        {
            _start = new Point() { X = x, Y = y };
            _destination = new Point() { X = endX, Y = endY };
        }

        public Point Start { get { return _start; } set { _start = value; } }
        public Point Destination { get { return _destination; } set { _destination = value; } }
    }
}
