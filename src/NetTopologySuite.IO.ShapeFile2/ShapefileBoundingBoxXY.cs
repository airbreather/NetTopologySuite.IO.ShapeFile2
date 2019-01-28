using System;
using System.Runtime.InteropServices;

namespace NetTopologySuite.IO
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ShapefileBoundingBoxXY : IEquatable<ShapefileBoundingBoxXY>
    {
        public double MinX { get; set; }

        public double MinY { get; set; }

        public double MaxX { get; set; }

        public double MaxY { get; set; }

        public static bool operator ==(ShapefileBoundingBoxXY first, ShapefileBoundingBoxXY second) => BitTwiddlers.Equals(ref first, ref second);

        public static bool operator !=(ShapefileBoundingBoxXY first, ShapefileBoundingBoxXY second) => !BitTwiddlers.Equals(ref first, ref second);

        public override bool Equals(object obj) => obj is ShapefileBoundingBoxXY other && BitTwiddlers.Equals(ref this, ref other);

        public bool Equals(ShapefileBoundingBoxXY other) => BitTwiddlers.Equals(ref this, ref other);

        public override int GetHashCode() => BitTwiddlers.GetHashCode(ref this);

        public override string ToString() => $"ShapefileBoundingBoxXY[MinX={this.MinX}, MinY={this.MinY}, MaxX={this.MaxX}, MaxY={this.MaxY}]";
    }
}
