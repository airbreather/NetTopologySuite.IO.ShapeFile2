using System.Runtime.InteropServices;

using static NetTopologySuite.IO.BitTwiddlers;

namespace NetTopologySuite.IO
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ShapefileBoundingBoxXYZM
    {
        private double minX;

        private double minY;

        private double maxX;

        private double maxY;

        private double minZ;

        private double maxZ;

        private double minM;

        private double maxM;

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

        public double MinZ
        {
            get => ToOrFromLittleEndian(this.minZ);
            set => this.minZ = ToOrFromLittleEndian(value);
        }

        public double MaxZ
        {
            get => ToOrFromLittleEndian(this.maxZ);
            set => this.maxZ = ToOrFromLittleEndian(value);
        }

        public double MinM
        {
            get => ToOrFromLittleEndian(this.minM);
            set => this.minM = ToOrFromLittleEndian(value);
        }

        public double MaxM
        {
            get => ToOrFromLittleEndian(this.maxM);
            set => this.maxM = ToOrFromLittleEndian(value);
        }

        public override string ToString() => $"ShapefileBoundingBoxXYZM[MinX={this.MinX}, MinY={this.MinY}, MaxX={this.MaxX}, MaxY={this.MaxY}, MinZ={this.MinZ}, MaxZ={this.MaxZ}, MinM={this.MinM}, MaxM={this.MaxM}]";
    }
}
