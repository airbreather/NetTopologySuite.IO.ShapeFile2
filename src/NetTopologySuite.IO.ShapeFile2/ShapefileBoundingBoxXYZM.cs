using System;
using System.Runtime.InteropServices;

namespace NetTopologySuite.IO
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ShapefileBoundingBoxXYZM : IEquatable<ShapefileBoundingBoxXYZM>
    {
        public double MinX { get; set; }

        public double MinY { get; set; }

        public double MaxX { get; set; }

        public double MaxY { get; set; }

        public double MinZ { get; set; }

        public double MaxZ { get; set; }

        public double MinM { get; set; }

        public double MaxM { get; set; }

        public static bool operator ==(ShapefileBoundingBoxXYZM first, ShapefileBoundingBoxXYZM second) => BitTwiddlers.Equals(ref first, ref second);

        public static bool operator !=(ShapefileBoundingBoxXYZM first, ShapefileBoundingBoxXYZM second) => !BitTwiddlers.Equals(ref first, ref second);

        public override bool Equals(object obj) => obj is ShapefileBoundingBoxXYZM other && BitTwiddlers.Equals(ref this, ref other);

        public bool Equals(ShapefileBoundingBoxXYZM other) => BitTwiddlers.Equals(ref this, ref other);

        public override int GetHashCode() => BitTwiddlers.GetHashCode(ref this);

        public override string ToString() => $"ShapefileBoundingBoxXYZM[MinX={this.MinX}, MinY={this.MinY}, MaxX={this.MaxX}, MaxY={this.MaxY}, MinZ={this.MinZ}, MaxZ={this.MaxZ}, MinM={this.MinM}, MaxM={this.MaxM}]";
    }
}
