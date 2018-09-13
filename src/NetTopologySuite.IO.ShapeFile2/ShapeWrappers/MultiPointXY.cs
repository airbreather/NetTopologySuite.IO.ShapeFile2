using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace NetTopologySuite.IO.ShapeWrappers
{
    public struct MultiPointXY
    {
        public ShapefileBoundingBoxXY Box { get; set; }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public ReadOnlyMemory<byte> RawPointsData { get; set; }

        public ReadOnlySpan<PointXY> Points => MemoryMarshal.Cast<byte, PointXY>(this.RawPointsData.Span);

        // not sure I'm ready for this yet...
#if false
        public void SetPointsToArrayCopiedFrom(ReadOnlySpan<PointXY> points)
        {
            var inputRawPointsData = MemoryMarshal.Cast<PointXY, byte>(points);
            byte[] finalRawPointsData = new byte[inputRawPointsData.Length];
            inputRawPointsData.CopyTo(finalRawPointsData);
            this.RawPointsData = finalRawPointsData;
        }
#endif
    }
}
