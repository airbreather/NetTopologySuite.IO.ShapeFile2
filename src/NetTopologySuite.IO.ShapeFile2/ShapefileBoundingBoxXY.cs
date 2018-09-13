using System.Runtime.InteropServices;

using static NetTopologySuite.IO.BitTwiddlers;

namespace NetTopologySuite.IO
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ShapefileBoundingBoxXY
    {
        private double minX;

        private double minY;

        private double maxX;

        private double maxY;

        public double MinX
        {
            get => ToOrFromLittleEndian(this.minX);
            set => this.minX = ToOrFromLittleEndian(value);
        }

        public double MinY
        {
            get => ToOrFromLittleEndian(this.minY);
            set => this.minY = ToOrFromLittleEndian(value);
        }

        public double MaxX
        {
            get => ToOrFromLittleEndian(this.maxX);
            set => this.maxX = ToOrFromLittleEndian(value);
        }

        public double MaxY
        {
            get => ToOrFromLittleEndian(this.maxY);
            set => this.maxY = ToOrFromLittleEndian(value);
        }

        public override string ToString() => $"ShapefileBoundingBoxXY[MinX={this.MinX}, MinY={this.MinY}, MaxX={this.MaxX}, MaxY={this.MaxY}]";
    }
}
