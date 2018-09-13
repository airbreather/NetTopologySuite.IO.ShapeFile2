using System.Runtime.InteropServices;

using static NetTopologySuite.IO.BitTwiddlers;

namespace NetTopologySuite.IO.ShapeWrappers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PointXY
    {
        private double x;

        private double y;

        public double X
        {
            get => ToOrFromLittleEndian(this.x);
            set => this.x = ToOrFromLittleEndian(value);
        }

        public double Y
        {
            get => ToOrFromLittleEndian(this.y);
            set => this.y = ToOrFromLittleEndian(value);
        }
    }
}
