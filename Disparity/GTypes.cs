using AForge;

namespace Disparity
{
    public struct IntLine
    {
        public IntPoint p;
        public IntPoint q;

        public Line floatline()
        {
            Line r;
            r.p = p;
            r.q = q;
            return r;
        }
    }

    public struct Line
    {
        public Point p;
        public Point q;
    }
}