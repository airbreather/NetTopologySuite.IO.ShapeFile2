using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using static NetTopologySuite.IO.BitTwiddlers;

namespace NetTopologySuite.IO
{
    public class ShapefileVisitorBase
    {
        private static readonly ValueTask<bool> CompletedTrueTask = new ValueTask<bool>(true);

        public virtual ValueTask VisitMainFileHeaderAsync(ShapefileHeader header, CancellationToken cancellationToken = default) => default;

        public virtual ValueTask VisitMainFileRecordHeaderAsync(ShapefileMainFileRecordHeader header, CancellationToken cancellationToken = default) => default;

        public async ValueTask VisitMainFileRecordAsync(ReadOnlyMemory<byte> rawRecordData, CancellationToken cancellationToken = default)
        {
            bool continueProcessing = await this.OnVisitRawMainFileRecordAsync(rawRecordData, cancellationToken).ConfigureAwait(false);

            if (continueProcessing)
            {
                await this.ProcessInnerRecordAsync(rawRecordData, cancellationToken).ConfigureAwait(false);
            }
        }

        protected virtual ValueTask<bool> OnVisitRawMainFileRecordAsync(ReadOnlyMemory<byte> rawRecordData, CancellationToken cancellationToken) => CompletedTrueTask;

        protected virtual ValueTask OnVisitNullShapeAsync(CancellationToken cancellationToken) => default;

        private ValueTask ProcessInnerRecordAsync(ReadOnlyMemory<byte> rawRecordData, CancellationToken cancellationToken)
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
    }
}
