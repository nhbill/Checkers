using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.PD
{
    public class Jump : Move
    {
        protected Point _jumped ;

        public Jump(Point start, Point jumped, Point destination):
            base (start, destination)
        {
            _jumped = jumped;
            ChildJumps = new List<Jump>();
        }

        public Jump ( int x, int y, int moveX, int moveY, int destX, int destY) :
            base (x, y, destX, destY)
        {
            _jumped = new Point() { X = x, Y = y };
        }

        public Point Jumped { get { return _jumped; } set { _jumped = value; } }

        public List<Jump> ChildJumps { get; set; }

        public int JumpCount {  get { return ChildJumps.Count() + 1; } }
    }
}
