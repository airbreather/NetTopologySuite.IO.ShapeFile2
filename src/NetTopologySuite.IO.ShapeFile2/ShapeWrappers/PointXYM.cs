using System.Runtime.InteropServices;

namespace NetTopologySuite.IO.ShapeWrappers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PointXYM
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double M { get; set; }
    }
}
