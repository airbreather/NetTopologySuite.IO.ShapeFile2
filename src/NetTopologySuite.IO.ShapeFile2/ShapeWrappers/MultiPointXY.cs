using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace NetTopologySuite.IO.ShapeWrappers
{
    public struct MultiPointXY
    {
        public ShapefileBoundingBoxXY Box;

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public ReadOnlyMemory<byte> RawPointsData;

        public ReadOnlySpan<PointXY> Points => MemoryMarshal.Cast<byte, PointXY>(this.RawPointsData.Span);
    }
}
