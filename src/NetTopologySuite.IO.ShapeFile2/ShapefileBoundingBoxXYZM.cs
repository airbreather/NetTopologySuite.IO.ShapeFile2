using System.Runtime.InteropServices;

namespace NetTopologySuite.IO
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ShapefileBoundingBoxXYZM
    {
        public double MinX { get; set; }

        public double MinY { get; set; }

        public double MaxX { get; set; }

        public double MaxY { get; set; }

        public double MinZ { get; set; }

        public double MaxZ { get; set; }

        public double MinM { get; set; }

        public double MaxM { get; set; }

        public override string ToString() => $"ShapefileBoundingBoxXYZM[MinX={this.MinX}, MinY={this.MinY}, MaxX={this.MaxX}, MaxY={this.MaxY}, MinZ={this.MinZ}, MaxZ={this.MaxZ}, MinM={this.MinM}, MaxM={this.MaxM}]";
    }
}
