using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.PD
{
    public class Point
    {
        public Point()
        {
        }

        public Point ( int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point(Point point)
        {
            X = point.X;
            Y = point.Y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public static bool CheckPoint(Point point)
        {
            return CheckPoint(point.X, point.Y); 
        }

        public static bool CheckPoint( int x, int y)
        {
            if (x < 0 || x > 7 || y < 0 || y > 7 )
                return false;
            return true;
        }
    }
}
