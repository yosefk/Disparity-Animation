using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disparity
{
    class LinesInterp
    {
        static float interp(float n1, float n2, float w1)
        {
            return n1 * w1 + n2 * (1 - w1);
        }

        //Beier and Neely mention two methods:
        //* interpolate endpoints (cited drawback: rotating lines shrink)
        //* interpolate center, orientation and length (cited drawback: intuitiveness to user)
        public static Line interp_endpoints(Line l1, Line l2, float w1)
        {
            Line l3;
            l3.p.X = interp(l1.p.X, l2.p.X, w1);
            l3.p.Y = interp(l1.p.Y, l2.p.Y, w1);
            l3.q.X = interp(l1.q.X, l2.q.X, w1);
            l3.q.Y = interp(l1.q.Y, l2.q.Y, w1);
            return l3;
        }
    }
}
