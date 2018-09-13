using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using NetTopologySuite.IO.ShapeWrappers;

using static NetTopologySuite.IO.BitTwiddlers;

namespace NetTopologySuite.IO
{
    public abstract class ShapefileRecordVisitorBase : ShapefileVisitorBase
    {
        public sealed override ValueTask VisitMainFileRecordAsync(ReadOnlyMemory<byte> rawRecordData, CancellationToken cancellationToken = default)
        {
            var shapeType = (ShapeType)ToOrFromLittleEndian(MemoryMarshal.Read<int>(rawRecordData.Span));
            var innerRecordData = rawRecordData.Slice(sizeof(ShapeType));
            var innerRecordSpan = innerRecordData.Span;

            switch (shapeType)
            {
                case ShapeType.Null:
                    return this.OnVisitNullShapeAsync(cancellationToken);

                case ShapeType.Point:
                    return this.OnVisitPointXYAsync(MemoryMarshal.Read<PointXY>(innerRecordSpan), cancellationToken);

                // a Polygon is just a PolyLine with extra rules
                case ShapeType.PolyLine:
                case ShapeType.Polygon:
                    int numParts = ToOrFromLittleEndian(MemoryMarshal.Read<int>(innerRecordSpan.Slice(32)));
                    var rawPartsData = innerRecordData.Slice(40, sizeof(int) * numParts);
                    var polyLineXY = new PolyLineXY
                    {
                        Box = MemoryMarshal.Read<ShapefileBoundingBoxXY>(innerRecordSpan),
                        RawPartsData = rawPartsData,
                        RawPointsData = innerRecordData.Slice(40 + rawPartsData.Length),
                    };

                    return shapeType == ShapeType.PolyLine
                        ? this.OnVisitPolyLineXYAsync(polyLineXY, cancellationToken)
                        : this.OnVisitPolygonXYAsync(polyLineXY, cancellationToken);

                case ShapeType.MultiPoint:
                    var multiPointXY = new MultiPointXY
                    {
                        Box = MemoryMarshal.Read<ShapefileBoundingBoxXY>(innerRecordSpan),
                        RawPointsData = innerRecordData.Slice(36),
                    };

                    return this.OnVisitMultiPointXYAsync(multiPointXY, cancellationToken);

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

        protected virtual ValueTask OnVisitPointXYAsync(PointXY point, CancellationToken cancellationToken) => default;

        protected virtual ValueTask OnVisitMultiPointXYAsync(MultiPointXY multiPoint, CancellationToken cancellationToken) => default;

        protected virtual ValueTask OnVisitPolyLineXYAsync(PolyLineXY polyLine, CancellationToken cancellationToken) => default;

        protected virtual ValueTask OnVisitPolygonXYAsync(PolyLineXY polygon, CancellationToken cancellationToken) => default;
    }
}
