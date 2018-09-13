using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using static NetTopologySuite.IO.BitTwiddlers;

namespace NetTopologySuite.IO
{
    public abstract class ShapefileRecordVisitorBase : ShapefileVisitorBase
    {
        public override ValueTask VisitMainFileRecordAsync(ReadOnlyMemory<byte> rawRecordData, CancellationToken cancellationToken = default)
        {
            var shapeType = (ShapeType)ToOrFromLittleEndian(Unsafe.ReadUnaligned<int>(ref Unsafe.AsRef(rawRecordData.Span[0])));
            var innerRecordData = rawRecordData.Slice(sizeof(ShapeType));

            switch (shapeType)
            {
                case ShapeType.Null:
                    return this.OnVisitNullShapeAsync(cancellationToken);

                case ShapeType.Point:
                case ShapeType.PolyLine:
                case ShapeType.Polygon:
                case ShapeType.MultiPoint:
                case ShapeType.PointZ:
                case ShapeType.PolyLineZ:
                case ShapeType.PolygonZ:
                case ShapeType.MultiPointZ:
                case ShapeType.PointM:
                case ShapeType.PolyLineM:
                case ShapeType.PolygonM:
                case ShapeType.MultiPointM:
                case ShapeType.MultiPatch:
                    throw new NotImplementedException("Still working on it.");

                default:
                    throw new NotSupportedException("Unrecognized shape type: " + shapeType);
            }
        }

        protected virtual ValueTask OnVisitNullShapeAsync(CancellationToken cancellationToken) => default;
    }
}
