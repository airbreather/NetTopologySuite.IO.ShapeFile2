using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace NetTopologySuite.IO.ShapeWrappers
{
    public struct PolyLineXY
    {
        public ShapefileBoundingBoxXY Box { get; set; }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public ReadOnlyMemory<byte> RawPartsData { get; set; }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public ReadOnlyMemory<byte> RawPointsData { get; set; }

        public PartsEnumerator GetEnumerator() => new PartsEnumerator(in this);

        public ref struct PartsEnumerator
        {
            private ReadOnlySpan<int> parts;

            private ReadOnlySpan<PointXY> allPoints;

            private int currentPartIndex;

            private int offset;

            private int count;

            internal PartsEnumerator(in PolyLineXY polyLine)
            {
                this.parts = MemoryMarshal.Cast<byte, int>(polyLine.RawPartsData.Span);
                this.allPoints = MemoryMarshal.Cast<byte, PointXY>(polyLine.RawPointsData.Span);
                this.currentPartIndex = -1;
                this.offset = 0;
                this.count = -1;
            }

            public ReadOnlySpan<PointXY> Current => this.allPoints.Slice(this.offset, this.count);

            public bool MoveNext()
            {
                int nextPartIndex = this.currentPartIndex + 1;
                if (nextPartIndex < this.parts.Length)
                {
                    this.currentPartIndex = nextPartIndex;
                    this.offset = this.parts[nextPartIndex];
                    int end = nextPartIndex == this.parts.Length - 1
                        ? this.allPoints.Length
                        : this.parts[nextPartIndex + 1];
                    this.count = end - this.offset;
                    return true;
                }

                return false;
            }
        }
    }
}
