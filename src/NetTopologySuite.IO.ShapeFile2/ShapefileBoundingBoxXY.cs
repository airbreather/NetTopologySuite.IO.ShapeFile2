using System.Runtime.InteropServices;

namespace NetTopologySuite.IO
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ShapefileBoundingBoxXY
    {
        public double MinX { get; set; }

        public double MinY { get; set; }

        public double MaxX { get; set; }

        public double MaxY { get; set; }

        public override string ToString() => $"ShapefileBoundingBoxXY[MinX={this.MinX}, MinY={this.MinY}, MaxX={this.MaxX}, MaxY={this.MaxY}]";
    }
}
