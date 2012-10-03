using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disparity
{
    class LinesInterp
    {
        static int interp_ints(int n1, int n2, float w1)
        {
            return (int)Math.Round(n1 * w1 + n2 * (1 - w1));
        }

        //Beier and Neely mention two methods:
        //* interpolate endpoints (cited drawback: rotating lines shrink)
        //* interpolate center, orientation and length (cited drawback: intuitiveness to user)
        public static IntLine interp_endpoints(IntLine l1, IntLine l2, float w1)
        {
            IntLine l3;
            l3.p.X = interp_ints(l1.p.X, l2.p.X, w1);
            l3.p.Y = interp_ints(l1.p.Y, l2.p.Y, w1);
            l3.q.X = interp_ints(l1.q.X, l2.q.X, w1);
            l3.q.Y = interp_ints(l1.q.Y, l2.q.Y, w1);
            return l3;
        }
    }
}
